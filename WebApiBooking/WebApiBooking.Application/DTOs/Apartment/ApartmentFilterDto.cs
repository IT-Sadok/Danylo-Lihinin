namespace WebApiBooking.Application.DTOs;

public record ApartmentFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}