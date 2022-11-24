using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public PeopleService(GenericContext dataContext, ILogger<PeopleService> logger, IValidator<Person> validator) 
            : base(dataContext, logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
            this.validator = validator;
        }

        public async Task<IEnumerable<Person>> GetListAsync()
        {
            var peopleDbList = await dataContext.People.AsNoTracking().ToListAsync();
            var peopleList = peopleDbList.Select(p => new Person() { Id = p.Id, FirstName = p.FirstName, LastName = p.LastName });

            return peopleList;
        }

        public async Task<Person> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id can't be empty");
            }

            var dbPerson = await dataContext.People.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (dbPerson is null) 
            {
                return null;
            }

            var person = new Person() { Id = dbPerson.Id, FirstName = dbPerson.FirstName, LastName = dbPerson.LastName };
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
                throw new ArgumentException("The same person already exists");
            }

            var dbPerson = new DAL.Model.Person() { FirstName = person.FirstName, LastName = person.LastName };

            await dataContext.People.AddAsync(dbPerson);

            return await Task.FromResult(Guid.NewGuid());
        }

        public async Task<Person> UpdateAsync(Guid id, Person person)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentException("Id can't be empty");
            }

            if (id != person.Id)
            {
                throw new ArgumentException("The specified Id isn't the same of the Person.Id");
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