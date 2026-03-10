using MediatR;
using Microservice.Order.Application.Contracts.Repositories;
using Microservice.Order.Application.Contracts.UnitOfWorks;
using Microservice.Order.Domain.Entitites;
using Microservices.Shared;
using Microservices.Shared.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Microservice.Order.Application.Features.Orders.Create
{
    public class CreateOrderCommandHandler(IOrderRepository orderRepository,IGenericRepository<int,Address> addressRepository,IIdentityService identityService,IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderComand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateOrderComand request, CancellationToken cancellationToken)
        {
            if (!request.Orders.Any()) {
            return ServiceResult.Error("Order is empty","Orders can not be empty", HttpStatusCode.BadRequest);

            }
           
            //TODO: transaction başlatılıcak
            var newAdress = new Address
            {
                Province = request.Address.Province,
                City = request.Address.City,
                Region = request.Address.Region,
                Street = request.Address.Street,
                PostalCode = request.Address.PostalCode,
            };
           
           

            var buyerId = identityService.GetUserId;
            var order = Domain.Entitites.Order.CreateUnPaidOrder(buyerId, request.Discount, newAdress.Id);
            foreach (var item in request.Orders)
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice);
            }
            order.Address = newAdress;
          

            await orderRepository.AddAsync(order);
            await unitOfWork.CommitAsync(cancellationToken);
            var paymentId = Guid.Empty;

            //payment
            order.MarkAsPaid(paymentId);
            orderRepository.Update(order);
            await unitOfWork.CommitAsync(cancellationToken);



            return ServiceResult.SuccesAsNoContent();
            // Artık order metodlarını çağırabilirsiniz:
            // order.AddOrderItem(productId, productName, unitPrice);
            // order.ApplyDiscount(discount);
            // order.MarkAsPaid(paymentId);


        }
    }
}
