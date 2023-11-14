using API_Models.Models;
using Nest;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddDbContext<BackendSrContext>();

builder.Services.AddSingleton<IElasticClient>(x =>
{
    var configuration = x.GetRequiredService<IConfiguration>();
    Uri uri = new(configuration["Elasticsearch:Uri"]);
    var settings = new ConnectionSettings(uri).DefaultIndex("apiindex");
    return new ElasticClient(settings);
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel
    .Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) 
    .CreateLogger();

builder.Services.AddSingleton(Log.Logger);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Reto Backend Sr", Version = "v1", Contact = new OpenApiContact { Name = "Gustavo Fernández Saavedra", Email = "soyGustavoFernandez@gmail.com", Url = new Uri("https://www.linkedin.com/in/gustavofernandezsaavedra") } });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"); });

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();