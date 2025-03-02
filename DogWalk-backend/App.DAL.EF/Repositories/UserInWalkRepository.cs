using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class UserInWalkRepository: BaseEntityRepository<APPDomain.UserInWalk, DALDTO.UserInWalk, AppDbContext>, IUserInWalkRepository
{
    private AppDbContext _context;
    
    public UserInWalkRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.UserInWalk, DALDTO.UserInWalk>(mapper))
    {
        _context = dbContext;
    }
    
}