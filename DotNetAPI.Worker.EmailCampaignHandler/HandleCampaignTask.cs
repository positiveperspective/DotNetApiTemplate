using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using DotNetAPI.Core.Common;
using DotNetAPI.Domain.Common.Interfaces;

namespace DotNetAPI.Worker.EmailCampaignHandler;

[DisallowConcurrentExecution]
public class HandleCampaignTask : IJob
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<HandleCampaignTask> _logger;
    private readonly IDateTime _dateTime;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUnitOfWork _unitOfWork;

    public HandleCampaignTask(IConfiguration configuration, ILogger<HandleCampaignTask> logger, IDateTime dateTime, IServiceProvider serviceProvider, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _logger = logger;
        _dateTime = dateTime;
        _serviceProvider = serviceProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Handle campaign started at: {_dateTime.Current}");

    }
}
