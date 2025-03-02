using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class DogRepository: BaseEntityRepository<APPDomain.Dog, DALDTO.Dog, AppDbContext>, IDogRepository
{
    private AppDbContext _context;
    
    public DogRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.Dog, DALDTO.Dog>(mapper))
    {
        _context = dbContext;
    }

    public async Task<IEnumerable<DALDTO.Dog>> GetAllSortedAsync(Guid userId)
    {
        var query = CreateQuery(userId);
        query = query.OrderBy(d => d.DogName);
        var res = await query.ToListAsync();
        return res.Select(d => Mapper.Map(d));
    }
    
}