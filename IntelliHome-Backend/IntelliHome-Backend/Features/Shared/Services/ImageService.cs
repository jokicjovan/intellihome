using IntelliHome_Backend.Features.Shared.Exceptions;

namespace IntelliHome_Backend.Features.Shared.Services
{
    public class ImageService : IImageService
    {
        public ImageService() { }

        public String SaveDeviceImage(IFormFile image) {
            if (!IsImageFile(image.FileName))
            {
                throw new InvalidInputException("Invalid file format. Only images are allowed.");
            }

            string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string SavePath = Path.Combine("static/devices", ImageName);
            using (var stream = new FileStream(SavePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }
            return SavePath;
        }

        private bool IsImageFile(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            return allowedExtensions.Contains(extension);
        }
    }
}
