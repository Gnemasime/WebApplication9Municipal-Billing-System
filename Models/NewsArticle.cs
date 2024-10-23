namespace WebApplication9Municipal_Billing_System.Models
{
    public class NewsArticle
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public string PublishedAt { get; set; }
        public string Source { get; set; }
    }

    public class NewsResponse
    {
        public List<NewsArticle> Data { get; set; }
    }
}
