using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Contracts.Refit.PaymentService
{
    public record GetPaymentStatusResponse(Guid? PaymentId,bool IsPaid);
}
