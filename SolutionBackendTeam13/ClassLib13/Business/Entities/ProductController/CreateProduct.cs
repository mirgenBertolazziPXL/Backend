using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Business.Entities.ProductController
{
    public class CreateProduct
    {
        public string Type { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}
