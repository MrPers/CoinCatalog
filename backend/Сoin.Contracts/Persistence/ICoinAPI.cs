using Сoin.DTO;
using Сoin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сoin.Contracts.Persistence
{
    public interface ICoinAPI
    {
        Task<CoinDto> TakeCoinNameFromAPIAsync(string name);
        Task<IList<CoinRateDto>> TakeCoinsFromAPIAsync(string name, DateTime date);
    }
}
