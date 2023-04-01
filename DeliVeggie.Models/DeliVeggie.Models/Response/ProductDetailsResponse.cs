using System;

namespace DeliVeggie.Models.Response
{
    public class ProductDetailsResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public double PriceWithReduction { get; set; }
    }
}