using Microservice.Discount.API.Features.Discount;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Microservice.Discount.API.Repositories
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Microservice.Discount.API.Features.Discount.Discount>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Microservice.Discount.API.Features.Discount.Discount> builder)
        {
            builder.ToCollection("discounts")
                 .HasKey(c => c.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Code).HasMaxLength(100).HasElementName("code");
            builder.Property(x => x.Rate).HasElementName("rate");
            builder.Property(x => x.Created).HasElementName("created");
            builder.Property(x => x.Updated).HasElementName("updated");
            builder.Property(x => x.Expire).HasElementName("expired");
            builder.Property(x => x.UserId).HasElementName("user_id");

        }
    }
}
