namespace DoAnWeb.Models.ViewModel;

public class CreateOrderModel
{
    public int CarId { get; set; }
    public int? Quantity { get; set; }
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool UseDefaultInfo { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}