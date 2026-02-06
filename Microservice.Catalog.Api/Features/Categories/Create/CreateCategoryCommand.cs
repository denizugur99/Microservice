using MediatR;
using Microservices.Shared;

namespace Microservice.Catalog.Api.Features.Categories.Create
{
    public record CreateCategoryCommand(string categoryName):IRequest<ServiceResult<CreateCategoryResponse>>;
    
}
