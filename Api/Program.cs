using Application.Images;
using Infrastructure.Images;

const string corsOrigins  = "reactApp";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
app.UseAuthorization();

app.MapControllers();

app.Run();