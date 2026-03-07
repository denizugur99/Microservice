using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Domain.Entitites
{
   public class BaseEntity<TEntityId>
    {

        public TEntityId Id { get; set; }
    }
}
