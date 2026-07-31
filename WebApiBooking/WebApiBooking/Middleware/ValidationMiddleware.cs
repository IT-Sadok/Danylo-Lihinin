using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.Metadata;

namespace WebApiBooking.Middleware;

public class ValidationMiddleware
{
    private readonly  RequestDelegate _next;
    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var acceptsMetadata = context.GetEndpoint()?.Metadata.GetMetadata<IAcceptsMetadata>();
        var dtoType = acceptsMetadata?.RequestType;

        if (dtoType == null || !context.Request.HasJsonContentType())
        {
            await _next(context);
            return;
        }
        
        context.Request.EnableBuffering();

        object? dto;
        try
        {
            dto = await context.Request.ReadFromJsonAsync(dtoType);
        }
        finally
        {
            context.Request.Body.Position = 0;
        }

        if (dto == null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
        var validatorType =  typeof(IValidator<>).MakeGenericType(dtoType);
        if (context.RequestServices.GetService(validatorType) is IValidator validator)
        {
            var validationContext = new ValidationContext<object>(dto);
            ValidationResult validationResult = await validator.ValidateAsync(validationContext, context.RequestAborted);
            if (!validationResult.IsValid)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(validationResult.ToDictionary());
                return;
            }
        }
        await _next(context);
    }
}