using AutoMapper;
using Base.DAL.EF;
using Base.Test.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Base.Test.DAL;

public class BaseRepositoryTest
{
    private readonly TestDbContext _ctx;
    private readonly TestEntityRepository _testEntityRepository;

    public BaseRepositoryTest()
    {
        // set up mock database - inmemory
        // var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        //
        // // use random guid as db instance id
        // optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var options = CreateNewContextOptions();
        
        _ctx = new TestDbContext(options);

        // reset db
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        
        var config = new MapperConfiguration(cfg => cfg.CreateMap<TestEntity, TestEntity>());
        var mapper = config.CreateMapper();
        _testEntityRepository =
            new TestEntityRepository(
                _ctx,
                new BaseDalDomainMapper<TestEntity, TestEntity>(mapper)
            );
    }
    
    public DbContextOptions<TestDbContext> CreateNewContextOptions()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        return new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(connection)
            .Options;
    }

    [Fact]
    public async Task TestAdd()
    {
        // arrange
        _testEntityRepository.Add(new TestEntity() {Value = "Foo"});
        _ctx.SaveChanges();

        // act
        var data = await _testEntityRepository.GetAllAsync();

        // assert
        Assert.Equal(1, data.Count());
    }
    
    [Fact]
    public async Task TestUpdate()
    {
        // arrange
        var entity = _testEntityRepository.Add(new TestEntity() {Value = "Foo"});
        _ctx.SaveChanges(); 
        entity.Value = "XXX";
        _ctx.ChangeTracker.Clear();
        _testEntityRepository.Update(entity);
        _ctx.SaveChanges();

        // act
        var data = await _testEntityRepository.FirstOrDefaultAsync(entity.Id);

        // assert
        Assert.Equal("XXX", data!.Value);
    }
    
    [Fact]
    public async Task TestRemoveByEntity()
    {
        // arrange
        var entity = _testEntityRepository.Add(new TestEntity() {Value = "Foo"});
        _ctx.SaveChanges(); 
        _testEntityRepository.Remove(entity);
        _ctx.SaveChanges();

        // act
        var data = await _testEntityRepository.FirstOrDefaultAsync(entity.Id);

        // assert
        Assert.Null(data);
    }
    
    [Fact]
    public async Task TestRemoveById()
    {
        // arrange
        var entity = _testEntityRepository.Add(new TestEntity() {Value = "Foo"});
        _ctx.SaveChanges(); 
        _testEntityRepository.Remove(entity.Id);
        _ctx.SaveChanges();

        // act
        var data = await _testEntityRepository.FirstOrDefaultAsync(entity.Id);

        // assert
        Assert.Null(data);
    }
    
    [Fact]
    public async Task TestGetAll()
    {
        // arrange
        _testEntityRepository.Add(new TestEntity() {Value = "Foo"});
        _testEntityRepository.Add(new TestEntity() {Value = "XXX"});
        _ctx.SaveChanges();

        // act
        var data = _testEntityRepository.GetAll();

        // assert
        Assert.Equal(2,data.Count());
    }
    
    [Fact]
    public async Task TestGetAllAsync()
    {
        // arrange
        _testEntityRepository.Add(new TestEntity() {Value = "Foo"});
        _testEntityRepository.Add(new TestEntity() {Value = "XXX"});
        _ctx.SaveChanges();

        // act
        var data = await _testEntityRepository.GetAllAsync();

        // assert
        Assert.Equal(2,data.Count());
    }
    
    [Fact]
    public async Task TestRemoveByEntityAsync()
    {
        // arrange
        var entity = _testEntityRepository.Add(new TestEntity() {Value = "Foo"});
        _ctx.SaveChanges(); 
        await _testEntityRepository.RemoveAsync(entity);
        _ctx.SaveChanges();

        // act
        var data = await _testEntityRepository.FirstOrDefaultAsync(entity.Id);

        // assert
        Assert.Null(data);
    }
    
    [Fact]
    public async Task TestRemoveByIdAsync()
    {
        // arrange
        var entity = _testEntityRepository.Add(new TestEntity() {Value = "Foo"});
        _ctx.SaveChanges(); 
        await _testEntityRepository.RemoveAsync(entity.Id);
        _ctx.SaveChanges();

        // act
        var data = await _testEntityRepository.FirstOrDefaultAsync(entity.Id);

        // assert
        Assert.Null(data);
    }
    
    [Fact]
    public  void TestFirstOrDefault()
    {
        // arrange
        var entity = _testEntityRepository.Add(new TestEntity() {Value = "Foo"});
        _ctx.SaveChanges(); 

        // act
        var data = _testEntityRepository.FirstOrDefault(entity.Id);

        // assert
        Assert.Equal(entity.Id, data!.Id);
    }
    
    [Fact]
    public async Task TestFirstOrDefaultAsync()
    {
        // arrange
        var entity = _testEntityRepository.Add(new TestEntity() {Value = "Foo"});
        _ctx.SaveChanges(); 

        // act
        var data = await _testEntityRepository.FirstOrDefaultAsync(entity.Id);

        // assert
        Assert.Equal(entity.Id, data!.Id);
    }
    
}