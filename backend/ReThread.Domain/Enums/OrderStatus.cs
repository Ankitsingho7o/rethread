namespace ReThreaded.Domain.Enums;

public enum OrderStatus
{
    Pending = 1,      // Created, awaiting payment
    Paid = 2,         // Payment successful
    Processing = 3,   // Designer preparing
    Shipped = 4,      // Shipped with tracking
    Delivered = 5,    // Delivered to buyer
    Cancelled = 6     // Cancelled
}