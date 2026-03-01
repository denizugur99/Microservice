using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace Microservice.Discount.API.Options
{
    public class MongoOptions
    {
        [Required]
        public string DatabaseName { get; set; }=default!;
        [Required]
        public string ConnectionString { get; set; }=default!;
    }
}
