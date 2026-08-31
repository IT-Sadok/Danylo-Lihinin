namespace WebApiBooking.Application.DTOs;

public record ApartmentFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}