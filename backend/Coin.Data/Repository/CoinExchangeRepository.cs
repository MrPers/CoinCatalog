using AutoMapper;
using Coin.Contracts.Repo;
using Coin.DTO;
using Microsoft.EntityFrameworkCore;
using Coin.Entity.DB;
using Base.Data;

namespace Coin.Data.Repository
{
    public class CoinExchangeRepository : BaseDataContext<CoinPrice, CoinPriceDto, int>, ICoinExchangeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CoinExchangeRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение истории курсов (возможно через хранимую процедуру или оптимизированный запрос)
        /// </summary>
        public async Task<ICollection<CoinPriceDto>> GetCoinsPricesByIdAsync(int coinId, int step)
        {
            var entities = await _context.GetCoins(coinId, step).ToListAsync();

            return _mapper.Map<List<CoinPriceDto>>(entities);
        }
    }
}