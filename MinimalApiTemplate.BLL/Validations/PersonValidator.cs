using FluentValidation;
using MinimapApiTemplate.Shared.Model;

namespace MinimapApiTemplate.BLL.Validations
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty()
                //We can override the message
                //.WithMessage(ValidationMessagges.FieldRequired) 
                .MaximumLength(30);

            RuleFor(p => p.LastName)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
