using Infrastructure;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Application
{
    public class AppService : IAppService
    {
        private readonly ICrwaler _crwaler;
        private readonly IRepository _repository;
        private readonly ILogger<AppService> _logger;

        public AppService(ICrwaler crwaler, IRepository repository, ILogger<AppService> logger)
        {
            _logger = logger;
            _crwaler = crwaler;
            _repository = repository;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Start the Crawling procedure: ");
            await foreach (var (csv, alphabet) in _crwaler.ExecuteAsync(AppConfig.WebsiteUrl))
            {
                _logger.LogTrace("Repository: " + (char)(65 + alphabet) + " is started.");

                await _repository.StoreAsync(csv, alphabet);

                _logger.LogTrace("Repository: " + (char)(65 + alphabet) + " is completed.");
                _logger.LogInformation($"Repository: {(char)(65 + alphabet)}.csv file is completed.");
            }
        }
    }
}
