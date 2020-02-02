using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using BE;
using Utilities;

namespace IDAL
{
    public static class XmlConfigurations
    {
        public const string OrdersPath = @"Orders.xml";
        public const string GuestRequestPath = @"GuestRequest.xml";
        public const string CustomerPath = @"Customer.xml";
        public const string HostsPath = @"Host.xml";
        public const string SiteOwnerPath = @"SiteOwner.xml";
        public const string BankBranchesPath = @"BankBranches.xml";
     //   public const string BOIListPath = @"snifim_dnld_he.xml";
        public const string ConfigPath = @"Config.xml";
        public static XElement ConfigRoot;
        public static XElement OrderRoot;
        public static XElement BankBranchesRoot;
       // public static XElement BOIListRoot;


        public static void SaveConfig()
        {
            try
            {
                ConfigRoot = new XElement("config");
                ConfigRoot.Add(
                    new XElement("GuestRequestKey", Configuration.GuestRequestKey),
                    new XElement("AccommodationKey", Configuration.AccommodationKey),
                    new XElement("HostingUnitKey", Configuration.HostingUnitKey),
                    new XElement("OrderKey", Configuration.OrderKey),
                    new XElement("LastTimeOpen", Configuration.LastTimeOpen),
                    new XElement("Commission", Configuration.Commission),
                    new XElement("AdminPassword", Configuration.AdminPassword),
                    new XElement("AdminUser", Configuration.AdminUser),
                    new XElement("MinAgeForOrder", Configuration.MinAgeForOrder),
                    new XElement("MinDaysForOrder", Configuration.MinDaysForOrder),
                    new XElement("MaxMonthForOrder", Configuration.MaxMonthForOrder));
                ConfigRoot.Save(ConfigPath);

            }
            catch (Exception)
            {
               //Do nothing
            }
        }

        public static void LoadXmlConfigFile()
        {
            ConfigRoot = XElement.Load(ConfigPath);
            Configuration.GuestRequestKey =
                Convert.ToInt32(ConfigRoot.Element("GuestRequestKey")?.Value);
            Configuration.AccommodationKey =
                Convert.ToInt32(ConfigRoot.Element("AccommodationKey")?.Value);
            Configuration.HostingUnitKey =
                Convert.ToInt32(ConfigRoot.Element("HostingUnitKey")?.Value);
            Configuration.OrderKey = Convert.ToInt32(ConfigRoot.Element("OrderKey")?.Value);
            Configuration.LastTimeOpen =
                Convert.ToDateTime(ConfigRoot.Element("LastTimeOpen")?.Value);
            Configuration.Commission =
                Convert.ToDecimal(ConfigRoot.Element("Commission")?.Value);
            Configuration.AdminPassword =
                Convert.ToString(ConfigRoot.Element("AdminPassword")?.Value);
            Configuration.AdminUser =
                Convert.ToString(ConfigRoot.Element("AdminUser")?.Value);
            Configuration.MinAgeForOrder =
                Convert.ToUInt32(ConfigRoot.Element("AdminUser")?.Value);
            Configuration.MinDaysForOrder =
                Convert.ToUInt32(ConfigRoot.Element("AdminUser")?.Value);
            Configuration.MaxMonthForOrder =
                Convert.ToUInt32(ConfigRoot.Element("AdminUser")?.Value);
        }
        public static Order GetAllXmlOrder(int orderKey)
        {
            return (from order in OrderRoot.Elements().Where(x => x.Element("OrderKey")?.Value == orderKey.ToString())
                select new Order
                {
                    HostId = order.Element("OrderKey")?.Value,
                    AccommodationKey = Convert.ToInt32(order.Element("AccommodationKey")?.Value),
                    HostingUnitKey = Convert.ToInt32(order.Element("HostingUnitKey")?.Value),
                    GuestRequestKey = Convert.ToInt32(order.Element("GuestRequestKey")?.Value),
                    OrderKey = Convert.ToInt32(order.Element("OrderKey")?.Value),
                    OrderStatus = (enums.OrderStatus)Enum.Parse(typeof(enums.OrderStatus), order.Element("OrderStatus")?.Value),
                    OrderCreationDate = DateTime.Parse(order.Element("OrderCreationDate")?.Value),
                    SendingEmailDate = DateTime.Parse(order.Element("SendingEmailDate")?.Value),
                }).FirstOrDefault().Clone();
            
        }

        //public static void LoadAllXmlBankBranches()
        //{
        //    BankBranchesRoot = XElement.Load(BankBranchesPath);
        //    BOIListRoot = XElement.Load(BOIListPath);


        //    foreach (var branch in BOIListRoot.Elements())
        //    {
        //        var temp = new XElement("BankBranch");
        //        var BankNumber = 
        //        temp.Add(new XElement("BankNumber").Element(branch.Element("Bank_Code")?.Value));
        //        BankNumber = ;
        //    }


        //    var a = new BankBranch
        //        {
        //            BankNumber = null,
        //            BankName = null,
        //            BranchNumber = null,
        //            BranchAddress = new Address
        //            {
        //                Street = null,
        //                Building = null,
        //                City = null,
        //                ZipCode = null,
        //                Country = null
        //            },
        //            BranchPhoneNumber = null,
        //            BranchFaxNumber = null
        //        }
            


        //}

        //public static void SaveBankBranches()
        //{ 
        //    try
        //    {
        //        BankBranchesRoot = new XElement("BankBranches");
        //        BankBranchesRoot.Add(
        //            new XElement("AccommodationKey", Configuration.AccommodationKey),
        //            new XElement("HostingUnitKey", Configuration.HostingUnitKey),
        //            new XElement("OrderKey", Configuration.OrderKey),
        //            new XElement("LastTimeOpen", Configuration.LastTimeOpen),
        //            new XElement("Commission", Configuration.Commission),
        //            new XElement("AdminPassword", Configuration.AdminPassword),
        //            new XElement("AdminUser", Configuration.AdminUser),
        //            new XElement("MinAgeForOrder", Configuration.MinAgeForOrder),
        //            new XElement("MinDaysForOrder", Configuration.MinDaysForOrder),
        //            new XElement("MaxMonthForOrder", Configuration.MaxMonthForOrder));
        //        ConfigRoot.Save(ConfigPath);

        //    }
        //    catch (Exception)
        //    {
        //        //Do nothing
        //    }

        //}



    }
}