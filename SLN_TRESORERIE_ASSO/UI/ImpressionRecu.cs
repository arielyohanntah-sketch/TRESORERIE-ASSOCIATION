// =====================================================================================
//  UI/ImpressionRecu.cs — DESSIN DES REÇUS ET BONS DE DÉPENSE À IMPRIMER
// -------------------------------------------------------------------------------------
//  Une page A4 contient deux exemplaires identiques séparés par un trait de découpe :
//    - en haut : l'exemplaire remis à la personne (le versant pour une recette,
//      le bénéficiaire pour une dépense) ;
//    - en bas  : la souche, gardée par le trésorier avec les pièces justificatives.
//
//  Contenu d'un exemplaire : coordonnées de l'association (Data/Association.cs),
//  titre REÇU ou BON DE DÉPENSE, n° de pièce, date, montant en chiffres et en lettres,
//  tiers, objet, catégorie, mode de paiement, compte, cases de signature.
//  Une pièce annulée est imprimée avec la mention « ANNULÉ » en filigrane.
//
//  Tout est dessiné en millimètres (g.PageUnit = Millimeter) : les nombres ci-dessous
//  sont des positions en mm. ➜ Pour décaler un élément, changez sa valeur en mm.
//  L'aperçu et les boutons Imprimer / PDF sont dans Forms/ApercuRecuForm.cs.
// =====================================================================================

using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Text;
using SLN_TRESORERIE_ASSO.Data;

namespace SLN_TRESORERIE_ASSO.UI;

public static class ImpressionRecu
{
    // Format A4 portrait et marges, en mm
    public const float LargeurPage = 210f, HauteurPage = 297f;
    private const float Marge = 12f;
    // Hauteur d'un exemplaire (deux par page) et espace pour le trait de découpe
    private const float HauteurExemplaire = 130f, Ecart = 13f;

    /// <summary>Prépare le document à imprimer (A4 portrait, une page avec deux exemplaires).</summary>
    public static PrintDocument Document(RecuMouvement r)
    {
        var doc = new PrintDocument { DocumentName = $"{Titre(r)} {r.NumeroPiece}" };
        try
        {
            // Choisit le format A4 de l'imprimante si elle le propose
            doc.DefaultPageSettings.Landscape = false;
            doc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            var a4 = doc.PrinterSettings.PaperSizes.Cast<PaperSize>().FirstOrDefault(p => p.Kind == PaperKind.A4);
            doc.DefaultPageSettings.PaperSize = a4 ?? new PaperSize("A4", 827, 1169);
        }
        catch (Exception)
        {
            // aucune imprimante installée : l'aperçu reste possible
        }
        doc.PrintPage += (_, e) =>
        {
            var g = e.Graphics!;
            g.PageUnit = GraphicsUnit.Millimeter;
            // Sur une vraie imprimante, (0,0) est le bord de la zone imprimable : on revient au coin de la feuille
            try { g.TranslateTransform(-e.PageSettings.HardMarginX * 0.254f, -e.PageSettings.HardMarginY * 0.254f); }
            catch (Exception) { }
            DessinerPage(g, r);
            e.HasMorePages = false;
        };
        return doc;
    }

    /// <summary>Dessine la page entière (unité : mm, origine au coin haut-gauche de la feuille).</summary>
    public static void DessinerPage(Graphics g, RecuMouvement r)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        float largeur = LargeurPage - 2 * Marge;

        // Exemplaire remis à la personne, puis souche
        DessinerExemplaire(g, new RectangleF(Marge, Marge, largeur, HauteurExemplaire), r,
            r.Recette ? "EXEMPLAIRE DU VERSANT" : "EXEMPLAIRE DU BÉNÉFICIAIRE");

        // Trait de découpe entre les deux
        float yCoupe = Marge + HauteurExemplaire + Ecart / 2;
        using (var pointille = new Pen(Color.FromArgb(150, 150, 150), 0.25f) { DashStyle = DashStyle.Dash })
            g.DrawLine(pointille, Marge + 14, yCoupe, LargeurPage - Marge, yCoupe);
        using (var ciseaux = new Font("Segoe UI Symbol", 11f))
        using (var gris = new SolidBrush(Color.FromArgb(120, 120, 120)))
        using (var petite = new Font("Segoe UI", 6.5f))
        {
            g.DrawString("✂", ciseaux, gris, Marge - 1, yCoupe - 3.2f);
            g.DrawString("découper ici", petite, gris, Marge + 16, yCoupe - 3.6f);
        }

        DessinerExemplaire(g, new RectangleF(Marge, Marge + HauteurExemplaire + Ecart, largeur, HauteurExemplaire), r,
            "SOUCHE — À CONSERVER");
    }

    // Titre principal selon le sens de l'opération
    private static string Titre(RecuMouvement r) => r.Recette ? "REÇU" : "BON DE DÉPENSE";

    // Code du mode de paiement → texte lisible
    private static string Mode(string code) => code switch
    {
        "ESPECES" => "Espèces",
        "CHEQUE" => "Chèque",
        "VIREMENT" => "Virement bancaire",
        "MOBILE_MONEY" => "Mobile Money",
        "CARTE" => "Carte bancaire",
        _ => code
    };

    /// <summary>Un exemplaire complet dans le rectangle z (en mm).</summary>
    private static void DessinerExemplaire(Graphics g, RectangleF z, RecuMouvement r, string exemplaire)
    {
        float x0 = z.X, y0 = z.Y, w = z.Width, h = z.Height;
        float gauche = x0 + 7, droite = x0 + w - 7;

        using var nuit = new SolidBrush(Theme.Nuit);
        using var texte = new SolidBrush(Color.FromArgb(30, 30, 30));
        using var gris = new SolidBrush(Color.FromArgb(105, 105, 105));
        using var fNom = new Font("Segoe UI Semibold", 13f);
        using var fPetit = new Font("Segoe UI", 7.5f);
        using var fTitre = new Font("Segoe UI", 17f, FontStyle.Bold);
        using var fNumero = new Font("Consolas", 10.5f, FontStyle.Bold);
        using var fNormal = new Font("Segoe UI", 9.5f);
        using var fLibelle = new Font("Segoe UI", 8f);
        using var fGras = new Font("Segoe UI Semibold", 10f);
        var aDroite = new StringFormat { Alignment = StringAlignment.Far };
        var uneLigne = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };

        // Cadre arrondi
        using (var cadre = Theme.Arrondi(new RectangleF(x0, y0, w, h), 2.5f))
        using (var stylo = new Pen(Theme.Nuit, 0.35f))
            g.DrawPath(stylo, cadre);

        // ---- En-tête : association à gauche, titre / numéro / date à droite
        g.DrawString(Association.Nom, fNom, nuit, new RectangleF(gauche, y0 + 5, 112, 8), uneLigne);
        if (Association.Mention.Length > 0)
            g.DrawString(Association.Mention, fPetit, gris, new RectangleF(gauche, y0 + 12.5f, 112, 4.5f), uneLigne);
        string coordonnees = string.Join("  ·  ", new[] { Association.Adresse, Association.Telephone, Association.Email }.Where(s => s.Length > 0));
        g.DrawString(coordonnees, fPetit, gris, new RectangleF(gauche, y0 + 16.5f, 112, 4.5f), uneLigne);

        g.DrawString(Titre(r), fTitre, nuit, new RectangleF(droite - 66, y0 + 3.5f, 66, 9), aDroite);
        g.DrawString($"N° {r.NumeroPiece}", fNumero, texte, new RectangleF(droite - 66, y0 + 12.5f, 66, 5.5f), aDroite);
        g.DrawString($"Date : {r.Date:dd/MM/yyyy}", fNormal, texte, new RectangleF(droite - 66, y0 + 18f, 66, 5.5f), aDroite);

        // Filet doré sous l'en-tête
        using (var or = new SolidBrush(Theme.Or))
            g.FillRectangle(or, gauche, y0 + 25, droite - gauche, 0.7f);

        // ---- Encadré du montant (à droite)
        var boite = new RectangleF(droite - 60, y0 + 31, 60, 21);
        using (var chemin = Theme.Arrondi(boite, 2f))
        using (var fond = new SolidBrush(Theme.BleuClair))
        using (var bord = new Pen(Theme.Accent, 0.3f))
        {
            g.FillPath(fond, chemin);
            g.DrawPath(bord, chemin);
        }
        g.DrawString("MONTANT", fPetit, gris, boite.X + 3, boite.Y + 2);
        using (var fMontant = new Font("Segoe UI", 15f, FontStyle.Bold))
            g.DrawString($"{Theme.Montant(r.Montant)} {Association.Monnaie}", fMontant, nuit,
                new RectangleF(boite.X, boite.Y + 8, boite.Width, 10), new StringFormat { Alignment = StringAlignment.Center });

        // ---- Lignes d'information (colonne de gauche)
        float xValeur = gauche + 29, finValeur = boite.X - 6;
        float y = y0 + 31;
        void Ligne(string libelle, string? valeur, float fin)
        {
            g.DrawString(libelle, fLibelle, gris, gauche, y + 0.8f);
            if (string.IsNullOrWhiteSpace(valeur))
                valeur = "";
            g.DrawString(valeur, fNormal, texte, new RectangleF(xValeur, y, fin - xValeur, 5.5f), uneLigne);
            // pointillé « carnet de reçus » sous la valeur (on peut y écrire à la main si elle est vide)
            using var pointille = new Pen(Color.FromArgb(185, 185, 185), 0.2f) { DashStyle = DashStyle.Dot };
            g.DrawLine(pointille, xValeur, y + 5.6f, fin, y + 5.6f);
            y += 8.2f;
        }
        Ligne(r.Recette ? "Reçu de" : "Payé à", r.Tiers, finValeur);
        Ligne("Objet", r.Libelle, finValeur);
        Ligne("Catégorie", r.Categorie, finValeur);
        Ligne("Paiement", Mode(r.ModePaiement) + (string.IsNullOrWhiteSpace(r.Reference) ? "" : $"  ·  réf. {r.Reference}"), finValeur);
        Ligne(r.Recette ? "Encaissé sur" : "Payé depuis", r.Compte, finValeur);

        // ---- Montant en toutes lettres (toute la largeur, deux lignes possibles)
        g.DrawString("La somme de", fLibelle, gris, gauche, y + 0.8f);
        using (var fLettres = new Font("Segoe UI Semibold", 10f, FontStyle.Italic))
            g.DrawString(NombreEnLettres.Montant(r.Montant, Association.MonnaieLettres), fLettres, texte,
                new RectangleF(xValeur, y, droite - xValeur, 10));
        using (var pointille = new Pen(Color.FromArgb(185, 185, 185), 0.2f) { DashStyle = DashStyle.Dot })
            g.DrawLine(pointille, xValeur, y + 10.2f, droite, y + 10.2f);
        y += 13.5f;

        // ---- « Fait à … le … », ou la mention d'annulation
        if (r.Statut == "ANNULE")
        {
            using var rouge = new SolidBrush(Theme.Rouge);
            g.DrawString($"Pièce ANNULÉE — motif : {r.MotifAnnulation}", fGras, rouge, new RectangleF(gauche, y, droite - gauche, 5.5f), uneLigne);
        }
        else
        {
            g.DrawString($"Fait à {Association.Ville}, le {r.Date:d MMMM yyyy}", fNormal, texte, gauche, y);
        }
        y += 6.5f;

        // ---- Cases de signature
        float hSignature = y0 + h - 9 - y, lSignature = 78;
        CaseSignature(g, new RectangleF(gauche, y, lSignature, hSignature), r.Recette ? "Signature du versant" : "Signature du bénéficiaire", fPetit, gris);
        CaseSignature(g, new RectangleF(droite - lSignature, y, lSignature, hSignature), "Le trésorier (signature et cachet)", fPetit, gris);

        // ---- Pied : qui a établi la pièce, et de quel exemplaire il s'agit
        using (var fPied = new Font("Segoe UI", 6.5f))
            g.DrawString($"Établi par {r.Auteur} le {r.DateSaisie:dd/MM/yyyy}  ·  imprimé le {DateTime.Now:dd/MM/yyyy à HH:mm}  ·  Trésorerie Asso",
                fPied, gris, new RectangleF(gauche, y0 + h - 6.5f, 120, 4), uneLigne);
        using (var fExemplaire = new Font("Segoe UI", 7f, FontStyle.Bold))
            g.DrawString(exemplaire, fExemplaire, nuit, new RectangleF(droite - 66, y0 + h - 6.6f, 66, 4), aDroite);

        // ---- Filigrane pour une pièce annulée
        if (r.Statut == "ANNULE")
        {
            var etat = g.Save();
            g.TranslateTransform(x0 + w / 2, y0 + h / 2);
            g.RotateTransform(-18);
            using var fFiligrane = new Font("Segoe UI", 54f, FontStyle.Bold);
            using var pale = new SolidBrush(Color.FromArgb(55, Theme.Rouge));
            var taille = g.MeasureString("ANNULÉ", fFiligrane);
            g.DrawString("ANNULÉ", fFiligrane, pale, -taille.Width / 2, -taille.Height / 2);
            g.Restore(etat);
        }
    }

    // Case de signature : cadre gris clair avec son intitulé en haut à gauche
    private static void CaseSignature(Graphics g, RectangleF z, string intitule, Font police, Brush pinceau)
    {
        using (var chemin = Theme.Arrondi(z, 1.5f))
        using (var bord = new Pen(Color.FromArgb(190, 190, 190), 0.25f))
            g.DrawPath(bord, chemin);
        g.DrawString(intitule, police, pinceau, z.X + 2.5f, z.Y + 1.8f);
    }
}
