using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Theam.Models;

namespace Theam.Controllers
{
    public class ManageOrdersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ManageOrdersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Orders List
        public async Task<IActionResult> ManageOrders()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("http://localhost:5128/api/Order/GetAllOrders");
            var orders = JsonConvert.DeserializeObject<List<OrderModel>>(response);
            return View(orders);
        }

        // GET: Order Details
        public async Task<IActionResult> DetailOrders(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync($"http://localhost:5128/api/Order/user/{id}");
            var orders = JsonConvert.DeserializeObject<List<OrderModel>>(response);

            return View(orders); // Pass entire list
        }


        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            if (request == null || request.OrderID <= 0)
            {
                return BadRequest(new { message = "Invalid request data." });
            }

            var client = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await client.PutAsync("http://localhost:5128/api/Order/update-status", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { message = "Order status updated successfully!" });
            }

            return BadRequest(new { message = "Failed to update order status." });
        }
    }

    public class UpdateOrderStatusRequest
    {
        public int OrderID { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
    }
}
