using MongoDB.Bson.Serialization.Attributes;

namespace Microservice.Discount.API.Repositories
{
    public class BaseEntity
    {
       
        public Guid Id { get; set; }
    }
}
