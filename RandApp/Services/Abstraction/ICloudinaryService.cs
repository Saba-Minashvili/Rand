using System.Threading.Tasks;

namespace RandApp.Services.Abstraction
{
    public interface ICloudinaryService
    {
        Task<bool> UploadPhoto(int id, string fileName);
    }
}
