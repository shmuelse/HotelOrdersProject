using System;

namespace BE
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public enums.Gender Gender { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime SystemRegistrationDate { get; set; }
        public Login LoginDetails { get; set; }

        public override string ToString()
        {
            var contactDetails = string.Empty;
            return contactDetails += "First Name: " + FirstName + " Last Name: " + LastName;
        }
    }
}