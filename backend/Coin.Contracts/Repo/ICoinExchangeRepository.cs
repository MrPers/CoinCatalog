using Coin.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin.Entity.DB;
using Base.Contracts;

namespace Coin.Contracts.Repo
{
    public interface ICoinExchangeRepository : IBaseRepository<CoinPrice, CoinPriceDto, int>
    {
        Task<ICollection<CoinPriceDto>> GetCoinsPricesByIdAsync(int id, int step);
    }
}
