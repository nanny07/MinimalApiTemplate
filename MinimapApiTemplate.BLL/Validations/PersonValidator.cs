using FluentValidation;
using MinimapApiTemplate.BLL.Model;
using MinimapApiTemplate.BLL.Resources;

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
