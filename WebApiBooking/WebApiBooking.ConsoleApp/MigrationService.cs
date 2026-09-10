using System.Text.Json;
using System.Text.Json.Serialization;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApiBooking.ConsoleApp.DTOs;
using WebApiBooking.Domain;
using WebApiBooking.Domain.Constants;
using WebApiBooking.Infrastructure.Persistence;

namespace WebApiBooking.ConsoleApp;

public class MigrationService
{
    private const string DefaultPassword = "1234567a";
    private const string CompanyPrefix = "TestCompany";
    private const int BatchSize = 50;

    private readonly BookingDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<MigrationService> _logger;

    public MigrationService(BookingDbContext dbContext, UserManager<User> userManager, ILogger<MigrationService> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task RunAsync(string filePath, CancellationToken cancellationToken = default)
    {
        _dbContext.Database.SetCommandTimeout(600);

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var hostProcessed = 0;

        try
        {
            await using var fileStream = new FileStream(filePath, FileMode.Open);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow};

            await foreach (var hostDto in JsonSerializer.DeserializeAsyncEnumerable<HostExportDto>(fileStream, options,
                               cancellationToken))
            {
                if (hostDto is null)
                    continue;
                var dtoExternalId = CompanyPrefix + "_" + hostDto.Id;
                if (await _userManager.Users.FirstOrDefaultAsync(user => user.ExternalId == dtoExternalId,
                        cancellationToken) is not null)
                    continue;

                var user = hostDto.Adapt<User>();
                user.Id = 0;
                user.UserName = hostDto.Email;
                user.ExternalId = dtoExternalId;
                var result = await _userManager.CreateAsync(user, DefaultPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(";", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException(errors);
                }

                await _userManager.AddToRoleAsync(user, Roles.Host);
                foreach (var apartmentDto in hostDto.Apartments)
                {
                    var apartment = apartmentDto.Adapt<Apartment>();
                    apartment.ExternalId = CompanyPrefix + "_" + apartment.Id;
                    apartment.Id = 0;
                    apartment.HostId = user.Id;
                    apartment.Host = user;
                    user.OwnedApartments.Add(apartment);
                }

                hostProcessed++;
                if (hostProcessed % BatchSize == 0)
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    _dbContext.ChangeTracker.Clear();
                    _logger.LogInformation("Processed {HostCount} hosts", hostProcessed);
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            if(hostProcessed == 0)
                _logger.LogWarning("No hosts were found in the file — check if the file is empty or has an unexpected format");
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}