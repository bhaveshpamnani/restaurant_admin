namespace Theam.Models
{
    public class DashBoardViewModel
    {
        public List<DashboardMetric> Counts { get; set; } = new List<DashboardMetric>();
        public List<InventoryItem> Inventory { get; set; }
    }

    public class DashboardMetric
    {
        public string Metric { get; set; }
        public int Value { get; set; }
    }
    public class InventoryItem
    {
        public int InventoryID { get; set; }
        public string ItemName { get; set; }
        public double QuantityAvailable { get; set; }
        public double QuantityWanted { get; set; }
        public string ImageURl { get; set; }
    }
}
