namespace RunAsSvc
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Listener? listener;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            try
            {
                listener = new(int.Parse(Environment.GetCommandLineArgs()[1]), _logger);
                _logger.LogInformation("Service started successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Couldn't properly start service; perhaps parameter 'Port' is absent or invalid", ex.Message);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Task.Run(() => listener?.Start(), stoppingToken);
            }
            catch
            {
                Environment.Exit(1);
            }
        }
    }
}