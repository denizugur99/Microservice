namespace Microservice.Catalog.Api.Features.Categories.GetById
{
    public record GetCategoryByIdQuery(Guid Id) :IrequestByServiceResult<CategoryDto>;
   
    public class GetCategoryByIdHandler(AppDbContext context, IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, ServiceResult<CategoryDto>>
    {
        public async Task<ServiceResult<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        { 
            var hasCategory = await context.Categories.FindAsync(request.Id,cancellationToken);
            if(hasCategory == null)
            {
                return ServiceResult<CategoryDto>.Error("Not Found", $"Category with id {request.Id} not found", System.Net.HttpStatusCode.NotFound);
            } 
            var categorydto = mapper.Map<CategoryDto>(hasCategory);
            return ServiceResult<CategoryDto>.SuccesAsOkay(categorydto);
        }
    }
    public static class GetCategoryByIdEndpoint
    {
        public static RouteGroupBuilder GetByIdCategoryGroupItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapGet("/{id:guid}",
                async (IMediator mediator,Guid id) => (await mediator.Send(new GetCategoryByIdQuery(id))).ToGenericResult()).WithName("getCategoryById");

            return groupBuilder;
        }
    }
}
