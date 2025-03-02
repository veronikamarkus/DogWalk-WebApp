using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class DogInWalkRepository: BaseEntityRepository<APPDomain.DogInWalk, DALDTO.DogInWalk, AppDbContext>, IDogInWalkRepository
{
    private AppDbContext _context;
    
    public DogInWalkRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.DogInWalk, DALDTO.DogInWalk>(mapper))
    {
        _context = dbContext;
    } 
}