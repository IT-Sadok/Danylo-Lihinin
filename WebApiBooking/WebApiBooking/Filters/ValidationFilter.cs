using FluentValidation;

namespace WebApiBooking.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var entity = context.Arguments.OfType<T>().FirstOrDefault();
        if (entity is null)
        {
            return Results.BadRequest("Request body is missing");
        }
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is not null)
        {
            var validationResult = await validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());
        }
        return await next(context);
    }
}