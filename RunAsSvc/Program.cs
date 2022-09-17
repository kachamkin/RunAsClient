using Microsoft.Extensions.Logging.EventLog;
using RunAsSvc;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(options =>
        options.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information)
    )
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.Configure<EventLogSettings>(config =>
            config.SourceName = "Run As Service"
        );
    })
    .UseWindowsService(options =>
        options.ServiceName = "Run As Service"
    )
    .Build();

await host.RunAsync();
