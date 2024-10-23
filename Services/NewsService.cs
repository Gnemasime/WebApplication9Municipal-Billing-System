using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApplication9Municipal_Billing_System.Services
{
    public class NewsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<NewsService> _logger;

        public NewsService(HttpClient httpClient, IConfiguration configuration, ILogger<NewsService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GNewsApiKey"]; // Use configuration for the API key
            _logger = logger;
        }

        public async Task<List<NewsArticle>> GetElectricityNewsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://gnews.io/api/v4/search?q=electricity+South+Africa&token={_apiKey}", cancellationToken);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var newsData = JsonConvert.DeserializeObject<NewsResponse>(json);
                return newsData?.Articles ?? new List<NewsArticle>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching electricity news");
                return new List<NewsArticle>(); // Return an empty list on error
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing news data");
                return new List<NewsArticle>();
            }
        }

        internal async Task<string?> GetNewsAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class NewsResponse  
    {  
        public List<NewsArticle> Articles { get; set; }  
    }  

    public class NewsArticle  
    {  
        public string Title { get; set; }  
        public string Description { get; set; }  
        public string Url { get; set; }  
        public string Source { get; set; }  
    }  
}
