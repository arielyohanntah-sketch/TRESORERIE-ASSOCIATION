// =====================================================================================
//  Views/BudgetVue.cs — RAPPORT « BUDGET VS RÉALISÉ »
// -------------------------------------------------------------------------------------
//  Pour chaque centre analytique : recettes et dépenses prévues (table budget_centre)
//  comparées aux montants réalisés (ventilations des mouvements).
//  Contrôles (BudgetVue.Designer.cs) :
//      _parametres : _exercice, _au (date d'arrêté), _brouillons (inclure les brouillons), _btnGenerer
//      _feuille    : _titreRapport, _sousTitre, _grille, _alerte (dépassements, en rouge)
//  La colonne « Consommation » est dessinée en barre de progression (PeindreConsommation) :
//  bleu < 90 %, orange de 90 à 100 %, rouge au-delà de 100 %.
// =====================================================================================

using System.Data;
using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Views;

/// <summary>Rapport : budget prévisionnel et réalisé par centre analytique (mise en page : BudgetVue.Designer.cs).</summary>
public partial class BudgetVue : VueBase
{
    // La liste des exercices n'est remplie qu'une fois
    private bool _initialise;

    public override string Titre => "Budget vs réalisé";

    public BudgetVue()
    {
        InitializeComponent();
        _au.Value = DateTime.Today;
        Theme.StyleGrille(_grille);
        _grille.CellPainting += PeindreConsommation;
        // Ligne « Total » en gras sur fond gris ; « Reste à dépenser » négatif en rouge
        _grille.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0) return;
            if (_grille.Rows[e.RowIndex].Cells["total"].Value is true)
            {
                e.CellStyle!.Font = Theme.PoliceGras;
                e.CellStyle.BackColor = Theme.EnTeteGrille;
            }
            if (_grille.Columns[e.ColumnIndex].Name == "Reste à dépenser" && e.Value is decimal d && d < 0)
                e.CellStyle!.ForeColor = Theme.Rouge;
        };
        // Après chargement : colonne technique « total » cachée, largeurs relatives ajustées
        _grille.DataBindingComplete += (_, _) =>
        {
            _grille.ColumnHeadersHeight = Theme.Px(46);   // titres sur deux lignes
            if (_grille.Columns.Contains("total")) _grille.Columns["total"]!.Visible = false;
            if (_grille.Columns.Contains("Consommation")) _grille.Columns["Consommation"]!.FillWeight = 120;
            if (_grille.Columns.Contains("Centre")) _grille.Columns["Centre"]!.FillWeight = 140;
        };
        _btnGenerer.Click += (_, _) => Executer(Charger, false);
    }

    public override IEnumerable<ToolStripItem> Outils()
    {
        var outils = new List<ToolStripItem>();
        if (Session.Peut("PARAMETRER"))
        {
            outils.Add(Theme.Outil("Saisir les budgets", Theme.IcoModifier, Theme.Accent, (_, _) => Principale?.Naviguer("param_budget")));
            outils.Add(Separateur());
        }
        outils.Add(Theme.Outil("Exporter (Excel)", Theme.IcoExporter, Theme.Vert, (_, _) => Ui.ExporterCsv(_grille, "budget_vs_realise")));
        outils.Add(Theme.Outil("Actualiser", Theme.IcoActualiser, Theme.Gris, (_, _) => Executer(Actualiser, false)));
        return outils;
    }

    public override void Actualiser()
    {
        // Premier affichage : remplissage de la liste des exercices
        if (!_initialise)
        {
            var ex = Depot.Exercices();
            _exercice.DisplayMember = "lib";
            _exercice.ValueMember = "id";
            _exercice.DataSource = ex;
            // exercice qui contient aujourd'hui, sinon le plus récent
            foreach (DataRow r in ex.Rows)
                if (DateTime.Today >= Convert.ToDateTime(r["date_debut"]) && DateTime.Today <= Convert.ToDateTime(r["date_fin"]))
                    _exercice.SelectedValue = r["id"];
            _initialise = true;
        }
        Charger();
    }

    // Génère le rapport pour l'exercice et la date choisis
    private void Charger()
    {
        if (Ui.IdSelectionne(_exercice) is not int idExercice)
        {
            _grille.DataSource = null;
            _sousTitre.Text = "Aucun exercice : créez-en un dans Paramétrage > Exercices.";
            return;
        }
        var source = Depot.BudgetVsRealise(idExercice, _au.Value, _brouillons.Checked);

        // Tableau affiché (titres de colonnes lisibles) ; « total » marque la dernière ligne
        var dt = new DataTable();
        dt.Columns.Add("Centre", typeof(string));
        dt.Columns.Add("Recettes prévues", typeof(decimal));
        dt.Columns.Add("Recettes réalisées", typeof(decimal));
        dt.Columns.Add("% recettes", typeof(string));
        dt.Columns.Add("Dépenses prévues", typeof(decimal));
        dt.Columns.Add("Dépenses réalisées", typeof(decimal));
        dt.Columns.Add("Consommation", typeof(int));
        dt.Columns.Add("Reste à dépenser", typeof(decimal));
        dt.Columns.Add("total", typeof(bool));

        // Une ligne par centre + cumul des totaux ; on note les centres qui dépassent leur budget
        decimal trp = 0, trr = 0, tdp = 0, tdr = 0;
        var depassements = new List<string>();
        foreach (DataRow r in source.Rows)
        {
            decimal rp = Convert.ToDecimal(r["rp"]), rr = Convert.ToDecimal(r["rr"]);
            decimal dp = Convert.ToDecimal(r["dp"]), dr = Convert.ToDecimal(r["dr"]);
            trp += rp; trr += rr; tdp += dp; tdr += dr;
            dt.Rows.Add(r["centre"], rp, rr, Pourcent(rr, rp), dp, dr, PourcentEntier(dr, dp), dp - dr, false);
            if (dr > dp && dp > 0) depassements.Add($"{r["code"]} (+{Theme.Montant(dr - dp)})");
        }
        dt.Rows.Add("Total", trp, trr, Pourcent(trr, trp), tdp, tdr, PourcentEntier(tdr, tdp), tdp - tdr, true);
        _grille.DataSource = dt;

        // Sous-titre et bandeau d'alerte
        _sousTitre.Text = $"Exercice {_exercice.Text} · arrêté au {_au.Value:dd/MM/yyyy} · " +
                          (_brouillons.Checked ? "validés et brouillons" : "mouvements validés uniquement") +
                          " · montants en FCFA (selon la ventilation analytique)";
        _alerte.Visible = depassements.Count > 0;
        _alerte.Text = depassements.Count == 0 ? "" :
            $"{depassements.Count} centre(s) dépassent leur budget de dépenses : {string.Join(", ", depassements)}";
    }

    // Pourcentage réalisé / prévu (vide ou 0 si rien n'était prévu)
    private static string Pourcent(decimal reel, decimal prevu) => prevu == 0 ? "" : $"{Math.Round(reel / prevu * 100)} %";
    private static int PourcentEntier(decimal reel, decimal prevu) => prevu == 0 ? 0 : (int)Math.Round(reel / prevu * 100);

    /// <summary>Barre de consommation colorée : bleu, orange au-delà de 90 %, rouge au-delà de 100 %.</summary>
    private void PeindreConsommation(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0 || _grille.Columns[e.ColumnIndex].Name != "Consommation") return;
        e.PaintBackground(e.CellBounds, true);
        int pct = e.Value is int v ? v : 0;
        int texte = Theme.Px(58);
        var zone = new Rectangle(e.CellBounds.X + Theme.Px(8), e.CellBounds.Y + e.CellBounds.Height / 2 - Theme.Px(5),
            Math.Max(10, e.CellBounds.Width - texte - Theme.Px(16)), Theme.Px(10));
        using (var piste = new SolidBrush(Color.FromArgb(230, 230, 230)))
            e.Graphics!.FillRectangle(piste, zone);
        var couleur = pct > 100 ? Theme.Rouge : pct >= 90 ? Color.FromArgb(194, 106, 0) : Theme.Accent;
        using (var plein = new SolidBrush(couleur))
            e.Graphics!.FillRectangle(plein, zone.X, zone.Y, zone.Width * Math.Min(pct, 100) / 100, zone.Height);
        TextRenderer.DrawText(e.Graphics, pct + " %", pct > 100 ? Theme.PoliceGras : Theme.Police,
            new Rectangle(zone.Right + Theme.Px(4), e.CellBounds.Y, texte, e.CellBounds.Height),
            pct > 100 ? Theme.Rouge : Color.FromArgb(27, 27, 27), TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        e.Handled = true;
    }
}
