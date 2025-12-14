using Coin.DTO;
using Coin.Entity.DB;
using Base.Contracts;

namespace Coin.Data
{
    public interface ICoinRepository : IBaseRepository<Entity.DB.Coin, CoinDto, int>
    {
        Task<ICollection<CoinDto>> GetCoinsAllWithPreviousInformationAsync();
        Task<CoinDto> GetCoinsAllFullInformationAsync(int id);
    }
}
