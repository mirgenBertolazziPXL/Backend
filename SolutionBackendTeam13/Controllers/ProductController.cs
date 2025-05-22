using ClassLib13.Business;
using ClassLib13.Business.Entities;
using ClassLib13.Business.Entities.ProductController;
using ClassLib13.Data;
using ClassLib13.Data.Framework;
using ClassLib13.Utils;
using WebAppTeam13.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509.Qualified;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System;
using System.Linq.Expressions;

namespace WebAppTeam13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        [Route("CreateProduct")]
        public IActionResult CreateProduct([FromBody] CreateProduct createProduct)
        {
            (object instance, string error) = ObjectCreator.CreateProduct(createProduct.Type, createProduct.Properties);

            if (error != null) { return ResponseService.GetResponse(400, error); }

            try
            {
                InsertResult result = Products.AddProduct(instance, new ProductData(), createProduct.Type);


                return result.NewId > 0
                    ? ResponseService.GetResponse(201, $"You created a new {createProduct.Type} product")
                    : ResponseService.GetResponse(500, "Database error: product creation failed");
                
            }
            catch (Exception exception)
            {
                // exception.GetType() == typeof(SqlException)
                return ResponseService.IsException(exception);
            }
        }

        [HttpGet]
        [Route("GetProducts")]
        public IActionResult GetProducts([FromQuery] string productType, [FromQuery]Dictionary<string, string> filters)
        {
            List<object> results = new List<object>();

            string error = DataValidator.ValidateData(productType.ToLower(), ref filters);
            if (error != null) { return ResponseService.GetResponse(400, error); }

            try
            {
                SelectResult result = Products.GetProducts(filters, new ProductData(), productType);

                if (productType.ToUpper() == "ALL")
                {
                    results.Add(new
                    {
                        productType,
                        filters,
                        Tables = result.DataTableList.Select((table, index) => new
                        {
                            table.TableName,
                            RowCount = table.Rows.Count,
                            Message = ProductData.ReturnSuccesMessage(table.Rows.Count, productType),
                            Data = ProductData.DataTableToList(table)
                        }).ToList()
                    });
                    return Ok(new { message = $"Succesfully gathered all products", data = results });
                }
                else
                {
                    return Ok(new
                    {
                        message = ProductData.ReturnSuccesMessage(result.DataTable.Rows.Count, productType),
                        data = ProductData.DataTableToList(result.DataTable)
                    });
                }
            }
            catch (Exception exception)
            {
                return ResponseService.IsException(exception);
            }
        }

        [HttpDelete]
        [Route("DeleteProducts")]
        public IActionResult DeleteProduct([FromQuery] Dictionary<string, string> productsToDelete)
        {
            string error = DataValidator.ValidateRemoveData(productsToDelete);
            if (error != null) { return ResponseService.GetResponse(400, error); }

            try
            {
                if (!new ProductData().DeleteProduct(productsToDelete))
                {
                    return ResponseService.GetResponse(404, $"productIDs: ({productsToDelete.Values} have no records in the database");
                }
                return Ok(new { message = "yaya" });
            }
            catch (Exception exception)
            {
                return ResponseService.IsException(exception);
            }
        }

        [HttpPut("UpdateProduct")]      
        public IActionResult UpdateProduct(string tableName, int productId, [FromBody]Dictionary<string, string> values)
        {
            string error = DataValidator.ValidateUpdateData(tableName, productId, values);
            if (error != null) { return ResponseService.GetResponse(400, error); }

            try
            {
                if (!new ProductData().UpdateProduct(tableName, productId, values))
                {
                    return ResponseService.GetResponse(404, $"productID: '{productId}' has no records in the database");
                }
                return Ok(new { message = "yaya" });
            }
            catch (Exception exception)
            {
                return ResponseService.IsException(exception);
            }
        }
    }
}
