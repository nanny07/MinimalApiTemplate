using MinimapApiTemplate.BLL.Model;

namespace MinimapApiTemplate.BLL.Services
{
    public interface ICityService
    {
        Task<City> GetAsync(Guid id);
        Task<IEnumerable<City>> GetListAsync();
        Task<Guid> InsertAsync(City city);
        Task<int> DeleteAsync(Guid id);
    }
}