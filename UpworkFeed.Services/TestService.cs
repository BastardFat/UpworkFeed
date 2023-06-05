using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpworkFeed.Core.Options;

namespace UpworkFeed.Services;

public interface ITestInjection
{
    string GetId();
    string GetOption();
}

public class TestInjection : ITestInjection
{
    private readonly ApplicationOptions _options;
    public TestInjection(IOptions<ApplicationOptions> options)
    {
        _options = options.Value;
    }

    private Guid _id = Guid.NewGuid();
    public string GetId() => _id.ToString();
    public string GetOption() => _options.Database.DbName;
}

public class TestService : BackgroundService
{
    private readonly ITestInjection _injection;
    private readonly ILogger<TestService> _logger;

    public TestService(ITestInjection injection, ILogger<TestService> logger)
    {
        _injection = injection;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Ping from worker Id:{Id}, DbName:{DbName}",
                _injection.GetId(),
                _injection.GetOption());

            await Task.Delay(2000, cancellationToken);
        }
    }
}
