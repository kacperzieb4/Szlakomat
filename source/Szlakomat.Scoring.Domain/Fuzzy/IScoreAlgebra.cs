namespace Szlakomat.Scoring.Domain.Fuzzy;

public interface IScoreAlgebra
{
    double And(IEnumerable<double> values);
    double Or(IEnumerable<double> values);
    double Not(double value);
}
