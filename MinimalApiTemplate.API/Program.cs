using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalApiTemplate.API.Helpers;
using MinimalApiTemplate.API.Swagger;
using MinimalApiTemplate.DAL;
using MinimalApiTemplate.Routing;
using MinimapApiTemplate.BLL.Services;
using MinimapApiTemplate.BLL.Validations;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

//Serilog
//Needed to clear the default Microsoft Logger to Console
builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();
builder.Logging.AddSerilog(logger);

//Cors
builder.Services.AddCors();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //Add the option to select the language header
    options.OperationFilter<AcceptLanguageHeaderOperationFilter>();
});

//Globalization
var supportedCultures = new CultureInfo[] { new("en"), new("it") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.DefaultRequestCulture = new RequestCulture(supportedCultures.First());
});

//FluentValidation
//Only one validator's type per Assembly it's needed 
builder.Services.AddValidatorsFromAssemblyContaining<PersonValidator>();
//Seems it will be fixed on the v.6.0.0 of the MicroElements.Swashbuckle.FluentValidation
//For now we will use the pre-release or you can uncomment the next row
//builder.Services.TryAddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
builder.Services.AddFluentValidationRulesToSwagger();

//ProblemDetails
builder.Services.TryAddSingleton<IActionResultExecutor<ObjectResult>, ProblemDetailsResultExecutor>();
builder.Services.AddProblemDetails(options =>
{
    //It can be setted even for a specific Exception class (ex: ArgumentNullException)
    //If you want to override the default Title/Detail messages.
    //options.Map<Exception>(ex =>
    //{
    //    var error = new StatusCodeProblemDetails(StatusCodes.Status500InternalServerError)
    //    {
    //        Title = "Internal error - Custom",
    //        Detail = ex.Message
    //    };

    //    return error;
    //});
});

//Other Services
builder.Services.AddDbContext<GenericContext>(options =>
{
    options.UseInMemoryDatabase("GenericDb");
    if (builder.Environment.IsDevelopment())
    {
        options.LogTo(Console.WriteLine);
        options.EnableSensitiveDataLogging();
    }
});
builder.Services.AddScoped<IPeopleService, PeopleService>();
builder.Services.AddScoped<ICityService, CityService>();


var app = builder.Build();

//Cors
app.UseCors(builder => builder
 .AllowAnyOrigin()
 .AllowAnyMethod()
 .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRequestLocalization();
app.UseProblemDetails();

//Map all the endpoint implementing IEndpointRouteHandler
app.MapEndpoints();

app.Run();