using Microservice.Catalog.Api.Features.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using System.Reflection.Emit;

namespace Microservice.Catalog.Api.Repositories
{
    public class CourseEntityConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
           builder.ToCollection("Courses")
                .HasKey(c => c.Id);
           builder.Property(x => x.Id).ValueGeneratedNever();
           builder.Property(x => x.Name).HasMaxLength(100);
           builder.Property(x => x.Description).HasElementName("description").HasMaxLength(100);
           builder.Property(x => x.UserId).HasElementName("user_id");
           builder.Property(x => x.Picture).HasElementName("picture");
           builder.Property(x => x.CreatedDate).HasElementName("created_date");
           builder.Property(x => x.CategoryId).HasElementName("category_id");
           builder.OwnsOne(c => c.Feature, f =>
           {
               f.HasElementName("feature");
               f.Property(x => x.Duration).HasElementName("duration");
               f.Property(x => x.Rating).HasElementName("rating");
               f.Property(x => x.EducatorFullName).HasElementName("educator_full_name").HasMaxLength(100);
           });
           builder.Ignore(x => x.Category);
        }
    }
}
