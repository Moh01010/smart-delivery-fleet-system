using Smart_Delivery___Fleet_Management_System.ExternalModels;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Smart_Delivery___Fleet_Management_System.Services
{
    public class OpenStreetMapService
    {
        private readonly HttpClient _http;

        public OpenStreetMapService(HttpClient http)
        {
            _http = http;
            _http.DefaultRequestHeaders.UserAgent.ParseAdd("SmartDeliveryApp/1.0");
        }

        // 1) Geocode: address -> lat/lng
        public async Task<(double lat, double lng)> GeocodeAddress(string address)
        {
            var url =
                $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";

            var response = await _http.GetFromJsonAsync<List<NominatimResponse>>(url);

            if (response == null || response.Count == 0)
                throw new Exception($"Geocoding failed for address: {address}");

            var result = response[0];

            return (double.Parse(result.lat), double.Parse(result.lon));
        }

        // 2) Travel time between two points
        public async Task<int> GetTravelTimeInSeconds(
            double originLat, double originLng,
            double destLat, double destLng)
        {
            var url =
                $"http://router.project-osrm.org/route/v1/driving/{originLng},{originLat};{destLng},{destLat}?overview=false";

            var response = await _http.GetFromJsonAsync<OsrmResponse>(url);

            if (response == null || response.routes.Count == 0)
                throw new Exception("OSRM routing failed.");

            return (int)response.routes[0].duration;
        }
    }
}

