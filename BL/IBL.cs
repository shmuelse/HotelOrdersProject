using System;
using System.Collections.Generic;
using System.Linq;
using BE;

namespace BL
{
    public interface IBL
    {

        /// <summary>
        /// return a specific host unit
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <returns></returns>
        HostingUnit GetHostingUnit(int unitKey, int accommodationKey, string hostKey);

        /// <summary>
        /// return a specific Host from Data Base
        /// </summary>
        /// <param name="hostKey"></param>
        /// <returns></returns>
        Host GetHost(string hostKey);

        /// <summary>
        /// return a specific Host Id from Data Base
        /// </summary>
        /// <param name="hostLoginDetails"></param>
        /// <returns></returns>
        string GetHostKey(Login hostLoginDetails);


        /// <summary>
        /// return a specific guest request the from Data Base
        /// </summary>
        /// <param name="guestRequestKey"></param>
        /// <returns></returns>
        GuestRequest GetGuestRequest(int guestRequestKey);

        /// <summary>
        /// check if the user name already exist in  Data Base
        /// </summary>
        /// <param name="userNameToBeCheck"></param>
        /// <returns></returns>
        bool IsUserNameExist(string userNameToBeCheck);

        /// <summary>
        /// check if the user name Match the Password
        /// </summary>
        /// <param name="loginDetails">The login details we want to be check</param>
        /// <param name="authorizationType"></param>
        /// <returns></returns>
        bool IsUsernameMatchPassword(Login loginDetails, string authorizationType);

        /// <summary>
        /// Add Customer contact Info to Data Base
        /// </summary>
        /// <param name="customerContactInfo">The customer Contact Info</param>
        void AddCustomerContactInfo(Customer customerContactInfo);


        /// <summary>
        /// Add a new customer requirement to Data Base
        /// </summary>
        /// <param name="newGuestRequest">Customer's requirement</param>
        void AddACustomerRequirement(GuestRequest newGuestRequest);


        /// <summary>
        /// This function updating the status of the guest request To Data Base
        /// </summary>
        /// <param name="orderStatus">The status of the Order</param>
        /// <param name="guestRequestKey">The Key of the order request</param>
        void CustomerRequirementStatusUpdate(bool orderStatus, int guestRequestKey);

        /// <summary>
        /// Add a new Host contact to Data Base
        /// </summary>
        /// <param name="hostContactInfo">The Host Contact Info</param>
        void AddHostContactInfo(Host hostContactInfo);

        /// <summary>
        /// Add Accommodation for a specific Host in Data Base
        /// </summary>
        /// <param name="addAccommodation">Accommodation</param>
        void AddAccommodation(Accommodations addAccommodation);


        /// <summary>
        /// Add a new host unit to Data Base
        /// </summary>
        /// <param name="addHostingUnit">host unit to be added</param>
        void AddAHostingUnit(HostingUnit addHostingUnit);

        /// <summary>
        /// Delete host from Data Base
        /// </summary>
        /// <param name="hostId">host id we want to be deleted</param>
        void DeleteHost(string hostId);

        /// <summary>
        /// Delete customer from Data Base
        /// </summary>
        /// <param name="customerKey">Customer Key to be deleted</param>
        void DeleteCustomer(Login customerKey);


        /// <summary>
        /// Delete Accommodation from Data Base
        /// </summary>
        /// <param name="accommodationKey">accommodation Key</param>
        /// <param name="hostKey">host Key</param>
        void DeleteAccommodation(int accommodationKey, string hostKey);

        /// <summary>
        /// Delete host unit from Data Base
        /// </summary>
        /// <param name="unitKey">unit Key</param>
        /// <param name="accommodationKey">accommodation Key</param>
        /// <param name="hostKey">host Key</param>
        void DeleteHostingUnit(int unitKey, int accommodationKey, string hostKey);

        /// <summary>
        /// Update Accommodation Stars in Data Base
        /// </summary>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="stars"></param>
        void AccommodationStarsUpdate(int accommodationKey, string hostKey, uint stars);


        /// <summary>
        /// Update the sum of units in the Accommodation in Data Base
        /// </summary>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="sumOfUnits"></param>
        void AccommodationSumOfUnitsUpdate(int accommodationKey, string hostKey, uint sumOfUnits);


        /// <summary>
        /// Update the booking log of the hosting unit
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="diary"></param>
        void HostingUnitDiaryUpdate(int unitKey, int accommodationKey, string hostKey, DateTime checkIn, DateTime checkOut);


        /// <summary>
        /// Update the sum of the hosting unit
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="unitPrice"></param>
        void HostingUnitPriceUpdate(int unitKey, int accommodationKey, string hostKey, decimal unitPrice);

        /// <summary>
        /// Update the elements in the hosting unit
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="elements"></param>
        void HostingUnitElementsUpdate(int unitKey, int accommodationKey, string hostKey, FilterElements elements);

        /// <summary>
        /// Add a new order to list of orders in Data Base
        /// </summary>
        /// <param name="order"></param>
        void AddNewOrder(Order order);

        /// <summary>
        /// Update Order status in Data Base
        /// </summary>
        /// <param name="orderKey"></param>
        /// <param name="statusOfOrder"></param>
        void OrderUpdate(int orderKey, enums.OrderStatus statusOfOrder);


        /// <summary>
        /// Updating Accommodation details
        /// </summary>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="detailsInAccommodationUpdate"></param>
        /// <param name="newValue">can be Stars, sum of rooms etc' </param>
        void AccommodationUpdate(int accommodationKey, string hostKey, string detailsInAccommodationUpdate,
            object newValue);

        /// <summary>
        /// Updating hostingUnit details
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="detailsInUnitUpdate"></param>
        /// <param name="newValue">can be sum of orders, elements in unit, price, etc'...</param>
        void HostingUnitUpdate(int unitKey, int accommodationKey, string hostKey, enums.UnitUpdate detailsInUnitUpdate,
            object newValue);

        /// <summary>
        /// return a list of all Accommodations
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        IEnumerable<Accommodations> GetAllAccommodations(Func<Accommodations, bool> predicate = null);


        /// <summary>
        /// return a list of all Hosting Units
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        IEnumerable<HostingUnit> GetAllHostingUnits(Func<HostingUnit, bool> predicate = null);

        /// <summary>
        /// return a list of all Guest Request from Data Base
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        IEnumerable<GuestRequest> GetAllGuestRequests(Func<GuestRequest, bool> predicate = null);

        /// <summary>
        /// return a list of all Orders from Data Base
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        IEnumerable<Order> GetAllOrders(Func<Order, bool> predicate = null);

        /// <summary>
        /// return a list of all Customers from Data Base
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        IEnumerable<Customer> GetAllCustomers(Func<Customer, bool> predicate = null);

        /// <summary>
        /// return a list of all Hosts from Data Base
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        IEnumerable<Host> GetAllHosts(Func<Host, bool> predicate = null);


        /// <summary>
        /// return all Bank Branches from Data Base
        /// </summary>
        /// <param name="predicate">A function that accepts conditions for receiving an instance</param>
        /// <returns></returns>
        IEnumerable<BankBranch> GetAllBankBranches(Func<BankBranch, bool> predicate = null);

        /// <summary>
        /// return list of available units starts from a certain date and for the given time
        /// </summary>
        /// <param name="checkInDate"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        List<HostingUnit> ListOfAvailableUnits(DateTime checkInDate, int duration);


        /// <summary>
        /// Returns the sum of days between the received dates or the current day
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        int GetDays(DateTime fromDate, object toDate = null);


        /// <summary>
        /// Returns the orders created before the number of days we received
        /// </summary>
        /// <param name="sumDays"></param>
        /// <returns></returns>
        List<Order> GetOrdersCreatedSince(int sumDays);


        /// <summary>
        /// Returns the amount of orders sent for a specific customer requirement
        /// </summary>
        /// <param name="guestRequestKey"></param>
        /// <returns></returns>
        int GetSumOfOrders(int guestRequestKey);


        /// <summary>
        ///  Returns the amount of bookings sent to the customer for this unit
        /// or the amount of successful orders made for the accommodation unit.
        /// </summary>
        /// <param name="unitKey"></param>
        /// <param name="accommodationKey"></param>
        /// <param name="hostKey"></param>
        /// <param name="openOrCloseOrder">choosing between orders close (True) and orders that only sent(False)</param>
        /// <returns></returns>
        int GetUnitSumOfOrders(int unitKey, int accommodationKey, string hostKey, bool openOrCloseOrder);


        /// <summary>
        /// Get Guest Requests Group By Area
        /// </summary>
        /// <param name="sorted"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<enums.Area, GuestRequest>> GetGuestRequestsGroupByArea(bool sorted = false);

        /// <summary>
        /// Get Guest Requests Group By sum of hosted
        /// </summary>
        /// <param name="sorted"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<uint, GuestRequest>> GetGuestRequestGroupBySumOfHosted(bool sorted = false);

        /// <summary>
        /// Get Hosts Group By sum of units
        /// </summary>
        /// <param name="sorted"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<int, Host>> GetHostsGroupBySumOfUnits(bool sorted = false);

        /// <summary>
        /// Get Accommodations Group By Area
        /// </summary>
        /// <param name="sorted"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<enums.Area, Accommodations>> GetAccommodationsGroupByArea(bool sorted = false);

        /// <summary>
        /// Get Hosting Unit Group By Area
        /// </summary>
        /// <param name="sorted"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<enums.Area, HostingUnit>> GetHostingUnitGroupByArea(bool sorted = false);


        void SaveSettings();




    }
}