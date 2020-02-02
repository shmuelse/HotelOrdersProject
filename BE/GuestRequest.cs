using System;

namespace BE
{
    public class GuestRequest
    {
        public int GuestRequestKey { get; set; }
        public Login ClientLoginDetails { get; set; }
        public bool OrderStatus { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public enums.Area Area { get; set; }
        public enums.Districts SubArea { get; set; }
        public Type TypeOfAccommodationRequested { get; set; }
        public enums.Style StyleOfUnitRequested { get; set; }
        public uint AmountOfAdults { get; set; }
        public uint AmountOfChildren { get; set; }
        public FilterElementsForGuest SpecificRequirements { get; set; }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}