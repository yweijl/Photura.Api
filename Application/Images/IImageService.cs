namespace Application.Images;

public interface IImageService
{
    Task<List<Image>> GetAsync();
}