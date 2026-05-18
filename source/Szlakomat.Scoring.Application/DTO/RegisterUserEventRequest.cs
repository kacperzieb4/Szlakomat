namespace Szlakomat.Scoring.Application.DTO;

public class RegisterUserEventRequest
{
    public Guid UserId { get; set; }

    public required string Category { get; set; }

    public required string EventType { get; set; }
}