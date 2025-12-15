using Coin.DTO;
using Coin.Entity.DB;
using Base.Contracts;

namespace Coin.Data
{
    public interface ICoinRepository : IBaseRepository<CoinDetails, CoinDetailsDto, int>
    {
        Task<ICollection<CoinDetailsDto>> GetCoinsAllWithPreviousInformationAsync();
        Task<CoinDetailsDto> GetCoinsAllFullInformationAsync(int id);
    }
}
