using Сoin.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Сoin.Entity.DB;
using Base.Contracts;

namespace Сoin.Contracts.Repo
{
    public interface ICoinExchangeRepository : IBaseRepository<CoinRate, CoinRateDto, int>
    {
        Task<ICollection<CoinRateDto>> GetCoinRateAllByIdAsync(int id, int step);
        Task<DateTime> GetLastCoinRepositoryAsync(int id);
    }
}
