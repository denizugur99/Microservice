using MicroserviceWebApp.Options;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MicroserviceWebApp.TagHelpers
{
    public class CoursePictureTagHelper(MicroserviceOptions microserviceOptions): TagHelper
    {
        public string? Src { get; set; } 
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            var blankCourseThumbnailImages = "/images/course_thumbnail.png";
            if(string.IsNullOrEmpty(Src))
            {
                output.Attributes.SetAttribute("src", blankCourseThumbnailImages);
            }
            else
            {
                var path=$"{microserviceOptions.File.BaseAddress}/{Src}";
                output.Attributes.SetAttribute("src", path);
            }
            return base.ProcessAsync(context, output);
        }
    }
}
