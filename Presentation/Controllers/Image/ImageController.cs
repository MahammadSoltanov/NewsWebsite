using Application.CQRS.Images.Commands.CreateImage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Controllers.Image
{
    public class ImageController : Controller
    {
        private readonly IMediator _mediator;

        private const long MaxFileSizeInBytes = 10 * 1024 * 1024; // 10 MB

        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Image/UploadImage")]
        public async Task<JsonResult> UploadImage()
        {
            var uploadedFile = Request.Form.Files[0];

            // Check if a file was actually uploaded
            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                return Json(new { error = "No file uploaded." });
            }

            // Check the file's content type (MIME type)
            if (!IsSupportedContentType(uploadedFile.ContentType))
            {
                return Json(new { error = "Invalid file type." });
            }

            if (uploadedFile.Length > MaxFileSizeInBytes)
            {
                return Json(new { error = "File size exceeds the maximum limit." });
            }

            var randomComponent = Guid.NewGuid().ToString("N").Substring(0, 8); // Generate an 8-character random string
            var uniqueFilename = $"{randomComponent}_{uploadedFile.FileName}";

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFilename);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                uploadedFile.CopyTo(stream);
            }

            // Create a new ImageModel instance with the image URL
            var imageUrl = Url.Content("~/images/" + uniqueFilename);
            
            CreateImageCommand createImageCommand = new CreateImageCommand() { Url = imageUrl };

            await _mediator.Send(createImageCommand);

            return Json(new { Url = imageUrl });
        }

        private bool IsSupportedContentType(string contentType)
        {
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif" };

            return allowedContentTypes.Contains(contentType);
        }
    }
}
