// =====================================================================================
//  Forms/VirementForm.cs — NOUVEAU VIREMENT ENTRE DEUX COMPTES
// -------------------------------------------------------------------------------------
//  Exemple : retirer 200 000 FCFA de la banque pour alimenter la caisse.
//  Contrôles : _source / _destination (comptes), _montant, _date, _libelle,
//              _soldeSource / _apresSource et _soldeDest / _apresDest (soldes avant/après),
//              _fleche (icône), _btnEnregistrer.
//  Les soldes « après virement » se recalculent dès qu'on change un compte ou le montant ;
//  ils passent en rouge si le compte source dépasse son découvert autorisé.
//  La base refuse de toute façon un virement qui rendrait le solde insuffisant (trigger).
// =====================================================================================

using System.Data;
using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Forms;

/// <summary>Virement entre deux comptes de l'association (mise en page : VirementForm.Designer.cs).</summary>
public partial class VirementForm : DialogueBase
{
    // Texte de la notification à afficher par MainForm après un enregistrement réussi
    public (string Titre, string Message, TypeNotification Type)? Resultat { get; private set; }

    public VirementForm()
    {
        InitializeComponent();
        DefinirIcone(Theme.IcoVirement);
        _fleche.Image = Theme.Glyphe("", Theme.Accent, 30);   // flèche vers la droite
        _btnEnregistrer.Click += (_, _) => Enregistrer();
        // Load = juste avant l'affichage : on remplit les listes de comptes
        Load += (_, _) => Charger();
    }

    // Remplit les deux listes de comptes et branche le recalcul des soldes
    private void Charger()
    {
        try
        {
            // Chaque liste reçoit sa propre copie des comptes (sinon les deux sélections seraient liées)
            foreach (var c in new[] { _source, _destination })
            {
                c.DisplayMember = "lib";
                c.ValueMember = "id";
                c.DataSource = Depot.Comptes();
            }
            // Par défaut, la destination est le 2e compte (pour qu'elle soit différente de la source)
            if (_destination.Items.Count > 1) _destination.SelectedIndex = 1;
            _source.SelectedIndexChanged += (_, _) => MajSoldes();
            _destination.SelectedIndexChanged += (_, _) => MajSoldes();
            _montant.ValueChanged += (_, _) => MajSoldes();
            MajSoldes();
            if (_source.Items.Count < 2) Ui.Info("Il faut au moins deux comptes actifs pour faire un virement.");
        }
        catch (Exception ex) { Ui.Erreur(ex, "Chargement impossible"); }
    }

    // Met à jour les soldes : le compte source diminue, le compte destination augmente
    private void MajSoldes()
    {
        Afficher(_source, _soldeSource, _apresSource, -_montant.Value);
        Afficher(_destination, _soldeDest, _apresDest, _montant.Value);
    }

    // Affiche le solde actuel et le solde après virement du compte choisi dans la liste c
    private static void Afficher(ComboBox c, Label solde, Label apres, decimal variation)
    {
        // La ligne choisie contient aussi les colonnes « solde » et « decouvert » (voir Depot.Comptes)
        if (c.SelectedItem is not DataRowView r) return;
        decimal s = Convert.ToDecimal(r["solde"]);
        decimal decouvert = Convert.ToDecimal(r["decouvert"]);
        solde.Text = $"Solde actuel : {Theme.Montant(s)}";
        apres.Text = $"Après virement : {Theme.Montant(s + variation)}";
        // Rouge vif si on dépasse le découvert autorisé, rouge foncé si le compte baisse, vert s'il augmente
        apres.ForeColor = variation < 0 ? (s + variation < -decouvert ? Theme.Rouge : Color.FromArgb(150, 40, 30)) : Theme.Vert;
    }

    // Clic sur « Enregistrer » : contrôles de saisie puis création du virement
    private void Enregistrer()
    {
        if (Ui.IdSelectionne(_source) is not int src || Ui.IdSelectionne(_destination) is not int dst) { Ui.Info("Choisissez les deux comptes."); return; }
        if (src == dst) { Ui.Info("Le compte source et le compte destination doivent être différents."); return; }
        if (_montant.Value <= 0) { Ui.Info("Le montant doit être supérieur à zéro."); return; }
        if (string.IsNullOrWhiteSpace(_libelle.Text)) { Ui.Info("Le libellé est obligatoire."); return; }
        try
        {
            Depot.CreerVirement(_date.Value, src, dst, _montant.Value, _libelle.Text);
            // Message de la notification verte qu'affichera la fenêtre principale
            Resultat = ("Virement enregistré",
                $"{Theme.Montant(_montant.Value)} FCFA : {(_source.SelectedItem as DataRowView)?["lib"]} → {(_destination.SelectedItem as DataRowView)?["lib"]}",
                TypeNotification.Succes);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { Ui.Erreur(ex); }
    }
}
