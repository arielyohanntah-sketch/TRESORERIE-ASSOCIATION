// =====================================================================================
//  UI/Notification.cs — PETIT MESSAGE ANIMÉ (« toast ») EN BAS À DROITE
// -------------------------------------------------------------------------------------
//  Utilisation (depuis n'importe quel écran) :
//      Notification.Afficher(this, "Opération validée", "N° DEP-2026-00012", TypeNotification.Succes);
//  Le message apparaît en glissant, reste environ 3,5 s, puis disparaît en fondu.
//  Un clic dessus le fait disparaître tout de suite.
//
//  Réglages : TicsApparition / TicsAffichage / TicsDisparition (1 tic = 15 ms).
//  Ce fichier n'a pas de partie « Concepteur » : tout est dessiné dans OnPaint.
// =====================================================================================

using System.Drawing.Drawing2D;

namespace SLN_TRESORERIE_ASSO.UI;

/// <summary>Succes = vert avec coche, Alerte = orange, Info = bleu.</summary>
public enum TypeNotification { Succes, Info, Alerte }

/// <summary>
/// Petite notification animée qui glisse en bas à droite de la fenêtre principale
/// après une validation, un enregistrement, une annulation…
/// </summary>
[System.ComponentModel.DesignerCategory("")]   // contrôle dessiné : s'ouvre en code dans Visual Studio
public sealed class Notification : Form
{
    // Notification affichée en ce moment (une seule à la fois)
    private static Notification? _courante;

    // Minuterie de l'animation : un « tic » toutes les 15 ms
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 15 };
    private readonly string _titre;
    private readonly string _message;
    private readonly Color _couleur;
    private readonly string _glyphe;
    private readonly int _yFinal;
    private int _tic;
    // Durées en tics : apparition ≈ 0,25 s, affichage ≈ 3,5 s, disparition ≈ 0,3 s
    private const int TicsApparition = 16, TicsAffichage = 230, TicsDisparition = 20;

    // La notification ne prend pas le focus : l'utilisateur peut continuer à taper
    protected override bool ShowWithoutActivation => true;

    private Notification(Form proprietaire, string titre, string message, TypeNotification type)
    {
        _titre = titre;
        _message = message;
        // Couleur et icône selon le type de notification
        (_couleur, _glyphe) = type switch
        {
            TypeNotification.Succes => (Theme.Vert, Theme.IcoValider),
            TypeNotification.Alerte => (Color.FromArgb(194, 106, 0), Theme.IcoAnnuler),
            _ => (Theme.Accent, "")
        };

        // Petite fenêtre sans bordure ni bouton dans la barre des tâches, invisible au départ (Opacity = 0)
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        BackColor = Color.White;
        DoubleBuffered = true;
        Size = new Size(Theme.Px(380), Theme.Px(78));
        Opacity = 0;
        Owner = proprietaire;

        // Position : coin bas-droit de la fenêtre principale, un peu plus bas au départ (elle remonte en apparaissant)
        var zone = proprietaire.RectangleToScreen(proprietaire.ClientRectangle);
        int x = zone.Right - Width - Theme.Px(20);
        _yFinal = zone.Bottom - Height - Theme.Px(40);
        Location = new Point(x, _yFinal + Theme.Px(24));

        // Clic = passer directement à la disparition
        Click += (_, _) => _tic = Math.Max(_tic, TicsApparition + TicsAffichage);
        _timer.Tick += (_, _) => Animer();
    }

    /// <summary>Affiche une notification (remplace la précédente).</summary>
    public static void Afficher(Form? proprietaire, string titre, string message, TypeNotification type = TypeNotification.Succes)
    {
        if (proprietaire == null || proprietaire.IsDisposed) return;
        _courante?.Close();
        var n = new Notification(proprietaire, titre, message, type);
        _courante = n;
        n.FormClosed += (_, _) => { if (_courante == n) _courante = null; };
        n.Show(proprietaire);
        n._timer.Start();
    }

    // Appelée à chaque tic : fait apparaître, attend, puis fait disparaître la notification
    private void Animer()
    {
        _tic++;
        if (_tic <= TicsApparition)
        {
            // Phase 1 : fondu d'entrée + glissement vers le haut (courbe « douce » qui ralentit à la fin)
            float t = _tic / (float)TicsApparition;
            float douce = 1 - (1 - t) * (1 - t);
            Opacity = douce;
            Top = _yFinal + (int)((1 - douce) * Theme.Px(24));
        }
        else if (_tic > TicsApparition + TicsAffichage)
        {
            // Phase 3 : fondu de sortie puis fermeture (entre les deux, phase 2 : affichage fixe)
            int k = _tic - TicsApparition - TicsAffichage;
            Opacity = Math.Max(0, 1 - k / (double)TicsDisparition);
            if (k >= TicsDisparition)
            {
                _timer.Stop();
                Close();
            }
        }
        // Demande à Windows de redessiner (appelle OnPaint)
        Invalidate();
    }

    // Dessin de la notification : bordure, bande colorée à gauche, pastille, textes, barre de temps
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var r = ClientRectangle;

        // Bordure grise + bande de couleur sur le bord gauche
        using (var bord = new Pen(Color.FromArgb(205, 205, 205)))
            g.DrawRectangle(bord, 0, 0, r.Width - 1, r.Height - 1);
        using (var bande = new SolidBrush(_couleur))
            g.FillRectangle(bande, 0, 0, Theme.Px(6), r.Height);

        // Pastille ronde avec l'icône, qui « pousse » à l'apparition
        float echelle = Math.Min(1f, _tic / (float)TicsApparition * 1.15f);
        int diametre = (int)(Theme.Px(38) * echelle);
        int cx = Theme.Px(38), cy = r.Height / 2;
        using (var fond = new SolidBrush(_couleur))
            g.FillEllipse(fond, cx - diametre / 2, cy - diametre / 2, diametre, diametre);
        if (echelle > 0.8f)
        {
            var icone = Theme.Glyphe(_glyphe, Color.White, 20);
            g.DrawImage(icone, cx - icone.Width / 2, cy - icone.Height / 2);
        }

        // Barre de temps restant
        int restant = Math.Clamp(TicsApparition + TicsAffichage - _tic, 0, TicsAffichage);
        using (var barre = new SolidBrush(Color.FromArgb(60, _couleur)))
            g.FillRectangle(barre, Theme.Px(6), r.Height - Theme.Px(3), (r.Width - Theme.Px(6)) * restant / TicsAffichage, Theme.Px(3));

        // Titre en gras puis message sur deux lignes maximum (« … » si trop long)
        var zoneTexte = new Rectangle(Theme.Px(68), Theme.Px(12), r.Width - Theme.Px(80), Theme.Px(24));
        TextRenderer.DrawText(g, _titre, Theme.PoliceGras, zoneTexte, Color.FromArgb(27, 27, 27),
            TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        zoneTexte = new Rectangle(Theme.Px(68), Theme.Px(36), r.Width - Theme.Px(80), Theme.Px(36));
        TextRenderer.DrawText(g, _message, Theme.Police, zoneTexte, Theme.TexteSecondaire,
            TextFormatFlags.Left | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }
}
