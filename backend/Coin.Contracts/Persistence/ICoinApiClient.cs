using Coin.DTO;

namespace Coin.Contracts.Persistence
{
    public interface ICoinApiClient
    {
        Task<CoinDetailsDto> GetCoinDetailsAsync(string name);
        Task<IList<CoinPriceDto>> GetCoinHistoryAsync(string coinId, DateTime date);
    }
}
