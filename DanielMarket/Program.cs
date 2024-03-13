using DanielMarket.Models;
using DanielMarket.Services;
using Microsoft.OpenApi.Models;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var settings = new ConnectionSettings(new Uri(Environment.GetEnvironmentVariable("ELASTICSEARCH_URI")))
    .DefaultIndex("products")
    .BasicAuthentication(Environment.GetEnvironmentVariable("ELASTICSEARCH_USERNAME"), Environment.GetEnvironmentVariable("ELASTICSEARCH_PASSWORD"))
    .EnableApiVersioningHeader().DisableDirectStreaming(true)
    .ServerCertificateValidationCallback((o, certificate, chain, errors) => true);


var client = new ElasticClient(settings);
builder.Services.AddSingleton(client);
builder.Services.AddScoped<IElasticSearchService<Product>, ElasticSearchService<Product>>();
builder.Services.AddScoped<IElasticSearchService<Order>, ElasticSearchService<Order>>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Elastic Search API", Version = "v1" });
});

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
