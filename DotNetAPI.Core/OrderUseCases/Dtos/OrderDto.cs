namespace DotNetAPI.Core.OrderUseCases.Dtos;

public class OrderDto
{
    public int OrderID { get; init; }

    public DateTime OrderDate { get; init; }

    public int OrderCustomerID { get; init; }

    public int EventId { get; init; }

    public int RequestQty { get; init; }

    public DateTime ServiceDate1 { get; init; }

    public DateTime ServiceDate2 { get; init; }

    public DateTime ServiceDate3 { get; init; }

    public bool OrderBondIsPaid { get; init; }

    public DateTime OrderBondPaidDate { get; init; }

    public bool OrderDifferenceIsPaid { get; init; }

    public DateTime OrderDifferenceDate { get; init; }

    public int OrderPriority { get; init; }

    public bool OrderIsDispatched { get; init; }

    public DateTime OrderShippedDate { get; init; }

    public int OrderProgress { get; init; }

    public bool OrderIsDelivered { get; init; }

    public DateTime OrderDeliveredDate { get; init; }

    public string PayUOrderID { get; init; }

    public string PayPalOrderID { get; init; }

    public int StoreID { get; init; }

    public int StaffID { get; init; }
}
