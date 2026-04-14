using MassTransit;
using microservice.Email.Services;
using Microservice.Bus.Events;
using Microsoft.Extensions.Logging;

namespace microservice.Email.Consumers
{
    public class OrderNotificationEventConsumer(EmailService emailService, ILogger<OrderNotificationEventConsumer> logger)
        : IConsumer<OrderCreatedNotificationEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedNotificationEvent> context)
        {
            try
            {
                logger.LogInformation("Order notification received for email: {Email}, OrderId: {OrderId}",
                    context.Message.Email, context.Message.OrderId);

                await emailService.SendOrderCreatedEmailAsync(
                    context.Message.Email,
                    context.Message.OrderId.ToString());

                logger.LogInformation("Order notification email sent successfully to {Email}", context.Message.Email);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send order notification email to {Email}", context.Message.Email);
                throw;
            }
        }
    }
}
