using Base.Contracts.Domain;

namespace Base.Test.BLL;

public class TestBllEntity : IDomainEntityId
{
    public Guid Id { get; set; }
}