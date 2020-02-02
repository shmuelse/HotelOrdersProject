using System.Collections.Generic;

namespace BE
{

    public class Customer
    {
        public Contact ClientInfo { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class Host
    {
        public Contact HostInfo { get; set; }
        public string Id { get; set; }
        public BankBranch BankBranchDetails { get; set; }
        public string BankAccountNumber { get; set; }
        public bool CollectionClearance { get; set; }
        public int SumOfUnits { get; set; }
        public decimal CommissionSum { get; set; }
        public List<Accommodations> HostAccommodationsList { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class SiteOwner
    {
        public Login OwnerLogin { get; set; }
        //public List<OrdersCompleted> OrdersCompleted { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
