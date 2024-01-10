using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public long CustomerId { get; set; }
        public int CarId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalFee { get; set; }
        public int? StatusId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public int? BookingDays { get; set; }
        public int? Quantity { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Email { get; set; }

        public virtual Car Car { get; set; } = null!;
        public virtual User Customer { get; set; } = null!;
        public virtual Status? Status { get; set; }
    }
}
