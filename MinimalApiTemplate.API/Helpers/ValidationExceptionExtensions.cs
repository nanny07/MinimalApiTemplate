using FluentValidation;

namespace MinimalApiTemplate.API.Helpers
{
    public static class ValidationExceptionExtensions
    {
        public static Dictionary<string, string[]> ToDictionary(this ValidationException validationException)
        {
            return validationException.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );
        }
    }
}
