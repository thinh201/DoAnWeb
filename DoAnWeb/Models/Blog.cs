using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public string? BlogTitle { get; set; }
        public string? BlogSlug { get; set; }
        public string? BlogDetail { get; set; }
        public string? BlogImage { get; set; }
        public string? BlogDesc { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public int? BlogViewCount { get; set; }

        public virtual User? CreatedBy { get; set; }
    }
}
