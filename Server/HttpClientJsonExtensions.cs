using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScoreTracker.Server
{
    public static class HttpClientJsonExtensions
    {
        public static async Task<T> GetAsync<T>(this HttpClient httpClient, string requestUri)
        {

            var stringContent = await httpClient.GetStringAsync(requestUri);
            return JsonConvert.DeserializeObject<T>(stringContent);
        }
    }
}