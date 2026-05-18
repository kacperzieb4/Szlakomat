using System;

namespace Szlakomat.Scoring.Domain.Fuzzy;

public class ScoreNormalizer : IScoreNormalizer
{
    public double Normalize(double rawScore)
    {
        // Wartości ujemne interpretowane są jako całkowity brak dopasowania.
        if (rawScore <= 0.0) return 0.0;

        // Prawidłowe wartości w domenie logiki rozmytej (0.0 - 1.0) pozostają niezmienione.
        if (rawScore <= 1.0) return rawScore;

        // Wartości przekraczające dozwolony zakres (np. generowane przez stałe węzły punktowe)
        // są przycinane do maksymalnej wartości dopasowania wynoszącej 1.0.
        return 1.0;
    }
}
