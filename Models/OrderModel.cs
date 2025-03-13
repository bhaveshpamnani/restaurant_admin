namespace Theam.Models;

public class OrderModel
{
    public int OrderID { get; set; }
    public int UserID { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; }
    public string OrderStatus { get; set; }
    public string CreatedAt { get; set; }
    public List<OrderDetail> Details { get; set; }
}

public class OrderDetail
{
    public int MenuID { get; set; }
    public int Quantity { get; set; }
    public decimal SubTotal { get; set; }
    public MenuItem Menu { get; set; }
}

public class MenuItem
{
    public string DishName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImgUrl { get; set; }
    public bool AvailabilityStatus { get; set; }
}