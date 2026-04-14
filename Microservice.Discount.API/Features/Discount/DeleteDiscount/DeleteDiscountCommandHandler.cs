using Microservice.Discount.API.Repositories;
using Microservices.Shared.Services;

namespace Microservice.Discount.API.Features.Discount.DeleteDiscount
{
    public class DeleteDiscountCommandHandler(AppDbContext context)
        : IRequestHandler<DeleteDiscountCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
           

            var discount = await context.Discounts
                .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken);

            if (discount is null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            context.Discounts.Remove(discount);
            await context.SaveChangesAsync(cancellationToken);

            return ServiceResult.SuccesAsNoContent();
        }
    }
}
