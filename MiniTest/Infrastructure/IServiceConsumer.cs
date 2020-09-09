using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Infrastructure
{
    public interface IServiceConsumer<T>
        where T : class, IModel
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
