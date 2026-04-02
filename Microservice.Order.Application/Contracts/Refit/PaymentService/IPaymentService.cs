using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Contracts.Refit.PaymentService
{
    public interface IPaymentService
    {
        [Post("/api/v1/payments")]
        Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);
        [Get("/api/v1/payments/status/{orderCode}")]
        Task<GetPaymentStatusResponse> GetPaymentAsync(string orderCode);
    }
}
