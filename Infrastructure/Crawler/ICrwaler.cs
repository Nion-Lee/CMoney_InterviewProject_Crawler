using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface ICrwaler
    {
        public IAsyncEnumerable<ValueTuple<string, byte>> ExecuteAsync(string url);
    }
}
