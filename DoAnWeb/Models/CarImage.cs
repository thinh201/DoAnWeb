using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class CarImage
    {
        public int? CarId { get; set; }
        public string? ImageUrl { get; set; }
        public int ImageId { get; set; }

        public virtual Car? Car { get; set; }
    }
}
