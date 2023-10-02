using Application.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ImageController : ControllerBase
{

    private readonly ILogger<ImageController> _logger;
    private readonly IImageService _imageService;

    public ImageController(ILogger<ImageController> logger, IImageService imageService)
    {
        _logger = logger;
        _imageService = imageService;
    }
    
    [HttpGet("/test")]
    public async Task<IActionResult> TestAsync()
    {
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var images = await _imageService.GetAsync();
        return Ok(images);
    }
}