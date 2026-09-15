using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApiBooking.ConsoleApp;
using WebApiBooking.Infrastructure;

if (args.Length == 0)
{
    Console.WriteLine("Usage: WebApiBooking.Migration <path-to-json-file>");
    return 1;
}

var filePath = args[0];

if (!File.Exists(filePath))
{
    Console.WriteLine($"File {filePath} not found");
    return 1;
}

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<MigrationService>();

using var host = builder.Build();

using var scope = host.Services.CreateScope();
var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();

try
{
    await migrationService.RunAsync(filePath);
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}

Console.WriteLine("Migration completed");
return 0;