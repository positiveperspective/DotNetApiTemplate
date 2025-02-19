using Autofac.Core;
using DotNetAPI.Core;
using DotNetAPI.Infrastructure.Database;
using DotNetAPI.Infrastructure.Database.DatabaseConfig;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment _environment = builder.Environment;
IConfiguration _configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(assemblies);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddApplication();
builder.Services.AddInfrastructureDatabase(_configuration, _environment);
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", builder => {
        //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
        builder//.WithOrigins(new string[] { "http://localhost:55000/", "https://localhost:55000/" })
        .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
          //.WithMethods("*")
          //.WithHeaders("*").AllowAnyHeader()
          //.WithExposedHeaders(new string[] { "X-Pagination", "Access-Control-Allow-Origin", HeaderNames.ContentDisposition, HeaderNames.AccessControlAllowOrigin })
          ;
    });
});

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    try
    {
        IDatabaseService databaseService = scope.ServiceProvider.GetService<IDatabaseService>()!;
        await databaseService.CreateAsync(CancellationToken.None);
    } catch( Exception exception)
    {
        throw;
    }
}
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("DefaultPolicy");

app.Run();
