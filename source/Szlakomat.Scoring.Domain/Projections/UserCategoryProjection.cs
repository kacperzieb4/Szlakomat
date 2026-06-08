using System;
using Szlakomat.Scoring.Domain.Events;
using Szlakomat.Scoring.Domain.ValueObjects;

namespace Szlakomat.Scoring.Domain.Projections;

public class UserCategoryProjection
{
    public Guid UserId { get; private set; }

    public TrailCategory Category { get; private set; }

    public int RecentClicks { get; private set; }

    public int HistoryPurchases { get; private set; }

    public int RecentSkips { get; private set; }

    public double AverageRating { get; private set; }

    private int _highRatings;
    private int _lowRatings;

    public UserCategoryProjection(Guid userId, TrailCategory category)
    {
        UserId = userId;
        Category = category;
    }

    // Metoda używana wyłącznie przez repozytorium (lub stępy) do odtwarzania stanu z bazy danych
    public static UserCategoryProjection Restore(
        Guid userId, TrailCategory category, 
        int recentClicks, int historyPurchases, int recentSkips, double averageRating)
    {
        return new UserCategoryProjection(userId, category)
        {
            RecentClicks = recentClicks,
            HistoryPurchases = historyPurchases,
            RecentSkips = recentSkips,
            AverageRating = averageRating
        };
    }

    public void Apply(UserEvent evt, bool isRecent)
    {
        switch (evt.Type)
        {
            case EventType.Click when isRecent:
                RecentClicks++;
                break;
            case EventType.Skip when isRecent:
                RecentSkips++;
                break;
            case EventType.Purchase:
                HistoryPurchases++;
                break;
            case EventType.HighRating:
                _highRatings++;
                break;
            case EventType.LowRating:
                _lowRatings++;
                break;
        }

        var totalRatings = _highRatings + _lowRatings;
        AverageRating = totalRatings > 0 
            ? ((_highRatings * 5.0) + (_lowRatings * 1.0)) / totalRatings 
            : 0.0;
    }
}
