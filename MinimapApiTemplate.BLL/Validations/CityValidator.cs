using FluentValidation;
using MinimapApiTemplate.BLL.Model;

namespace MinimapApiTemplate.BLL.Validations
{

    public class CityValidator : AbstractValidator<City>
    {
        public CityValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(c => c.State)
                .NotEmpty()
                .MaximumLength(2);
        }
    }
}
