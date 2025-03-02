using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class WalkRepository: BaseEntityRepository<App.Domain.Walk, DALDTO.Walk, AppDbContext>, IWalkRepository
{
    public WalkRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<App.Domain.Walk, DALDTO.Walk>(mapper))
    {
    }
    
    public async Task<IEnumerable<DALDTO.Walk>> GetActiveWalksByLocation(string location)
    {
        var query = RepoDbSet.AsQueryable().Include("Location");
        var res = await query.ToListAsync();
        var filtered = res.Where(w => (w.Location!.City.ToLower().Contains(location.ToLower())
                                      || w.Location!.District.ToLower().Contains(location.ToLower()))
                                      && w.Closed == false);
        return filtered.Select(d => Mapper.Map(d));
    }
}