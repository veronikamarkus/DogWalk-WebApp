using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class ProfileRepository: BaseEntityRepository<APPDomain.Profile, DALDTO.Profile, AppDbContext>, IProfileRepository
{
    public ProfileRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.Profile, DALDTO.Profile>(mapper))
    {
        
    }
    
    public async Task<DALDTO.Profile> FindProfileByUserId(Guid userId)
    {
        var query = CreateQuery(userId);

        var res = await query.FirstAsync();

        return Mapper.Map(res)!;
    }

    public async Task<string> GetProfileDescription(Guid userId)
    {
        var query = CreateQuery(userId);

        var profile =  Mapper.Map(await query.FirstAsync());

        var res = profile!.Description;
        
        return res;
    }
}