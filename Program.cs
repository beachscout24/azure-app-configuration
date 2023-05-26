global using azure_app_configuration.Settings;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
string? connectionString = builder.Configuration.GetConnectionString("AzureAppConfiguration");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<Settings>(builder.Configuration.GetSection("appsettings"));
builder.Configuration.AddAzureAppConfiguration(connectionString);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
