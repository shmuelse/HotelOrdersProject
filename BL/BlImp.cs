using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;
using BE;
using IDAL;
using Utilities;

namespace BL
{
    public class BlImp : IBL
    {

        private readonly IDAL.IDAL _idalImp = FactoryDal.Instance;

        /// <summary>
        /// deny Access to the c-tor
        /// </summary>
        internal BlImp()
        {
            //Block creating new instance
        }

        #region Authentication

        /// <summary>
        /// check if the user name already exist in  Data Base
        /// </summary>
        /// <param name="userNameToBeCheck"></param>
        /// <returns></returns>
        public bool IsUserNameExist(string userNameToBeCheck) => _idalImp.IsUserNameExist(userNameToBeCheck);

        /// <summary>
        /// check if the user name Match the Password
        /// </summary>
        /// <param name="loginDetails">The login details we want to be check</param>
        /// <param name="authorizationType"></param>
        /// <returns></returns>
        public bool IsUsernameMatchPassword(Login loginDetails, string authorizationType) =>
            (_idalImp.IsUsernameMatchPassword(loginDetails, authorizationType));

        /// <summary>
        /// check if unit occupied in a range of days
        /// </summary>
        /// <param name="checkIn"></param>
        /// <param name="duration"></param>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <returns></returns>
        public bool IsUnitOccupied(DateTime checkIn, int duration, int unitKey, int accommodationKey, string hostKey)
        {
            //get the specific unit to check
            var getUnit = _idalImp.GetHostingUnit(unitKey, accommodationKey, hostKey).Clone();

            //check if unit occupied
            for (var i = 0; i < duration; ++i)
            {
                if (getUnit[checkIn])
                    return false;

                checkIn = checkIn.AddDays(1);
            }

            return true;
        }

        #endregion

        #region getOneAppeerance

        /// <summary>
        /// return a specific host unit
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <returns></returns>
        public HostingUnit GetHostingUnit(int unitKey, int accommodationKey, string hostKey) =>
            _idalImp.GetHostingUnit(unitKey, accommodationKey, hostKey);

        /// <summary>
        /// Get a specific Host from Data Base
        /// </summary>
        /// <param name="hostKey"></param>
        /// <returns></returns>
        public Host GetHost(string hostKey) => _idalImp.GetHost(hostKey);

        /// <summary>
        /// Get a specific guest request the from Data Base
        /// </summary>
        /// <param name="guestRequestKey"></param>
        /// <returns></returns>
        public GuestRequest GetGuestRequest(int guestRequestKey) => _idalImp.GetGuestRequest(guestRequestKey);

        #endregion

        #region AddNewItem

        /// <summary>
        /// Add Customer contact Info to Data Base
        /// </summary>
        /// <param name="customerContactInfo">The customer Contact Info</param>
        public void AddCustomerContactInfo(Customer customerContactInfo)
        {
            if (!_idalImp.IsUserNameExist(customerContactInfo.ClientInfo.LoginDetails.UserName))
                throw new Exception("The user name already exist");
            if (DateTime.Now.Year - customerContactInfo.ClientInfo.DateOfBirth.Year < 18)
                throw new Exception("You are not allowed to sign up for the website under 18");

            customerContactInfo.ClientInfo.SystemRegistrationDate = DateTime.Now;
            _idalImp.AddCustomerContactInfo(customerContactInfo.Clone());
        }

        /// <summary>
        /// Add a new customer requirement to Data Base
        /// </summary>
        /// <param name="newGuestRequest">Customer's requirement</param>
        public void AddACustomerRequirement(GuestRequest newGuestRequest)
        {
            //check if the user name match the password
            if (!_idalImp.IsUsernameMatchPassword(newGuestRequest.ClientLoginDetails, "Customer"))
                throw new Exception("The username or password is invalid");

            //check if the check out is before the check in date
            var sumOfDays = newGuestRequest.CheckOutDate.Subtract(newGuestRequest.CheckInDate);
            if (sumOfDays.Days < 1)
                throw new Exception("The check out date must be after the check in date");

            //check if the request is not for more than 11 month
            if ((newGuestRequest.CheckOutDate.Month - DateTime.Now.Month + 11) > 0)
                throw new Exception("You cannot place an order for more than eleven months ahead");

            //Checks whether the check in date has not passed
            if (newGuestRequest.CheckInDate < DateTime.Now)
                throw new Exception("The check in date has passed");

            //Checks whether there is at least one adult person on the invitation
            if (newGuestRequest.AmountOfAdults < 1)
                throw new Exception("You must select at least one adult person");


            newGuestRequest.OrderStatus = false;
            newGuestRequest.GuestRequestKey = IDAL.Configuration.GuestRequestKey++;
            _idalImp.AddACustomerRequirement(newGuestRequest.Clone());

        }

        /// <summary>
        /// Add a new Host contact to Data Base
        /// </summary>
        /// <param name="hostContactInfo">The Host Contact Info</param>
        public void AddHostContactInfo(Host hostContactInfo)
        {
            //check if the user exist in data base
            if (!_idalImp.IsUserNameExist(hostContactInfo.HostInfo.LoginDetails.UserName))
                throw new Exception("The user name already exist");

            //check if the host is at least 18 years old
            if (DateTime.Now.Year - hostContactInfo.HostInfo.DateOfBirth.Year < 18)
                throw new Exception("You are not allowed to sign up for the website under 18");

            //check if the ID is correct
            if (!Validation.IsValidId(hostContactInfo.Id))
                throw new Exception("The ID is incorrect");

            hostContactInfo.CollectionClearance = false;
            hostContactInfo.HostInfo.SystemRegistrationDate = DateTime.Now;
            hostContactInfo.HostAccommodationsList = null;
            hostContactInfo.SumOfUnits = 0;

            //add the new host to Data Base
            _idalImp.AddHostContactInfo(hostContactInfo.Clone());
        }

        /// <summary>
        /// Add Accommodation for a specific Host in Data Base
        /// </summary>
        /// <param name="addAccommodation">Accommodation</param>
        public void AddAccommodation(Accommodations addAccommodation)
        {
            addAccommodation.AccommodationKey = IDAL.Configuration.AccommodationKey++;
            addAccommodation.ListOfAllUnits = null;

            _idalImp.AddAccommodation(addAccommodation.Clone());

        }

        /// <summary>
        /// Add a new host unit to Data Base
        /// </summary>
        /// <param name="addHostingUnit">host unit to be added</param>
        public void AddAHostingUnit(HostingUnit addHostingUnit)
        {
            //check if the price for the unit is not less than 0
            if (addHostingUnit.UnitPrice < 0)
                throw new Exception("Price must be greater than or equal to zero");

            addHostingUnit.Diary = new bool[12, 31];
            addHostingUnit.DiaryFlatten = new bool[12 * 31];
            addHostingUnit.SumOfferOrders = 0;
            addHostingUnit.SumCompletedOrders = 0;
            addHostingUnit.HostingUnitKey = IDAL.Configuration.HostingUnitKey++;

            _idalImp.AddAHostingUnit(addHostingUnit.Clone());
        }

        /// <summary>
        /// Add a new order to list of orders in Data Base
        /// </summary>
        /// <param name="order"></param>
        public void AddNewOrder(Order order)
        {
            //check if the id of the host correct
            if (!Validation.IsValidId(order.HostId))
                throw new Exception("the ID is invalid");

            //get the specific unit
            var getUnit = GetHostingUnit(order.HostingUnitKey, order.AccommodationKey, order.HostId);

            // get the specific guest request 
            var guestRequest = _idalImp.GetAllGuestRequests(unit => unit.GuestRequestKey == order.GuestRequestKey)
                .FirstOrDefault().Clone();

            if (guestRequest == null)
                throw new Exception("Guest request not found in database");

            // check if the room calender is not occupied in the specifics date
            for (var i = guestRequest.CheckInDate; i < guestRequest.CheckOutDate; i = i.AddDays(1))
                if (getUnit[i])
                    throw new Exception("The Date is occupied");

            order.OrderStatus = enums.OrderStatus.Pending;
            order.OrderCreationDate = DateTime.Now;
            order.OrderKey = IDAL.Configuration.OrderKey++;
            _idalImp.HostingUnitSumOfOrdersUpdate(getUnit.HostingUnitKey, getUnit.AccommodationKey, getUnit.HostId, "offered");
            _idalImp.AddNewOrder(order.Clone());
        }

        #endregion

        #region RemoveItems

        /// <summary>
        /// Delete host from Data Base
        /// </summary>
        /// <param name="hostId">host id we want to be deleted</param>
        public void DeleteHost(string hostId)
        {
            //Receives the list of all host invitations
            var hostOrderList = _idalImp.GetAllOrders(order => order.HostId == hostId).Clone().ToList();

            //Checking for open orders
            if (hostOrderList.Any(order => order.OrderStatus != enums.OrderStatus.Completed))
                throw new Exception("Unable to unsubscribe from system, there are open orders");

            //Deletes the host from the database
            _idalImp.DeleteHost(hostId);
        }

        /// <summary>
        /// Delete customer from Data Base
        /// </summary>
        /// <param name="customerKey">Customer Key to be deleted</param>
        public void DeleteCustomer(Login customerKey)
        {
            //check if the user exist in the system
            if (!_idalImp.IsUsernameMatchPassword(customerKey, "Customer"))
                throw new Exception("The username or password is invalid");

            //כאן צריך למחוק דרישות לקוח וכמו כן לבטל את כל ההזמנות שלא נסגרו
            // int



            _idalImp.DeleteCustomer(customerKey);
        }

        /// <summary>
        /// Delete Accommodation from Data Base
        /// </summary>
        /// <param name="accommodationKey">accommodation Key</param>
        /// <param name="hostKey">host Key</param>
        public void DeleteAccommodation(int accommodationKey, string hostKey)
        {
            //Receives the list of all Accommodations invitations
            var accommodationOrderList =
                _idalImp.GetAllOrders(orders => orders.AccommodationKey == accommodationKey).Clone();

            //Checking for open orders
            if (accommodationOrderList.Any(order => order.OrderStatus != enums.OrderStatus.Completed))
                throw new Exception("The Accommodation cannot be deleted from the system, there are open orders");

            //Deletes the accommodation from the database
            _idalImp.DeleteAccommodation(accommodationKey, hostKey);

        }

        /// <summary>
        /// Delete host unit from Data Base
        /// </summary>
        /// <param name="unitKey">unit Key</param>
        /// <param name="accommodationKey">accommodation Key</param>
        /// <param name="hostKey">host Key</param>
        public void DeleteHostingUnit(int unitKey, int accommodationKey, string hostKey)
        {
            //Gets the room we want to delete from the database
            var unitToDelete = _idalImp.GetAllOrders(x => x.HostingUnitKey == unitKey).FirstOrDefault().Clone();

            //Checking for open orders
            if (unitToDelete.OrderStatus != enums.OrderStatus.Completed)
                throw new Exception("The unit cannot be deleted from the system, there are open orders");

            //Deleting the hosting unit from database
            _idalImp.DeleteHostingUnit(unitKey, accommodationKey, hostKey);
        }

        #endregion

        #region Updates

        /// <summary>
        /// Updating the status of the guest request To Data Base
        /// </summary>
        /// <param name="orderStatus">The status of the Order</param>
        /// <param name="guestRequestKey">The Key of the order request</param>
        public void CustomerRequirementStatusUpdate(bool orderStatus, int guestRequestKey) =>
            _idalImp.CustomerRequirementStatusUpdate(orderStatus, guestRequestKey);

        /// <summary>
        /// Update Accommodation Stars in Data Base
        /// </summary>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="stars"></param>
        public void AccommodationStarsUpdate(int accommodationKey, string hostKey, uint stars)
        {
            if (stars <= 0 || stars > 5)
                throw new Exception("Accepts values between 1 and 5 only");

            _idalImp.AccommodationStarsUpdate(accommodationKey, hostKey, stars);
        }

        /// <summary>
        /// Update the sum of units in the Accommodation in Data Base
        /// </summary>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="sumOfUnits"></param>
        public void AccommodationSumOfUnitsUpdate(int accommodationKey, string hostKey, uint sumOfUnits)
        {
            _idalImp.AccommodationSumOfUnitsUpdate(accommodationKey, hostKey, sumOfUnits);
        }

        /// <summary>
        /// Update the booking log of the hosting unit
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="checkIn"></param>
        /// <param name="checkOut"></param>
        public void HostingUnitDiaryUpdate(int unitKey, int accommodationKey, string hostKey, DateTime checkIn, DateTime checkOut)
        {
            //check if the check out is before the check in date
            var sumOfDays = checkOut.Subtract(checkIn);
            if (sumOfDays.Days < 1)
                throw new Exception("The check out date must be after the check in date");

            //check if the request is not for more than 11 month
            if ((checkOut.Month - DateTime.Now.Month + 11) > 0)
                throw new Exception("You cannot place an order for more than eleven months ahead");

            //Checks whether the check in date has not passed
            if (checkIn < DateTime.Now)
                throw new Exception("The check in date has passed");

            _idalImp.HostingUnitDiaryUpdate(unitKey, accommodationKey, hostKey, checkIn, checkOut);
        }

        /// <summary>
        /// Update the sum of the hosting unit
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="unitPrice"></param>
        public void HostingUnitPriceUpdate(int unitKey, int accommodationKey, string hostKey, decimal unitPrice)
        {
            if (unitPrice <= 0)
                throw new Exception("the price must be more than zero");

            _idalImp.HostingUnitPriceUpdate(unitKey, accommodationKey, hostKey, unitPrice.Clone());

        }

        /// <summary>
        /// Update the elements in the hosting unit
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="elements"></param>
        public void HostingUnitElementsUpdate(int unitKey, int accommodationKey, string hostKey,
            FilterElements elements)
        {
            _idalImp.HostingUnitElementsUpdate(unitKey, accommodationKey, hostKey, elements.Clone());
        }

        /// <summary>
        /// Updating Accommodation details
        /// </summary>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="detailsInAccommodationUpdate"></param>
        /// <param name="newValue">can be Stars, sum of rooms etc' </param>
        public void AccommodationUpdate(int accommodationKey, string hostKey, string detailsInAccommodationUpdate,
            object newValue)
        {
            switch (detailsInAccommodationUpdate)
            {
                case "Stars" when (uint)newValue <= 0 || (uint)newValue > 5:
                    throw new Exception("Accepts values between 1 and 5 only");
                case "Stars":
                    _idalImp.AccommodationUpdate(accommodationKey, hostKey, detailsInAccommodationUpdate, newValue);
                    break;
                case "SumOfUnits" when (uint)newValue <= 0:
                    throw new Exception("The sum of units is invalid");
                case "SumOfUnits":
                    _idalImp.AccommodationUpdate(accommodationKey, hostKey, detailsInAccommodationUpdate, newValue);
                    break;
                default:
                    throw new Exception("This value cannot be updated");
            }
        }

        /// <summary>
        /// Updating hostingUnit details
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="detailsInUnitUpdate"></param>
        /// <param name="newValue">can be sum of orders, elements in unit, price, etc'...</param>
        public void HostingUnitUpdate(int unitKey, int accommodationKey, string hostKey, enums.UnitUpdate detailsInUnitUpdate,
            object newValue)
        {
            switch (detailsInUnitUpdate)
            {
                case enums.UnitUpdate.Price:
                    if ((decimal)newValue <= 0)
                        throw new Exception("the price must be more than zero");

                    _idalImp.HostingUnitUpdate(unitKey, accommodationKey, hostKey, enums.UnitUpdate.Price, newValue.Clone());
                    break;
                case enums.UnitUpdate.ElementsUpdate:
                    _idalImp.HostingUnitUpdate(unitKey, accommodationKey, hostKey, enums.UnitUpdate.ElementsUpdate, newValue.Clone());
                    break;
                case enums.UnitUpdate.OrderCompleted:
                    _idalImp.HostingUnitUpdate(unitKey, accommodationKey, hostKey, enums.UnitUpdate.OrderCompleted, newValue.Clone());
                    break;
                case enums.UnitUpdate.OrderOffer:
                    _idalImp.HostingUnitUpdate(unitKey, accommodationKey, hostKey, enums.UnitUpdate.OrderOffer, newValue.Clone());
                    break;
                default:
                    throw new Exception("This value cannot be updated");

            }
        }

        /// <summary>
        /// Update Order status in Data Base
        /// </summary>
        /// <param name="orderKey"></param>
        /// <param name="statusOfOrder"></param>
        public void OrderUpdate(int orderKey, enums.OrderStatus statusOfOrder)
        {
            var ourOrder = GetAllOrders(getOrder => getOrder.OrderKey == orderKey).FirstOrDefault();

            if (ourOrder == null)
                throw new Exception("The order is not exist");

            if (ourOrder.OrderStatus == enums.OrderStatus.Completed)
                throw new Exception("Reservation changes cannot be made once closed");


            if (statusOfOrder == enums.OrderStatus.Completed)
            {
                //get the guest request 
                var getRequest = GetAllGuestRequests(x => x.GuestRequestKey == ourOrder.GuestRequestKey)
                    .FirstOrDefault();

                //Saves the days for booking in the guest unit book
                _idalImp.HostingUnitDiaryUpdate(ourOrder.HostingUnitKey, ourOrder.AccommodationKey, ourOrder.HostId,
                    getRequest.CheckInDate, getRequest.CheckOutDate);

                //change the guestRequest status
                _idalImp.CustomerRequirementStatusUpdate(true, getRequest.GuestRequestKey);

                //Updating the status of all open orders for a customer's cancellation request after
                //the customer has selected one of the offers
                foreach (var getOrder in _idalImp.GetAllOrders(others =>
                    others.GuestRequestKey == getRequest.GuestRequestKey && others.OrderKey != orderKey))
                {
                    _idalImp.OrderUpdate(getOrder.OrderKey, enums.OrderStatus.Cancelled);
                }


                //Update in database about the new order and the commission
                _idalImp.UpdateOrderCompletion(CommissionCalculate(getRequest.CheckInDate, getRequest.CheckOutDate),
                    ourOrder.HostId);

                //Update the sum of orders-completed on the specific unit
                HostingUnitUpdate(ourOrder.HostingUnitKey, ourOrder.AccommodationKey, ourOrder.HostId, enums.UnitUpdate.OrderCompleted, true);

            }

            if (statusOfOrder == enums.OrderStatus.EmailSent)
                Console.WriteLine("you got an order");


            //update the status of order in database
            _idalImp.OrderUpdate(orderKey, statusOfOrder.Clone());
        }


        #endregion

        #region Return Lists

        /// <summary>
        /// return a list of all Accommodations
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        public IEnumerable<Accommodations> GetAllAccommodations(Func<Accommodations, bool> predicate = null) =>
            _idalImp.GetAllAccommodations(predicate);


        /// <summary>
        /// return a list of all Hosting Units
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        public IEnumerable<HostingUnit> GetAllHostingUnits(Func<HostingUnit, bool> predicate = null) =>
            _idalImp.GetAllHostingUnits(predicate);

        /// <summary>
        /// return a list of all Guest Request from Data Base
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        public IEnumerable<GuestRequest> GetAllGuestRequests(Func<GuestRequest, bool> predicate = null) =>
            _idalImp.GetAllGuestRequests(predicate);

        /// <summary>
        /// return a list of all Orders from Data Base
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        public IEnumerable<Order> GetAllOrders(Func<Order, bool> predicate = null) =>
            _idalImp.GetAllOrders(predicate);

        /// <summary>
        /// return a list of all Customers from Data Base
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        public IEnumerable<Customer> GetAllCustomers(Func<Customer, bool> predicate = null) =>
            _idalImp.GetAllCustomers(predicate);

        /// <summary>
        /// return a list of all Hosts from Data Base
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        public IEnumerable<Host> GetAllHosts(Func<Host, bool> predicate = null) =>
            _idalImp.GetAllHosts(predicate);


        /// <summary>
        /// return all Bank Branches from Data Base
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        public IEnumerable<BankBranch> GetAllBankBranches(Func<BankBranch, bool> predicate = null) =>
            _idalImp.GetAllBankBranches(predicate).Clone();


        /// <summary>
        /// return list of available units starts from a certain date and for the given time
        /// </summary>
        /// <param name="checkInDate"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public List<HostingUnit> ListOfAvailableUnits(DateTime checkInDate, int duration)
        {
            return GetAllHostingUnits(x =>
                    IsUnitOccupied(checkInDate, duration, x.HostingUnitKey, x.AccommodationKey, x.HostId))
                .ToList();
        }

        #endregion

        #region Grooping

        /// <summary>
        /// Get Guest Requests Group By Area
        /// </summary>
        /// <param name="sorted"></param>
        /// <returns></returns>
        public IEnumerable<IGrouping<enums.Area, GuestRequest>> GetGuestRequestsGroupByArea(bool sorted = false)
        {
            return sorted
                ? GetAllGuestRequests(null).OrderBy(x => x.GuestRequestKey).GroupBy(y => y.Area)
                : GetAllGuestRequests(null).GroupBy(y => y.Area);
        }

        /// <summary>
        /// Get Guest Requests Group By sum of hosted
        /// </summary>
        /// <param name="sorted"></param>
        /// <returns></returns>
        public IEnumerable<IGrouping<uint, GuestRequest>> GetGuestRequestGroupBySumOfHosted(bool sorted = false)
        {
            return sorted
                ? GetAllGuestRequests(null).OrderBy(x => x.GuestRequestKey)
                    .GroupBy(y => y.AmountOfAdults + y.AmountOfChildren)
                : GetAllGuestRequests(null).GroupBy(y => y.AmountOfAdults + y.AmountOfChildren);
        }

        /// <summary>
        /// Get Hosts Group By sum of units
        /// </summary>
        /// <param name="sorted"></param>
        /// <returns></returns>
        public IEnumerable<IGrouping<int, Host>> GetHostsGroupBySumOfUnits(bool sorted = false)
        {
            return
                sorted
                    ? from hosts in GetAllHosts(null)
                    orderby hosts.Id
                    group hosts by hosts.SumOfUnits
                    : from hosts in GetAllHosts(null)
                    group hosts by hosts.SumOfUnits;
        }

        /// <summary>
        /// Get Accommodations Group By Area
        /// </summary>
        /// <param name="sorted"></param>
        /// <returns></returns>
        public IEnumerable<IGrouping<enums.Area, Accommodations>> GetAccommodationsGroupByArea(bool sorted = false)
        {
            return sorted
                ? GetAllAccommodations(null).OrderBy(x => x.AccommodationKey).GroupBy(y => y.Area)
                : GetAllAccommodations(null).GroupBy(y => y.Area);
        }

        /// <summary>
        /// Get Hosting Unit Group By Area
        /// </summary>
        /// <param name="sorted"></param>
        /// <returns></returns>
        public IEnumerable<IGrouping<enums.Area, HostingUnit>> GetHostingUnitGroupByArea(bool sorted = false)
        {
            return sorted
                ? GetAllHostingUnits(null).OrderBy(x => x.HostingUnitKey).GroupBy(y => y.Area)
                : GetAllHostingUnits(null).GroupBy(y => y.Area);
        }

        #endregion

        #region Help functions


        /// <summary>
        /// calculate the commission for a specific order
        /// </summary>
        /// <param name="checkIn"></param>
        /// <param name="checkOut"></param>
        /// <returns></returns>
        public decimal CommissionCalculate(DateTime checkIn, DateTime checkOut) =>
            (checkOut - checkIn).Days * IDAL.Configuration.Commission;


        /// <summary>
        /// Returns the sum of days between the received dates or the current day
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public int GetDays(DateTime fromDate, object toDate = null)
        {
            var sumOfDays = 0;
            if (toDate == null)
            {
                if (fromDate > DateTime.Now)
                    throw new Exception("the date is invalid");

                for (var date = fromDate; date <= DateTime.Now; date = date.AddDays(1))
                    sumOfDays++;
            }
            else
            {
                if (fromDate > (DateTime)toDate)
                    throw new Exception("the date is invalid");

                for (var date = fromDate; date <= (DateTime)toDate; date = date.AddDays(1))
                    sumOfDays++;
            }

            return sumOfDays;
        }


        /// <summary>
        /// Returns the orders created before the number of days we received
        /// </summary>
        /// <param name="sumDays"></param>
        /// <returns></returns>
        public List<Order> GetOrdersCreatedSince(int sumDays) =>
            GetAllOrders(x => GetDays(x.OrderCreationDate) >= sumDays).ToList();


        /// <summary>
        /// Returns the amount of orders offers sent to a specific customer requirement
        /// </summary>
        /// <param name="guestRequestKey"></param>
        /// <returns></returns>
        public int GetSumOfOrders(int guestRequestKey)
        {
            return _idalImp.GetAllOrders(x => x.GuestRequestKey == guestRequestKey).ToList().Clone().Count;
        }

        /// <summary>
        ///  Returns the amount of bookings sent to the customer for this unit
        /// or the amount of successful orders made for the accommodation unit.
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="openOrCloseOrder">choosing between orders close (True) and orders that only sent(False)</param>
        /// <returns></returns>
        public int GetUnitSumOfOrders(int unitKey, int accommodationKey, string hostKey, bool openOrCloseOrder)
        {
            switch (openOrCloseOrder)
            {
                //if we want to get the sum of orders completed for this unit
                case true:
                    return _idalImp.GetHostingUnit(unitKey, accommodationKey, hostKey).SumCompletedOrders.Clone();
                //if we want to get the sum of orders offers for this unit
                case false:
                    return _idalImp.GetHostingUnit(unitKey, accommodationKey, hostKey).SumOfferOrders.Clone();
                default:
                    throw new Exception("you have a mistake");
            }
        }





        #endregion


        #region configurations

        /// <summary>
        ///     Save Configuration to xml file
        /// </summary>
        public void SaveSettings()
        {
            _idalImp.SaveConfigurations();
        }

        #endregion

        public string GetHostKey(Login hostLoginDetails)
        {
            var host = _idalImp.GetAllHosts(x =>
                x.HostInfo.LoginDetails.UserName == hostLoginDetails.UserName &&
                x.HostInfo.LoginDetails.Password == hostLoginDetails.Password).Single().Clone();

            return host.Id;
        }


        /// <summary>
        /// return distance between the 2 addresses. if the return value small then 0, there was an error.
        /// addressError will be true if unlist one of the addresses was incorrect.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="addressError"></param>
        /// <returns></returns>

        double GetDistanceBetweenTwoAddresses(Address a, Address b, ref bool addressError)
        {
            double CalculatedDistance = 0;
            bool isNetBuisy = false;
            int i = 0;
            do
            {
                //string origin = "pisga 45 st. jerusalem"; //or "תקווה פתח 100 העם אחד "etc.
                string origin = a.Street + " " + a.Building + " " + a.City;

                //string destination = "gilgal 78 st. ramat-gan";//or "גן רמת 10 בוטינסקי'ז "etc.
                var destination = b.Street + " " + b.Building + " " + b.City;
                var KEY = @"	jVe1SpSfMbu1xR9wiyI1JRtQ4mKqUpe0";
                var url = @"https://www.mapquestapi.com/directions/v2/route" +
                          @"?key=" + KEY +
                          @"&from=" + origin +
                          @"&to=" + destination +
                          @"&outFormat=xml" +
                          @"&ambiguities=ignore&routeType=fastest&doReverseGeocode=false" +
                          @"&enhancedNarrative=false&avoidTimedConditions=false";

                //request from MapQuest service the distance between the 2 addresses
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = request.GetResponse();
                var dataStream = response.GetResponseStream();
                var sreader = new StreamReader(dataStream);
                var responsereader = sreader.ReadToEnd();
                response.Close();

                //the response is given in an XML format
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(responsereader);
                if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "0")
                    //we have the expected answer
                {
                    //display the returned distance
                    XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                    CalculatedDistance = Convert.ToDouble(distance[0].ChildNodes[0].InnerText) * 1.609344;

                    //Console.WriteLine("Distance In KM: " + distInMiles * 1.609344);
                    //display the returned driving time
                    XmlNodeList formattedTime = xmldoc.GetElementsByTagName("formattedTime");
                    //CalculatedDistance = double.Parse(formattedTime[0].ChildNodes[0].InnerText);
                    //Console.WriteLine("Driving Time: " + CalculatedDistance);
                }
                else if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText ==
                         "402")
                    //we have an answer that an error occurred, one of the addresses is not found
                {
                    if (!addressError)
                        addressError = true;
                    return -1;
                }
                else //busy network or other error...
                {
                    Thread.Sleep(2000);
                }
            }
            while (isNetBuisy && i++ < 10);
            if (i >= 10)
                throw new InternalBufferOverflowException();
            return CalculatedDistance;
        }
        //public void LoadAddressesFromApi()
        //{
        //    {
        //        var URL = "https://data.gov.il/api/action/datastore_search";
        //        var urlParameters = "?resource_id=d4901968-dad3-4845-a9b0-a57d027f11ab&limit=1000000000";

        //        var client = new HttpClient();
        //        client.BaseAddress = new Uri(URL);
        //        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36");
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/Json"));

        //        HttpResponseMessage response = client.GetAsync(urlParameters).Result;
        //        XElement root = new XElement("Citys");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var dataObjects = response.Content.ReadAsAsync<DataJsonResultContainer>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
        //            var strings = dataObjects.result.records.ToList().OfType<Object>().ToList();
        //            foreach (var item in strings)
        //            {
        //                JsonTextReader reader = new JsonTextReader(new StringReader(item.ToString()));
        //                for (int i = 0; i < 9; ++i, reader.Read()) ;
        //                string city = ((string)reader.Value).Trim();
        //                root.Add(new XElement("City", ReverseBracketes(city)));
        //            }
        //        }
        //        if (File.Exists(@"..\..\..\Cities and Streets xml\CitiesList.xml"))
        //            File.Delete(@"..\..\..\Cities and Streets xml\CitiesList.xml");
        //        root.Save(@"..\..\..\Cities and Streets xml\CitiesList.xml");
        //    }
        //    {
        //        string URL = "https://data.gov.il/api/action/datastore_search";
        //        string urlParameters = "?resource_id=a7296d1a-f8c9-4b70-96c2-6ebb4352f8e3&limit=1000000000";

        //        HttpClient client = new HttpClient();
        //        client.BaseAddress = new Uri(URL);
        //        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36");
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/Json"));

        //        HttpResponseMessage response = client.GetAsync(urlParameters).Result;
        //        XElement root = new XElement("Streets");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var dataObjects = response.Content.ReadAsAsync<DataJsonResultContainer>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
        //            var strings = dataObjects.result.records.ToList().OfType<Object>().ToList();
        //            foreach (var item in strings)
        //            {
        //                JsonTextReader reader = new JsonTextReader(new StringReader(item.ToString()));
        //                for (int i = 0; i < 9; ++i, reader.Read()) ;
        //                string city = ((string)reader.Value).Trim();

        //                for (int i = 0; i < 4; ++i, reader.Read()) ;
        //                string street = ((string)reader.Value).Trim();

        //                root.Add(new XElement("Address", new XElement("City", ReverseBracketes(city)), new XElement("Street", ReverseBracketes(street))));
        //            }
        //        }
        //        if (File.Exists(@"..\..\..\Cities and Streets xml\StreetsList.xml"))
        //            File.Delete(@"..\..\..\Cities and Streets xml\StreetsList.xml");
        //        root.Save(@"..\..\..\Cities and Streets xml\StreetsList.xml");
        //    }
        //}


        //[DataContract]
        //public class DataJsonResultContainer
        //{
        //    [DataMember(Name = "result")]
        //    public Result result { get; set; }
        //}

        //[DataContract]
        //public class Result
        //{
        //    [DataMember(Name = "records")]
        //    public dynamic[] records { get; set; }
        //}

        //private char[] ReverseBracketes(string x)
        //{
        //    char[] arr = new char[x.Length];
        //    for (int i = 0; i < x.Length; ++i)
        //    {
        //        if (x[i] == '(')
        //            arr[i] = ')';
        //        else if (x[i] == ')')
        //            arr[i] = '(';
        //        else
        //            arr[i] = x[i];
        //    }
        //    return arr;
        //}


        public void AddAccommodationImage(int accommodationKey, string newImagePath)
        {
            var photoPath = @"..\..\..\AccommodationImages\" + accommodationKey + @".jpg";
            (File.Create(photoPath)).Close();
            File.Copy(newImagePath, photoPath, true);
        }

        public void AddHostingUnitImage(int hostUnitKey, string newImagePath)
        {
            var photoPath = @"AccommodationImages\" + hostUnitKey + @".jpg";
            (File.Create(photoPath)).Close();
            File.Copy(newImagePath, photoPath, true);
        }

        public void ChangeAccommodationImage(int accommodationKey, string newImagePath)
        {
            var destination = @"AccommodationImages\" + accommodationKey + @".jpg";
            File.Delete(destination);
            (File.Create(destination)).Close();
            File.Copy(newImagePath, destination, true);
        }

        public void ChangeHostingUnitImage(int accommodationKey, string newImagePath)
        {
            var destination = @"HostingUnitImages\" + accommodationKey + @".jpg";
            File.Delete(destination);
            (File.Create(destination)).Close();
            File.Copy(newImagePath, destination, true);
        }

        /// <summary>
        /// Free up the Calendar when day is past
        /// </summary>
        public void FreeUpDiaryDays(DateTime? fromDate = null) => _idalImp.FreeUpDiaryDays(fromDate);
    }
}