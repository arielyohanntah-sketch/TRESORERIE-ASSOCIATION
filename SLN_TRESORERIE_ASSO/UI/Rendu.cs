// =====================================================================================
//  UI/Rendu.cs — COULEURS DES BARRES (barre d'outils, menus déroulants, barre d'état)
// -------------------------------------------------------------------------------------
//  WinForms dessine les ToolStrip / MenuStrip / StatusStrip avec un « renderer ».
//  Ici on lui donne nos couleurs (celles de Theme.cs) au lieu des dégradés par défaut.
//  Utilisé dans MainForm : _outils.Renderer = new RenduBarres();
//  ➜ Pour changer la couleur de la barre d'état du bas, modifiez StatusStripGradient….
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.UI;

/// <summary>Couleurs des menus, barres d'outils et barre d'état.</summary>
public sealed class CouleursBarres : ProfessionalColorTable
{
    // Fond de la barre d'outils (même couleur au début, au milieu et à la fin = pas de dégradé)
    public override Color ToolStripGradientBegin => Theme.FondBarre;
    public override Color ToolStripGradientMiddle => Theme.FondBarre;
    public override Color ToolStripGradientEnd => Theme.FondBarre;
    public override Color ToolStripBorder => Theme.Bordure;

    // Menus déroulants (ex. menu de l'utilisateur en haut à droite)
    public override Color MenuStripGradientBegin => Color.White;
    public override Color MenuStripGradientEnd => Color.White;
    public override Color MenuItemSelected => Theme.Selection;
    public override Color MenuItemBorder => Theme.Accent;
    public override Color MenuItemSelectedGradientBegin => Theme.Selection;
    public override Color MenuItemSelectedGradientEnd => Theme.Selection;
    public override Color MenuItemPressedGradientBegin => Theme.Selection;
    public override Color MenuItemPressedGradientEnd => Theme.Selection;
    public override Color ImageMarginGradientBegin => Theme.FondBarre;
    public override Color ImageMarginGradientMiddle => Theme.FondBarre;
    public override Color ImageMarginGradientEnd => Theme.FondBarre;

    // Boutons de la barre d'outils : survolés (Selected) et enfoncés (Pressed)
    public override Color ButtonSelectedGradientBegin => Theme.Selection;
    public override Color ButtonSelectedGradientMiddle => Theme.Selection;
    public override Color ButtonSelectedGradientEnd => Theme.Selection;
    public override Color ButtonSelectedBorder => Theme.Accent;
    public override Color ButtonPressedGradientBegin => Color.FromArgb(170, 210, 245);
    public override Color ButtonPressedGradientMiddle => Color.FromArgb(170, 210, 245);
    public override Color ButtonPressedGradientEnd => Color.FromArgb(170, 210, 245);
    public override Color ButtonPressedBorder => Theme.Accent;
    public override Color SeparatorDark => Color.FromArgb(205, 205, 205);
    public override Color SeparatorLight => Color.White;

    // Barre d'état en bas de la fenêtre principale (bleue)
    public override Color StatusStripGradientBegin => Theme.Accent;
    public override Color StatusStripGradientEnd => Theme.Accent;
}

/// <summary>Dessinateur des barres qui utilise les couleurs ci-dessus, sans coins arrondis.</summary>
public sealed class RenduBarres : ToolStripProfessionalRenderer
{
    public RenduBarres() : base(new CouleursBarres()) { RoundedEdges = false; }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        // pas de bordure arrondie autour de la barre d'outils ni de la barre d'état
        if (e.ToolStrip is ToolStripDropDown) base.OnRenderToolStripBorder(e);
    }
}
