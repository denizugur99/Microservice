using MediatR;
using Microservices.Shared;
using Microsoft.Extensions.FileProviders;
using System.Net;

namespace Microservice.File.Api.Features.File.Upload
{
    public class UploadFileCommandHandler(IFileProvider provider) : IRequestHandler<UploadFileCommand, ServiceResult<UploadFileCommandResponse>>
    {
        public async Task<ServiceResult<UploadFileCommandResponse>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
           if(request.File.Length==0)
           {
                return ServiceResult<UploadFileCommandResponse>.Error("File is empty", HttpStatusCode.BadRequest);
           }
           var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
           var uploadPath = Path.Combine(provider.GetFileInfo("files").PhysicalPath!,newFileName);
            await using var stream=new FileStream(uploadPath, FileMode.Create);

            await request.File.CopyToAsync(stream, cancellationToken);
            var response=new UploadFileCommandResponse(newFileName,$"/files/{newFileName}",request.File.FileName);
            return ServiceResult<UploadFileCommandResponse>.SuccesAsCreated(response,$"/files/{newFileName}");
        }
    }
}
