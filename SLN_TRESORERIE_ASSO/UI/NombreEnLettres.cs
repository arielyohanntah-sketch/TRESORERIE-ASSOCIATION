// =====================================================================================
//  UI/NombreEnLettres.cs — ÉCRIRE UN MONTANT EN TOUTES LETTRES (français)
// -------------------------------------------------------------------------------------
//  Utilisé sur les reçus : 120 000 → « Cent vingt mille francs CFA ».
//  Règles de l'orthographe traditionnelle :
//    - « et » dans 21, 31, 41, 51, 61, 71 (vingt et un, soixante et onze), pas dans 81, 91 ;
//    - « quatre-vingts » et « deux cents » prennent un s seulement en fin de nombre ;
//    - « mille » est invariable ; « million » et « milliard » prennent un s au pluriel ;
//    - trait d'union seulement entre les dizaines et les unités (dix-sept, quatre-vingt-dix).
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.UI;

public static class NombreEnLettres
{
    private static readonly string[] Unites =
    {
        "zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf",
        "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf"
    };
    private static readonly string[] Dizaines =
        { "", "", "vingt", "trente", "quarante", "cinquante", "soixante", "soixante", "quatre-vingt", "quatre-vingt" };

    /// <summary>Montant arrondi à l'unité, en lettres, avec la monnaie : « Cent vingt mille francs CFA ».</summary>
    public static string Montant(decimal montant, string monnaie)
    {
        long n = (long)Math.Round(Math.Abs(montant), MidpointRounding.AwayFromZero);
        string texte = Convertir(n);
        // « un million de francs », « deux millions de francs » (mais « un million cinq cents francs »)
        bool de = n > 0 && (n % 1_000_000 == 0);
        texte = $"{texte} {(de ? "de " : "")}{monnaie}";
        return char.ToUpper(texte[0]) + texte[1..];
    }

    /// <summary>Un entier positif en lettres : 1981 → « mille neuf cent quatre-vingt-un ».</summary>
    public static string Convertir(long n)
    {
        if (n == 0) return "zéro";
        var parties = new List<string>();
        long milliards = n / 1_000_000_000, millions = n / 1_000_000 % 1000, milliers = n / 1000 % 1000, reste = n % 1000;
        if (milliards > 0) parties.Add($"{Centaines((int)milliards, false)} milliard{(milliards > 1 ? "s" : "")}");
        if (millions > 0) parties.Add($"{Centaines((int)millions, false)} million{(millions > 1 ? "s" : "")}");
        if (milliers > 0) parties.Add(milliers == 1 ? "mille" : $"{Centaines((int)milliers, false)} mille");
        if (reste > 0) parties.Add(Centaines((int)reste, true));
        return string.Join(" ", parties);
    }

    // 0 à 999. « fin » = vrai si rien ne suit (règle du s de « cents » et « quatre-vingts »)
    private static string Centaines(int n, bool fin)
    {
        int c = n / 100, r = n % 100;
        string texte = c switch
        {
            0 => "",
            1 => "cent",
            _ => $"{Unites[c]} cent{(r == 0 && fin ? "s" : "")}"
        };
        if (r > 0) texte = (texte.Length > 0 ? texte + " " : "") + Dizaine(r, fin);
        return texte;
    }

    // 1 à 99
    private static string Dizaine(int n, bool fin)
    {
        if (n < 20) return Unites[n];
        int d = n / 10, u = n % 10;
        // 70-79 et 90-99 : soixante-dix…, quatre-vingt-dix…
        if (d == 7 || d == 9) u += 10;
        string dizaine = Dizaines[d];
        if (u == 0) return d == 8 && fin ? "quatre-vingts" : dizaine;
        if ((u == 1 || u == 11) && d < 8) return $"{dizaine} et {Unites[u]}";
        return $"{dizaine}-{Unites[u]}";
    }
}
