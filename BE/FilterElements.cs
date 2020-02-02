namespace BE
{
    /// <summary>
    /// In this class (FilterElements) we can get all possible extras for a room reservation
    /// </summary>
    public class FilterElements
    {
        public bool Pool { get; set; }
        public bool Jacuzzi { get; set; }
        public bool Garden { get; set; }
        public bool Terrace { get; set; }
        public bool ChildrenAttractions { get; set; }
        public bool Tv { get; set; }
        public bool SmokingRoom { get; set; }
        public bool BabyCrib { get; set; }
        public bool Elevator { get; set; }
        public bool Spa { get; set; }
        public bool AirConditioning { get; set; }
        public bool WiFi { get; set; }
        public bool RoomService { get; set; }
        public bool DoubleBed { get; set; }
        public bool TwinBeds { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public bool FreeParking { get; set; }
        public bool Gym { get; set; }
        public bool PrivateBathroom { get; set; }
        public bool Bathtub { get; set; }
        public bool WashingMachine { get; set; }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}