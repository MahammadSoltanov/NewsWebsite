using Application.CQRS.Images.Commands.CreateImage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                var responseFile = Json(new { success = false, error = "No file uploaded." });
                responseFile.ContentType = "application/json";
                return responseFile;
            }

            if (uploadedFile.Length > MaxFileSizeInBytes)
            {
                var responseSize = Json(new { success = false, error = "File size exceeds the maximum limit." });
                responseSize.ContentType = "application/json";
                return responseSize;
            }

            var randomComponent = Guid.NewGuid().ToString("N").Substring(0, 8);
            var uniqueFilename = $"{randomComponent}_{uploadedFile.FileName}";

            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(imagesFolder);

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFilename);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                uploadedFile.CopyTo(stream);
            }

            var imageUrl = Url.Content("~/images/" + uniqueFilename);

            CreateImageCommand createImageCommand = new CreateImageCommand() { Url = imageUrl };

            await _mediator.Send(createImageCommand);

            JsonResult response = Json(new { success = true, Url = imageUrl });
            response.ContentType = "application/json";
            return response;
        }
    }
}
