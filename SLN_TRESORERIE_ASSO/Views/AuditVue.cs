// =====================================================================================
//  Views/AuditVue.cs — JOURNAL D'AUDIT / MON ACTIVITÉ
// -------------------------------------------------------------------------------------
//  Le journal est rempli AUTOMATIQUEMENT par les triggers de la base : chaque création,
//  modification, validation ou annulation y est enregistrée avec son auteur.
//  Deux usages :
//    - « Journal d'audit » : tout le journal (administrateur, président, commissaire)
//    - « Mon activité »     : seulement mes propres actions (tout le monde)
//  Contrôles (AuditVue.Designer.cs) : _entete et _grille. Lecture seule.
// =====================================================================================

using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Views;

/// <summary>
/// Journal d'audit alimenté automatiquement par les triggers de la base,
/// ou « Mon activité » (seulement mes actions). Mise en page : AuditVue.Designer.cs.
/// </summary>
public partial class AuditVue : VueBase
{
    private readonly bool _seulementMoi;

    public override string Titre => _seulementMoi ? "Mon activité" : "Journal d'audit";

    /// <summary>Constructeur utilisé par le concepteur de Visual Studio.</summary>
    public AuditVue() : this(false) { }

    /// <param name="seulementMoi">Vrai pour « Mon activité » : uniquement les actions de l'utilisateur connecté.</param>
    public AuditVue(bool seulementMoi)
    {
        InitializeComponent();
        _seulementMoi = seulementMoi;
        if (seulementMoi) _entete.Text = $"Mon activité — {Session.NomComplet}";

        // Tag "datetime" : la colonne Date est affichée avec l'heure (voir Theme.AjusterColonnes)
        _grille.Tag = "datetime";
        Theme.StyleGrille(_grille);
        // Colonne « Action » en gras et colorée : vert = validation, rouge = annulation/suppression…
        _grille.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0 || _grille.Columns[e.ColumnIndex].Name != "Action") return;
            e.CellStyle!.Font = Theme.PoliceGras;
            e.CellStyle.ForeColor = e.Value?.ToString() switch
            {
                "VALIDATION" => Theme.Vert,
                "ANNULATION" or "SUPPRESSION" => Theme.Rouge,
                "CREATION" => Theme.Accent,
                _ => Theme.Orange
            };
        };
        // La colonne « Détails » (contenu JSON du changement) est plus large que les autres
        _grille.DataBindingComplete += (_, _) =>
        {
            if (_grille.Columns.Contains("Détails")) _grille.Columns["Détails"]!.FillWeight = 260;
        };
    }

    public override IEnumerable<ToolStripItem> Outils() => new ToolStripItem[]
    {
        Theme.Outil("Exporter (Excel)", Theme.IcoExporter, Theme.Vert, (_, _) => Ui.ExporterCsv(_grille, "journal_audit")),
        Theme.Outil("Actualiser", Theme.IcoActualiser, Theme.Gris, (_, _) => Executer(Actualiser, false))
    };

    public override void Actualiser() => _grille.DataSource = Depot.JournalAudit(_seulementMoi);
}
