using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using RandApp.Models;
using RandApp.Repositories.Abstraction;
using RandApp.Services.Abstraction;
using System.Linq;
using System.Threading.Tasks;

namespace RandApp.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly IRepository<Item> _itemRepo = default;
        private readonly Cloudinary _cloud = new Cloudinary(new CloudinaryDotNet.Account("", "", ""));
        private readonly string _cloudinaryBaseImageUrl = "https://res.cloudinary.com/dkfjpuddb/image/upload/";

        public CloudinaryService(IRepository<Item> itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public async Task<bool> UploadPhoto(int itemId, string fileName)
        {
            var item = await _itemRepo.ReadByIdAsync(itemId);
            try
            {
                var imageLocation = fileName;
                var imageUpload = new ImageUploadParams()
                {
                    File = new FileDescription(imageLocation),
                    PublicId = imageLocation.Split('\\').LastOrDefault().Split('.').FirstOrDefault()
                };

                var cloudinaryFullImageUrl = $"{_cloudinaryBaseImageUrl}{imageLocation.Split('\\').LastOrDefault()}";
                _cloud.Upload(imageUpload);

                item.ItemPhoto = cloudinaryFullImageUrl;
                return await _itemRepo.UpdateAsync(item);
            }
            catch
            {
                return false;
            }
        }
    }
}
