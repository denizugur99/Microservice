using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Shared.Filter
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validator == null) {
                return await next(context);
            }
            var requestmodel=context.Arguments.OfType<T>().FirstOrDefault();
            if (requestmodel is null)
            {
                return await next(context);
            }
            var validateResult = await validator.ValidateAsync(requestmodel);
            if (!validateResult.IsValid)
            {
                return Results.ValidationProblem(validateResult.ToDictionary());
            }

           return await next(context);
        }
    }
}
