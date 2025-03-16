using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ISC_ELIB_SERVER.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        // Hàm upload ảnh
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Quality(80).FetchFormat("jpg")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.AbsoluteUri;
        }

        public void DeleteImage(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) return;

        try
        {
            var publicId = GetPublicIdFromUrl(imageUrl);
            var deletionParams = new DeletionParams(publicId);
            _cloudinary.Destroy(deletionParams);
            // Console.WriteLine($"Xóa thành công ảnh: {imageUrl}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Lỗi khi xóa ảnh {imageUrl}: {ex.Message}");
        }
    }

    // 🔹 Lấy `public_id` từ URL ảnh Cloudinary
    private string GetPublicIdFromUrl(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        var filename = System.IO.Path.GetFileNameWithoutExtension(uri.AbsolutePath);
        return filename;
    }
    }
}
