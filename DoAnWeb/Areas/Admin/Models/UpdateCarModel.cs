namespace DoAnWeb.Areas.Admin.Models
{
    public class UpdateCarModel
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public string? Color { get; set; }
        public decimal? PricePerDay { get; set; }
        public string? Description { get; set; }
        public List<IFormFile>? CarImages { get; set; }
    }
}