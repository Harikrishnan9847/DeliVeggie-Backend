using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeliVeggie.Microservice.Data.Entity
{
    public class ProductEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public DateTime EntryDate { get; set; }
        public double Price { get; set; }
    }
}
