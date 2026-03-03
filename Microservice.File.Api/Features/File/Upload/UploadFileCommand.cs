using Microservices.Shared;

namespace Microservice.File.Api.Features.File.Upload
{
    public record UploadFileCommand(IFormFile File) : IrequestByServiceResult<UploadFileCommandResponse>;
    
}
