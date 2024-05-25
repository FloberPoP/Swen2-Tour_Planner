using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Tour_Planner.BL
{
    public class RouteService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public RouteService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public JObject GetRoute(string from, string to)
        {
            var response = _httpClient.GetAsync($"https://api.openrouteservice.org/v2/directions/driving-car?api_key={_apiKey}&start={from}&end={to}").Result;
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            return JObject.Parse(content);
        }
    }
}
