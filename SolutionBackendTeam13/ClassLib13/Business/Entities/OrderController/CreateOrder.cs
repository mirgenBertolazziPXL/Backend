using ClassLib13.Business.Entities.OrderController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Business.Entities
{
    public class CreateOrder
    {
        public int CustomerId { get; set; }

        public string Status { get; set; }
        public DateTime OrderDate { get; set; }

        public Dictionary<string, List<PublicInfo>> ProductDictionary { get; set; }
    }
}
