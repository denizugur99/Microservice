using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Microservices.Shared.Extensions
{
    public static class EndpointResultExt
    {
        public static IResult ToGenericResult<T>(this ServiceResult<T> serviceResult)
        {
            return serviceResult.Status switch
            {
                HttpStatusCode.OK => Results.Ok(serviceResult.Data),
                HttpStatusCode.Created => Results.Created(serviceResult.UrlAsCreated, serviceResult.Data),
                HttpStatusCode.NotFound => Results.NotFound(serviceResult.ProblemDetails!),
                _ => Results.Problem(serviceResult.ProblemDetails!),
            };
        }
        public static IResult ToGenericResult(this ServiceResult serviceResult)
        {
            return serviceResult.Status switch
            {
          
                HttpStatusCode.NoContent => Results.NoContent(),
                HttpStatusCode.NotFound => Results.NotFound(serviceResult.ProblemDetails!),
                _ => Results.Problem(serviceResult.ProblemDetails!),
            };
        }
    }
}
