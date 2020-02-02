namespace BE
{
    public class BankBranch
    {
        public string BankNumber { get; set; }
        public string BankName { get; set; }
        public string BranchNumber { get; set; }
        public Address BranchAddress { get; set; }
        public string BranchPhoneNumber { get; set; }
        public string BranchFaxNumber { get; set; }
        public override string ToString()
        {
            return "Bank num: "+ BankNumber + ", Branch: "+ BranchNumber;
        }
    }
}