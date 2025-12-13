using Сoin.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сoin.Contracts.Services
{
    public interface ICoinService
    {
        Task<CoinDto> GetCoinsAllFullInformationAsync(int id);
        Task<ICollection<CoinDto>> GetCoinsAllPreviousInformationAsync();
        Task<ICollection<CoinRateDto>> GetCoinRateAllByIdAsync(int id, int step);
        Task DeleteCoinAsync(int id);
        Task UpdateCoinsByCoinIdAsync(int id);
        Task AddСoinСoinExchangesAsync(string name, long ticks = 1577836800000);
    }
}
