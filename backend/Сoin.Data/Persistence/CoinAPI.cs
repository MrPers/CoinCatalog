using AutoMapper;
using Сoin.Contracts.Persistence;
using Сoin.DTO;
using Сoin.Entity.JSON;
using Newtonsoft.Json;

namespace Сoin.Data.Persistence
{
    public class CoinAPI: ICoinAPI
    {
        private readonly IMapper _mapper;
        private readonly string Key = "3F7B52C1-065A-49AE-9953-78CA0534C9BC";

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

        public async Task<IList<CoinRateDto>> TakeCoinsFromAPIAsync(string name, DateTime date)
        {
            string result = await TakeCoinsFromResponseContentAsync(name, date);
            var rootObjects = JsonConvert.DeserializeObject<List<CoinRateJSON>>(result);
            if (rootObjects.Count() == 0)
            {
                throw new ArgumentException("The number of trades for this period is 0");
            }
            var answer = _mapper.Map<List<CoinRateDto>>(rootObjects);

            return answer;
        }
        
        private async Task<string> TakeCoinsFromResponseContentAsync(string name, DateTime dateTime)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-CoinAPI-Key", Key);
            HttpResponseMessage response = await client
                .GetAsync($"https://rest.coinapi.io/v1/ohlcv/BITSTAMP_SPOT_{name}_USD/history?period_id=8HRS&time_start={dateTime.ToString("s")}&limit=8200");
            
            client.Dispose();

            if ((int)response.StatusCode != 200)
            {
                throw new ArgumentException(await response.Content.ReadAsStringAsync());
            }

            return await response.Content.ReadAsStringAsync();
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
    }
}
