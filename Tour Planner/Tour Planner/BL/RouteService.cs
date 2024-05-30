using System.Net.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tour_Planner.Models;

namespace Tour_Planner.BL
{
    public class RouteService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "5b3ce3597851110001cf6248d157c6099b64438e81765a43f9ecc90b";
        private readonly string _urlSearchGeocode = "https://api.openrouteservice.org/geocode/search";
        private readonly string _urlDirections = "https://api.openrouteservice.org/v2/directions/";

        public RouteService()
        {
            _httpClient = new HttpClient();
        }

        static Dictionary<string, string> TransportDictionary = new()
        {
            {"Walk", "foot-walking"},
            {"Bike", "cycling-regular"},
            {"Car", "driving-car"}
        };

        /*
        public async Task<(double Distance, double Duration, string MapImageUrl)> GetRouteInfo(string address1, string address2, string transportation)
        {
            try
            {
                var locationA = await GetGeoAddress(address1);
                var locationB = await GetGeoAddress(address2);
            }

            catch
            {

            }
        }
        */

        private async Task<List<double>> GetGeoAddress(string address)
        {
            try
            {
                string requestUrl = $"{_urlSearchGeocode}?api_key={_apiKey}&text={Uri.EscapeDataString(address)}";
                //logger.Info($"Requesting geocode with URL: {requestUrl}");

                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var geocodeData = JsonConvert.DeserializeObject<GeoSearchResponse>(responseBody);

                if (geocodeData?.Features == null || geocodeData.Features.Count == 0)
                {
                    //logger.Error($"No geocode data found for address: {address}");
                    return null;
                }

                return geocodeData.Features[0].Geometry.Coordinates;
            }

            catch (Exception e)
            {
                //logger.Error($"An error occurred while geocoding address: {e.Message}");
                return null;
            }
        }
    }
}
