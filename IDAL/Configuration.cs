using System;
using System.Xml.Linq;

namespace IDAL
{
    public static class Configuration
    {

        public static int GuestRequestKey = 10000000;
        public static int AccommodationKey = 20000000;
        public static int HostingUnitKey = 30000000;
        public static int OrderKey = 40000000;
        public static decimal Commission = 10m;


        /// <summary>
        /// Admin password
        /// </summary>
        public static string AdminPassword = "Popsim770";

        /// <summary>
        /// Admin User
        /// </summary>
        public static string AdminUser = "Administrator";


        /// <summary>
        ///  If the program is opened for the first time
        /// </summary>
        public static bool FirstOpenProgram = true;

        public static uint MinAgeForOrder = 18;

        public static uint MinDaysForOrder = 1;

        public static uint MaxMonthForOrder = 11;

        public static DateTime LastTimeOpen = new DateTime();

    }
}