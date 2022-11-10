using FluentValidation;

namespace MinimapApiTemplate.BLL.Model
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
