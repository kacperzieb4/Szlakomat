using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Rules;

namespace Szlakomat.Scoring.Application.Stubs;

public class StubRuleNode : IRuleNode
{
    public double Evaluate(UserCategoryProjection projection)
    {
        // Symulacja zwrócenia surowej oceny o wartości 80.0 z silnika reguł (AST)
        return 80.0;
    }
}
