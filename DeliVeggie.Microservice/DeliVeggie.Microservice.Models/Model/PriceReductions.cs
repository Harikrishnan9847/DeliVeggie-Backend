using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DeliVeggie.Microservice.Models.Model
{
    public class PriceReductions
    {
        [BsonId]
        public Guid _id {  get; set; }
        public int DayOfWeek { get; set; }
        public double Reduction { get; set; }
    }
}
