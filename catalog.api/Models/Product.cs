using System;
using System.Collections.Generic;

namespace catalog.api.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string ProductStatus { get; set; }
    }
}
