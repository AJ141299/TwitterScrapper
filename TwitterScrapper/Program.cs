using TwitterScrapper.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
       .WithOrigins("http://localhost:4200", "https://localhost:4200")
       .AllowCredentials()
       .AllowAnyMethod()
       .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
