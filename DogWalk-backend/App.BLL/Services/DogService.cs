using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Dog = App.BLL.DTO.Dog;

namespace App.BLL.Services;

public class DogService :
    BaseEntityService<App.DAL.DTO.Dog, App.BLL.DTO.Dog, IDogRepository>,
    IDogService
{
    public DogService(IAppUnitOfWork uoW, IDogRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Dog, App.BLL.DTO.Dog>(mapper))
    {
    }

    public async Task<IEnumerable<Dog>> GetAllSortedAsync(Guid userId)
    {
        return (await Repository.GetAllSortedAsync(userId)).Select(e => Mapper.Map(e));
    }
}