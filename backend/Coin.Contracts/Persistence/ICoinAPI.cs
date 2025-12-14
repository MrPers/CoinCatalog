using Coin.DTO;

namespace Coin.Contracts.Persistence
{
    public interface ICoinAPI
    {
        Task<CoinDto> TakeCoinNameFromAPIAsync(string name);
        Task<CoinRateDto> TakeCoinsFromAPIAsync(string name, DateTime date);
    }
}
