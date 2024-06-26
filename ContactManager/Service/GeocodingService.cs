using System.Text.Json;

namespace ContactManager.Service
{
    public class GeocodingService
    {
        private readonly HttpClient _httpClient;

        public GeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string address)
        {
            var requestUri = $"https://nominatim.openstreetmap.org/search?format=jsonv2&q={address}&accept-language=de&limit=50&dedupe=1&addressdetails=1";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("User-Agent", "ContactManager/1.0");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var results = JsonSerializer.Deserialize<NominatimResponse[]>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });

                if (results != null && results.Length > 0)
                {
                    var result = results[0];
                    return (double.Parse(result.Lat), double.Parse(result.Lon));
                }
            }

            return (0, 0);
        }

        public class NominatimResponse
        {
            public string Lat { get; set; }
            public string Lon { get; set; }
        }

    }
}
