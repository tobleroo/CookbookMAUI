using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCookbook.Utils
{
    public static class ImageService
    {
        public static async Task<string> PickImage()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    return await ConvertToBase64(stream);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PickImage: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> TakePhoto()
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    return await ConvertToBase64(stream);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TakePhoto: {ex.Message}");
                return null;
            }
        }

        private static async Task<string> ConvertToBase64(Stream stream)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ConvertToBase64: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> SaveImageToLocalStorage(string base64Image, string fileName)
        {
            try
            {
                var bytes = Convert.FromBase64String(base64Image);
                var filePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, fileName);
                await File.WriteAllBytesAsync(filePath, bytes);
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> LoadImageFromStorage(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    byte[] imageBytes = await File.ReadAllBytesAsync(path);
                    return Convert.ToBase64String(imageBytes);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                return null;
            }
        }

        public static async Task<bool> DeleteImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                    Console.WriteLine("image deleted!");
                    return true; // Successfully deleted
                }
                return false; // File does not exist or path is null/empty
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image: {ex.Message}");
                return false; // Error occurred during deletion
            }
        }
    }
}
