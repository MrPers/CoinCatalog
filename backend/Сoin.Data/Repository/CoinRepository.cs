using AutoMapper;
using Сoin.DTO;
using Microsoft.EntityFrameworkCore;
using Сoin.Entity.DB;
using System.ComponentModel.DataAnnotations;
using Base.Data;

namespace Сoin.Data.Repository
{
    public class CoinRepository : BaseDataContext<Coin, CoinDto, int>, ICoinRepository
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CoinRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //public CoinRepository(DataContext context, IMapper mapper) : base(context, mapper)
        //{
        //}

        public async Task<CoinDto> GetCoinsAllFullInformationAsync([Range(0, long.MaxValue)] int id)
        {
            var productDto = await _context.Coins
                .Where(p => p.Id == id).FirstAsync();

            return _mapper.Map<CoinDto>(productDto);
        }

        public async Task<ICollection<CoinDto>> GetCoinsAllWithPreviousInformationAsync()
        {
            ICollection<CoinDto> productsDto = new List<CoinDto>();

            foreach (var item in await _context.Coins.ToListAsync())
            {
                var productDto = await _context.Coins
                    .Join(_context.CoinRates
                    .Where(x => x.CoinId == item.Id)
                    .Where(t => t.Time == _context.CoinRates
                        .Where(p => p.CoinId == item.Id).Max(v => v.Time)),
                    p => p.Id,
                    t => t.CoinId,
                    (p, t) => new CoinDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Prices = t.Prices,
                        VolumeTraded = t.VolumeTraded,
                        UrlIcon = p.UrlIcon,
                    }
                    )
                .FirstAsync();

                productsDto.Add(productDto);
            }

            return productsDto;
        }
    }
}
