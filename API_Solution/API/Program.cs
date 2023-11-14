using API_Models;
using API_Models.Models;
using Nest;
using System.Text.Json.Serialization;

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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
