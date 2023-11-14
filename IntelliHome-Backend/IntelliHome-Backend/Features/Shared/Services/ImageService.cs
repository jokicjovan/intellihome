namespace IntelliHome_Backend.Features.Shared.Services
{
    public class ImageService : IImageService
    {
        public ImageService() { }

        public String SaveDeviceImage(IFormFile image) {
            string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string SavePath = Path.Combine("static/devices", ImageName);
            using (var stream = new FileStream(SavePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }
            return SavePath;
        }
    }
}
