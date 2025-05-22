using ClassLib13.Business.Entities;
using ClassLib13.Data.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Data
{
    public class OrderItemData : SqlServer
    {
        public OrderItemData()
        {
            TableName = "OrderItems";
        }

        public string TableName { get; set; }

        public SelectResult Select()
        {
            return base.Select(TableName);
        }
        public SelectResult GetOrderItems(int id)
        {
            string query = $"SELECT * FROM {TableName}";
            using (SqlCommand command = new SqlCommand(query))
            {
                command.Parameters.AddWithValue("@Value", id);
                return Select(command);
            }
        }

        public InsertResult Insert(OrderItem orderItem)
        {
            var result = new InsertResult();
            try
            {
                // SQL Command
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append($"INSERT INTO {TableName} ");
                insertQuery.Append("(orderid, producttype, productid, quantity) VALUES ");
                insertQuery.Append("(@orderid, @producttype, @productid,@quantity);");

                using (SqlCommand insertCommand = new SqlCommand(insertQuery.ToString()))
                {

                    insertCommand.Parameters.AddWithValue("@orderid", orderItem.OrderId);
                    insertCommand.Parameters.AddWithValue("@producttype", orderItem.ProductType);
                    insertCommand.Parameters.AddWithValue("@productid", orderItem.ProductId);
                    insertCommand.Parameters.AddWithValue("@quantity", orderItem.Quantity);


                    result = InsertRecord(insertCommand);
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }
    }
}
    

