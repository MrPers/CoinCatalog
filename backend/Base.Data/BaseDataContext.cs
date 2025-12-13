using AutoMapper;
using Base.Contracts;
using Base.Entity;
using Microsoft.EntityFrameworkCore;

namespace Base.Data
{
    public class BaseDataContext<TTable, TDto, TId> : IBaseRepository<TTable, TDto, TId> where TTable : class, IBaseEntity<TId>
    {
        private readonly BaseDataContext _context;
        private readonly IMapper _mapper;
        public BaseDataContext(BaseDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<TId> AddAsync(TDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var time = _mapper.Map<TTable>(dto);

            if (time == null)
            {
                throw new ArgumentNullException(nameof(time));
            }

            await _context.Set<TTable>().AddAsync(time);

            await _context.SaveChangesAsync();

            return time.Id;
        }

        public async Task<ICollection<TId>> AddCollectionAsync(ICollection<TDto> dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var times = _mapper.Map<ICollection<TTable>>(dto);

            if (times == null)
            {
                throw new ArgumentNullException(nameof(times));
            }

            await _context.Set<TTable>().AddRangeAsync(times);

            await _context.SaveChangesAsync();

            var anser = new List<TId>();
            times.ToList().ForEach(n => anser.Add(n.Id));

            return anser;
        }

        public async Task UpdateAsync(TId Id, TDto meaning)
        {
            if (meaning == null)
            {
                throw new ArgumentNullException(nameof(meaning));
            }

            var result = await _context.Set<TTable>().FindAsync(Id);

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            _context.Entry(result)
                .CurrentValues
                .SetValues(_mapper.Map<TTable>(meaning));

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TId Id)
        {
            var obj = await _context.Set<TTable>().FindAsync(Id);
            
            if (obj != null)
            {
                _context.Entry<TTable>(obj).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }

        }

        public async Task<ICollection<TDto>> GetAllAsync()
        {
            var dbItems = await _context.Set<TTable>().ToListAsync();
            return _mapper.Map<ICollection<TDto>>(dbItems);
        }

        public async Task<TDto> GetByIdAsync(TId Id)
        {
            var dbItem = await _context.Set<TTable>().FindAsync(Id);
            return _mapper.Map<TDto>(dbItem);
        }

    }
}
