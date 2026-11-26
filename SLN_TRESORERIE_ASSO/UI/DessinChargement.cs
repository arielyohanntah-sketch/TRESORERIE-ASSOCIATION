// =====================================================================================
//  UI/DessinChargement.cs — DESSIN DE L'ÉCRAN DE CHARGEMENT
// -------------------------------------------------------------------------------------
//  Placé dans SplashForm. SplashForm change Progres (0 → 1) et Message au fur et à
//  mesure des vérifications ; ce contrôle redessine alors les pièces et la barre.
//
//  Le dessin est fait pour une taille de 640 × 380 puis agrandi/réduit (ScaleTransform)
//  pour remplir le contrôle : les coordonnées ci-dessous sont donc dans ce repère 640 × 380.
//  ➜ Pour changer les textes (titre, sous-titre, version), modifiez les DrawString de Dessiner().
// =====================================================================================

using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace SLN_TRESORERIE_ASSO.UI;

/// <summary>
/// Dessin de l'écran de chargement : fond bleu nuit, piles de pièces d'or, titre et barre de progression.
/// C'est un contrôle à part pour que Visual Studio puisse l'afficher dans le concepteur.
/// </summary>
[DesignerCategory("")]
public sealed class DessinChargement : Control
{
    // Taille de référence du dessin
    private const float Largeur = 640f, Hauteur = 380f;
    // Nombre de pièces dans chacune des 3 piles quand la progression atteint 100 %
    private static readonly int[] CapacitePiles = { 9, 13, 7 };
    // Pour chaque pièce (dans l'ordre d'arrivée), le numéro de la pile où elle tombe
    private readonly List<int> _sequence = new();

    /// <summary>Progression de 0 à 1 (0,45 par défaut pour l'aperçu dans le concepteur).</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public float Progres { get; set; } = 0.45f;

    /// <summary>Texte affiché au-dessus de la barre de progression.</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Message { get; set; } = "Ouverture du coffre-fort…";

    public DessinChargement()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Nuit;

        // Ordre d'arrivée des pièces : index de pile pour chaque pièce
        var restant = (int[])CapacitePiles.Clone();
        for (int tour = 0; _sequence.Count < CapacitePiles.Sum(); tour++)
        {
            int pile = new[] { 1, 0, 2, 1, 0, 1, 2 }[tour % 7];
            if (restant[pile] > 0) { _sequence.Add(pile); restant[pile]--; }
            else
            {
                for (int k = 0; k < 3; k++)
                    if (restant[k] > 0) { _sequence.Add(k); restant[k]--; break; }
            }
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        // Une erreur de dessin ne doit jamais faire planter l'application ni le concepteur de Visual Studio
        try { Dessiner(e); } catch (Exception) { }
    }

    private void Dessiner(PaintEventArgs e)
    {
        var g = e.Graphics;
        if (Width <= 0 || Height <= 0) return;
        float s = Math.Min(Width / Largeur, Height / Hauteur);   // le dessin s'adapte à la taille du contrôle
        g.ScaleTransform(s, s);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        // Fond dégradé bleu nuit + fin cadre doré
        var zone = new RectangleF(0, 0, Largeur, Hauteur);
        using (var fond = new LinearGradientBrush(zone, Theme.Nuit, Theme.NuitFonce, LinearGradientMode.Vertical))
            g.FillRectangle(fond, zone);
        using (var liseré = new Pen(Color.FromArgb(120, Theme.Or), 1))
            g.DrawRectangle(liseré, 0, 0, Largeur - 1, Hauteur - 1);

        // Moitié gauche : les piles de pièces
        DessinerPieces(g);

        // Moitié droite (à partir de x = 300) : titre, sous-titre, message et barre de progression
        float x = 300;
        using var blanc = new SolidBrush(Color.White);
        using var bleuPale = new SolidBrush(Color.FromArgb(190, 210, 230));
        using var or = new SolidBrush(Theme.Or);
        using var discret = new SolidBrush(Color.FromArgb(130, 160, 190));
        using var titre = new Font("Segoe UI Semibold", 30f, GraphicsUnit.Pixel);
        using var sousTitre = new Font("Segoe UI", 14f, GraphicsUnit.Pixel);
        using var petit = new Font("Segoe UI", 12f, GraphicsUnit.Pixel);
        using var message = new Font("Segoe UI", 13f, GraphicsUnit.Pixel);

        g.DrawString("Trésorerie Asso", titre, blanc, x, 92);
        g.DrawString("Gestion de la trésorerie de l'association", sousTitre, bleuPale, x + 2, 138);
        g.FillRectangle(or, x + 4, 172, 46, 3);
        g.DrawString("Caisse · Banque · Mobile Money", petit, bleuPale, x + 2, 186);

        // Message, puis barre de progression : piste grise transparente + partie remplie dorée
        g.DrawString(Message, message, blanc, x + 2, 252);
        var piste = new RectangleF(x + 4, 280, 300, 8);
        using (var chemin = Theme.Arrondi(piste, 4))
        using (var brosse = new SolidBrush(Color.FromArgb(45, 255, 255, 255)))
            g.FillPath(brosse, chemin);
        if (Progres > 0.01f)
        {
            var plein = new RectangleF(piste.X, piste.Y, Math.Max(8, piste.Width * Progres), piste.Height);
            using var chemin = Theme.Arrondi(plein, 4);
            using var brosse = new LinearGradientBrush(plein, Theme.Or, Color.FromArgb(247, 214, 120), LinearGradientMode.Horizontal);
            g.FillPath(brosse, chemin);
        }
        g.DrawString($"{(int)Math.Round(Progres * 100)} %", petit, or, x + 4, 294);
        g.DrawString("Version 1.0 · Projet L2 — Base de données", petit, discret, 24, Hauteur - 30);
    }

    /// <summary>Trois piles de pièces d'or qui montent avec la progression ; la dernière pièce tombe.</summary>
    private void DessinerPieces(Graphics g)
    {
        const float sol = 300;
        const float largeurPiece = 64, hauteurPiece = 20, pas = 8;
        float[] centres = { 82, 160, 238 };

        // Ombre ovale sous les piles
        using (var ombre = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
            g.FillEllipse(ombre, 30, sol + 8, 260, 22);

        // Nombre de pièces posées selon la progression ; la partie décimale = pièce en train de tomber
        double nb = Progres * _sequence.Count;
        int completes = (int)Math.Floor(nb);
        float fraction = (float)(nb - completes);
        var hauteurs = new int[3];

        for (int i = 0; i < Math.Min(completes + 1, _sequence.Count); i++)
        {
            int pile = _sequence[i];
            float y = sol - hauteurs[pile] * pas;
            hauteurs[pile]++;
            float decalage = 0;
            int alpha = 255;
            if (i == completes)
            {
                if (fraction <= 0.02f) break;
                float t = 1 - (float)Math.Pow(1 - fraction, 3);   // chute amortie
                decalage = -(1 - t) * 140;
                alpha = (int)(255 * Math.Min(1, fraction * 2.5f));
            }
            float cx = centres[pile] + ((i * 37) % 7 - 3);   // léger désalignement naturel
            DessinerPiece(g, cx - largeurPiece / 2, y + decalage - hauteurPiece, largeurPiece, hauteurPiece, alpha);
        }
    }

    /// <summary>Une pièce d'or vue en perspective (réutilisée par le bandeau du tableau de bord).</summary>
    public static void DessinerPiece(Graphics g, float x, float y, float l, float h, int alpha)
    {
        // Une pièce = une tranche foncée (ellipse + rectangle) puis le dessus clair par-dessus
        float epaisseur = 7;
        using var tranche = new SolidBrush(Color.FromArgb(alpha, Theme.OrFonce));
        using var dessus = new LinearGradientBrush(new RectangleF(x, y, l, h),
            Color.FromArgb(alpha, 250, 214, 110), Color.FromArgb(alpha, Theme.Or), LinearGradientMode.ForwardDiagonal);
        using var bord = new Pen(Color.FromArgb(alpha, 150, 105, 20), 1);
        using var anneau = new Pen(Color.FromArgb(alpha / 2, 255, 240, 190), 1.2f);

        g.FillEllipse(tranche, x, y + epaisseur, l, h);
        g.FillRectangle(tranche, x, y + h / 2, l, epaisseur);
        g.FillEllipse(dessus, x, y, l, h);
        g.DrawEllipse(bord, x, y, l, h);
        g.DrawEllipse(anneau, x + 9, y + 3.5f, l - 18, h - 7);
    }
}
