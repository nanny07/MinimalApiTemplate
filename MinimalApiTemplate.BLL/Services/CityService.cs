using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalApiTemplate.BLL.Resources;
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

        public CityService(GenericContext dataContext, ILogger<CityService> logger, IValidator<City> validator, IMapper mapper)
            : base(dataContext, logger, mapper)
        {
            this.dataContext = dataContext;
            this.logger = logger;
            this.validator = validator;
        }

        public async Task<IEnumerable<City>> GetListAsync()
        {
            var cities = await dataContext.Cities.AsNoTracking().ProjectTo<City>(mapper.ConfigurationProvider).ToListAsync();
            return cities;
        }

        public async Task<City> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(Messagges.IdCanNotBeEmpty);
            }

            var city = await dataContext.Cities.AsNoTracking().ProjectTo<City>(mapper.ConfigurationProvider).FirstOrDefaultAsync(c => c.Id == id);
            if (city is null)
            {
                return null;
            }

            return city;
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
                throw new ArgumentException(Messagges.SameCityExists);
            }

            var dbCity = mapper.Map<DAL.Model.City>(city);

            await dataContext.Cities.AddAsync(dbCity);

            await dataContext.SaveChangesAsync();

            return dbCity.Id;
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
                throw new ArgumentException(Messagges.IdCanNotBeEmpty);
            }

            if (id != city.Id)
            {
                throw new ArgumentException(string.Format(Messagges.SpecifiedIdNotTheSame, city.GetType().Name));
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