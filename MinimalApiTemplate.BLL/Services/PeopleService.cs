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
    public class PeopleService : BaseService, IPeopleService
    {
        private readonly GenericContext dataContext;
        private readonly ILogger<PeopleService> logger;
        private readonly IValidator<Person> validator;

        public PeopleService(GenericContext dataContext, ILogger<PeopleService> logger, IValidator<Person> validator, IMapper mapper)
            : base(dataContext, logger, mapper)
        {
            this.dataContext = dataContext;
            this.logger = logger;
            this.validator = validator;
        }

        public async Task<IEnumerable<Person>> GetListAsync()
        {
            var peopleList = await dataContext.People.AsNoTracking().ProjectTo<Person>(mapper.ConfigurationProvider).ToListAsync();
            return peopleList;
        }

        public async Task<Person> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(Messagges.IdCanNotBeEmpty);
            }

            var person = await dataContext.People.AsNoTracking().ProjectTo<Person>(mapper.ConfigurationProvider).FirstOrDefaultAsync(p => p.Id == id);
            if (person is null)
            {
                return null;
            }

            return person;
        }

        public async Task<Guid> InsertAsync(Person person)
        {
            var validationResult = await validator.ValidateAsync(person);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var alreadyExists = await dataContext.People.AnyAsync(p =>
                p.FirstName.Equals(person.FirstName, StringComparison.InvariantCultureIgnoreCase)
                && p.LastName.Equals(person.LastName, StringComparison.InvariantCultureIgnoreCase)
            );

            if (alreadyExists)
            {
                throw new ArgumentException(Messagges.SamePersonExists);
            }

            var dbPerson = mapper.Map<DAL.Model.Person>(person);

            await dataContext.People.AddAsync(dbPerson);

            await dataContext.SaveChangesAsync();

            return dbPerson.Id;
        }

        public async Task<Person> UpdateAsync(Guid id, Person person)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(Messagges.IdCanNotBeEmpty);
            }

            if (id != person.Id)
            {
                throw new ArgumentException(string.Format(Messagges.SpecifiedIdNotTheSame, person.GetType().Name));
            }

            var validationResult = await validator.ValidateAsync(person);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var dbPerson = await dataContext.People.FindAsync(id);
            if (dbPerson is null)
            {
                return null;
            }

            dbPerson.FirstName = person.FirstName;
            dbPerson.LastName = person.LastName;

            await dataContext.SaveChangesAsync();

            return person;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var person = await dataContext.People.FindAsync(id);
            if (person is not null)
            {
                dataContext.People.Remove(person);
            }

            var res = await dataContext.SaveChangesAsync();
            return res;
        }
    }
}