using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class Status
    {
        public Status()
        {
            Orders = new HashSet<Order>();
        }

        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? StatusDescription { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
