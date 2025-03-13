namespace Theam.Models;

public class ReservationModel
{
    public int ReservationID { get; set; }
    public int UserID { get; set; }
    public int? TableID { get; set; }
    public string BookDate { get; set; }
    public string TableCode { get; set; }
    public string BookTime { get; set; }
    public int PersonCount { get; set; }
    public string ReservationStatus { get; set; }
    public string UserName { get; set; }
}