using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IRepository
    {
        public Task StoreAsync(string response, short alphabet);
    }
}
