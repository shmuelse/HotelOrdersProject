using System;

namespace BE
{
    public class DateMismatchException : Exception { public DateMismatchException(string message) : base(message) { } }

    public class MailValidationException : Exception { public MailValidationException(string message) : base(message) { } }

    public class IncorrectPhoneException : Exception { public IncorrectPhoneException(string message) : base(message) { } }

    public class UnitIsAvailableException : Exception { public UnitIsAvailableException(string message) : base(message) { } }


    public class GuestStatusException : Exception { public GuestStatusException(string message) : base(message) { } }

    public class NoBankCreditException : Exception { public NoBankCreditException(string message) : base(message) { } }

    public class NoOpenOrderException : Exception { public NoOpenOrderException(string message) : base(message) { } }

    public class IncorrectStatusException : Exception { public IncorrectStatusException(string message) : base(message) { } }

    public class IdOrderException : Exception { public IdOrderException(string message) : base(message) { } }
   
    public class CustomersException : Exception { public CustomersException(string message) : base(message) { } }
   
    public class HostsException : Exception { public HostsException(string message) : base(message) { } }
  
    public class AccommodationsException : Exception { public AccommodationsException(string message) : base(message) { } }
   
    public class HostingUnitsException : Exception { public HostingUnitsException(string message) : base(message) { } }
    public class GuestRequestException : Exception { public GuestRequestException(string message) : base(message) { } }

    public class HostingUnitNotExistException : Exception { public HostingUnitNotExistException(string message) : base(message) { } }

    public class OrderProblemException : Exception { public OrderProblemException(string message) : base(message) { } }

    public class BankAccountException : Exception { public BankAccountException(string message) : base(message) { } }
     
}