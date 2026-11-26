// =====================================================================================
//  Views/MouvementsVue.cs — LISTE DES OPÉRATIONS (recettes et dépenses)
// -------------------------------------------------------------------------------------
//  Le même écran sert pour plusieurs entrées du menu, selon les paramètres reçus :
//      MouvementsVue(null, null)                      → « Mouvements » (tout)
//      MouvementsVue("RECETTE", null)                 → « Recettes »
//      MouvementsVue(null, "BROUILLON")               → « À valider »
//      MouvementsVue(null, "BROUILLON", mesSaisies)   → « Mes brouillons »
//      MouvementsVue(null, null, mesSaisies: true)    → « Mes saisies »
//
//  Contrôles (MouvementsVue.Designer.cs) :
//      _entete, _info          titre et bandeau « espace personnel »
//      _du, _au, _compte, _type, _statut, _recherche, _btnFiltrer   filtres
//      _grille                 liste (double-clic = ouvrir) ; _totaux = totaux en bas
//  ⚠ L'ordre des éléments des listes _type et _statut (propriété Items dans le
//    concepteur) doit correspondre aux index utilisés dans Charger().
// =====================================================================================

using System.Data;
using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.Forms;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Views;

/// <summary>
/// Liste filtrable des recettes et dépenses (aussi « Mes brouillons », « Mes saisies », « À valider »).
/// La mise en page est dans MouvementsVue.Designer.cs.
/// </summary>
public partial class MouvementsVue : VueBase
{
    private readonly string _titre = "Mouvements";
    // Vrai pour « Mes brouillons » / « Mes saisies » : seulement les opérations que j'ai saisies
    private readonly bool _mesSaisies;
    // La liste des comptes n'est chargée qu'au premier affichage
    private bool _chargementInitial = true;

    public override string Titre => _titre;

    /// <summary>Constructeur utilisé par le concepteur de Visual Studio.</summary>
    public MouvementsVue() : this(null, null) { }

    /// <param name="type">"RECETTE", "DEPENSE" ou null (les deux) : filtre type présélectionné.</param>
    /// <param name="statut">"BROUILLON" ou null : filtre statut présélectionné.</param>
    /// <param name="mesSaisies">Vrai pour ne montrer que mes propres saisies.</param>
    public MouvementsVue(string? type, string? statut, bool mesSaisies = false)
    {
        InitializeComponent();
        _mesSaisies = mesSaisies;
        // Titre de l'écran selon le cas
        _titre = mesSaisies
            ? (statut == "BROUILLON" ? "Mes brouillons" : "Mes saisies")
            : statut == "BROUILLON" ? "Opérations à valider" : type == "RECETTE" ? "Recettes" : type == "DEPENSE" ? "Dépenses" : "Mouvements";
        _entete.Text = _titre;
        _info.Visible = mesSaisies;

        // Filtres par défaut : toute l'année en cours
        _du.Value = new DateTime(DateTime.Today.Year, 1, 1);
        _au.Value = new DateTime(DateTime.Today.Year, 12, 31);
        _type.SelectedIndex = type == "RECETTE" ? 1 : type == "DEPENSE" ? 2 : 0;
        _statut.SelectedIndex = statut == "BROUILLON" ? 1 : 0;

        Theme.StyleGrille(_grille);
        Theme.ColorerOperations(_grille);
        // Double-clic sur une ligne = ouvrir l'opération
        _grille.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) Ouvrir(); };

        // Bouton « Filtrer » ou touche Entrée dans la zone de recherche
        _btnFiltrer.Click += (_, _) => Executer(Charger, false);
        _recherche.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Executer(Charger, false); }
        };
    }

    /// <summary>Seuls les boutons autorisés pour le rôle connecté sont affichés.</summary>
    public override IEnumerable<ToolStripItem> Outils()
    {
        var outils = new List<ToolStripItem>();
        if (Session.Peut("SAISIR"))
        {
            outils.Add(Theme.Outil("Nouvelle recette", Theme.IcoRecette, Theme.Vert, (_, _) => Principale?.NouvelleOperation("RECETTE")));
            outils.Add(Theme.Outil("Nouvelle dépense", Theme.IcoDepense, Theme.Rouge, (_, _) => Principale?.NouvelleOperation("DEPENSE")));
        }
        outils.Add(Theme.Outil("Ouvrir", Theme.IcoModifier, Theme.Accent, (_, _) => Ouvrir()));
        outils.Add(Separateur());
        if (Session.Peut("VALIDER"))
        {
            outils.Add(Theme.Outil("Valider", Theme.IcoValider, Theme.Vert, (_, _) => Valider()));
            outils.Add(Theme.Outil("Annuler la pièce", Theme.IcoAnnuler, Theme.Rouge, (_, _) => Annuler()));
        }
        if (Session.Peut("SAISIR"))
            outils.Add(Theme.Outil("Supprimer mon brouillon", Theme.IcoSupprimer, Theme.Gris, (_, _) => Supprimer()));
        outils.Add(Separateur());
        outils.Add(Theme.Outil("Exporter (Excel)", Theme.IcoExporter, Theme.Vert, (_, _) => Ui.ExporterCsv(_grille, "mouvements")));
        outils.Add(Theme.Outil("Actualiser", Theme.IcoActualiser, Theme.Gris, (_, _) => Executer(Actualiser, false)));
        return outils;
    }

    /// <summary>Effet visuel sur la ligne qui vient d'être validée / annulée / enregistrée.</summary>
    public void Surligner(int id, TypeNotification type) =>
        Theme.Surligner(_grille, "id_mouvement", id, type switch
        {
            // vert = validé, orange = annulé, bleu = brouillon enregistré
            TypeNotification.Succes => Color.FromArgb(170, 225, 185),
            TypeNotification.Alerte => Color.FromArgb(255, 214, 160),
            _ => Color.FromArgb(180, 215, 245)
        });

    public override void Actualiser()
    {
        if (_chargementInitial)
        {
            // Liste des comptes avec « Tous les comptes » en premier
            _compte.DataSource = Depot.Comptes(avecTous: true);
            _compte.DisplayMember = "lib";
            _compte.ValueMember = "id";
            _chargementInitial = false;
        }
        Charger();
    }

    // Charge la liste selon les filtres et calcule les totaux affichés en bas
    private void Charger()
    {
        // Index des listes → valeurs attendues par la base (0 = « Tous »)
        string? type = _type.SelectedIndex switch { 1 => "RECETTE", 2 => "DEPENSE", _ => null };
        string? statut = _statut.SelectedIndex switch { 1 => "BROUILLON", 2 => "VALIDE", 3 => "ANNULE", _ => null };
        var dt = Depot.Mouvements(_du.Value, _au.Value, _compte.SelectedValue, type, statut, _recherche.Text, _mesSaisies);
        _grille.DataSource = dt;

        // Totaux (les opérations annulées ne comptent pas)
        decimal recettes = 0, depenses = 0;
        foreach (DataRow r in dt.Rows)
        {
            if (r["Statut"].ToString() == "ANNULE") continue;
            if (r["type_mouvement"].ToString() == "RECETTE") recettes += Convert.ToDecimal(r["Montant"]);
            else depenses += Convert.ToDecimal(r["Montant"]);
        }
        _totaux.Text = $"{dt.Rows.Count} mouvement(s)      Recettes : + {Theme.Montant(recettes)}      " +
                       $"Dépenses : − {Theme.Montant(depenses)}      Solde de la période : {Theme.Montant(recettes - depenses)} FCFA";
    }

    // Id et statut de la ligne sélectionnée (null + message si rien n'est sélectionné)
    private (int Id, string Statut)? Selection()
    {
        if (_grille.CurrentRow == null)
        {
            Ui.Info("Sélectionnez d'abord un mouvement dans la liste.");
            return null;
        }
        return (Convert.ToInt32(_grille.CurrentRow.Cells["id_mouvement"].Value), _grille.CurrentRow.Cells["Statut"].Value?.ToString() ?? "");
    }

    // Bouton « Ouvrir » / double-clic
    private void Ouvrir()
    {
        if (Selection() is { } s) Principale?.OuvrirMouvement(s.Id);
    }

    // Bouton « Valider » : seulement pour un brouillon
    private void Valider()
    {
        if (Selection() is not { } s) return;
        if (s.Statut != "BROUILLON") { Ui.Info("Seul un brouillon peut être validé."); return; }
        if (!Ui.Confirmer("Valider ce mouvement ?\nIl ne pourra plus être modifié, seulement annulé.")) return;
        Executer(() => Depot.ValiderMouvement(s.Id));
        // Si un numéro de pièce existe maintenant, la validation a réussi
        if (Depot.NumeroPiece(s.Id) is { } num)
        {
            Principale?.Notifier("Opération validée", $"Pièce n° {num} enregistrée définitivement.");
            Surligner(s.Id, TypeNotification.Succes);
        }
    }

    // Bouton « Annuler la pièce » : demande un motif puis passe le mouvement en ANNULE
    private void Annuler()
    {
        if (Selection() is not { } s) return;
        if (s.Statut == "ANNULE") { Ui.Info("Ce mouvement est déjà annulé."); return; }
        using var f = new MotifForm("Annuler la pièce", "Le mouvement restera visible avec le statut ANNULE");
        if (f.ShowDialog(this) != DialogResult.OK) return;
        bool ok = false;
        Executer(() => { Depot.AnnulerMouvement(s.Id, f.Motif); ok = true; });
        if (ok)
        {
            Principale?.Notifier("Pièce annulée", $"Motif : {f.Motif}", TypeNotification.Alerte);
            Surligner(s.Id, TypeNotification.Alerte);
        }
    }

    // Bouton « Supprimer mon brouillon » : suppression définitive (brouillons uniquement)
    private void Supprimer()
    {
        if (Selection() is not { } s) return;
        if (s.Statut != "BROUILLON") { Ui.Info("Seuls les brouillons peuvent être supprimés. Annulez plutôt la pièce."); return; }
        if (!Ui.Confirmer("Supprimer définitivement ce brouillon ?")) return;
        bool ok = false;
        Executer(() => { Depot.SupprimerBrouillon(s.Id); ok = true; });
        if (ok) Principale?.Notifier("Brouillon supprimé", "Il a été retiré de vos saisies.", TypeNotification.Info);
    }
}
