using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Domain.Entitites
{
    public class Address:BaseEntity<int>
    {
        public string Province { get; set; }=null!;
        public string City { get; set; }=null!;
        public string Region { get; set; }=null !;
        public string PostalCode { get; set; } =null!;
        public string Street { get; set; }=null!;

    }
}
