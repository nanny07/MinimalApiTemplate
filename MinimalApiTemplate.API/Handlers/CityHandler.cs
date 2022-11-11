using FluentValidation;
using MinimalApiTemplate.API.Helpers;
using MinimalApiTemplate.Routing;
using MinimapApiTemplate.BLL.Services;
using MinimapApiTemplate.Shared.Model;
using System.Net.Mime;

namespace MinimalApiTemplate.Handlers
{
    public class CityHandler : IEndpointRouteHandler
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/city", GetListAsync)
                .Produces<IEnumerable<City>>(statusCode: StatusCodes.Status200OK);

            app.MapGet("/api/city/{id:guid}", GetAsync)
                .Produces<City>(statusCode: StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/api/city", InsertAsync)
                .Accepts<City>(MediaTypeNames.Application.Json)
                .Produces<Guid>(statusCode: StatusCodes.Status200OK)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError);

            app.MapPut("/api/city/{id:guid}", UpdateAsync)
                .Accepts<City>(MediaTypeNames.Application.Json)
                .Produces<Guid>(statusCode: StatusCodes.Status200OK)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError);

            app.MapDelete("/api/city/{id:guid}", DeleteAsync)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> GetListAsync(ICityService cityService) => Results.Ok(await cityService.GetListAsync());

        private async Task<IResult> GetAsync(Guid id, ICityService cityService)
        {
            var city = await cityService.GetAsync(id: id);
            if (city is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(city);
        }

        private async Task<IResult> InsertAsync(City city, ICityService cityService, IValidator<City> cityValidator)
        {
            try
            {
                var res = await cityService.InsertAsync(city: city);
                return Results.Ok(res);
            }
            catch (ValidationException validationException)
            {
                return Results.ValidationProblem(validationException.ToDictionary());
            }
        }

        private async Task<IResult> UpdateAsync(Guid id, City city, ICityService cityService)
        {
            try
            {
                return await Task.FromResult(Results.NoContent());
            }
            catch (ValidationException validationException)
            {
                return Results.ValidationProblem(validationException.ToDictionary());
            }
        }

        private static async Task<IResult> DeleteAsync(Guid id, ICityService cityService)
        {
            var res = await cityService.DeleteAsync(id: id);
            if (res == 0)
            {
                return Results.NotFound();
            }

            return await Task.FromResult(Results.NoContent());
        }
    }
}
