using MassTransit;
using Microservice.Bus.Events;
using Microservice.Discount.API.Features.Discount;
using Microservice.Discount.API.Repositories;

namespace Microservice.Discount.API.Consumer
{
    public class OrderCreatedEventConsumer(IServiceProvider serviceProvider,ITopicProducer<DiscountNotificationEvent> topicProducer) : IConsumer<OrderCreatedEvent>
    {


        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            using var scope = serviceProvider.CreateScope();
            var appdbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var discount = new Discount.API.Features.Discount.Discount
            {
                Id = NewId.NextSequentialGuid(),
                Code = DiscountCodeGenerator.GenerateCode(),
                Created = DateTimeOffset.UtcNow,
                Expire = DateTimeOffset.UtcNow.AddMonths(1),
                UserId = context.Message.UserId,
                Rate = 0.1f

            };
            var notificationEvent = new DiscountNotificationEvent(context.Message.Email, discount.Code);
            await topicProducer.Produce(notificationEvent);
           
            await appdbContext.Discounts.AddAsync(discount);
            await appdbContext.SaveChangesAsync();

        }
    }
}
