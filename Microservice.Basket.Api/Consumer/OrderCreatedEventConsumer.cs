using MassTransit;
using Microservice.Basket.Api.Features.Baskets;
using Microservice.Bus.Events;

namespace Microservice.Basket.Api.Consumer
{
    public class OrderCreatedEventConsumer(IServiceProvider serviceProvider) : IConsumer<OrderCreatedEvent>
    {
      
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
         using var scope = serviceProvider.CreateScope();
            var basketService = scope.ServiceProvider.GetRequiredService<BasketService>();
          await  basketService.DeleteBasketAsync(context.Message.UserId);

        }
    }
}
