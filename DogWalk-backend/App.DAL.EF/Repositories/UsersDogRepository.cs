using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class UsersDogRepository: BaseEntityRepository<APPDomain.UsersDog, DALDTO.UsersDog, AppDbContext>, IUsersDogRepository
{
    public UsersDogRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.UsersDog, DALDTO.UsersDog>(mapper))
    {
    }

    // public async Task<IEnumerable<DALDTO.Dog>> GetAllSortedAsync(Guid userId)
    // {
    //     var query = CreateQuery(userId);
    //     query = query.OrderBy(d => d.DogName);
    //     var res = await query.ToListAsync();
    //     return res.Select(d => Mapper.Map(d));
    // }

    // public async Task<IEnumerable<APPDomain.UsersDog>> GetUsersDogsIds(Guid userId)
    // {
    //     var query = CreateQuery(userId);
    //     query = query.Select(d => d.Id);
    //     var res = await query.ToListAsync();
    //     return res.Select(d => Mapper.Map(d));
    // }
    
}