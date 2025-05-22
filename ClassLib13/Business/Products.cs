using ClassLib13.Business.Entities.ProductController;
using ClassLib13.Data;
using ClassLib13.Data.Framework;
using ClassLib13.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib13.Business.Entities;
using System.Data.SqlClient;
using System.Net;


namespace ClassLib13.Business
{
    public class Products
    {
        public static InsertResult AddProduct(Object newProduct, ProductData productData, string objectType)
        {
            return productData.Insert(newProduct, objectType);
        }

        public static SelectResult GetProducts(Dictionary<string, string> filters, ProductData productData, string objectType)
        {
            return objectType.ToUpper() == "ALL" ? productData.SelectMultiple(filters, objectType) : productData.SelectSingle(filters, objectType);
        }
    }
}
