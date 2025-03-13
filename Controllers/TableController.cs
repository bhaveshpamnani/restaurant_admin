using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Theam.Models;

namespace Theam.Controllers
{
    public class TableController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string apiUrl = "http://localhost:5128/api/DinningTable";

        public TableController()
        {
            _httpClient = new HttpClient();
        }

        // GET: /Table/TableList
        public async Task<IActionResult> TableList()
        {
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var tables = JsonConvert.DeserializeObject<List<DiningTableModel>>(data);
                return View(tables);
            }
            else
            {
                return View("Error");
            }
        }

        // POST: /Table/AddDinningTable
        [HttpPost]
        public async Task<IActionResult> AddDinningTable(DiningTableModel table)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(table), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Dining table added successfully!";
                    return RedirectToAction("TableList");
                }
                else
                {
                    TempData["ErrorMessage"] = "There was an issue adding the dining table. Please try again.";
                    return RedirectToAction("TableList");
                }
            }

            TempData["ErrorMessage"] = "Please fill in all the fields correctly.";
            return RedirectToAction("TableList");
        }

// GET: /Table/EditDinningTable/{id}
    public async Task<IActionResult> EditDinningTable(int id)
    {
        try
        {
            // Fetch the dining table details by ID
            HttpResponseMessage response = await _httpClient.GetAsync($"{apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string tableData = await response.Content.ReadAsStringAsync();
                var table = JsonConvert.DeserializeObject<DiningTableModel>(tableData);

                if (table == null)
                {
                    TempData["ErrorMessage"] = "Dining table not found.";
                    return RedirectToAction("TableList");
                }

                return View(table);
            }
            else
            {
                TempData["ErrorMessage"] = "Error fetching dining table details.";
                return RedirectToAction("TableList");
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            return RedirectToAction("TableList");
        }
    }

    // POST: /Table/UpdateDinningTable/{id}
    [HttpPost]
    public async Task<IActionResult> UpdateDinningTable(DiningTableModel table)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Serialize the updated table object
                var json = JsonConvert.SerializeObject(table);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the PUT request to update the dining table
                HttpResponseMessage response = await _httpClient.PutAsync($"{apiUrl}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Dining table updated successfully!";
                    return RedirectToAction("TableList");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error updating dining table. Please try again.";
                    return RedirectToAction("TableList");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("TableList");
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Invalid data. Please check the form.";
            return RedirectToAction("TableList");
        }
    }


        // GET: /Table/DeleteDinningTable/{id}
        public async Task<IActionResult> DeleteDinningTable(int id)
        {
            var response = await _httpClient.DeleteAsync($"{apiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("TableList");
            }
            else
            {
                TempData["Error"] = "There was an issue deleting the dining table. Please try again.";
                return RedirectToAction("TableList");
            }
        }
    }
}
