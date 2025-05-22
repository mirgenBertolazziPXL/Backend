using ClassLib13.Business.Entities;
namespace NunitTests;

public class OrderTest
{
    private Order _order;
    private List<string> _allowedStatuses;
    [SetUp]
    public void Init()
    {
        _order = new Order() 
        { 
            OrderId = 1, 
            CustomerId = 2, 
            OrderDate = DateTime.Parse("2025-05-11"), 
            Status = "Ongoing" 
        };

        _allowedStatuses = new List<string> { "Ongoing", "Cancelled", "Completed", "Unpaid", "Paid" };
    }

    [Test]
    public void IsOrderIdValid()
    {
        Assert.That(_order.OrderId, Is.TypeOf<int>());
        Assert.That(_order.OrderId, Is.GreaterThan(0));
    }

    [Test]
    public void IsCustomerIdValid()
    {
        Assert.That(_order.CustomerId, Is.TypeOf<int>());
        Assert.That(_order.CustomerId, Is.GreaterThan(0));
    }

    [Test]
    public void IsStatusValid()
    {
        Assert.That(_allowedStatuses, Does.Contain(_order.Status));
        Assert.That(_order.Status, Is.Not.Empty);
    }

    [Test]
    public void IsOrderDateValid()
    {
        Assert.That(_order.OrderDate, Is.TypeOf<DateTime>());
        Assert.That(_order.OrderDate, Is.LessThanOrEqualTo(DateTime.Now));
    }

    [Test]
    public void IsOrderNotNull()
    {
        Assert.That(_order, Is.Not.Null);
    }
}
