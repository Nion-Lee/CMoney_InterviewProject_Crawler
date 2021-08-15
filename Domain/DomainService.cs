using Domain.Aggregates;

namespace Domain
{
    public class DomainService : IDomainService
    {
        public T GetAggregate<T>() where T : IAggregate, new()
        {
            return new T();
        }
    }
}
