using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Theam.Models;

namespace Theam.Controllers
{
    public class ManageReviewController : Controller
    {
        private readonly HttpClient _httpClient;

        public ManageReviewController()
        {
            _httpClient = new HttpClient();
        }

        // Fetch and display feedback
        public async Task<IActionResult> ManageReviewI()
        {
            List<FeedBackModel> feedbackList = new List<FeedBackModel>();

            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5128/api/Feedback");

            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                feedbackList = JsonConvert.DeserializeObject<List<FeedBackModel>>(jsonData);
            }

            return View(feedbackList);
        }

        // Delete feedback by ID
        [HttpPost]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:5128/api/Feedback/{id}");

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete feedback" });
            }
        }
    }
}