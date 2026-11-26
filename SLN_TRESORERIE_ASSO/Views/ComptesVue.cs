// =====================================================================================
//  Views/ComptesVue.cs — ÉCRAN « COMPTES ET SOLDES »
// -------------------------------------------------------------------------------------
//  Contrôles (ComptesVue.Designer.cs) :
//      _comptes      grille du haut : tous les comptes et leur solde actuel
//      _total        trésorerie totale des comptes actifs
//      _cadreReleve  cadre du bas « Relevé — compte sélectionné »
//      _du, _au, _btnAfficher   période du relevé
//      _releve       grille du relevé : solde de départ, opérations, solde final
//  Cliquer sur un compte en haut affiche automatiquement son relevé en bas.
// =====================================================================================

using System.Data;
using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Views;

/// <summary>
/// Comptes avec leur solde actuel, et relevé détaillé du compte sélectionné.
/// Mise en page : ComptesVue.Designer.cs.
/// </summary>
public partial class ComptesVue : VueBase
{
    public override string Titre => "Comptes et soldes";

    public ComptesVue()
    {
        InitializeComponent();
        Theme.StyleGrille(_comptes);
        Theme.StyleGrille(_releve);
        // Période par défaut : du 1er du mois à aujourd'hui
        _du.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        _au.Value = DateTime.Today;

        // Grille des comptes : comptes inactifs en gris, solde en gras (rouge s'il est négatif)
        _comptes.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0) return;
            var row = _comptes.Rows[e.RowIndex];
            if (!Convert.ToBoolean(row.Cells["Actif"].Value)) e.CellStyle!.ForeColor = Theme.Gris;
            if (_comptes.Columns[e.ColumnIndex].Name == "Solde actuel")
            {
                e.CellStyle!.Font = Theme.PoliceGras;
                if (e.Value is decimal d && d < 0) e.CellStyle.ForeColor = Theme.Rouge;
            }
        };
        // Changement de compte sélectionné → nouveau relevé
        _comptes.SelectionChanged += (_, _) => ChargerReleve();

        // Grille du relevé : lignes de solde (report) en gras sur fond gris, entrées en vert, sorties en rouge
        _releve.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0) return;
            var row = _releve.Rows[e.RowIndex];
            if (row.Cells["report"].Value is true)
            {
                e.CellStyle!.BackColor = Theme.EnTeteGrille;
                e.CellStyle.Font = Theme.PoliceGras;
            }
            var col = _releve.Columns[e.ColumnIndex].Name;
            if (col == "Entrée") e.CellStyle!.ForeColor = Theme.Vert;
            if (col == "Sortie") e.CellStyle!.ForeColor = Theme.Rouge;
        };
        _btnAfficher.Click += (_, _) => ChargerReleve();
    }

    // Boutons selon les droits : gérer les fiches (PARAMETRER), virement (OPERER), export, actualiser
    public override IEnumerable<ToolStripItem> Outils()
    {
        var outils = new List<ToolStripItem>();
        if (Session.Peut("PARAMETRER"))
            outils.Add(Theme.Outil("Gérer les fiches des comptes", Theme.IcoModifier, Theme.Accent, (_, _) => Principale?.Naviguer("param_compte")));
        if (Session.Peut("OPERER"))
            outils.Add(Theme.Outil("Virement", Theme.IcoVirement, Theme.Accent, (_, _) => Principale?.NouveauVirement()));
        if (outils.Count > 0) outils.Add(Separateur());
        outils.Add(Theme.Outil("Exporter le relevé (Excel)", Theme.IcoExporter, Theme.Vert, (_, _) => Ui.ExporterCsv(_releve, "releve")));
        outils.Add(Theme.Outil("Actualiser", Theme.IcoActualiser, Theme.Gris, (_, _) => Executer(Actualiser, false)));
        return outils;
    }

    public override void Actualiser()
    {
        var dt = Depot.SoldesComptes();
        _comptes.DataSource = dt;
        // Total des soldes des comptes actifs uniquement
        decimal total = dt.Rows.Cast<DataRow>().Where(r => Convert.ToBoolean(r["Actif"])).Sum(r => Convert.ToDecimal(r["Solde actuel"]));
        _total.Text = $"Trésorerie totale (comptes actifs) : {Theme.Montant(total)} FCFA";
        ChargerReleve();
    }

    // Charge le relevé du compte sélectionné sur la période _du → _au
    private void ChargerReleve()
    {
        // Au premier affichage la grille n'a pas encore de ligne courante : on prend la première.
        var ligne = _comptes.CurrentRow ?? (_comptes.Rows.Count > 0 ? _comptes.Rows[0] : null);
        if (ligne == null || !_comptes.Columns.Contains("id_compte"))
        {
            _releve.DataSource = null;
            _cadreReleve.Text = "Relevé";
            return;
        }
        try
        {
            int id = Convert.ToInt32(ligne.Cells["id_compte"].Value);
            _cadreReleve.Text = $"Relevé — {ligne.Cells["Code"].Value} · {ligne.Cells["Libellé"].Value}";
            _releve.DataSource = Depot.Releve(id, _du.Value, _au.Value);
        }
        catch (Exception ex)
        {
            Ui.Erreur(ex, "Relevé");
        }
    }
}
