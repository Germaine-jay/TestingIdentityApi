namespace TestingIdentityApi.Services
{
    public interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
        Task SeedTask();
    }
}
