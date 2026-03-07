using Microservice.Order.Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Persistence.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Domain.Entitites.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Entitites.Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();

            builder.Property(o => o.OrderCode).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Created).IsRequired();
            builder.Property(o => o.BuyerId).IsRequired();
            builder.Property(o => o.Status).IsRequired();
            builder.Property(o => o.AddressId).IsRequired();
            builder.Property(o => o.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(o => o.PaymentId);
            builder.Property(o => o.Discount);

            builder.HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId);



            builder.HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey("OrderId");
              
        }
    }
}
