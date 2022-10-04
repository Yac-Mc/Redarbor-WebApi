using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Queries;

namespace WebApi.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(bool withColums = true);
        Task<IEnumerable<T>> SPGetAllByFilterAsync(string commandTextSP, object parameters);
        Task<IEnumerable<T>> GetAllByFilterAsync(List<Filter> filters, bool withColums = true);
        Task<T> GetByFilterAsync(List<Filter> filters, bool withColums = true);
        Task<Pagination<T>> GetAllPaginationByFilterAsync(Pagination<T> pagination, bool withColums = true);
        Task UpdateByIdAsync(T t, string id);
        Task DeleteByIdAsync(List<Filter> filters);
        Task InsertAsync(T t);
        Task<int> InsertListAsync(IEnumerable<T> list);
    }
}