using ClassLib13.Business.Entities;
using ClassLib13.Data.Framework;
using ClassLib13.Data.Repositories;
using ClassLib13.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using ClassLib13.Business.Entities.OrderController;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace ClassLib13.Business
{
    public class Orders
    {
        public static IEnumerable<Order> List()
        {
            return OrderRepository.OrderList;
        }

        public static SelectResult GetOrders()
        {
            OrderData orderData = new OrderData();
            SelectResult result = orderData.Select();
            return result;
        }
      

        public static InsertResult Add(int customerid, string status, DateTime orderdate, OrderData orderData)
        {
            Order order = new Order();
            {
                order.CustomerId = customerid;
                order.Status = status;
                order.OrderDate = orderdate;

            };
            InsertResult result = orderData.Insert(order);
            return result;
        }
        public static InsertResult AddOrderItems(Dictionary<string, List<PublicInfo>> productInfo, int orderId)
        {
            InsertResult result = new InsertResult(); 
            result.Succeeded = true;

            foreach (KeyValuePair<string, List<PublicInfo>> entry in productInfo)
            {
                string productType = entry.Key;
                List<PublicInfo> infos = entry.Value;

                foreach (var info in infos)
                {
                    OrderItem orderItem = new OrderItem
                    {
                        ProductType = productType,
                        ProductId = info.ProductId,
                        Quantity = info.Quantity,
                        OrderId = orderId
                    };


                    OrderItemData orderItemData = new OrderItemData();
                    InsertResult insertResult = orderItemData.Insert(orderItem);


                    result.NewId = insertResult.NewId;
                }
            }
            
            return result;
        }

    }
}