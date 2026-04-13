using MassTransit;
using MediatR;
using Microservice.Bus.Events;
using Microservice.Order.Application.Contracts.Refit.PaymentService;
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
    public class CreateOrderCommandHandler(IOrderRepository orderRepository,IGenericRepository<int,Address> addressRepository,IIdentityService identityService,IUnitOfWork unitOfWork,ITopicProducer<OrderCreatedEvent> topicProducer,IPaymentService paymentService) : IRequestHandler<CreateOrderComand, ServiceResult<CreateOrderResponse>>
    {
        public async Task<ServiceResult<CreateOrderResponse>> Handle(CreateOrderComand request, CancellationToken cancellationToken)
        {
            if (!request.Orders.Any()) {
            return ServiceResult<CreateOrderResponse>.Error("Order is empty","Orders can not be empty", HttpStatusCode.BadRequest);

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

            CreatePaymentRequest createPaymentRequest = new CreatePaymentRequest(order.OrderCode, request.Payment.CardNumber, request.Payment.CardName, request.Payment.Expiration, request.Payment.CVV, order.TotalPrice);
            var paymentResponse=await paymentService.CreatePaymentAsync(createPaymentRequest);

            string paymentStatus;
            if (paymentResponse.Status == false)
            {
                paymentStatus = "Failed";
                var response = new CreateOrderResponse(order.Id, paymentStatus);
                return ServiceResult<CreateOrderResponse>.SuccesAsOkay(response);
            }


            order.MarkAsPaid(paymentResponse.PaymentId!.Value);
            orderRepository.Update(order);
            await unitOfWork.CommitAsync(cancellationToken);

            
            paymentStatus = order.Status.ToString();
            await topicProducer.Produce(new OrderCreatedEvent(order.Id,identityService.GetUserId));

            var successResponse = new CreateOrderResponse(order.Id, paymentStatus);
            return ServiceResult<CreateOrderResponse>.SuccesAsOkay(successResponse);
            // Artık order metodlarını çağırabilirsiniz:
            // order.AddOrderItem(productId, productName, unitPrice);
            // order.ApplyDiscount(discount);
            // order.MarkAsPaid(paymentId);


        }
    }
}
