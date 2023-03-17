namespace DataAccessLayer.ViewModel
{
    public class OrdersModel
    {        
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCountry { get; set; }
        public string? EmployeeName { get; set; } = string.Empty;
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
    }
}
