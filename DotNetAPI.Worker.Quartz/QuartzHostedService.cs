using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace DotNetAPI.Worker.Quartz;

public class QuartzHostedService : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IJobFactory _jobFactory;
    private readonly IEnumerable<JobSchedule> _jobSchedules;

    public IScheduler Scheduler { get; set; } = default!;

    public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, IEnumerable<JobSchedule> jobSchedules)
    {
        _schedulerFactory = schedulerFactory;
        _jobFactory = jobFactory;
        _jobSchedules = jobSchedules;
    }

    private static IJobDetail CreateJob(JobSchedule schedule)
    {
        Type jobType = schedule.JobType;

        return JobBuilder
            .Create(jobType)
            .WithIdentity(jobType.FullName!)
            .WithDescription(jobType.Name)
            .Build();
    }

    private static ITrigger CreateTrigger(JobSchedule schedule)
    {
        TriggerBuilder trigger = TriggerBuilder.Create()
            .WithIdentity($"{schedule.JobType.FullName}.trigger")
            .WithDescription(schedule.CronExpression);

        if(schedule.RunImmediately)
        {
            trigger.StartNow();
        }
        else
        {
            trigger.WithCronSchedule(schedule.CronExpression);
        }

        return trigger.Build();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        Scheduler.JobFactory = _jobFactory;

        foreach(JobSchedule jobSchedule in _jobSchedules)
        {
            IJobDetail job = CreateJob(jobSchedule);
            ITrigger trigger = CreateTrigger(jobSchedule);

            await Scheduler.ScheduleJob(job, trigger, cancellationToken);
        }

        await Scheduler.Start(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Scheduler?.Shutdown(cancellationToken)!;
    }
}
