
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Microservice.Order.Domain.Entitites
{
    public class Order:BaseEntity<Guid>
    {
        public string OrderCode { get; set; }=null!;
        public DateTimeOffset Created { get; set; }
        public Guid BuyerId { get; set; }
        public OrderStatus Status { get; set; }
        public int AddressId { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid? PaymentId { get; set; }
        public float? Discount { get; set; }
        public List<OrderItem> OrderItems { get; set; }=new ();
        public Address Address { get; set; }=null!;
        public static string GenerateCode()
        {
            return Guid.CreateVersion7().ToString();
        }
        public static Order CreateUnPaidOrder(Guid buyerId,float? discount,int addresId)
        {
            return new Order
            {
                Id = Guid.CreateVersion7(),
                OrderCode = GenerateCode(),
                Created = DateTimeOffset.UtcNow,
                BuyerId = buyerId,
                Discount = discount,
                Status = OrderStatus.Pending,
                AddressId = addresId,
                TotalPrice = 0, // This should be calculated based on the order items and discount

            };
        }
        public static Order CreateUnPaidOrder(Guid buyerId, float? discount)
        {
            return new Order
            {
                Id = Guid.CreateVersion7(),
                OrderCode = GenerateCode(),
                Created = DateTimeOffset.UtcNow,
                BuyerId = buyerId,
                Discount = discount,
                Status = OrderStatus.Pending,
                TotalPrice = 0, // This should be calculated based on the order items and discount

            };
        }
        public void AddOrderItem(Guid productId ,string productName,decimal unitPrice)
        {
            var orderItem = new OrderItem();
            
            if(Discount.HasValue)
            {
                unitPrice = unitPrice * (decimal)(1 - Discount.Value / 100);
            }
            orderItem.SetItem(productId, productName, unitPrice);
            OrderItems.Add(orderItem);
            CalculateTotalPrice();
           
        }
       
        public void ApplyDiscount(float discount)
        {
            if (discount < 0 || discount > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(discount), "Discount must be between 0 and 100.");
            }
            this.Discount = discount;
            CalculateTotalPrice();
        }
        public void MarkAsPaid(Guid paymentId)
        {
            if (Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Only pending orders can be marked as paid.");
            }
            Status=OrderStatus.Paid;
            this.PaymentId = paymentId;
           
        }
       private void CalculateTotalPrice()
        {
            TotalPrice = OrderItems.Sum(x => x.UnitPrice);
   
        }
    }


    public enum OrderStatus
    {
        Pending=1,
        Paid=2,
        Cancelled=3,
    }
}
