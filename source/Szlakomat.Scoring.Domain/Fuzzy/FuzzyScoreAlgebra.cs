using System.Linq;
using System.Collections.Generic;

namespace Szlakomat.Scoring.Domain.Fuzzy;

public class FuzzyScoreAlgebra : IScoreAlgebra
{
    public double And(IEnumerable<double> values)
    {
        return values.DefaultIfEmpty(0.0).Min();
    }

    public double Or(IEnumerable<double> values)
    {
        return values.DefaultIfEmpty(0.0).Max();
    }

    public double Not(double value)
    {
        return 1.0 - value;
    }
}
