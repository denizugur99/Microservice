using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Domain.Entitites
{
    //anemic model=> rich model
    public class OrderItem:BaseEntity<int>
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }=null!;
        public decimal UnitPrice { get; set; }

        //todo : bu silinicek
        public Guid Guid { get; set; }
        public Order Order { get; set; }=null!;

        public void SetItem(Guid productId, string productName, decimal unitPrice)
        {
          if(string.IsNullOrEmpty(productName))
          {
            throw new ArgumentException("Product name cannot be null or empty.", nameof(productName));
          }
          if(unitPrice <= 0)
          {
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");
            }
            this.ProductId = productId;
          this.ProductName = productName;
          this.UnitPrice = unitPrice;
        }
        public void UpdatePrice(decimal newPrice)
        {
            if(newPrice <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newPrice), "Unit price cannot be negative.");
            }
            this.UnitPrice = newPrice;
        }
        public void ApplyDiscount(float discount) {
            if (discount < 0 || discount > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(discount), "Discount must be between 0 and 100.");
            }
            this.UnitPrice = this.UnitPrice * (decimal)( discount / 100);
        }
        public bool IsSameItem(OrderItem other)
        {
            if (other == null) return false;
            return this.ProductId == other.ProductId;
        }
      
    }
}
