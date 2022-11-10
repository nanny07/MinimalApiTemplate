using FluentValidation;
using MinimapApiTemplate.BLL.Model;

namespace MinimapApiTemplate.BLL.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IValidator<Person> validator;

        public PeopleService(IValidator<Person> validator)
        {
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