using FluentValidation;
using MinimalApiTemplate.API.Helpers;
using MinimalApiTemplate.Routing;
using MinimapApiTemplate.BLL.Model;
using MinimapApiTemplate.BLL.Services;
using System.Net.Mime;

namespace MinimalApiTemplate.Handlers
{
    public class PersonHandler : IEndpointRouteHandler
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/people", GetListAsync)
                .Produces<IEnumerable<Person>>(statusCode: StatusCodes.Status200OK);

            app.MapGet("/api/people/{id:guid}", GetAsync)
                .Produces<Person>(statusCode: StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/api/people", InsertAsync)
                .Accepts<Person>(MediaTypeNames.Application.Json)
                .Produces<Guid>(statusCode: StatusCodes.Status200OK)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);

            app.MapPut("/api/people/{id:guid}", UpdateAsync)
                .Accepts<Person>(MediaTypeNames.Application.Json)
                .Produces<Guid>(statusCode: StatusCodes.Status200OK)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError);

            app.MapDelete("/api/people/{id:guid}", DeleteAsync)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> GetListAsync(IPeopleService peopleService) => Results.Ok(await peopleService.GetListAsync());

        private async Task<IResult> GetAsync(Guid id, IPeopleService peopleService, ILogger<PersonHandler> logger)
        {
            var person = await peopleService.GetAsync(id: id);
            if (person is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(person);
        }

        private async Task<IResult> InsertAsync(Person person, IPeopleService peopleService, IValidator<Person> personValidator, ILogger<PersonHandler> logger)
        {
            try
            {
                var res = await peopleService.InsertAsync(person: person);
                return Results.Ok(res);
            }
            catch (ValidationException validationException)
            {
                logger.LogError(validationException, validationException.Message);
                return Results.ValidationProblem(validationException.ToDictionary());
            }
        }

        private async Task<IResult> UpdateAsync(Guid id, Person person, IPeopleService peopleService)
        {
            try
            {
                var res = await peopleService.UpdateAsync(id: id, person: person);
                if (res is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(res);
            }
            catch (ValidationException validationException)
            {
                return Results.ValidationProblem(validationException.ToDictionary());
            }
        }

        private static async Task<IResult> DeleteAsync(Guid id, IPeopleService peopleService)
        {
            var res = await peopleService.DeleteAsync(id: id);
            if (res == 0)
            {
                return Results.NotFound();
            }

            return await Task.FromResult(Results.NoContent());
        }
    }
}
