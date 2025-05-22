using ClassLib13.Business.Entities;
using WebAppTeam13.ViewModels.Orders;

namespace WebAppTeam13.Mappers
{
    public static class OrderMapper
    {
        public static OrderWithCustomer Map(Order order, string customerName)
        {
            return new OrderWithCustomer
            {
                OrderData = order,
                CustomerName = customerName
            };
        }

        public static List<OrderWithCustomer> Map(Dictionary<Order, string> ordersWithNames)
        {
            return ordersWithNames.Select(order => Map(order.Key, order.Value)).ToList();
        }
    }
}
