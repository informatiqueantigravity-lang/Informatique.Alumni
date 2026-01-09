namespace Informatique.Alumni.Certificates;

public enum CertificateLanguage
{
    Arabic,
    English
}

public enum DeliveryMethod
{
    OfficePickup,
    WaslaDelivery
}

public enum RequestStatus
{
    InProgress,
    ReadyForPickup,
    OutForDelivery,
    Delivered
}

public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    Cash,
    BankTransfer
}
