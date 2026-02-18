using MediatR;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microservices.Shared
{
    public interface IrequestByServiceResult<T> : IRequest<ServiceResult<T>>;
    public interface IrequestByServiceResult : IRequest<ServiceResult>;
    
   
    public class ServiceResult
    {
        [JsonIgnore]
        public HttpStatusCode Status { get; set; }
        public Microsoft.AspNetCore.Mvc.ProblemDetails? ProblemDetails { get; set; }
        [JsonIgnore]
        public bool IsSuccess => ProblemDetails == null;
        [JsonIgnore]
        public bool IsFail=>!IsSuccess;

        public static ServiceResult SuccesAsNoContent()
        {
            return new ServiceResult { Status = HttpStatusCode.NoContent };
        }
        public static ServiceResult ErrorAsNotFound()
        {
            return new ServiceResult
            {
                Status = HttpStatusCode.NotFound,
                ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Title = "Not Found",
                    Detail = "The requested resource was not found."
                }
            };
           
        }
        public static ServiceResult ErrorFromProblemDetails(ApiException exception)
        {
            if (string.IsNullOrEmpty(exception.Content))
            {
                return new ServiceResult
                {
                    Status = exception.StatusCode,
                    ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails()
                    {
                        Title = exception.Message
                    },
                };
            }
            var problemDetails = JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(exception.Content, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return new ServiceResult
            {
                Status = exception.StatusCode,
                ProblemDetails = problemDetails
            };
        }
        public static ServiceResult Error(Microsoft.AspNetCore.Mvc.ProblemDetails problemDetails, HttpStatusCode status)
        {
            return new ServiceResult
            {
                Status = status,
                ProblemDetails = problemDetails
            };
        }
        public static ServiceResult Error(string title, string description, HttpStatusCode status)
        {
            return new ServiceResult
            {
                Status = status,
                ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Title = title,
                    Detail = description,
                    Status = status.GetHashCode()
                }
            };
        }
        public static ServiceResult Error(string title, HttpStatusCode status)
        {
            return new ServiceResult
            {
                Status = status,
                ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Title = title,
                    Status = status.GetHashCode()
                }
            };
        }
        public static ServiceResult ErrorFromValidation(IDictionary<string, object?> errors)
        {
            return new ServiceResult
            {
                Status = HttpStatusCode.BadRequest,
                ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Title = "Validation Failed",
                    Detail = "One or more validation errors occurred.",
                    Extensions = errors,
                    Status = HttpStatusCode.BadRequest.GetHashCode()
                }
            };
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        [JsonIgnore]
        public string? UrlAsCreated { get; set; }
        //200
        public static ServiceResult<T> SuccesAsOkay(T data)
        {
            return new ServiceResult<T>
            { Status = HttpStatusCode.OK,
            Data=data};
        }

        //201
        public static ServiceResult<T> SuccesAsCreated(T data,string url)
        {
            return new ServiceResult<T>
            {
                Status = HttpStatusCode.Created,
                Data = data,
                UrlAsCreated=url
            };
        }
        public new static ServiceResult<T> ErrorFromProblemDetails(ApiException exception)
        {
            if (string.IsNullOrEmpty(exception.Content))
            {
                return new ServiceResult<T>
                {
                    Status = exception.StatusCode,
                    ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails()
                    {
                        Title = exception.Message
                    },
                };
            }
            var problemDetails = JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(exception.Content,new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive= true
            });
            return new ServiceResult<T>
            {
                Status = exception.StatusCode,
                ProblemDetails = problemDetails
            };
        }
        public  new static ServiceResult<T> Error(Microsoft.AspNetCore.Mvc.ProblemDetails problemDetails, HttpStatusCode status)
        {
            return new ServiceResult<T>
            {
                Status = status,
                ProblemDetails = problemDetails
            };
        }
        public new static ServiceResult<T> Error(string title,string description, HttpStatusCode status)
        {
            return new ServiceResult<T>
            {
                Status = status,
                ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Title = title,
                    Detail = description,
                    Status=status.GetHashCode()
                }
            };
        }
        public new static ServiceResult<T> Error(string title, HttpStatusCode status)
        {
            return new ServiceResult<T>
            {
                Status = status,
                ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Title = title,
                    Status = status.GetHashCode()
                }
            };
        }
        public new static ServiceResult<T> ErrorFromValidation(IDictionary<string,object?> errors)
        {
            return new ServiceResult<T>
            {
                Status = HttpStatusCode.BadRequest,
                ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Title = "Validation Failed",
                    Detail = "One or more validation errors occurred.",
                    Extensions = errors,
                    Status = HttpStatusCode.BadRequest.GetHashCode()
                }
            };
        }
    }
}
