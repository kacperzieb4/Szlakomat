using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.Fuzzy.Nodes;
using Szlakomat.Scoring.Domain.Rules.Composite;

namespace Szlakomat.Scoring.Domain.Rules;

/// <summary>
/// Fabryka domyślnego drzewa reguł AST.
/// Buduje kompozytowe drzewo z fuzzy nodes, composite nodes i wagami.
/// To jest miejsce, gdzie definiuje się "logikę biznesową" scoringu.
///
/// Drzewo domyślne:
///   Weighted(
///     clicks     * waga 2.0,
///     purchases  * waga 3.0,
///     rating     * waga 1.5,
///     skips      * waga 1.0
///   )
///
/// Wynik = średnia ważona czterech fuzzy nodes.
/// </summary>
public static class DefaultRuleTreeFactory
{
    public static IRuleNode Create(IScoreAlgebra algebra)
    {
        var builder = new RuleTreeBuilder(algebra);

        // Liście (leaf nodes) — istniejące fuzzy nodes
        var clicks = new FuzzyClicksNode(maxClicks: 10);
        var purchases = new FuzzyPurchasesNode(maxPurchases: 5);
        var rating = new FuzzyRatingNode();
        var skips = new FuzzySkipsNode(maxSkips: 10);

        // Drzewo AST — kompozycja ważona
        return builder.Weighted(
            (clicks, 2.0),
            (purchases, 3.0),
            (rating, 1.5),
            (skips, 1.0)
        );
    }
}
