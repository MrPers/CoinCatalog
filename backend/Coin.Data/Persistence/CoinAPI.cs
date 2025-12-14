using AutoMapper;
using Coin.Contracts.Persistence;
using Coin.DTO;
using Coin.Entity.JSON;
using Newtonsoft.Json;

namespace Coin.Data.Persistence
{
    public class CoinAPI: ICoinAPI
    {
        private readonly IMapper _mapper;

        public CoinAPI(
            IMapper mapper
        ){
            _mapper = mapper;
        }

        public async Task<CoinDto> TakeCoinNameFromAPIAsync(string name)
        {
            string result = await TakeCoinsNameFromResponseContentAsync(name);
            var rootObjects = JsonConvert.DeserializeObject<CoinJSON>(result);

            return _mapper.Map<CoinDto>(rootObjects);
        }

        public async Task<CoinRateDto> TakeCoinsFromAPIAsync(string name, DateTime date)
        {
            string result = await TakeCoinsFromResponseContentAsync(name, date);
            var rawData = JsonConvert.DeserializeObject<CoinRateJSON>(result);
            if (rawData.Prices.Count() == 0)
            {
                throw new ArgumentException("The number of trades for this period is 0");
            }
            //var answer = _mapper.Map<List<CoinRateDto>>(rootObjects);
            var finalData = MapToCoinRateData(rawData);

            return finalData;
        }

        private async Task<string> TakeCoinsFromResponseContentAsync(string coinId, DateTime fromDate)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "MyCryptoApp/1.0");

            // 1. Начало периода берем из аргумента
            long fromUnix = ((DateTimeOffset)fromDate).ToUnixTimeSeconds();

            // 2. Конец периода всегда "сейчас" (UTC)
            // API CoinGecko требует указывать "to", даже если это текущий момент
            long toUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // 3. Формируем URL
            // coinId = "bitcoin", "solana" и т.д.
            var url = $"https://api.coingecko.com/api/v3/coins/{coinId.ToLower()}/market_chart/range?vs_currency=usd&from={fromUnix}&to={toUnix}";

            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            client.Dispose();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"CoinGecko API Error {(int)response.StatusCode}: {response.ReasonPhrase}. Body: {body}", null, response.StatusCode);
            }

            return body;
        }

        private async Task<string> TakeCoinsNameFromResponseContentAsync(string name)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client
                .GetAsync($"https://api.coingecko.com/api/v3/coins/{name}?tickers=false&market_data=false&community_data=false&developer_data=false&sparkline=false");

            client.Dispose();

            if ((int)response.StatusCode != 200)
            {
                throw new ArgumentException(await response.Content.ReadAsStringAsync());
            }

            return await response.Content.ReadAsStringAsync();
        }

        private static CoinRateDto MapToCoinRateData(CoinRateJSON raw)
        {
            var result = new CoinRateDto();

            if (raw == null) return result;

            // 1. Обрабатываем Цены и Время (берем время отсюда)
            if (raw.Prices != null)
            {
                foreach (var item in raw.Prices)
                {
                    // item[0] - время, item[1] - цена
                    if (item.Count >= 2)
                    {
                        // Конвертация Unix Timestamp (мс) в DateTime
                        var date = DateTimeOffset.FromUnixTimeMilliseconds((long)item[0]).DateTime;

                        result.Time.Add(date);
                        result.Prices.Add(item[1]);
                    }
                }
            }

            // 2. Обрабатываем Market Caps (только значения)
            if (raw.MarketCaps != null)
            {
                // Используем LINQ для краткости: берем второй элемент (значение) из каждого под-списка
                result.MarketCaps = raw.MarketCaps
                                       .Where(x => x.Count >= 2)
                                       .Select(x => x[1])
                                       .ToList();
            }

            // 3. Обрабатываем Volumes (только значения)
            if (raw.TotalVolumes != null)
            {
                result.TotalVolumes = raw.TotalVolumes
                                         .Where(x => x.Count >= 2)
                                         .Select(x => x[1])
                                         .ToList();
            }

            return result;
        }
    }
}
