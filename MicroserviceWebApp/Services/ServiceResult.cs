using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MicroserviceWebApp.Services
{
    public class ServiceResult
    {
      
        public ProblemDetails? ProblemDetails { get; set; }
        [JsonIgnore]
        public bool IsSuccess => ProblemDetails == null;
        [JsonIgnore]
        public bool IsFail => !IsSuccess;

        public static ServiceResult Succest()
        {
            return new ServiceResult();
        }

       

        public static ServiceResult Error(ProblemDetails problemDetails)
        {
            return new ServiceResult
            {
               
                ProblemDetails = problemDetails
            };
        }

        public static ServiceResult Error(string title, string description)
        {
            return new ServiceResult
            {
               
                ProblemDetails = new ProblemDetails
                {
                    Title = title,
                    Detail = description,
                    
                }
            };
        }

        public static ServiceResult Error(string title)
        {
            return new ServiceResult
            {
                
                ProblemDetails = new ProblemDetails
                {
                    Title = title,
                   
                }
            };
        }

      
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

   

        //200
        public static ServiceResult<T> SuccesAsOkay(T data)
        {
            return new ServiceResult<T>
            {
                
                Data = data
            };
        }

    

        public new static ServiceResult<T> Error(ProblemDetails problemDetails)
        {
            return new ServiceResult<T>
            {
               
                ProblemDetails = problemDetails
            };
        }

        public new static ServiceResult<T> Error(string title, string description)
        {
            return new ServiceResult<T>
            {
               
                ProblemDetails = new ProblemDetails
                {
                    Title = title,
                    Detail = description,
                   
                }
            };
        }

        public new static ServiceResult<T> Error(string title)
        {
            return new ServiceResult<T>
            {
                
                ProblemDetails = new ProblemDetails
                {
                    Title = title,
                   
                }
            };
        }

       
    }
}
