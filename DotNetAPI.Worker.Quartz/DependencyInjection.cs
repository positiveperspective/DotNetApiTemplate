using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace DotNetAPI.Worker.Quartz;

public static class DependencyInjection
{
    public static IServiceCollection AddQuartz(this IServiceCollection services)
    {
        services.AddHostedService<QuartzHostedService>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.AddSingleton<IJobFactory, JobFactory>();
        services.AddSingleton<JobExecuter>();

        return services;
    }
}
