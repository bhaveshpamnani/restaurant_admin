using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Theam.Models;

namespace Theam.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string menuApiUrl = "http://localhost:5128/api/Menu";
        private readonly string categoryApiUrl = "http://localhost:5128/api/Category";

        public MenuItemController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> MenuItemList()
        {
            var response = await _httpClient.GetAsync(menuApiUrl);
            var categoryResponse = await _httpClient.GetAsync(categoryApiUrl);

            if (response.IsSuccessStatusCode && categoryResponse.IsSuccessStatusCode)
            {
                var menuData = await response.Content.ReadAsStringAsync();
                var categoriesData = await categoryResponse.Content.ReadAsStringAsync();

                var menus = JsonConvert.DeserializeObject<List<GetMenuModel>>(menuData);
                var categories = JsonConvert.DeserializeObject<List<CategorySelectModel>>(categoriesData);

                ViewBag.Categories = categories; // Pass categories to the view
                return View(menus);
            }
            else
            {
                TempData["ErrorMessage"] = "Error fetching menu items or categories.";
                return View("Error");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateMenuItem(MenuEditModel menuItem)
        {
            if (ModelState.IsValid)
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(menuItem.MenuID.ToString()), "MenuID");
                content.Add(new StringContent(menuItem.DishName), "DishName");
                content.Add(new StringContent(menuItem.Description), "Description");
                content.Add(new StringContent(menuItem.CategoryID.ToString()), "CategoryID");
                content.Add(new StringContent(menuItem.Price.ToString()), "Price");
                content.Add(new StringContent(menuItem.Rating.ToString()), "Rating");
                if (menuItem.AvailabilityStatus != null)
                {
                    content.Add(new StringContent(menuItem.AvailabilityStatus.ToString()), "AvailabilityStatus");
                }
                else
                {
                    content.Add(new StringContent("false"), "AvailabilityStatus"); // Default value if missing
                }


                if (menuItem.ImagePath != null)
                {
                    var fileContent = new StreamContent(menuItem.ImagePath.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(menuItem.ImagePath.ContentType);
                    content.Add(fileContent, "ImagePath", menuItem.ImagePath.FileName);
                }
                else
                {
                    content.Add(new StringContent(menuItem.ImageString), "ImageURL");
                }

                var response = await _httpClient.PutAsync(menuApiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Menu item updated successfully!";
                    return RedirectToAction("MenuItemList");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Error updating menu item: {errorMessage}";
                    return RedirectToAction("MenuItemList");
                }
            }

            TempData["ErrorMessage"] = "Invalid data. Please check the form.";
            return RedirectToAction("MenuItemList");
        }


        [HttpPost]
        public async Task<IActionResult> AddMenuItem(MenuModel menuItem)
        {
            if (ModelState.IsValid)
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(menuItem.CategoryID.ToString()), "CategoryID");
                content.Add(new StringContent(menuItem.DishName), "DishName");
                content.Add(new StringContent(menuItem.Description), "Description");
                content.Add(new StringContent(menuItem.Price.ToString()), "Price");
                content.Add(new StringContent(menuItem.AvailabilityStatus.ToString()), "AvailabilityStatus");
                content.Add(new StringContent(menuItem.Rating.ToString()), "Rating");

                if (menuItem.ImagePath != null)
                {
                    var fileContent = new StreamContent(menuItem.ImagePath.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(menuItem.ImagePath.ContentType);
                    content.Add(fileContent, "ImagePath", menuItem.ImagePath.FileName);
                }

                var response = await _httpClient.PostAsync(menuApiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Menu item added successfully!";
                    return RedirectToAction("MenuItemList");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Error adding menu item: {errorMessage}";
                    return RedirectToAction("MenuItemList");
                }
            }

            TempData["ErrorMessage"] = "Please fill in all fields correctly.";
            return RedirectToAction("MenuItemList");
        }
        
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"{menuApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MenuItemList");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Error deleting category: {errorMessage}";
                return RedirectToAction("MenuItemList");
            }
        }
    }
    

    public class CategorySelectModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
