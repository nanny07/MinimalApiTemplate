using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalApiTemplate.DAL;
using MinimapApiTemplate.BLL.Services.Common;
using MinimapApiTemplate.Shared.Model;

namespace MinimapApiTemplate.BLL.Services
{
    public class CityService : BaseService, ICityService
    {
        private readonly GenericContext dataContext;
        private readonly ILogger<CityService> logger;
        private readonly IValidator<City> validator;

        public CityService(GenericContext dataContext, ILogger<CityService> logger, IValidator<City> validator)
            : base(dataContext, logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
            this.validator = validator;
        }

        public async Task<IEnumerable<City>> GetListAsync()
        {
            var dbCities = await dataContext.Cities.AsNoTracking().ToListAsync();
            var cities = dbCities.Select(c => new City() { Id = c.Id, Name = c.Name, State = c.State }).ToList();

            return cities;
        }

        public async Task<City> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id can't be empty");
            }

            var dbCity = await dataContext.Cities.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if(dbCity is null)
            {
                return null;
            }

            var city = new City() { Id = dbCity.Id, Name = dbCity.Name, State = dbCity.State };

            return await Task.FromResult(city);
        }

        public async Task<Guid> InsertAsync(City city)
        {
            var validationResult = await validator.ValidateAsync(city);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var alreadyExists = await dataContext.Cities.AnyAsync(c =>
                c.Name.Equals(city.Name, StringComparison.InvariantCultureIgnoreCase)
                && c.State.Equals(city.State, StringComparison.InvariantCultureIgnoreCase)
            );

            if (alreadyExists)
            {
                throw new ArgumentException("The same person already exists");
            }

            var dbCity = new DAL.Model.City() { Name = city.Name, State = city.State };
            await dataContext.Cities.AddAsync(dbCity);

            return await Task.FromResult(Guid.NewGuid());
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var city = await dataContext.Cities.FindAsync(id);
            if (city is not null)
            {
                dataContext.Cities.Remove(city);
            }

            var res = await dataContext.SaveChangesAsync();
            return res;
        }

        public async Task<City> UpdateAsync(Guid id, City city)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id can't be empty");
            }

            if (id != city.Id)
            {
                throw new ArgumentException("The specified Id isn't the same of the City.Id");
            }

            var validationResult = await validator.ValidateAsync(city);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var dbCity = await dataContext.Cities.FindAsync(id);
            if (dbCity is null)
            {
                return null;
            }

            dbCity.Name = city.Name;
            dbCity.State = city.State;

            await dataContext.SaveChangesAsync();

            return city;
        }
    }
}