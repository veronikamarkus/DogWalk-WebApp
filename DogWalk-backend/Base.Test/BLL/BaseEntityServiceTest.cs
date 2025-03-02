using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Moq;

namespace Base.Test.BLL;

public class BaseEntityServiceTest
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IEntityRepository<TestDalEntity, Guid>> _repositoryMock;
    private readonly Mock<IBLLMapper<TestDalEntity, TestBllEntity>> _mapperMock;
    private readonly BaseEntityService<TestDalEntity,
        TestBllEntity, IEntityRepository<TestDalEntity, Guid>> _baseEntityService;

    public BaseEntityServiceTest()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _repositoryMock = new Mock<IEntityRepository<TestDalEntity, Guid>>();
        _mapperMock = new Mock<IBLLMapper<TestDalEntity, TestBllEntity>>();
        _baseEntityService = new BaseEntityService<TestDalEntity, TestBllEntity, IEntityRepository<TestDalEntity, Guid>>(
            _uowMock.Object, _repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void Add()
    {
        var bllEntity = new TestBllEntity { Id = Guid.NewGuid() };
        var dalEntity = new TestDalEntity { Id = bllEntity.Id };
        
        _mapperMock.Setup(m => m.Map(bllEntity)).Returns(dalEntity);
        _mapperMock.Setup(m => m.Map(dalEntity)).Returns(bllEntity);
        _repositoryMock.Setup(r => r.Add(dalEntity)).Returns(dalEntity);
        
        var result = _baseEntityService.Add(bllEntity);
        Assert.Equal(bllEntity, result);
        
        _repositoryMock.Verify(r => r.Add(dalEntity), Times.Once);
        _mapperMock.Verify(m => m.Map(bllEntity), Times.Once);
        _mapperMock.Verify(m => m.Map(dalEntity), Times.Once);
    }

    [Fact]
    public void Update()
    {
        var bllEntity = new TestBllEntity { Id = Guid.NewGuid() };
        var dalEntity = new TestDalEntity { Id = bllEntity.Id };
        
        _mapperMock.Setup(m => m.Map(bllEntity)).Returns(dalEntity);
        _mapperMock.Setup(m => m.Map(dalEntity)).Returns(bllEntity);
        _repositoryMock.Setup(r => r.Update(dalEntity)).Returns(dalEntity);
        
        var result = _baseEntityService.Update(bllEntity);
        Assert.Equal(bllEntity, result);
        
        _repositoryMock.Verify(r => r.Update(dalEntity), Times.Once);
        _mapperMock.Verify(m => m.Map(bllEntity), Times.Once);
        _mapperMock.Verify(m => m.Map(dalEntity), Times.Once);
    }

    [Fact]
    public void Remove()
    {
        var bllEntity = new TestBllEntity { Id = Guid.NewGuid() };
        var dalEntity = new TestDalEntity { Id = bllEntity.Id };
        _mapperMock.Setup(m => m.Map(bllEntity)).Returns(dalEntity);
        
        _repositoryMock.Setup(r => 
            r.Remove(dalEntity, It.IsAny<Guid>())).Returns(1);
        
        var result = _baseEntityService.Remove(bllEntity);
        
        _repositoryMock.Verify(r => r.Remove(dalEntity,
            It.IsAny<Guid>()), Times.Once);
        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task FirstOrDefaultAsync()
    {
        var id = Guid.NewGuid();
        var dalEntity = new TestDalEntity { Id = id };
        var bllEntity = new TestBllEntity { Id = id };
        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(id,
                It.IsAny<Guid>(), true))
            .ReturnsAsync(dalEntity);
         
        _mapperMock.Setup(m => m.Map(dalEntity)).Returns(bllEntity);
         
        var result = await _baseEntityService.FirstOrDefaultAsync(id);
         
        _repositoryMock.Verify(r => 
            r.FirstOrDefaultAsync(id, It.IsAny<Guid>(), true), Times.Once);
        
        _mapperMock.Verify(m => m.Map(dalEntity), Times.Once);
        Assert.Equal(bllEntity, result);
    }
}
