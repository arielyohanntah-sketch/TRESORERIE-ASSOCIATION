// =====================================================================================
//  Views/VirementsVue.cs — ÉCRAN « VIREMENTS »
// -------------------------------------------------------------------------------------
//  Contrôles (VirementsVue.Designer.cs) : _entete (titre) et _grille (liste).
//  Données : Depot.Virements(). Boutons « Nouveau » et « Annuler » : droit OPERER.
//  C'est l'écran le plus simple : un bon modèle à copier pour créer un nouvel écran.
// =====================================================================================

using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.Forms;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Views;

/// <summary>Liste des virements entre comptes (mise en page : VirementsVue.Designer.cs).</summary>
public partial class VirementsVue : VueBase
{
    public override string Titre => "Virements";

    public VirementsVue()
    {
        InitializeComponent();
        // Style commun des grilles + pastilles de statut colorées
        Theme.StyleGrille(_grille);
        Theme.ColorerOperations(_grille);
    }

    // Boutons de la barre d'outils (les deux premiers seulement si le rôle a le droit OPERER)
    public override IEnumerable<ToolStripItem> Outils()
    {
        var outils = new List<ToolStripItem>();
        if (Session.Peut("OPERER"))
        {
            outils.Add(Theme.Outil("Nouveau virement", Theme.IcoVirement, Theme.Accent, (_, _) => Principale?.NouveauVirement()));
            outils.Add(Theme.Outil("Annuler le virement", Theme.IcoAnnuler, Theme.Rouge, (_, _) => Annuler()));
            outils.Add(Separateur());
        }
        outils.Add(Theme.Outil("Exporter (Excel)", Theme.IcoExporter, Theme.Vert, (_, _) => Ui.ExporterCsv(_grille, "virements")));
        outils.Add(Theme.Outil("Actualiser", Theme.IcoActualiser, Theme.Gris, (_, _) => Executer(Actualiser, false)));
        return outils;
    }

    public override void Actualiser() => _grille.DataSource = Depot.Virements();

    // Annule le virement sélectionné après avoir demandé un motif
    private void Annuler()
    {
        if (_grille.CurrentRow == null) { Ui.Info("Sélectionnez un virement."); return; }
        if (_grille.CurrentRow.Cells["Statut"].Value?.ToString() == "ANNULE") { Ui.Info("Ce virement est déjà annulé."); return; }
        // id_virement est une colonne cachée de la grille
        int id = Convert.ToInt32(_grille.CurrentRow.Cells["id_virement"].Value);
        using var f = new MotifForm("Annuler le virement", "Les soldes des deux comptes seront rétablis");
        if (f.ShowDialog(this) != DialogResult.OK) return;
        bool ok = false;
        Executer(() => { Depot.AnnulerVirement(id, f.Motif); ok = true; });
        if (ok)
        {
            // Notification orange + ligne surlignée en orange qui s'estompe
            Principale?.Notifier("Virement annulé", $"Motif : {f.Motif}", TypeNotification.Alerte);
            Theme.Surligner(_grille, "id_virement", id, Color.FromArgb(255, 214, 160));
        }
    }
}
