using Application.Images;

namespace Infrastructure.Images;

public class ImageService : IImageService
{
    public async Task<List<Image>> GetAsync()
    {
        var directory = "/Users/yweijl/Repo/Photura/Photura/public/images";
        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException(directory);
        }

        var files = Directory.GetFiles(directory).Skip(1).Select(Path.GetFileName);
        
        var images = files.Select(file => new Image
        {
            Name = file!,
            Uri = $"http://127.0.0.1:8080/{file}",
        }).ToList();

        return images;
    }

    private string GetContentType(string imagePath)
    {
        // Determine the content type based on the file extension.
        var extension = Path.GetExtension(imagePath).ToLowerInvariant();

        return extension switch
        {
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream", // Default to binary data.
        };
    }
}