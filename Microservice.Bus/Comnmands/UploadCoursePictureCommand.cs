using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Bus.Comnmands
{
    public record UploadCoursePictureCommand(Guid courseId, Byte[] picture,string fileName);
   
}
