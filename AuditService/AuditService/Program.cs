using System.Text;
using AuditService.Application;
using AuditService.Endpoints;
using AuditService.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var jwt = builder.Configuration.GetSection("Jwt");
var secretKey = jwt["SecretKey"];
 if(string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 32)
     throw new InvalidOperationException("Secret key is missing or secret key must be at least 32 symbols long");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
    });
builder.Services.AddAuthorization();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddServer("http://localhost:5000", "ReverseProxy");
    });
}
app.Use(async(context, next) =>
{
    context.Response.Headers["X-Served-By"] = context.Connection.LocalPort.ToString();
    await next.Invoke();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapAuditEndpoints();
app.MapHealthChecks("/health");


app.Run();