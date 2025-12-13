using AutoMapper;
using Сoin.Contracts.Repo;
using Сoin.Entity;
using Сoin.DTO;
using Microsoft.EntityFrameworkCore;
using Сoin.Entity.DB;
using System.ComponentModel.DataAnnotations;
using Base.Contracts;
using Base.Data;

namespace Сoin.Data.Repository
{
    public class CoinExchangeRepository : BaseDataContext<CoinRate, CoinRateDto, int>, ICoinExchangeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CoinExchangeRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<CoinRateDto>> GetCoinRateAllByIdAsync([Range(1, int.MaxValue)] int id, [Range(24, int.MaxValue)] int step)
        {

            var oldCoinRate = await this.PrivateGetByCoinIdAsync(id, step);

            if (oldCoinRate.Count < 1 || oldCoinRate == null)
            {
                throw new ArgumentNullException(nameof(oldCoinRate));
            }

            return _mapper.Map<ICollection<CoinRateDto>>(oldCoinRate);
        }

        public async Task<DateTime> GetLastCoinRepositoryAsync([Range(0, long.MaxValue)] int id)
        {
            var result = _context.CoinRates
                .Where(p => p.CoinId == id).OrderByDescending(x => x.Time)
              .FirstOrDefault();

            return result.Time;
        }

        private async Task<ICollection<CoinRate>> PrivateGetByCoinIdAsync([Range(0, long.MaxValue)] int id, [Range(24, int.MaxValue)] int step)
        {
            var oldCoinRate = await _context.GetCoins(id, step).ToListAsync();

            return oldCoinRate;
        }
    }
}
