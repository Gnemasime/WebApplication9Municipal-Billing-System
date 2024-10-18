using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication9Municipal_Billing_System.Models;

namespace WebApplication9Municipal_Billing_System.Services
{
    public class NewsService
    {
        private readonly HttpClient _httpClient;
        private const string AccessKey = "2ff337bf331d8947a5493c2d45fd61eb"; // Replace with your actual API key
        private const string BaseUrl = "http://api.mediastack.com/v1/news";

        public NewsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(List<NewsArticle> articles, string errorMessage)> GetNewsAsync()
        {
            var url = $"{BaseUrl}?access_key={AccessKey}&countries=za&languages=en&keywords=electricity&limit=27&sort=published_desc";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var newsResponse = JsonSerializer.Deserialize<NewsResponse>(response);

                if (newsResponse?.Data != null && newsResponse.Data.Count > 0)
                {
                    return (newsResponse.Data, null);
                }

                return (null, "No relevant articles found.");
            }
            catch (Exception ex)
            {
                return (null, $"Error fetching news: {ex.Message}");
            }
        }
    }
}
