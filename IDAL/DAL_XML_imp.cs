using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Xml.Linq;
using BE;
using Utilities;

namespace IDAL
{
    public class DAL_XML_imp : IDAL
    {



        public static List<GuestRequest> _guests;
        public static List<Customer> _customers;
        public static List<Host> _hosts;
      //  public static List<BankBranch> _bankBranches; 
        public static SiteOwner _SiteOwner;

        #region c-tor & d-tor

        public DAL_XML_imp()
        {
            try
            {
                if (!File.Exists(XmlConfigurations.ConfigPath))
                {
                    SaveConfigurations();
                }
                else
                {
                   LoadOrderXmlFile();
                }
              
                if (!File.Exists(XmlConfigurations.OrdersPath))
                {
                    XmlConfigurations.OrderRoot = new XElement("Orders");
                    XmlConfigurations.OrderRoot.Save(XmlConfigurations.OrdersPath);
                }

                if (!File.Exists(XmlConfigurations.CustomerPath))
                {
                    Tools.SaveToXML(new List<Customer>(), XmlConfigurations.CustomerPath);
                }

                if (!File.Exists(XmlConfigurations.GuestRequestPath))
                {
                    Tools.SaveToXML(new List<GuestRequest>(), XmlConfigurations.GuestRequestPath);
                }

                if (!File.Exists(XmlConfigurations.HostsPath))
                {
                    Tools.SaveToXML(new List<Host>(), XmlConfigurations.HostsPath);
                }

                if (!File.Exists(XmlConfigurations.SiteOwnerPath))
                {
                    Tools.SaveToXML(new SiteOwner(), XmlConfigurations.SiteOwnerPath);
                }

                if (!File.Exists(XmlConfigurations.BankBranchesPath))
                {
                    XmlConfigurations.BankBranchesRoot = new XElement("BankBranches");
                    XmlConfigurations.BankBranchesRoot.Save(XmlConfigurations.BankBranchesPath);
                }
                XmlConfigurations.OrderRoot = XElement.Load(XmlConfigurations.OrdersPath);
                XmlConfigurations.BankBranchesRoot = XElement.Load(XmlConfigurations.BankBranchesPath);
                _customers = Tools.LoadFromXML<List<Customer>>(XmlConfigurations.CustomerPath);
                _guests = Tools.LoadFromXML<List<GuestRequest>>(XmlConfigurations.GuestRequestPath);
                _hosts = Tools.LoadFromXML<List<Host>>(XmlConfigurations.HostsPath);
                _SiteOwner = Tools.LoadFromXML<SiteOwner>(XmlConfigurations.SiteOwnerPath);

            }
            catch (Exception)
            {
                //do nothing
            }
        }

        ~DAL_XML_imp()
        {
            SaveConfigurations();
        }

        #endregion

        #region Orders 

        public void AddNewOrder(Order newOrder)
        {
            if (GetAllOrders(x => x.OrderKey == newOrder.OrderKey).FirstOrDefault() != null)
            {
                throw new IdOrderException("The order already exist in the system");
            }

            try
            {
                var orderXml = new XElement("Order");
                orderXml.Add(
                    new XElement("HostId", newOrder.HostId),
                    new XElement("AccommodationKey", newOrder.HostId),
                    new XElement("HostingUnitKey", newOrder.HostingUnitKey),
                    new XElement("GuestRequestKey", newOrder.GuestRequestKey),
                    new XElement("OrderKey", newOrder.OrderKey),
                    new XElement("OrderStatus", newOrder.OrderStatus),
                    new XElement("OrderCreationDate", newOrder.OrderCreationDate),
                    new XElement("SendingEmailDate", newOrder.SendingEmailDate));
                XmlConfigurations.OrderRoot.Add(orderXml);
                XmlConfigurations.OrderRoot.Save(XmlConfigurations.OrdersPath);
            }
            catch (Exception)
            {
                throw new OrderProblemException("XML Order file problem");
            }

          
        }

        public void OrderUpdate(int orderKey, enums.OrderStatus statusOfOrder)
        {
            var orderToUpdate = (from xmlOrder in XmlConfigurations.OrderRoot.Elements()
                where xmlOrder.Element("OrderKey")?.Value == orderKey.ToString()
                select xmlOrder).FirstOrDefault();

            if (orderToUpdate == null)
                throw new OrderProblemException("something wrong with the order key");



            orderToUpdate.Element("HostId").Value = orderToUpdate.Element("HostId")?.Value;
            orderToUpdate.Element("AccommodationKey").Value = orderToUpdate.Element("AccommodationKey")?.Value;
            orderToUpdate.Element("HostingUnitKey").Value = orderToUpdate.Element("HostingUnitKey")?.Value;
            orderToUpdate.Element("GuestRequestKey").Value = orderToUpdate.Element("GuestRequestKey")?.Value;
            orderToUpdate.Element("OrderKey").Value = orderToUpdate.Element("OrderKey")?.Value;
            orderToUpdate.Element("OrderStatus").Value = statusOfOrder.ToString();
            orderToUpdate.Element("OrderCreationDate").Value = orderToUpdate.Element("OrderCreationDate")?.Value;
            orderToUpdate.Element("SendingEmailDate").Value = orderToUpdate.Element("SendingEmailDate")?.Value;



            XmlConfigurations.OrderRoot.Save(XmlConfigurations.OrdersPath);

        }

        public void UpdateOrderCompletion(decimal commission, string hostKey)
        {
            foreach (var x in _hosts.Where(x => x.Id == hostKey))
            {
                x.CommissionSum += commission.Clone();
                Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
                return;
            }
            throw new HostsException("Host not found");

        }


        #endregion

        #region Customers

        public void AddCustomerContactInfo(Customer customerContactInfo)
        {
            if (_customers.Any(x =>
                x.ClientInfo.LoginDetails.UserName == customerContactInfo.ClientInfo.LoginDetails.UserName &&
                x.ClientInfo.LoginDetails.Password == customerContactInfo.ClientInfo.LoginDetails.Password))
                throw new CustomersException("Customer Already exist in system");

            _customers.Add(customerContactInfo.Clone());
           Tools.SaveToXML(_customers,XmlConfigurations.CustomerPath);
        }

        public void DeleteCustomer(Login customerKey)
        {
            //check if customer exist in the system
            if (_customers.All(x =>
                x.ClientInfo.LoginDetails.UserName != customerKey.UserName &&
                x.ClientInfo.LoginDetails.Password != customerKey.Password))
                throw new CustomersException("Customer doesn't exist");
            
            //remove customer from the system
            _customers.RemoveAll(x =>
                x.ClientInfo.LoginDetails.UserName == customerKey.UserName &&
                x.ClientInfo.LoginDetails.Password == customerKey.Password);
            
            Tools.SaveToXML(_customers, XmlConfigurations.CustomerPath);
            
        }


        #endregion

        #region Guest Requests

        //Add new Guest request
        public void AddACustomerRequirement(GuestRequest newGuestRequest)
        {
            if (newGuestRequest.GuestRequestKey == 0)
            {
                newGuestRequest.GuestRequestKey = Configuration.GuestRequestKey++;
                SaveConfigurations();
            }

            if (_guests.Any(x => x.GuestRequestKey == newGuestRequest.GuestRequestKey))
                throw new GuestStatusException("Guest key already exist in the system");

            _guests.Add(newGuestRequest);
            Tools.SaveToXML(_guests, XmlConfigurations.GuestRequestPath);

        }

        //Update status for guest request
        public void CustomerRequirementStatusUpdate(bool orderStatus, int guestRequestKey)
        {
            var ourGuest = _guests.Single(x => x.GuestRequestKey == guestRequestKey);
            if(ourGuest == null)
                throw new GuestStatusException("Guest not found");

            ourGuest.OrderStatus = orderStatus;
            Tools.SaveToXML(_guests,XmlConfigurations.GuestRequestPath);
            
        }


        #endregion

        #region Hosts
        public void AddHostContactInfo(Host hostContactInfo)
        {
            if (_hosts.Any(x =>
                x.HostInfo.LoginDetails.UserName == hostContactInfo.HostInfo.LoginDetails.UserName &&
                x.HostInfo.LoginDetails.Password == hostContactInfo.HostInfo.LoginDetails.Password))
                throw new HostsException("Host Already exist in system");
            
            _hosts.Add(hostContactInfo.Clone());
            Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);

        }
        public void DeleteHost(string hostId)
        {
            if (_hosts.All(x => x.Id != hostId))
                throw new HostsException("Host doesn't exist");

            var hostToRemove = _hosts.Single(r => r.Id == hostId);
            _hosts.Remove(hostToRemove);

            Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
        }


        #endregion

        #region Accommodations

        public void AddAccommodation(Accommodations addAccommodation)
        {
            if (_hosts.Any(hosts => hosts.Id != addAccommodation.HostId))
                throw new AccommodationsException("The Host is not exist in the system");

            if (addAccommodation.AccommodationKey == 0)
            {
                addAccommodation.AccommodationKey = Configuration.AccommodationKey++;
                SaveConfigurations();
            }

            var accommodationForHost = _hosts.SelectMany(x => x.HostAccommodationsList)
                .Where(y => y.HostId == addAccommodation.HostId).ToList();

            accommodationForHost.Add(addAccommodation.Clone());
            Tools.SaveToXML(_hosts,XmlConfigurations.HostsPath);

        }

        public void DeleteAccommodation(int accommodationKey, string hostKey)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).All(y => y.AccommodationKey != accommodationKey))
                throw new AccommodationsException("Accommodation doesn't exist");

            _hosts.ForEach(x => x.HostAccommodationsList.RemoveAll(y => y.AccommodationKey == accommodationKey && y.HostId == hostKey));

            Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
        }

        public void AccommodationStarsUpdate(int accommodationKey, string hostKey, uint stars)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).All(y => y.AccommodationKey != accommodationKey))
                throw new AccommodationsException("Accommodation doesn't exist");

            foreach (var y in from x in _hosts
                from y in x.HostAccommodationsList
                where y.AccommodationKey == accommodationKey && y.HostId == hostKey
                select y)
            {
                y.Stars = stars.Clone();
                Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
                return;
            }

            throw new HostsException("Host not found");


        }

        public void AccommodationSumOfUnitsUpdate(int accommodationKey, string hostKey, uint sumOfUnits)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).All(y => y.AccommodationKey != accommodationKey))
                throw new AccommodationsException("Accommodation doesn't exist");

            foreach (var y in from x in _hosts
                from y in x.HostAccommodationsList
                where y.AccommodationKey == accommodationKey && y.HostId == hostKey
                select y)
            {
                y.Stars = sumOfUnits.Clone();
                Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
                return;
            }

            throw new HostsException("Host not found");

        }

        public void AccommodationUpdate(int accommodationKey, string hostKey, string detailsInAccommodationUpdate, object newValue)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).All(y => y.AccommodationKey != accommodationKey))
                throw new AccommodationsException("Accommodation doesn't exist");


            switch (detailsInAccommodationUpdate)
            {
                case "Stars":
                    foreach (var getAccommodation in from getHost in _hosts
                        where getHost.Id == hostKey
                        from getAccommodation in getHost.HostAccommodationsList
                        where getAccommodation.AccommodationKey == accommodationKey
                        select getAccommodation)
                        getAccommodation.Stars = (uint)newValue.Clone();
                    break;
                case "SumOfUnits":
                    foreach (var getAccommodation in from getHost in _hosts
                        where getHost.Id == hostKey
                        from getAccommodation in getHost.HostAccommodationsList
                        where getAccommodation.AccommodationKey == accommodationKey
                        select getAccommodation) getAccommodation.SumOfUnits = (uint)newValue.Clone();
                    break;
                default:
                    throw new AccommodationsException("This string value: " + detailsInAccommodationUpdate + " are not legal");

            }

            Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);

        }


        #endregion

        #region HostinUnits

        public void AddAHostingUnit(HostingUnit addHostingUnit)
        {

            if (_hosts.Any(hosts => hosts.Id != addHostingUnit.HostId))
                throw new HostingUnitsException("The Host for this unit not exist in the system");

            if (addHostingUnit.HostingUnitKey == 0)
            {
                addHostingUnit.HostingUnitKey = Configuration.HostingUnitKey++;
               SaveConfigurations();
            }


            var hostOwner = _hosts.Find(a => a.Id == addHostingUnit.HostId);
            var unitList =
                hostOwner.HostAccommodationsList.Find(u => u.AccommodationKey == addHostingUnit.AccommodationKey);
           
            unitList.ListOfAllUnits.Add(addHostingUnit.Clone());
            Tools.SaveToXML(_hosts,XmlConfigurations.HostsPath);

        }

        public void DeleteHostingUnit(int unitKey, int accommodationKey, string hostKey)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).All(y => y.AccommodationKey != accommodationKey))
                throw new AccommodationsException("Accommodation doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).SelectMany(y => y.ListOfAllUnits)
                .All(z => z.HostingUnitKey != unitKey))
                throw new HostingUnitsException("host unit doesn't exist");

            _hosts.ForEach(x => x.HostAccommodationsList.ForEach(y => y.ListOfAllUnits.RemoveAll(z =>
                z.HostingUnitKey == unitKey && z.AccommodationKey == accommodationKey && z.HostId == hostKey)));


            Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
        }

        public void HostingUnitDiaryUpdate(int unitKey, int accommodationKey, string hostKey, DateTime checkIn, DateTime checkOut)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).All(y => y.AccommodationKey != accommodationKey))
                throw new AccommodationsException("Accommodation doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).SelectMany(y => y.ListOfAllUnits)
                .All(z => z.HostingUnitKey != unitKey))
                throw new HostingUnitsException("host unit doesn't exist");


            foreach (var z in from x in _hosts
                from y in x.HostAccommodationsList
                from z in y.ListOfAllUnits
                where z.HostingUnitKey == unitKey && z.AccommodationKey == accommodationKey && z.HostId == hostKey
                select z)
            {
                for (var i = checkIn; i < checkOut; i = i.AddDays(1)) z[i] = true;
                Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
                return;
            }

            throw new HostsException("Host not found");
        }

        public void HostingUnitPriceUpdate(int unitKey, int accommodationKey, string hostKey, decimal unitPrice)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).All(y => y.AccommodationKey != accommodationKey))
                throw new AccommodationsException("Accommodation doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).SelectMany(y => y.ListOfAllUnits)
                .All(z => z.HostingUnitKey != unitKey))
                throw new HostingUnitsException("host unit doesn't exist");


            foreach (var z in from x in _hosts
                from y in x.HostAccommodationsList
                from z in y.ListOfAllUnits
                where z.HostingUnitKey == unitKey && z.AccommodationKey == accommodationKey && z.HostId == hostKey
                select z)
            {
                z.UnitPrice = unitPrice.Clone();
                Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
                return;
            }

            throw new HostsException("Host not found");

        }

        public void HostingUnitElementsUpdate(int unitKey, int accommodationKey, string hostKey, FilterElements elements)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).All(y => y.AccommodationKey != accommodationKey))
                throw new AccommodationsException("Accommodation doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).SelectMany(y => y.ListOfAllUnits)
                .All(z => z.HostingUnitKey != unitKey))
                throw new HostingUnitsException("host unit doesn't exist");


            foreach (var z in from x in _hosts
                from y in x.HostAccommodationsList
                from z in y.ListOfAllUnits
                where z.HostingUnitKey == unitKey && z.AccommodationKey == accommodationKey && z.HostId == hostKey
                select z)
            {
                z.UnitOptions = elements.Clone();
                Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
                return;
            }

            throw new HostsException("Host not found");
        }

        public void HostingUnitSumOfOrdersUpdate(int unitKey, int accommodationKey, string hostKey, string offerOrCompleted)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).All(y => y.AccommodationKey != accommodationKey))
                throw new AccommodationsException("Accommodation doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).SelectMany(y => y.ListOfAllUnits)
                .All(z => z.HostingUnitKey != unitKey))
                throw new HostingUnitsException("host unit doesn't exist");

            foreach (var z in from x in _hosts
                from y in x.HostAccommodationsList
                from z in y.ListOfAllUnits
                where z.HostingUnitKey == unitKey && z.AccommodationKey == accommodationKey && z.HostId == hostKey
                select z)
            {
                switch (offerOrCompleted.ToLower())
                {
                    case "offered":
                        z.SumOfferOrders++;
                        break;
                    case "completed":
                        z.SumCompletedOrders++;
                        break;
                    default:
                        throw new HostingUnitsException("You need to send offered or completed in the string value");

                }
                Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
                return;
            }

            throw new HostsException("Host not found");

        }

        public void HostingUnitUpdate(int unitKey, int accommodationKey, string hostKey, enums.UnitUpdate detailsInUnitUpdate,
        object newValue)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).All(y => y.AccommodationKey != accommodationKey))
                throw new AccommodationsException("Accommodation doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).SelectMany(y => y.ListOfAllUnits)
                .All(z => z.HostingUnitKey != unitKey))
                throw new HostingUnitsException("host unit doesn't exist");


            foreach (var z in from x in _hosts
                              from y in x.HostAccommodationsList
                              from z in y.ListOfAllUnits
                              where z.HostingUnitKey == unitKey && z.AccommodationKey == accommodationKey && z.HostId == hostKey
                              select z)
            {
                switch (detailsInUnitUpdate)
                {
                    case enums.UnitUpdate.Price:
                        z.UnitPrice = (decimal)newValue.Clone();
                        Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
                        return;
                    case enums.UnitUpdate.ElementsUpdate:
                        z.UnitOptions = (FilterElements)newValue.Clone();
                        Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
                        return;
                    case enums.UnitUpdate.OrderCompleted:
                        z.SumCompletedOrders++;
                        Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
                        return;
                    case enums.UnitUpdate.OrderOffer:
                        Tools.SaveToXML(_hosts, XmlConfigurations.HostsPath);
                        return;
                    default:
                        throw new Exception("This value cannot be updated");

                }
            }

            throw new HostsException("Host not found");


        }


        #endregion

        #region Free Up Rooms after 11 months

        public void FreeUpDiaryDays(DateTime? fromDate = null)
        {
            foreach (var z in from x in _hosts from y in x.HostAccommodationsList from z in y.ListOfAllUnits select z) 
            {
                if (fromDate == null)
                    z[DateTime.Now.AddMonths(-11)] = false;
                else
                {
                    var dateToUpdate = DateTime.Now.AddMonths(-11);
                    for (var i = DateTime.Now; i > fromDate; i = i.AddDays(1))
                    {
                        z[dateToUpdate] = false;
                        dateToUpdate = dateToUpdate.AddDays(-1);
                    }
                }
            }

            Tools.SaveToXML(_hosts,XmlConfigurations.HostsPath);

        }


        #endregion

        #region Get one appeerance

        public HostingUnit GetHostingUnit(int unitKey, int accommodationKey, string hostKey)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).All(y => y.AccommodationKey != accommodationKey))
                throw new AccommodationsException("Accommodation doesn't exist");

            if (_hosts.SelectMany(x => x.HostAccommodationsList).SelectMany(y => y.ListOfAllUnits)
                .All(z => z.HostingUnitKey != unitKey))
                throw new HostingUnitsException("host unit doesn't exist");

            return _hosts.SelectMany(x => x.HostAccommodationsList).SelectMany(y => y.ListOfAllUnits).Single(z =>
                z.HostId == hostKey && z.AccommodationKey == accommodationKey && z.HostingUnitKey == unitKey).Clone();
        }

        public Host GetHost(string hostKey)
        {
            if (_hosts.All(x => x.Id != hostKey))
                throw new HostsException("Host doesn't exist");

            return _hosts.Single(x => x.Id == hostKey).Clone();
        }

        public GuestRequest GetGuestRequest(int guestRequestKey)
        {
            if (_guests.All(x => x.GuestRequestKey != guestRequestKey))
                throw new GuestRequestException("Guest request doesn't exist");

            return _guests.Single(x => x.GuestRequestKey == guestRequestKey).Clone();
        }

        #endregion
        
        #region Authentication

        public bool IsUserNameExist(string userNameToBeCheck)
        {
            return _SiteOwner.OwnerLogin.UserName == userNameToBeCheck
                       || (_hosts.Any(userName => userName.HostInfo.LoginDetails.UserName == userNameToBeCheck) 
                       || _customers.Any(userName => userName.ClientInfo.LoginDetails.UserName == userNameToBeCheck));
        }

        public bool IsUsernameMatchPassword(Login loginDetails, string authorizationType)
        {
            return ((authorizationType == "SiteOwner") && (_SiteOwner.OwnerLogin.UserName == loginDetails.UserName) &&
                    (_SiteOwner.OwnerLogin.Password == loginDetails.Password) || authorizationType == "Host") &&
                   (_hosts.Any(userName =>
                        userName.HostInfo.LoginDetails.UserName == loginDetails.UserName &&
                        userName.HostInfo.LoginDetails.Password == loginDetails.Password) ||
                    authorizationType == "Customer" && _customers.Any(userName =>
                        userName.ClientInfo.LoginDetails.UserName == loginDetails.UserName &&
                        userName.ClientInfo.LoginDetails.Password == loginDetails.Password));
        }


        #endregion

        #region Return Lists

        public IEnumerable<Accommodations> GetAllAccommodations(Func<Accommodations, bool> predicate = null)
        {
            return predicate == null
                ? _hosts.SelectMany(x => x.HostAccommodationsList).Select(y => y.Clone()).ToList()
                : _hosts.SelectMany(x => x.HostAccommodationsList).Where(predicate).Select(y => y.Clone())
                    .ToList();
        }

        public IEnumerable<HostingUnit> GetAllHostingUnits(Func<HostingUnit, bool> predicate = null)
        {
            return predicate == null
                ? _hosts.SelectMany(x => x.HostAccommodationsList)
                    .SelectMany(y => y.ListOfAllUnits).Select(z => z.Clone()).ToList()
                : _hosts.SelectMany(x => x.HostAccommodationsList)
                    .SelectMany(y => y.ListOfAllUnits).Where(predicate).Select(z => z.Clone()).ToList();
        }

        public IEnumerable<GuestRequest> GetAllGuestRequests(Func<GuestRequest, bool> predicate = null)
        {
            return predicate == null
                ? _guests.Select(x => x.Clone()).ToList()
                : _guests.Where(predicate).Select(x => x.Clone()).ToList();
        }

        public IEnumerable<Order> GetAllOrders(Func<Order, bool> predicate = null)
        {
            try
            {

                return (from order in XmlConfigurations.OrderRoot.Elements()
                    let orderObj = new Order()
                    {
                        HostId = Convert.ToString(order.Element("HostId")?.Value),
                        AccommodationKey = Convert.ToInt32(order.Element("AccommodationKey")?.Value),
                        HostingUnitKey = Convert.ToInt32(order.Element("HostingUnitKey")?.Value),
                        GuestRequestKey = Convert.ToInt32(order.Element("GuestRequestKey")?.Value),
                        OrderKey = Convert.ToInt32(order.Element("HostingUnitKey")?.Value),
                        OrderStatus = (enums.OrderStatus)Enum.Parse(typeof(enums.OrderStatus), order.Element("Status")?.Value),
                        OrderCreationDate = DateTime.Parse(order.Element("OrderCreationDate")?.Value),
                        SendingEmailDate = DateTime.Parse(order.Element("SendingEmailDate")?.Value)



       
    }
                    where predicate == null ? true : predicate(orderObj)
                    select orderObj).ToList().Clone();

            }
            catch (Exception)
            {
                throw new OrderProblemException("Order file problem");
            }

        }

        public IEnumerable<Customer> GetAllCustomers(Func<Customer, bool> predicate = null)
        {
            return predicate == null
                ? _customers.Select(x => x.Clone())
                : _customers.Where(predicate).Select(x => x.Clone());
        }

        public IEnumerable<Host> GetAllHosts(Func<Host, bool> predicate = null)
        {
            return predicate == null
                ? _hosts.Select(x => x.Clone()).ToList()
                : _hosts.Where(predicate).Select(x => x.Clone()).ToList();
        }

        public IEnumerable<BankBranch> GetAllBankBranches(Func<BankBranch, bool> predicate = null)
        {
            try
            {
                return (from bankBranches in XmlConfigurations.BankBranchesRoot.Elements()
                    let branchObj = new BankBranch
                    {
                        BankNumber = Convert.ToString(bankBranches.Element("Bank_Code")?.Value),
                        BankName = Convert.ToString(bankBranches.Element("Bank_Name")?.Value),
                        BranchNumber = Convert.ToString(bankBranches.Element("Branch_Code")?.Value),
                        BranchAddress = new Address
                        {
                            Street = Convert.ToString(bankBranches.Element("Branch_Address")?.Value),//Element("Street")?.Value),
                            Building = "Empty for now",
                            City = Convert.ToString(bankBranches.Element("City")?.Value),
                            ZipCode = Convert.ToString(bankBranches.Element("Zip_Code")?.Value),
                            Country = "Israel",
                        },
                        BranchPhoneNumber = Convert.ToString(bankBranches.Element("BranchPhoneNumber")?.Value),
                        BranchFaxNumber = Convert.ToString(bankBranches.Element("BranchFaxNumber")?.Value),
                    }
                    where predicate?.Invoke(branchObj) ?? true
                    select branchObj).ToList().Clone();


            }
            catch (Exception)
            {
                throw new BankAccountException("We got problem with the bank branches file...");
            }


        }


        #endregion

        #region Configuration

        /// <summary>
        /// Save configurations to xml
        /// </summary>
        public void SaveConfigurations() => XmlConfigurations.SaveConfig();

        public void LoadOrderXmlFile() => XmlConfigurations.LoadXmlConfigFile();
     
        #endregion




    }
}