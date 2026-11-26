// =====================================================================================
//  UI/Graphiques.cs — CONTRÔLES DESSINÉS DU TABLEAU DE BORD
// -------------------------------------------------------------------------------------
//    BandeauBienvenue : bandeau bleu « Bienvenue, Rôle Prénom Nom » avec des pièces d'or
//    GraphiqueMois    : histogramme recettes (vert) / dépenses (rouge) sur 6 mois
//
//  Ces contrôles n'ont pas de fichier .Designer.cs : tout est dessiné avec Graphics dans
//  la méthode Dessiner(). Ils apparaissent quand même dans le concepteur de
//  TableauDeBordVue (on peut les déplacer / redimensionner là-bas).
//
//  Rappel sur le dessin : (0,0) = coin haut-gauche ; x vers la droite, y vers le bas.
//  Theme.Px(n) adapte une longueur à l'agrandissement de l'écran.
// =====================================================================================

using System.Drawing.Drawing2D;

namespace SLN_TRESORERIE_ASSO.UI;

/// <summary>Bandeau de bienvenue du tableau de bord (nom, rôle, date) avec des pièces qui s'empilent.</summary>
[System.ComponentModel.DesignerCategory("")]   // contrôle dessiné : s'ouvre en code dans Visual Studio
public sealed class BandeauBienvenue : Control
{
    // Minuterie de l'animation d'apparition (~60 images par seconde)
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 16 };
    private float _progres = 1f;   // complet par défaut : visible tel quel dans le concepteur
    // Les trois lignes de texte. Elles sont remplies par TableauDeBordVue.Actualiser() ;
    // les valeurs ci-dessous ne servent que d'aperçu dans le concepteur.
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Ligne1 { get; set; } = "Bienvenue, Rôle Prénom Nom";
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Ligne2 { get; set; } = "Description du rôle de l'utilisateur connecté.";
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Ligne3 { get; set; } = "Bonjour ! Date du jour · exercice en cours";

    public BandeauBienvenue()
    {
        // Double tampon = pas de scintillement ; ResizeRedraw = redessiner quand la taille change
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        _timer.Tick += (_, _) =>
        {
            // +0,025 par image : l'animation dure environ 40 images (≈ 0,65 s)
            _progres = Math.Min(1f, _progres + 0.025f);
            Invalidate();
            if (_progres >= 1f) _timer.Stop();
        };
    }

    /// <summary>Relance l'animation (texte qui glisse, pièces qui s'empilent) depuis le début.</summary>
    public void Animer()
    {
        _progres = 0;
        _timer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        // Une erreur de dessin ne doit jamais faire planter l'application ni le concepteur de Visual Studio
        try { Dessiner(e); } catch (Exception) { }
    }

    private void Dessiner(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var r = ClientRectangle;
        if (r.Width <= 0 || r.Height <= 0) return;

        // Fond dégradé bleu nuit → bleu, avec un liseré doré en bas
        using (var fond = new LinearGradientBrush(r, Theme.Nuit, Theme.Accent, LinearGradientMode.Horizontal))
            g.FillRectangle(fond, r);
        using (var or = new SolidBrush(Theme.Or))
            g.FillRectangle(or, 0, r.Height - Theme.Px(4), r.Width, Theme.Px(4));

        // Le texte apparaît en glissant légèrement
        float t = 1 - (1 - Math.Min(1f, _progres * 2.2f)) * (1 - Math.Min(1f, _progres * 2.2f));
        int decalage = (int)((1 - t) * Theme.Px(18));
        int alpha = (int)(255 * t);
        int x = Theme.Px(24) + decalage;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        using var titre = new Font("Segoe UI Semibold", 17f);
        using var blanc = new SolidBrush(Color.FromArgb(alpha, Color.White));
        using var bleu = new SolidBrush(Color.FromArgb(alpha, 215, 232, 250));
        using var or2 = new SolidBrush(Color.FromArgb(alpha, Theme.Or));
        // Ligne 1 : grand titre blanc ; ligne 2 : description bleu pâle ; ligne 3 : date en doré
        g.DrawString(Ligne1, titre, blanc, x, Theme.Px(12));
        g.DrawString(Ligne2, Theme.Police, bleu, x + Theme.Px(2), Theme.Px(52));
        g.DrawString(Ligne3, Theme.PoliceGras, or2, x + Theme.Px(2), Theme.Px(74));

        // Pièces d'or à droite : 3 piles de 4, 6 et 3 pièces qui apparaissent une à une
        int[] piles = { 4, 6, 3 };
        int total = piles.Sum();
        int visibles = (int)(_progres * total + 0.5f);
        float l = Theme.Px(44), h = Theme.Px(14), pas = Theme.Px(6);
        float sol = r.Height - Theme.Px(20);
        float baseX = r.Width - Theme.Px(200);
        int n = 0;
        var hauteurs = new int[3];
        for (int tour = 0; n < visibles; tour++)
        {
            // Ordre « naturel » de remplissage des piles (milieu, gauche, droite…)
            int pile = new[] { 1, 0, 2, 1, 0, 1, 2 }[tour % 7];
            if (hauteurs[pile] >= piles[pile]) { if (tour > 40) break; continue; }
            float px = baseX + pile * Theme.Px(56);
            DessinChargement.DessinerPiece(g, px, sol - hauteurs[pile] * pas - h, l, h, 255);
            hauteurs[pile]++;
            n++;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }
}

/// <summary>Histogramme animé des recettes et dépenses des six derniers mois.</summary>
[System.ComponentModel.DesignerCategory("")]   // contrôle dessiné : s'ouvre en code dans Visual Studio
public sealed class GraphiqueMois : Control
{
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 16 };
    // Une entrée par mois : (mois, total recettes, total dépenses) — fournie par Depot.SixDerniersMois()
    private List<(DateTime Mois, decimal Recettes, decimal Depenses)> _donnees = new();
    // 0 → 1 pendant l'animation de « pousse » des barres
    private float _progres;

    public GraphiqueMois()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        _timer.Tick += (_, _) =>
        {
            _progres = Math.Min(1f, _progres + 0.03f);
            Invalidate();
            if (_progres >= 1f) _timer.Stop();
        };
    }

    /// <summary>Donne les données à afficher et lance l'animation des barres.</summary>
    public void Definir(List<(DateTime, decimal, decimal)> donnees)
    {
        _donnees = donnees;
        _progres = 0;
        _timer.Start();
    }

    // Écriture courte d'un montant pour l'étiquette : 1 500 000 → « 1,5 M », 250 000 → « 250 k »
    private static string Court(decimal v) =>
        v >= 1_000_000 ? $"{v / 1_000_000:0.#} M" : v >= 1_000 ? $"{v / 1_000:0} k" : v.ToString("0");

    protected override void OnPaint(PaintEventArgs e)
    {
        // Une erreur de dessin ne doit jamais faire planter l'application ni le concepteur de Visual Studio
        try { Dessiner(e); } catch (Exception) { }
    }

    private void Dessiner(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(BackColor);
        if (_donnees.Count == 0)
        {
            // aperçu dans le concepteur, ou aucune donnée
            TextRenderer.DrawText(g, "Graphique des recettes et dépenses (6 derniers mois)", Theme.Police, ClientRectangle, Theme.TexteSecondaire,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            return;
        }

        // Zone des barres : marges gauche/droite, place en haut pour les étiquettes, en bas pour les mois
        int margeG = Theme.Px(10), margeD = Theme.Px(10), margeH = Theme.Px(34), margeB = Theme.Px(24);
        var zone = new Rectangle(margeG, margeH, Width - margeG - margeD, Height - margeH - margeB);
        if (zone.Width < 20 || zone.Height < 20) return;

        // La plus grande valeur donne la hauteur maximale ; 4 lignes de repère horizontales
        decimal max = Math.Max(1, _donnees.Max(d => Math.Max(d.Recettes, d.Depenses)));
        using (var grille = new Pen(Color.FromArgb(235, 235, 235)))
            for (int i = 0; i <= 3; i++)
            {
                int y = zone.Bottom - zone.Height * i / 3;
                g.DrawLine(grille, zone.Left, y, zone.Right, y);
            }

        // « douce » ralentit la fin de l'animation ; colonne = largeur réservée à un mois
        float douce = 1 - (1 - _progres) * (1 - _progres) * (1 - _progres);
        float colonne = zone.Width / (float)_donnees.Count;
        float barre = Math.Min(Theme.Px(22), colonne * 0.3f);
        // Couleurs des barres : modifiez ici pour changer le vert des recettes / le rouge des dépenses
        using var vert = new SolidBrush(Color.FromArgb(47, 125, 90));
        using var rouge = new SolidBrush(Color.FromArgb(214, 96, 77));
        using var petite = new Font("Segoe UI", 7.5f);

        for (int i = 0; i < _donnees.Count; i++)
        {
            // Pour chaque mois : barre verte à gauche du centre, barre rouge à droite
            var (mois, rec, dep) = _donnees[i];
            float cx = zone.Left + colonne * i + colonne / 2;
            float hr = (float)(rec / max) * zone.Height * douce;
            float hd = (float)(dep / max) * zone.Height * douce;
            var br = new RectangleF(cx - barre - Theme.Px(2), zone.Bottom - hr, barre, hr);
            var bd = new RectangleF(cx + Theme.Px(2), zone.Bottom - hd, barre, hd);
            if (hr > 0.5f) g.FillRectangle(vert, br);
            if (hd > 0.5f) g.FillRectangle(rouge, bd);

            // Étiquettes des montants, seulement une fois l'animation terminée
            if (_progres >= 1f)
            {
                if (rec > 0) TextRenderer.DrawText(g, Court(rec), petite, new Rectangle((int)br.X - Theme.Px(14), (int)br.Y - Theme.Px(16), (int)barre + Theme.Px(28), Theme.Px(14)), Color.FromArgb(31, 94, 66), TextFormatFlags.HorizontalCenter);
                // Si les deux barres ont presque la même hauteur, l'étiquette de la dépense est remontée pour ne pas chevaucher
                int remonte = Math.Abs(hr - hd) < Theme.Px(14) && rec > 0 ? Theme.Px(14) : 0;
                if (dep > 0) TextRenderer.DrawText(g, Court(dep), petite, new Rectangle((int)bd.X - Theme.Px(14), (int)bd.Y - Theme.Px(16) - remonte, (int)barre + Theme.Px(28), Theme.Px(14)), Color.FromArgb(150, 50, 35), TextFormatFlags.HorizontalCenter);
            }
            // Nom du mois sous les barres (le mois en cours en bleu foncé)
            TextRenderer.DrawText(g, mois.ToString("MMM"), Theme.Police,
                new Rectangle((int)(cx - colonne / 2), zone.Bottom + Theme.Px(4), (int)colonne, Theme.Px(18)),
                mois.Month == DateTime.Today.Month ? Theme.Nuit : Theme.TexteSecondaire, TextFormatFlags.HorizontalCenter);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }
}
