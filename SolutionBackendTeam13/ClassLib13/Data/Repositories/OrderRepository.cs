using ClassLib13.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Data.Repositories
{
    internal class OrderRepository
    {
        static OrderRepository()
        {
            OrderList = new List<Order>();

        }
        public static List<Order> OrderList { get; set; }
       
    }
}