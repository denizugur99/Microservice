using MassTransit;
using MediatR;
using Microservice.Catalog.Api.Repositories;
using Microservices.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Microservice.Catalog.Api.Features.Categories.Create
{
    public class CreateCategoryCommandHandler(AppDbContext context) : IRequestHandler<CreateCategoryCommand, ServiceResult<CreateCategoryResponse>>
    {
        public async Task<ServiceResult<CreateCategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingCategory = await context.Categories.AnyAsync(c => c.Name == request.categoryName, cancellationToken);
            if (existingCategory)
            {
                return ServiceResult<CreateCategoryResponse>.Error("Category Name already exists",$"The category name '{request.categoryName} already exists",HttpStatusCode.BadRequest);
            }
            var category =new Category
            {
                Name = request.categoryName,
                Id= NewId.NextSequentialGuid()
            };
            await context.Categories.AddAsync(category,cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return ServiceResult<CreateCategoryResponse>.SuccesAsCreated(new CreateCategoryResponse(category.Id), "<empty>");
          
        }
    }
}
