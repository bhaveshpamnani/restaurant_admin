using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using Theam.Models;

namespace Theam.Controllers;

public class CategoryController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string apiUrl = "http://localhost:5128/api/Category";

    public CategoryController()
    {
        _httpClient = new HttpClient();
    }

    // GET: /Category/CategoryList
    public async Task<IActionResult> CategoryList()
    {
        var response = await _httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CategoryGetModel>>(data);

            return View(categories);
        }
        else
        {
            TempData["ErrorMessage"] = "Error fetching categories.";
            return View("Error");
        }
    }


    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryModel category)
    {
        if (ModelState.IsValid)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(category.CategoryName), "CategoryName");
            content.Add(new StringContent(category.Description), "Description");

            if (category.ImageFile != null)
            {
                var fileContent = new StreamContent(category.ImageFile.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(category.ImageFile.ContentType);
                content.Add(fileContent, "ImagePath", category.ImageFile.FileName);  // Correct key as ImageFile
            }


            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Category added successfully!";
                return RedirectToAction("CategoryList");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Error adding category: {errorMessage}";
                return RedirectToAction("CategoryList");
            }
        }

        TempData["ErrorMessage"] = "Please fill in all fields correctly.";
        return RedirectToAction("CategoryList");
    }





    // GET: /Category/EditCategory/{id}
    public async Task<IActionResult> EditCategory(int id)
    {
        var response = await _httpClient.GetAsync($"{apiUrl}/{id}");
        if (response.IsSuccessStatusCode)
        {
            var categoryData = await response.Content.ReadAsStringAsync();
            var category = JsonConvert.DeserializeObject<CategoryModel>(categoryData);
            return View("CategoryList",category);
        }
        TempData["ErrorMessage"] = "Error fetching category details.";
        return RedirectToAction("CategoryList");
    }
    [HttpPost]
    public async Task<IActionResult> UpdateCategory(CategoryModel category)
    {
        if (ModelState.IsValid)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(category.CategoryID.ToString()), "CategoryID");
            content.Add(new StringContent(category.CategoryName), "CategoryName");
            content.Add(new StringContent(category.Description), "Description");

            // If an image file is selected, replace the old one
            if (category.ImageFile != null)
            {
                var fileContent = new StreamContent(category.ImageFile.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(category.ImageFile.ContentType);
                content.Add(fileContent, "ImagePath", category.ImageFile.FileName);
            }
            else
            {
                content.Add(new StringContent(category.ImgPath), "ImagePath");
            }

            var response = await _httpClient.PutAsync($"{apiUrl}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction("CategoryList");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Error updating category: {errorMessage}";
                return RedirectToAction("CategoryList");
            }
        }

        TempData["ErrorMessage"] = "Invalid data. Please check the form.";
        return RedirectToAction("CategoryList");
    }



    public async Task<IActionResult> DeleteCategory(int id)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl}/{id}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("CategoryList");
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = $"Error deleting category: {errorMessage}";
            return RedirectToAction("CategoryList");
        }
    }

}
