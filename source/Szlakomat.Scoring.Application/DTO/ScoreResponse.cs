namespace Szlakomat.Scoring.Application.DTO;

public class ScoreResponse
{
    public double Score { get; set; }

    public List<string> Reasons { get; set; } = [];
}