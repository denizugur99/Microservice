
namespace Microservice.Catalog.Api.Features.Categories.GetAll
{
    public class GetAllCategoryQuery : IrequestByServiceResult<List<CategoryDto>>;
    public class GetAllCategoryHandler(AppDbContext context,IMapper mapper) : IRequestHandler<GetAllCategoryQuery, ServiceResult<List<CategoryDto>>>
    {
        public async Task<ServiceResult<List<CategoryDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = await context.Categories.ToListAsync(cancellationToken);
            var categoryDtos = mapper.Map<List<CategoryDto>>(categories);
            return ServiceResult<List<CategoryDto>>.SuccesAsOkay(categoryDtos);
        }

    }
    public static class GetAllCategoryEndpoint
    {
        public static RouteGroupBuilder ListCategoryGroupItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapGet("/",
                async (IMediator mediator) => (await mediator.Send(new GetAllCategoryQuery())).ToGenericResult());

            return groupBuilder;
        }
    }

}
