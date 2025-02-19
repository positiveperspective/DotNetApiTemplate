namespace DotNetAPI.Domain.OrderDomain;

public class Order
{
    public int OrderID { get; private set; }

    public DateTime OrderDate { get; private set; } 

    public int OrderCustomerID { get; private set; }
    
    public int EventId { get; private set; }

    public int RequestQty { get; private set; }

    public DateTime ServiceDate1 { get; private set; }

    public DateTime ServiceDate2 { get; private set; }

    public DateTime ServiceDate3 { get; private set; }

    public bool OrderBondIsPaid { get; private set; }

    public DateTime OrderBondPaidDate { get; private set; }

    public bool OrderDifferenceIsPaid { get; private set; }

    public DateTime OrderDifferenceDate { get; private set; }

    public int OrderPriority { get; private set; }

    public bool OrderIsDispatched { get; private set; }

    public DateTime OrderShippedDate { get; private set; }

    public int OrderProgress { get; private set; }

    public bool OrderIsDelivered { get; private set; }

    public DateTime OrderDeliveredDate { get; private set; }

    public string PayUOrderID { get; private set; }

    public string PayPalOrderID { get; private set; }

    public int StoreID { get; private set; } 

    public int StaffID { get; private set; } 

    private Order()
    {

    }

    public Order(DateTime orderDate, int orderCustomerId, int eventId, int requestQty, DateTime serviceDate1, DateTime serviceDate2, DateTime serviceDate3, bool orderBondIsPaid, DateTime orderBondPaidDate, bool orderDifferenceisPaid, DateTime orderDifferenceDate, int orderPriority, bool orderIsDispatched, DateTime orderShippedDate, int orderProgress, DateTime orderDeliveredDate, int storeId, int staffId, string payUOrderId, string payPalOrderId)
    {
        OrderDate = orderDate;
        OrderCustomerID = orderCustomerId;
        EventId = eventId;
        RequestQty = requestQty;
        ServiceDate1 = serviceDate1;
        ServiceDate2 = serviceDate2;
        ServiceDate3 = serviceDate3;
        OrderBondIsPaid = orderBondIsPaid;
        OrderBondPaidDate = orderBondPaidDate;
        OrderDifferenceIsPaid = orderDifferenceisPaid;
        OrderDifferenceDate = orderDifferenceDate;
        OrderPriority = orderPriority;
        OrderIsDispatched = orderIsDispatched;
        OrderShippedDate = orderShippedDate;
        OrderProgress = orderProgress;
        OrderDeliveredDate = orderDeliveredDate;
        StoreID = storeId;
        StaffID = staffId;
        PayUOrderID = payUOrderId;
        PayPalOrderID = payPalOrderId;
    }
}