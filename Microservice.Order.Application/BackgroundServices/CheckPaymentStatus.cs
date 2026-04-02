using Microservice.Order.Application.Contracts.Refit.PaymentService;
using Microservice.Order.Application.Contracts.Repositories;
using Microservice.Order.Application.Contracts.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.BackgroundServices
{
    public class CheckPaymentStatus(IServiceProvider serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope=serviceProvider.CreateScope())
            {
                var paymentService=scope.ServiceProvider.GetRequiredService<IPaymentService>();
                var orderRepository=scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var unitOfWork=scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                while (stoppingToken.IsCancellationRequested)
                {
                    var orders = orderRepository.Where(x => x.Status == Domain.Entitites.OrderStatus.Pending).ToList();
                    foreach (var order in orders)
                    {
                        var paymentStatus = await paymentService.GetPaymentAsync(order.OrderCode);
                        if (paymentStatus.IsPaid)
                        {
                            await orderRepository.SetStatus(order.OrderCode, paymentStatus.PaymentId!.Value, Domain.Entitites.OrderStatus.Paid);
                            await unitOfWork.CommitAsync(stoppingToken);

                        }
                    }
                    await Task.Delay(2000, stoppingToken);

                }
               
            }
        }
    }
}
