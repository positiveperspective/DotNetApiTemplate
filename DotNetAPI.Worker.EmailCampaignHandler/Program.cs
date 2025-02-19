using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using NLog.Web;
using DotNetAPI.Core;
using DotNetAPI.Infrastructure.Database;
using DotNetAPI.Worker.EmailCampaignHandler;
using DotNetAPI.Worker.Quartz;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((hostingContext, logging) =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Trace);
    })
    .UseWindowsService()
    //.UseNLog()
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureServices((hostingContext, services) =>
    {
        IConfiguration configuration = hostingContext.Configuration;
        IHostEnvironment environment = hostingContext.HostingEnvironment;

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddMemoryCache();
        services.AddHttpClient();
        services.AddQuartz();
        services.AddApplication();
        services.AddInfrastructureDatabase(configuration, environment);

        services.AddScoped<HandleCampaignTask>();
        services.AddSingleton(new JobSchedule(typeof(HandleCampaignTask), configuration["Quartz:Schedule"]!));
    }).Build();

await host.RunAsync();
