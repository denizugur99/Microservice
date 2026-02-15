namespace Microservice.Catalog.Api.Features.Categories.Create
{
    public record CreateCategoryCommand(string categoryName):IrequestByServiceResult<CreateCategoryResponse>;
    
}
