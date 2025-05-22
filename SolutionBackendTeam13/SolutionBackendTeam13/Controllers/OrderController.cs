using ClassLib13.Business.Entities;
using ClassLib13.Business;
using ClassLib13.Data.Framework;
using ClassLib13.Data;
using ClassLib13;
using WebAppTeam13.ViewModels.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Data.SqlClient;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppTeam13.Mappers;

namespace WebAppTeam13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost]
        [Route("CreateOrder")]
        public IActionResult CreateOrder([FromBody] CreateOrder createOrder)
        {
            if (createOrder == null)
            {
                return BadRequest("Order data is required.");
            }

            Console.WriteLine($"Received: {JsonConvert.SerializeObject(createOrder)}");


            OrderData orderData = new OrderData();
            InsertResult result = Orders.Add(createOrder.CustomerId, createOrder.Status, createOrder.OrderDate, orderData);

            if (result != null && result.NewId > 0)
            {
                OrderItemData orderItem = new();
                InsertResult resultItems = Orders.AddOrderItems(createOrder.ProductDictionary, result.NewId);
                var email = orderData.Getemail(createOrder.CustomerId);
                EmailSender emailSender = new EmailSender();
                emailSender.SentMailOrder(email);
                return Ok(new { message = "Order created successfully!", orderId = result.NewId });
            }

            return StatusCode(500, new { message = "An error occurred while creating the order.", orderId = result.NewId });
        }
        [HttpGet]
        [Route("GetOrders")]
        public ActionResult GetOrders([FromQuery] int page)
        {
            OrderData orderData = new();
            var orders = orderData.SelectWithRange(page);
            var result = OrderMapper.Map(orders);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetOrderWithItems")]
        public ActionResult GetOrderWithITems([FromQuery] int Orderid)
        {
            OrderData orderData = new();
            var result = orderData.GetOrderWithItems(Orderid);
            return Ok(result);

        }
        [HttpGet]
        [Route("GetOrdersByStatus")]
        public ActionResult GetOrdersByStatus(int page,string status)
        {
            OrderData orderData = new();
            var result = orderData.OrderByStatus(page, status );
            var orders = OrderMapper.Map(result);
            return Ok(orders);
        }

        [HttpGet]
        [Route("GetOrdersByOrderId")]
        public ActionResult GetOrdersByOrderId(int orderId)
        {
            OrderData orderData = new();
            var result = orderData.GetOrdersByOrderId(orderId);
            var orders = OrderMapper.Map(result);
            return Ok(orders);
        }

        [HttpGet]
        [Route("GetOrdersByCustomerId")]
        public ActionResult GetOrdersByCustomerId(int page, int id)
        {
            OrderData orderData = new();
            var result = orderData.GetOrdersByCustomerId(page, id);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetItemDetails")]
        public ActionResult GetItemDetails(string productType, int id)
        {
            OrderData orderData = new();
            var result = orderData.GetItemDetails(productType.ToLower(), id);
            return Ok(result);
        }

        [HttpPut]
        [Route("ChangeOrderStatus")]
        public ActionResult ChangeOrderStatus(int orderid,string status)
        {
            OrderData orderData = new();
            var result = orderData.ChangeStatus(orderid,status);
            if (!result)
            {
                return StatusCode(500, new { message = "Error updating Status." });
            }
            var id = orderData.Getuserid(orderid);
            var email = orderData.Getemail(id);
            EmailSender emailSender = new EmailSender();
            emailSender.SentMailOrderStatusChanged(email,status);
            return Ok(new { message = "Status updated successfully." });
        }
        [HttpPut]
        [Route("ChangeOrderStatusToCanceld")]
        public ActionResult ChangeOrderStatusToCanceled(int orderid)
        {
            OrderData orderData = new();
            var result = orderData.ChangeStatusTocanceled(orderid);
            if (!result)
            {
                return StatusCode(500, new { message = "Error updating Status." });
            }
            var id = orderData.Getuserid(orderid);
            var email = orderData.Getemail(id);
            EmailSender emailSender = new EmailSender();
            emailSender.SentMailOrderCanceled(email);

            return Ok(new { message = "Status updated successfully." });
            
        }
        [HttpGet]
        [Route("GetAMountOfPages")]
        public ActionResult GetAMountOfPages()
        {
            OrderData orderData = new();
            var result = orderData.GetAmount();
            return Ok(result);
        }

    }
 }
