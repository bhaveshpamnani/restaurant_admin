using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Theam.Models;

namespace Theam.Controllers
{
    public class ReservationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string reservationApiUrl = "http://localhost:5128/api/Reservation";
        private readonly string tableApiUrl = "http://localhost:5128/api/DinningTable";

        public ReservationController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> ReservationList()
        {
            var reservations = new List<ReservationModel>();
            var tables = new List<DiningTableModel>();

            try
            {
                // Fetch Reservations
                HttpResponseMessage reservationResponse = await _httpClient.GetAsync(reservationApiUrl);
                if (reservationResponse.IsSuccessStatusCode)
                {
                    string reservationData = await reservationResponse.Content.ReadAsStringAsync();
                    reservations = JsonConvert.DeserializeObject<List<ReservationModel>>(reservationData);

                    if (reservations == null || reservations.Count == 0)
                    {
                        ViewBag.Error = "No reservations found from API.";
                    }
                }
                else
                {
                    ViewBag.Error = "Failed to fetch reservations: " + reservationResponse.StatusCode;
                }

                // Fetch Dining Tables
                HttpResponseMessage tableResponse = await _httpClient.GetAsync(tableApiUrl);
                if (tableResponse.IsSuccessStatusCode)
                {
                    string tableData = await tableResponse.Content.ReadAsStringAsync();
                    tables = JsonConvert.DeserializeObject<List<DiningTableModel>>(tableData);
                }

                ViewBag.Tables = tables;
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = "Error fetching data: " + ex.Message;
            }

            // Pass any TempData messages to the view
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            // Return the list of reservations to the view
            return View(reservations);
        }

        public class ReservationEditModel
        {
            public int ReservationID { get; set; }
            public int UserID { get; set; }
            public int? TableID { get; set; }
            public string ReservationStatus { get; set; }
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            ReservationModel reservation = null;
            List<DiningTableModel> tables = new List<DiningTableModel>();

            try
            {
                // Fetch Reservation
                HttpResponseMessage reservationResponse = await _httpClient.GetAsync($"{reservationApiUrl}/{id}");
                if (reservationResponse.IsSuccessStatusCode)
                {
                    string reservationData = await reservationResponse.Content.ReadAsStringAsync();
                    reservation = JsonConvert.DeserializeObject<ReservationModel>(reservationData);
                }

                // Fetch Available Dining Tables
                HttpResponseMessage tableResponse = await _httpClient.GetAsync(tableApiUrl);
                if (tableResponse.IsSuccessStatusCode)
                {
                    string tableData = await tableResponse.Content.ReadAsStringAsync();
                    tables = JsonConvert.DeserializeObject<List<DiningTableModel>>(tableData);
                }

                ViewBag.Tables = new SelectList(tables, "TableID", "TableCode", reservation?.TableID); // Bind the selected table
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = "Error fetching data: " + ex.Message;
            }
            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReservationEditModel reservation)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Request.Form["TableID"]))
                {
                    reservation.TableID = null;
                }

                // Explicitly set null if TableID is empty or zero (optional)
                if (!reservation.TableID.HasValue || reservation.TableID == 0)
                {
                    reservation.TableID = null;
                }

                var json = JsonConvert.SerializeObject(reservation, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include // Ensure nulls are included in JSON
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"{reservationApiUrl}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Reservation updated successfully!";
                    return RedirectToAction("ReservationList");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error updating reservation. Please try again.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid data. Please check the form.";
            }

            return RedirectToAction("ReservationList");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Send DELETE request to the API
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{reservationApiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Reservation deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error deleting reservation. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error occurred while deleting reservation: " + ex.Message;
            }

            // Redirect to the reservation list after deletion
            return RedirectToAction("ReservationList");
        }


    }
}
