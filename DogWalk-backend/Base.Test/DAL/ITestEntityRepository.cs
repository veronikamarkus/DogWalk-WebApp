using Base.Contracts.DAL;
using Base.Test.Domain;

namespace Base.Test.DAL;

public interface ITestEntityRepository : IEntityRepository<TestEntity>, ITestEntityRepositoryCustom<TestEntity>
{
    
}

public interface ITestEntityRepositoryCustom<TEntity>
{

}