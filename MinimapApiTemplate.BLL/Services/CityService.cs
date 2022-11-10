using MinimapApiTemplate.BLL.Model;

namespace MinimapApiTemplate.BLL.Services
{
    public class CityService : ICityService
    {
        public async Task<IEnumerable<City>> GetListAsync()
        {
            var cityList = new List<City>() {
                new City() { Id = Guid.NewGuid(), Name = "Arezzo", State = "IT" },
                new City() { Id = Guid.NewGuid(), Name = "Firenze", State = "IT" }
            };

            return await Task.FromResult(cityList);
        }

        public async Task<City> GetAsync(Guid id)
        {
            var city = new City() { Id = id, Name = "Arezzo", State = "IT" };

            return await Task.FromResult(city);
        }

        public async Task<Guid> InsertAsync(City city)
        {
            //... the real insert
            return await Task.FromResult(Guid.NewGuid());
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            //... the real delete
            return await Task.FromResult(0);
        }
    }
}