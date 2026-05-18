using System;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Rules;

namespace Szlakomat.Scoring.Domain.Fuzzy.Nodes;

public class FuzzyRatingNode : IRuleNode
{
    public double Evaluate(UserCategoryProjection projection)
    {
        return projection.AverageRating / 5.0;
    }
}
