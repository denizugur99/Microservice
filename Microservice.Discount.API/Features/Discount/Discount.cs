using Microservice.Discount.API.Repositories;

namespace Microservice.Discount.API.Features.Discount
{
    public class Discount: BaseEntity
    {
        public Guid UserId { get; set; }
        public float Rate { get; set; }
        public string Code { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public DateTimeOffset Expire { get; set; }
    }
}
