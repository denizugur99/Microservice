using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Bus.Events
{
    public record CoursePictureUploadedEvent(Guid CourseId, string ImageUrl);
    
}
