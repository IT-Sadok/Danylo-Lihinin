using Moq;
using Shouldly;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interfaces;
using WebApiBooking.Application.Services;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Tests.Services;

public class ApartmentServiceTests
{
    private readonly Mock<IApartmentRepository> _apartmentRepositoryMock;
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly ApartmentService _apartmentService;

    public ApartmentServiceTests()
    {
        _apartmentRepositoryMock = new Mock<IApartmentRepository>();
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _apartmentService = new ApartmentService(_apartmentRepositoryMock.Object, _bookingRepositoryMock.Object);
    }

    private static UpsertApartmentDto ValidDto(int? id = null) => new()
    {
        Id = id,
        Name = "Обухів",
        Price = 8000,
        Rooms = 2,
        CustomData = null
    };

    [Fact]
    public async Task UpsertApartmentAsync_NewApartment_DoesNotCheckExistingAndCallsUpsert()
    {
        var dto = ValidDto(id: null);
        _apartmentRepositoryMock
            .Setup(x => x.UpsertApartmentAsync(dto, 1))
            .ReturnsAsync(42);

        var result = await _apartmentService.UpsertApartmentAsync(dto, currentHostId: 1);

        result.ShouldBe(42);
        _apartmentRepositoryMock.Verify(x => x.GetApartmentByIdAsync(It.IsAny<int>()), Times.Never);
        _apartmentRepositoryMock.Verify(x => x.UpsertApartmentAsync(dto, 1), Times.Once);
    }

    [Fact]
    public async Task UpsertApartmentAsync_ExistingApartmentOwnedByCaller_CallsUpsert()
    {
        var dto = ValidDto(id: 917);
        var existingApartment = new Apartment { Id = 917, HostId = 1 };

        _apartmentRepositoryMock
            .Setup(x => x.GetApartmentByIdAsync(917))
            .ReturnsAsync(existingApartment);
        _apartmentRepositoryMock
            .Setup(x => x.UpsertApartmentAsync(dto, 1))
            .ReturnsAsync(917);

        var result = await _apartmentService.UpsertApartmentAsync(dto, currentHostId: 1);

        result.ShouldBe(917);
        _apartmentRepositoryMock.Verify(x => x.UpsertApartmentAsync(dto, 1), Times.Once);
    }

    [Fact]
    public async Task UpsertApartmentAsync_ApartmentNotFound_ThrowsKeyNotFoundException()
    {
        var dto = ValidDto(id: 999);
        _apartmentRepositoryMock
            .Setup(x => x.GetApartmentByIdAsync(999))
            .ReturnsAsync((Apartment?)null);

        await Should.ThrowAsync<KeyNotFoundException>(
            () => _apartmentService.UpsertApartmentAsync(dto, currentHostId: 1));

        _apartmentRepositoryMock.Verify(
            x => x.UpsertApartmentAsync(It.IsAny<UpsertApartmentDto>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task UpsertApartmentAsync_ApartmentOwnedByDifferentHost_ThrowsUnauthorizedAccessException()
    {
        var dto = ValidDto(id: 917);
        var existingApartment = new Apartment { Id = 917, HostId = 2 };

        _apartmentRepositoryMock
            .Setup(x => x.GetApartmentByIdAsync(917))
            .ReturnsAsync(existingApartment);

        await Should.ThrowAsync<UnauthorizedAccessException>(
            () => _apartmentService.UpsertApartmentAsync(dto, currentHostId: 1));

        _apartmentRepositoryMock.Verify(
            x => x.UpsertApartmentAsync(It.IsAny<UpsertApartmentDto>(), It.IsAny<int>()), Times.Never);
    }
}