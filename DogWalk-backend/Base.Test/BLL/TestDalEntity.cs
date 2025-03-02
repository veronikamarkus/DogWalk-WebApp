using Base.Contracts.Domain;

namespace Base.Test.BLL;

public class TestDalEntity : IDomainEntityId
{
    public Guid Id { get; set; }
}
