using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace DotNetAPI.Worker.Quartz;

public class JobExecuter : IJob
{
    private readonly IServiceProvider _serviceProvider;

    public JobExecuter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        Type jobType = context.JobDetail.JobType;
        IJob job = (scope.ServiceProvider.GetRequiredService(jobType) as IJob)!;

        await job.Execute(context);
    }
}
