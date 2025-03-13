using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Theam.Models;

namespace Theam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var apiUrl = "http://localhost:5128/api/DashBoard"; // Update as needed
            DashBoardViewModel dashboardData = new DashBoardViewModel();

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    dashboardData = JsonConvert.DeserializeObject<DashBoardViewModel>(jsonData);
                }
                else
                {
                    _logger.LogError("Error fetching dashboard data. Status Code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
            }

            return View(dashboardData);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddInventoryItem(InventoryCreateModel inventoryItem)
        {
            if (ModelState.IsValid)
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(inventoryItem.InventoryID.ToString()), "InventoryID");
                content.Add(new StringContent(inventoryItem.ItemName), "ItemName");
                content.Add(new StringContent(inventoryItem.QuantityAvailable.ToString()), "QuantityAvailable");
                content.Add(new StringContent(inventoryItem.QuantityWanted.ToString()), "QuantityWanted");

                if (inventoryItem.ImageURL != null)
                {
                    var fileContent = new StreamContent(inventoryItem.ImageURL.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(inventoryItem.ImageURL.ContentType);
                    content.Add(fileContent, "ImageURL", inventoryItem.ImageURL.FileName);
                }

                var response = await _httpClient.PostAsync("http://localhost:5128/api/Inventory", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Inventory item added successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Error adding Inventory item: {errorMessage}";
                    return RedirectToAction("Index");
                }
            }

            TempData["ErrorMessage"] = "Please fill in all fields correctly.";
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> UpdateInventoryItem(InventoryEditModel inventoryItem)
        {
            if (ModelState.IsValid)
            {
                var content = new MultipartFormDataContent();

                content.Add(new StringContent(inventoryItem.InventoryID.ToString()), "InventoryID");
                content.Add(new StringContent(inventoryItem.ItemName), "ItemName");
                content.Add(new StringContent(inventoryItem.QuantityAvailable.ToString()), "QuantityAvailable");
                content.Add(new StringContent(inventoryItem.QuantityWanted.ToString()), "QuantityWanted");

                if (inventoryItem.ImageURL != null)
                {
                    var fileContent = new StreamContent(inventoryItem.ImageURL.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(inventoryItem.ImageURL.ContentType);
                    content.Add(fileContent, "Image", inventoryItem.ImageURL.FileName); // ✅ Fix: Ensure name matches backend
                }
                else
                {
                    content.Add(new StringContent(inventoryItem.ImageString), "ImageURL");
                }

                var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:5128/api/Inventory")
                {
                    Content = content
                };

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data")); // ✅ Fix: Ensure correct content type

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Inventory item updated successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Error updating Inventory item: {errorMessage}";
                    return RedirectToAction("Index");
                }
            }

            TempData["ErrorMessage"] = "Invalid data. Please check the form.";
            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteInventoryItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5128/api/Inventory/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Item deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting item.";
            }
            return RedirectToAction("Index");
        }

    }
}