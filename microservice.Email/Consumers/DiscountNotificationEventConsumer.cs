using MassTransit;
using microservice.Email.Services;
using Microservice.Bus.Events;
using Microsoft.Extensions.Logging;

namespace microservice.Email.Consumers
{
    public class DiscountNotificationEventConsumer(EmailService emailService, ILogger<DiscountNotificationEventConsumer> logger)
        : IConsumer<DiscountNotificationEvent>
    {
        public async Task Consume(ConsumeContext<DiscountNotificationEvent> context)
        {
            try
            {
                logger.LogInformation("Discount notification received for email: {Email}, Code: {Code}",
                    context.Message.Email, context.Message.Code);

                await emailService.SendDiscountNotificationEmailAsync(
                    context.Message.Email,
                    context.Message.Code);

                logger.LogInformation("Discount notification email sent successfully to {Email}", context.Message.Email);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send discount notification email to {Email}", context.Message.Email);
                throw;
            }
        }
    }
}
