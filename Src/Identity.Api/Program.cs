using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

const string corsOrigins  = "identity";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddAuthentication("Identity.Application")
    .AddCookie("Identity.Application", options =>
    {
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

builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.Name = ".AspNet.PhoturaApp";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsOrigins,
        policy  =>
        {
            policy.WithOrigins("http://localhost:5173");
            policy.WithOrigins("http://localhost:5207");
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapGet("/login", () =>
    {
        return Results.SignIn(new ClaimsPrincipal(new[]
        {
            new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            }, CookieAuthenticationDefaults.AuthenticationScheme),
        }));
    })
    .WithName("Login")
    .WithOpenApi();

app.MapGet("/logout", () => Results.SignOut())
    .WithName("Logout")
    .WithOpenApi();

app.MapGet("/secret", () => Results.Ok("hoi"))
    .RequireAuthorization()
    .WithName("Secret")
    .WithOpenApi();

app.Run();