// =====================================================================================
//  Views/UtilisateursVue.cs — ÉCRAN « UTILISATEURS » (administrateur seulement)
// -------------------------------------------------------------------------------------
//  Contrôles (UtilisateursVue.Designer.cs) : _entete, _grille, _aide (texte d'aide en bas).
//  Actions : créer un utilisateur, activer/désactiver un compte, débloquer un compte
//  bloqué après 5 échecs de connexion.
//  Affichage : comptes désactivés en gris, compteur d'échecs ≥ 5 en rouge.
// =====================================================================================

using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.Forms;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Views;

/// <summary>Gestion des utilisateurs, réservée au rôle ADMIN (mise en page : UtilisateursVue.Designer.cs).</summary>
public partial class UtilisateursVue : VueBase
{
    public override string Titre => "Utilisateurs";

    public UtilisateursVue()
    {
        InitializeComponent();
        Theme.StyleGrille(_grille);
        // Couleurs particulières de certaines cellules
        _grille.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0) return;
            var row = _grille.Rows[e.RowIndex];
            // Compte désactivé : toute la ligne en gris
            if (row.Cells["Actif"].Value is false) e.CellStyle!.ForeColor = Theme.Gris;
            // Compte bloqué : cellule « Échecs » en rouge
            if (_grille.Columns[e.ColumnIndex].Name == "Échecs" && Convert.ToInt32(e.Value ?? 0) >= 5)
            {
                e.CellStyle!.BackColor = Theme.RougeClair;
                e.CellStyle.ForeColor = Theme.Rouge;
                e.CellStyle.Font = Theme.PoliceGras;
            }
        };
    }

    public override IEnumerable<ToolStripItem> Outils() => new ToolStripItem[]
    {
        Theme.Outil("Nouvel utilisateur", Theme.IcoAjouter, Theme.Vert, (_, _) =>
        {
            using var f = new CreerUtilisateurForm();
            if (f.ShowDialog(this) != DialogResult.OK) return;
            // Action vide : sert seulement à actualiser la liste
            Executer(() => { });
            Principale?.Notifier("Utilisateur créé", $"« {f.LoginCree} » peut maintenant se connecter.");
        }),
        Theme.Outil("Activer / désactiver", Theme.IcoCadenas, Theme.Orange, (_, _) =>
        {
            if (IdSelectionne() is not int id) return;
            // ok passe à vrai seulement si l'action a réussi (sinon Executer affiche l'erreur)
            bool ok = false;
            Executer(() => { Depot.BasculerActif(id); ok = true; });
            if (ok) Principale?.Notifier("Compte modifié", "L'accès de cet utilisateur a été activé ou désactivé.", TypeNotification.Info);
        }),
        Theme.Outil("Débloquer", Theme.IcoDeverrouiller, Theme.Accent, (_, _) =>
        {
            if (IdSelectionne() is not int id) return;
            bool ok = false;
            Executer(() => { Depot.DebloquerUtilisateur(id); ok = true; });
            if (ok) Principale?.Notifier("Compte débloqué", "Le compteur de tentatives a été remis à zéro.");
        }),
        Separateur(),
        Theme.Outil("Actualiser", Theme.IcoActualiser, Theme.Gris, (_, _) => Executer(Actualiser, false))
    };

    public override void Actualiser() => _grille.DataSource = Depot.Utilisateurs();

    // Id de l'utilisateur sélectionné dans la grille (message si aucune ligne n'est sélectionnée)
    private int? IdSelectionne()
    {
        if (_grille.CurrentRow == null) { Ui.Info("Sélectionnez un utilisateur."); return null; }
        return Convert.ToInt32(_grille.CurrentRow.Cells["id_utilisateur"].Value);
    }
}
