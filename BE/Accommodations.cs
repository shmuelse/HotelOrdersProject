using System;
using System.Collections.Generic;

namespace BE
{
    public class Accommodations
    {
        public int AccommodationKey { get; set; }
        public string HostId { get; set; }
        public Type TypeOfAccommodation { get; set; }
        public List<enums.Style> StyleOfAccommodation { get; set; }
        public List<string> Uris { get; set; }
        public uint Stars { get; set; }
        public string AccommodationName { get; set; }
        public Address AccommodationAddress { get; set; }
        public uint SumOfUnits { get; set; }
        public enums.Area Area { get; set; }
        public enums.Districts District { get; set; }
        public List<HostingUnit> ListOfAllUnits { get; set; }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}