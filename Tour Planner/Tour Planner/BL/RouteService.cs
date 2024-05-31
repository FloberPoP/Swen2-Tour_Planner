using System.Globalization;
using System.Net.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tour_Planner.BL.GeoLocationAPI;

namespace Tour_Planner.BL
{
    public class RouteService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey = "5b3ce3597851110001cf6248d157c6099b64438e81765a43f9ecc90b";
        private readonly string _urlSearchGeocode = "https://api.openrouteservice.org/geocode/search";
        private readonly string _urlDirections = "https://api.openrouteservice.org/v2/directions/";

        public RouteService() { }

        static Dictionary<string, string> TransportDictionary = new()
        {
            {"Walk", "foot-walking"},
            {"Bicycle", "cycling-regular"},
            {"Car", "driving-car"}
        };

        private async Task<string> GetGeocode(string url, string apiKey, string address)
        {
            string request = $"{url}?api_key={apiKey}&text={Uri.EscapeDataString(address)}";

            // openrouteservice GET /geocode/search
            HttpResponseMessage response = await _httpClient.GetAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetCoordinates(string address)
        {
            var coordinates = await GetGeocode(_urlSearchGeocode, _apiKey, address);

            if (!string.IsNullOrEmpty(coordinates))
            {
                var geocodeData = JsonConvert.DeserializeObject<GeoSearchResponse>(coordinates);

                if (geocodeData.Features != null && geocodeData.Features.Count > 0)
                {
                    var coordinatesList = geocodeData.Features[0].Geometry.Coordinates;

                    if (coordinatesList != null && coordinatesList.Count == 2)
                    {
                        return $"{coordinatesList[0].ToString(CultureInfo.InvariantCulture)},{coordinatesList[1].ToString(CultureInfo.InvariantCulture)}";
                    }
                }
            }

            return null;
        }

        // Find Route between 2 places
        public async Task<(string, CalculatedRouteResponse)> GetRoute(string address1, string address2, string transportation)
        {
            // Long, Lat
            var startCoordinates = await GetCoordinates(address1);
            var endCoordinates = await GetCoordinates(address2);

            if (startCoordinates != null && endCoordinates != null)
            {
                string transportType = TransportDictionary.ContainsKey(transportation) ? TransportDictionary[transportation] : TransportDictionary["Car"];
                string requestUrl = $"{_urlDirections}{transportType}?api_key={_apiKey}&start={startCoordinates}&end={endCoordinates}";

                // openrouteservice GET /v2/directions/{profile}
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                
                string responseBody = await response.Content.ReadAsStringAsync();

                return (responseBody, JsonConvert.DeserializeObject<CalculatedRouteResponse>(responseBody));
            }

            else
            {
                return (null, null);
            }
        }
    }
}
