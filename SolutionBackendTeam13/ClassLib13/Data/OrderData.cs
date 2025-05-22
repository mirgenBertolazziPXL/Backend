using ClassLib13.Business.Entities;
using ClassLib13.Data.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using ClassLib13.Business;
using System.Collections.ObjectModel;

namespace ClassLib13.Data
{
    public class OrderData : SqlServer
    {
        public OrderData()
        {
            TableName = "Orders";
        }

        public string TableName { get; set; }

        public SelectResult Select()
        {
            return base.Select(TableName);
        }
        public Dictionary<Order, string> SelectWithRange(int page)
        {
            string query;
            if (page == -1)
            {
                 query = $@"SELECT o.OrderId, o.CustomerId, o.Status, o.OrderDate, u.Lastname + ' ' + u.Firstname AS CustomerName
                      FROM ORDERS o
                      INNER JOIN USERS u ON o.CustomerId = u.Id
                      ORDER BY o.OrderId";


            }
            else
            {
                int offset = (page - 1) * 10;

                query = $@"SELECT o.OrderId, o.CustomerId, o.Status, o.OrderDate, u.Lastname + ' ' + u.Firstname AS CustomerName
                      FROM ORDERS o
                      INNER JOIN USERS u ON o.CustomerId = u.Id
                      ORDER BY o.OrderId
                      OFFSET {offset} ROWS FETCH NEXT 10 ROWS ONLY";
            }

            SqlCommand command = new SqlCommand(query);
            SelectResult result = Select(command);

            Dictionary<Order, string> ordersList = new Dictionary<Order, string>();

            if (!result.Succeeded || result.DataTable == null)
                return ordersList;

            foreach (DataRow row in result.DataTable.Rows)
            {
                Order order = new Order
                {
                    OrderId = Convert.ToInt32(row["OrderId"]),
                    CustomerId = Convert.ToInt32(row["CustomerId"]),
                    Status = Convert.ToString(row["Status"]),
                    OrderDate = Convert.ToDateTime(row["OrderDate"])
                };

                ordersList.Add(order, row["CustomerName"] == DBNull.Value ? "No Name Found" : row["CustomerName"].ToString());
            }

            return ordersList;
        }



        public SelectResult GetOrder(int id)
        {
            string query = $"SELECT * FROM {TableName} and SELECT * FROM ORDERITEMS WHERE ORDERID = thisid";
            using (SqlCommand command = new SqlCommand(query))
            {
                command.Parameters.AddWithValue("@Value", id);
                return Select(command);
            }
        }

        public List<Dictionary<string, object>> GetOrderWithItems(int id)
        {
            string query = $@"SELECT o.OrderId, o.CustomerId, o.Status, o.OrderDate,oi.OrderItemId, oi.ProductType, oi.ProductId, oi.Quantity
                        FROM Orders o                  
                        LEFT JOIN OrderItems oi ON o.OrderId = oi.OrderId
                        Where o.OrderId = @id";

            SqlCommand command = new SqlCommand(query);
            command.Parameters.AddWithValue("@id", id);
            SelectResult result = Select(command);

            if (!result.Succeeded || result.DataTable == null)
                return new List<Dictionary<string, object>>();

            Dictionary<int, Dictionary<string, object>> orders = new Dictionary<int, Dictionary<string, object>>();

            foreach (DataRow row in result.DataTable.Rows)
            {
                int orderId = Convert.ToInt32(row["OrderId"]);

                if (!orders.ContainsKey(orderId))
                {
                    var order = new Dictionary<string, object>
                    {
                        ["OrderId"] = orderId,
                        ["CustomerId"] = row["CustomerId"],
                        ["Status"] = row["Status"],
                        ["OrderDate"] = row["OrderDate"],
                        ["Items"] = new List<Dictionary<string, object>>()
                    };
                    orders[orderId] = order;
                }
                if (row["OrderItemId"] != DBNull.Value)
                {
                    var item = new Dictionary<string, object>
                    {
                        ["OrderItemId"] = row["OrderItemId"],
                        ["ProductType"] = row["ProductType"],
                        ["ProductId"] = row["ProductId"],
                        ["Quantity"] = row["Quantity"]
                    };

                    ((List<Dictionary<string, object>>)orders[orderId]["Items"]).Add(item);
                }
            }

            return orders.Values.ToList();
        }




        public InsertResult Insert(Order order)
        {
            var result = new InsertResult();
            try
            {

                StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append($"INSERT INTO {TableName} ");
                insertQuery.Append("(customerid, status, orderdate) VALUES ");
                insertQuery.Append("(@customerid, @status, @orderdate);");

                using (SqlCommand insertCommand = new SqlCommand(insertQuery.ToString()))
                {

                    insertCommand.Parameters.AddWithValue("@customerid", order.CustomerId);
                    insertCommand.Parameters.AddWithValue("@status", order.Status);
                    insertCommand.Parameters.AddWithValue("@orderdate", order.OrderDate);


                    result = InsertRecord(insertCommand);
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }
        public Dictionary<Order, string> OrderByStatus(int page, string status)
        {
            int offset = (page - 1) * 10;

            string query = $@"SELECT o.*, u.Lastname + ' ' + u.Firstname AS CustomerName FROM Orders o 
                              INNER JOIN USERS u ON o.CustomerId = u.Id 
                              WHERE status = @status ORDER BY o.OrderId OFFSET @offset ROWS FETCH NEXT 10 ROWS ONLY";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@offset", offset);
            
            SelectResult result = Select(command);

            Dictionary<Order, string> ordersList = new Dictionary<Order, string>();

            if (!result.Succeeded || result.DataTable == null)
                return ordersList;

            foreach (DataRow row in result.DataTable.Rows)
            {
                Order order = new Order
                {
                    OrderId = Convert.ToInt32(row["OrderId"]),
                    CustomerId = Convert.ToInt32(row["CustomerId"]),
                    Status = Convert.ToString(row["Status"]),
                    OrderDate = Convert.ToDateTime(row["OrderDate"])
                };

                string customerName = Convert.ToString(row["CustomerName"]);

                ordersList.Add(order, customerName);
            }

            return ordersList;
        }
        public List<Order> GetOrdersByCustomerId(int page, int id)
        {
            SqlCommand command;
            if (page == -1)
            {
                string query = $@"SELECT * FROM Orders WHERE customerId = @id ORDER BY OrderId";
                command = new SqlCommand(query);
                command.Parameters.AddWithValue("@id", id);
                

            }
            else
            {
                int offset = (page - 1) * 10;

                string query = $@"SELECT * FROM Orders WHERE customerId = @id ORDER BY OrderId OFFSET @offset ROWS FETCH NEXT 10 ROWS ONLY";
                command = new SqlCommand(query);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@offset", offset);

                
            }

            SelectResult result = Select(command);
            List<Order> ordersList = new List<Order>();

            if (!result.Succeeded || result.DataTable == null)
                return ordersList;

            foreach (DataRow row in result.DataTable.Rows)
            {
                Order order = new Order
                {
                    OrderId = Convert.ToInt32(row["OrderId"]),
                    CustomerId = Convert.ToInt32(row["CustomerId"]),
                    Status = Convert.ToString(row["Status"]),
                    OrderDate = Convert.ToDateTime(row["OrderDate"])
                };

                ordersList.Add(order);
            }

            return ordersList;
        }

        public Dictionary<Order, string> GetOrdersByOrderId(int orderId)
        {            
            string query = $@"SELECT o.OrderId, o.CustomerId, o.Status, o.OrderDate, u.Lastname + ' ' + u.Firstname AS CustomerName
                      FROM ORDERS o 
                      INNER JOIN USERS u ON o.CustomerId = u.Id
                      WHERE o.OrderId = @id
                      ORDER BY o.OrderId";

            SqlCommand command = new SqlCommand(query);
            command.Parameters.AddWithValue("@id", orderId);

            SelectResult result = Select(command);

            Dictionary<Order, string> ordersList = new Dictionary<Order, string>();

            if (!result.Succeeded || result.DataTable == null)
                return ordersList;

            foreach (DataRow row in result.DataTable.Rows)
            {
                Order order = new Order
                {
                    OrderId = Convert.ToInt32(row["OrderId"]),
                    CustomerId = Convert.ToInt32(row["CustomerId"]),
                    Status = Convert.ToString(row["Status"]),
                    OrderDate = Convert.ToDateTime(row["OrderDate"])
                };

                ordersList.Add(order, Convert.ToString(row["CustomerName"]));
            }

            return ordersList;
        }

        public List<object> GetItemDetails(string productType, int id)
        {
            List<object> ItemList = new List<object>();
            var allowedTables = new HashSet<string> { "challenges", "subscriptions", "excercises" };

            if (!allowedTables.Contains(productType))
            {
                return ItemList;
            }
            string query = $"SELECT * FROM {productType} WHERE Id = @id";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.AddWithValue("@id", id);

            SelectResult result = Select(command);



            if (!result.Succeeded || result.DataTable == null)
                return ItemList;

            foreach (DataRow row in result.DataTable.Rows)
            {
                if (productType == "challenges")
                {
                    ItemList.Add(new Challenge
                    {
                        Title = Convert.ToString(row["Title"]),
                        Description = Convert.ToString(row["Description"]),
                        Points = Convert.ToInt32(row["Points"])
                    });


                }
                if (productType == "subscriptions")
                {
                    ItemList.Add(new Subscription
                    {
                        Duration = Convert.ToString(row["Duration"]),
                        Description = Convert.ToString(row["Description"]),
                        Price = Convert.ToInt32(row["Price"]),
                    });

                }
                if (productType == "excercises")
                {
                    Exercise exercise = new Exercise
                    {
                        Sport = Convert.ToString(row["Sport"]),
                        Name = Convert.ToString(row["Name"]),
                        Grade = Convert.ToInt32(row["Grade"]),
                        Description = Convert.ToString(row["Description"]),
                        Video = Convert.ToString(row["Video"]),

                    };
                    ItemList.Add(exercise);
                }
            }
            return ItemList;
        }
        public bool ChangeStatus(int orderid, string status)
        {

            string query = $@"UPDATE ORDERS SET status = @status WHERE OrderId = @id";

            using (SqlConnection connection = new SqlConnection(Settings.GetConnectionString()))
            using (SqlCommand updateCommand = new SqlCommand(query, connection))
            {

                updateCommand.Parameters.AddWithValue("@status", status);
                updateCommand.Parameters.AddWithValue("@id", orderid);

                try
                {

                    connection.Open();
                    int rowsAffected = updateCommand.ExecuteNonQuery();


                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public bool ChangeStatusTocanceled(int orderid)
        {

            string query = $@"UPDATE ORDERS SET status = 'canceled' WHERE OrderId = @id";

            using (SqlConnection connection = new SqlConnection(Settings.GetConnectionString()))
            using (SqlCommand updateCommand = new SqlCommand(query, connection))
            {
                updateCommand.Parameters.AddWithValue("@id", orderid);

                try
                {

                    connection.Open();
                    int rowsAffected = updateCommand.ExecuteNonQuery();


                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public string Getemail(int userid)
        {
            string query = @"SELECT Email FROM Users WHERE Id = @id";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.AddWithValue("@id", userid);
            try
            {
                using (SqlConnection connection = new SqlConnection(Settings.GetConnectionString()))
                {
                    connection.Open();
                    command.Connection = connection;


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            return reader.GetString(reader.GetOrdinal("Email"));
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error occurred: {ex.Message}");
                return null;
            }
        }

        public int Getuserid(int orderid)
        {
            string query = @"SELECT Customerid FROM Orders WHERE orderid = @id";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.AddWithValue("@id", orderid);
            try
            {
                using (SqlConnection connection = new SqlConnection(Settings.GetConnectionString()))
                {
                    connection.Open();
                    command.Connection = connection;


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            return reader.GetInt32(reader.GetOrdinal("customerid"));
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error occurred: {ex.Message}");
                return 0;
            }
        }
        public int GetAmount()
        {
            string query = $"SELECT COUNT(*) FROM {TableName}";
            SqlCommand command = new SqlCommand(query);
            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.GetConnectionString()))
                {
                    conn.Open();
                    command.Connection = conn;


                    object result = command.ExecuteScalar();

                    int page = Convert.ToInt32(result);
                    return (int)Math.Ceiling(page / 10.0);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error occurred: {ex.Message}");
                return -1;
            }
        }

    }
}
