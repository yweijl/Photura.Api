using System.Net;
using Application.Images;
using Infrastructure.Images;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

const string corsOrigins  = "reactApp";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication("Identity.Application")
    .AddCookie("Identity.Application", options =>
    {
        options.Cookie.Name = ".AspNet.PhoturaApp";
        options.Events.OnRedirectToLogin = async context =>
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(Directory.GetParent(Directory.GetCurrentDirectory())!)
    .SetApplicationName("PhoturaApp");

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsOrigins,
        policy  =>
        {
            policy.WithOrigins("http://localhost:5173");
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();