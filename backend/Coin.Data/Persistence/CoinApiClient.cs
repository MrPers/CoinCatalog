using AutoMapper;
using Coin.Contracts.Persistence;
using Coin.DTO;
using Coin.Entity.JSON; // Убедись, что твои JSON классы поддерживают десериализацию
using Newtonsoft.Json;

namespace Coin.Data.Persistence
{
    public class CoinApiClient : ICoinApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private const string BaseUrl = "https://api.coingecko.com/api/v3";

        public CoinApiClient(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<CoinDetailsDto> GetCoinDetailsAsync(string coinId)
        {
            // Используем Uri.EscapeDataString для безопасности
            var safeName = Uri.EscapeDataString(coinId.ToLower());
            var url = $"{BaseUrl}/coins/{safeName}?tickers=false&market_data=false&community_data=false&developer_data=false&sparkline=false";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"CoinGecko API Error: {response.StatusCode}. Details: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject<CoinDetailsJSON>(content);

            return _mapper.Map<CoinDetailsDto>(jsonObject);
        }

        public async Task<IList<CoinPriceDto>> GetCoinHistoryAsync(string coinId, DateTime fromDate)
        {
            long fromUnix = ((DateTimeOffset)fromDate).ToUnixTimeSeconds();
            long toUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var safeName = Uri.EscapeDataString(coinId.ToLower());
            var url = $"{BaseUrl}/coins/{safeName}/market_chart/range?vs_currency=usd&from={fromUnix}&to={toUnix}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"CoinGecko API Error ({url}): {response.StatusCode}. Details: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var rawData = JsonConvert.DeserializeObject<CoinPriceJSON>(content);

            return MapToCoinRateData(rawData);
        }

        private static List<CoinPriceDto> MapToCoinRateData(CoinPriceJSON raw)
        {
            if (raw?.Prices == null) return new List<CoinPriceDto>();

            var result = new List<CoinPriceDto>();

            // Используем TryGetValue для безопасности, как и было, но код чище
            var capsMap = raw.MarketCaps?.ToDictionary(k => (long)k[0], v => v[1]) ?? new Dictionary<long, double>();
            var volumesMap = raw.TotalVolumes?.ToDictionary(k => (long)k[0], v => v[1]) ?? new Dictionary<long, double>();

            foreach (var item in raw.Prices)
            {
                if (item.Count < 2) continue;

                long timestamp = (long)item[0];
                double price = item[1];

                capsMap.TryGetValue(timestamp, out double marketCap);
                volumesMap.TryGetValue(timestamp, out double totalVolume);

                result.Add(new CoinPriceDto
                {
                    Time = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime,
                    Price = price,
                    MarketCap = marketCap,
                    TotalVolume = totalVolume,
                });
            }

            return result;
        }

    }
}
