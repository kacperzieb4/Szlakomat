namespace Szlakomat.Scoring.Domain.Fuzzy;

public interface IScoreNormalizer
{
    double Normalize(double rawScore);
}
