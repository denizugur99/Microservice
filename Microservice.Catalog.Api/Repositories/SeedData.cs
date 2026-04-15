using Microservice.Catalog.Api.Features.Categories;
using Microservice.Catalog.Api.Features.Courses;
using System.Runtime.CompilerServices;

namespace Microservice.Catalog.Api.Repositories
{
    public static class SeedData
    {
        public static async Task AddSeedDataExt(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
               var dbContext=scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
                if (!dbContext.Categories.Any())
                {
                    var categories = new List<Category>
                   {
                       new(){Id=NewId.NextSequentialGuid(),Name="Development"},
                       new(){Id=NewId.NextSequentialGuid(),Name="IT"},
                       new(){Id=NewId.NextSequentialGuid(),Name="Security"},
                       new(){Id=NewId.NextSequentialGuid(),Name="Development2"},
                   };
                     dbContext.Categories.AddRange(categories);
                    await dbContext.SaveChangesAsync();
                }
                if (!dbContext.Courses.Any()) {
                    var category = await dbContext.Categories.FirstAsync();

                    var courses = new List<Course>
                    {
                        new() {
                            Id = NewId.NextSequentialGuid(),
                            Name = "Complete C# Masterclass",
                            Description = "Learn C# from beginner to advanced level with real-world projects",
                            Price = 99.99m,
                            UserId = Guid.Parse("25c81586-0538-4ea2-a095-a9f41abe5585"),
                            ImageUrl = "https://example.com/csharp-course.jpg",
                            CreatedDate = DateTimeOffset.UtcNow,
                            CategoryId = category.Id,
                            Feature = new Feature { Duration = 40, Rating = 4.8f, EducatorFullName = "John Smith" }
                        },
                        new() {
                            Id = NewId.NextSequentialGuid(),
                            Name = "ASP.NET Core Web API Development",
                            Description = "Build production-ready RESTful APIs with ASP.NET Core",
                            Price = 149.99m,
                            UserId = Guid.Parse("fb8e3c4a-92d1-4b5e-a3c2-1d9e8f7a6b5c"),
                            ImageUrl = "https://example.com/aspnet-course.jpg",
                            CreatedDate = DateTimeOffset.UtcNow,
                            CategoryId = category.Id,
                            Feature = new Feature { Duration = 35, Rating = 4.7f, EducatorFullName = "Sarah Johnson" }
                        },
                        new() {
                            Id = NewId.NextSequentialGuid(),
                            Name = "Microservices Architecture with .NET",
                            Description = "Design and build scalable microservices using .NET and Docker",
                            Price = 199.99m,
                            UserId = Guid.Parse("7a3d8e2f-4c9b-4f1a-8e3d-2f9c8b4a1e3d"),
                            ImageUrl = "https://example.com/microservices-course.jpg",
                            CreatedDate = DateTimeOffset.UtcNow,
                            CategoryId = category.Id,
                            Feature = new Feature { Duration = 50, Rating = 4.9f, EducatorFullName = "Michael Chen" }
                        },
                        new() {
                            Id = NewId.NextSequentialGuid(),
                            Name = "SQL Server Database Design",
                            Description = "Master database design, optimization and advanced querying",
                            Price = 89.99m,
                            UserId = Guid.Parse("25c81586-0538-4ea2-a095-a9f41abe5585"),
                            ImageUrl = "https://example.com/sql-course.jpg",
                            CreatedDate = DateTimeOffset.UtcNow,
                            CategoryId = category.Id,
                            Feature = new Feature { Duration = 30, Rating = 4.6f, EducatorFullName = "Emily Davis" }
                        },
                        new() {
                            Id = NewId.NextSequentialGuid(),
                            Name = "Docker and Kubernetes for Developers",
                            Description = "Learn containerization and orchestration for modern applications",
                            Price = 129.99m,
                            UserId = Guid.Parse("9e5f2b8c-3a4d-4e7f-9a2b-8c3d4e5f6a7b"),
                            ImageUrl = "https://example.com/docker-course.jpg",
                            CreatedDate = DateTimeOffset.UtcNow,
                            CategoryId = category.Id,
                            Feature = new Feature { Duration = 45, Rating = 4.7f, EducatorFullName = "David Martinez" }
                        }
                    };

                    dbContext.Courses.AddRange(courses);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}