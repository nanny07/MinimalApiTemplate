using MinimapApiTemplate.Shared.Model;

namespace MinimapApiTemplate.BLL.Services
{
    public interface IPeopleService
    {
        Task<Person> GetAsync(Guid id);
        Task<IEnumerable<Person>> GetListAsync();
        Task<Guid> InsertAsync(Person person);
        Task<int> DeleteAsync(Guid id);
        Task<Person> UpdateAsync(Guid id, Person person);
    }
}