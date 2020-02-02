namespace BE
{
    public class OrdersCompleted
    {
        public string HostName { get; set; }
        public string HostKey { get; set; }
        public int NumOfOrders { get; set; }
        public decimal Commission { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}