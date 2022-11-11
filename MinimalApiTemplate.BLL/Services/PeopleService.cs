using FluentValidation;
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
            var peopleList = new List<Person>() {
                new Person() { Id = Guid.NewGuid(), FirstName = "Andrea", LastName = "Rossi" },
                new Person() { Id = Guid.NewGuid(), FirstName = "Gianni", LastName = "Verdi" }
            };

            return await Task.FromResult(peopleList);
        }

        public async Task<Person> GetAsync(Guid id)
        {
            var person = new Person() { Id = id, FirstName = "Andrea", LastName = "Rossi" };

            return await Task.FromResult(person);
        }

        public async Task<Guid> InsertAsync(Person person)
        {
            var validationResult = await validator.ValidateAsync(person);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            //... the real insert

            return await Task.FromResult(Guid.NewGuid());
        }

        public async Task<Person> UpdateAsync(Guid id, Person person)
        {
            if(id == Guid.Empty)
            {
                return null;
            }

            var validationResult = await validator.ValidateAsync(person);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            //... the real update

            return await Task.FromResult(person);
        }

        public Task<int> DeleteAsync(Guid id)
        {
            //... the real delete

            return Task.FromResult(1);
        }
    }
}