using System;

namespace BE
{
    /// <summary>
    /// This class represents an order (i.e. the relationship between a customer and a hosting unit)
    /// </summary>
    [Serializable]
    public class Order
    {
        public string HostId { get; set; }
        public int AccommodationKey { get; set; }
        public int HostingUnitKey { get; set; }
        public int GuestRequestKey { get; set; }
        public int OrderKey { get; set; }
        public enums.OrderStatus OrderStatus { get; set; }
        public DateTime OrderCreationDate { get; set; }
        public DateTime SendingEmailDate { get; set; }
      
      
        public override string ToString()
        {
            return base.ToString();
        }
    }
}