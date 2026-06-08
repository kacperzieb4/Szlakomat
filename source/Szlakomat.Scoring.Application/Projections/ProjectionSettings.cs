namespace Szlakomat.Scoring.Application.Projections;

public class ProjectionSettings
{
    public const string SectionName = "ProjectionSettings";

    public int RecentActivityWindowDays { get; set; } = 30;
    
    public int HistoryActivityWindowDays { get; set; } = 90;
}
