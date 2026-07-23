using FluentValidation;
using WebApiBooking.Application.Validators;
using WebApiBooking.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();

var app = builder.Build();


app.UseHttpsRedirection();

app.Run();