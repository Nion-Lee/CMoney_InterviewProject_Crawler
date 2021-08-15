using Domain.Aggregates;

namespace Domain
{
    public interface IDomainService
    {
        public T GetAggregate<T>() where T : IAggregate, new();
    }
}
