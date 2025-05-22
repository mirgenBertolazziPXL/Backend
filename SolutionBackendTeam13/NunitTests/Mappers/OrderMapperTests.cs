using ClassLib13.Business.Entities;
using ClassLib13.Data;
using Org.BouncyCastle.Utilities;
using WebAppTeam13.Mappers;
using WebAppTeam13.ViewModels.Orders;
namespace NunitTests;

[TestFixture]
public class OrderMapperTests
{
    private Order _order1;
    private Order _order2;
    private string _customerName1;
    private string _customerName2;

    [SetUp]
    public void Init()
    {
        _order1 = new Order
        {
            OrderId = 1,
            CustomerId = 5,
            Status = "Ongoing",
            OrderDate = DateTime.Parse("11-05-2025")
        };
        _customerName1 = "Kristof Palmaers";

        _order2 = new Order
        {
            OrderId = 2,
            CustomerId = 7,
            Status = "Unpaid",
            OrderDate = DateTime.Parse("11-05-2025")
        };
        _customerName2 = "Sander De Puydt";
    }
    [Test]
    public void MapperReturnsCorrectOrderWithCustomer_SingleOrder()
    {
        var result = OrderMapper.Map(_order1, _customerName1);

        Assert.That(_order1, Is.EqualTo(result.OrderData));
        Assert.That(_customerName1, Is.EqualTo(result.CustomerName));
    }

    [Test]
    public void MapperReturnsCorrectOrderWithCustomer_DictionaryOfOrders()
    {
        var result = OrderMapper.Map(new Dictionary<Order, string>() 
        { 
            { _order1, _customerName1 }, 
            { _order2, _customerName2 } 
        });

        Assert.That(result.Any(order => order.OrderData.Equals(_order1) && order.CustomerName.Equals(_customerName1)));
        Assert.That(result.Any(order => order.OrderData.Equals(_order2) && order.CustomerName.Equals(_customerName2)));
    }

    [Test]
    public void MapperHasNoEmptyValues()
    {
        var result = OrderMapper.Map(new Dictionary<Order, string>()
        {
            { _order1, _customerName1 },
            { _order2, _customerName2 }
        });

        Assert.That(result.All(order => order.OrderData != null));
        Assert.That(result.All(order => !string.IsNullOrEmpty(order.CustomerName)));
    }
}
