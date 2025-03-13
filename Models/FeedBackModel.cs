namespace Theam.Models;

public class FeedBackModel
{
    public int FeedbackID { get; set; }
    public string Description { get; set; }
    public int UserID { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public int FeedbackCategoryID { get; set; }
    public string CategoryName { get; set; }
}