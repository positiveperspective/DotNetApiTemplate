namespace DotNetAPI.Worker.Quartz;

public class JobSchedule
{
    public Type JobType { get; }

    public string CronExpression { get; }

    public bool RunImmediately { get; }

    public JobSchedule(Type jobType, string cronExpression)
    {
        JobType = jobType;
        CronExpression = cronExpression;
        RunImmediately = cronExpression == "now";
    }
}
