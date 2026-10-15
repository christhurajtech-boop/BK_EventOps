using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
//Scalar API Reference UI is available at the root path of the application
app.MapScalarApiReference("/", options =>
{
    options.WithTitle("Hospital Management API");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();