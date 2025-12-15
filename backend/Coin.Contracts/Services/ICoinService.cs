using Coin.Contracts.Persistence;
using Coin.Contracts.Repo;
using Coin.Data;
using Coin.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Contracts.Services
{
    public interface ICoinService
    {
        Task<CoinDetailsDto> GetCoinsAllFullInformationAsync(int id);
        Task<ICollection<CoinDetailsDto>> GetCoinsAllPreviousInformationAsync();
        Task<ICollection<CoinPriceDto>> GetCoinRateAllByIdAsync(int id, int step);
        Task DeleteCoinAsync(int id);
        Task UpdateCoinsByCoinIdAsync(int id);
        Task AddCoinHistoryAsync(string name, long ticks);
    }
}
