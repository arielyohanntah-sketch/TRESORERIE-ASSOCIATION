// =====================================================================================
//  Forms/CreerUtilisateurForm.cs — CRÉER UN UTILISATEUR
// -------------------------------------------------------------------------------------
//  Deux usages :
//    - depuis l'écran de connexion, quand la base est vide → premier administrateur
//      (le rôle est alors forcé à ADMIN) ;
//    - depuis l'écran « Utilisateurs » (administrateur) → nouvel utilisateur.
//  Contrôles : _nom, _prenoms, _login, _email, _mdp, _confirmation, _role, _btnCreer.
//  ➜ La liste des rôles proposés se modifie dans le concepteur : propriété Items de _role
//    (les valeurs doivent exister dans la base).
// =====================================================================================

using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Forms;

/// <summary>
/// Création d'un utilisateur via sp_utilisateur_creer (le mot de passe est salé et haché par la base).
/// La mise en page est dans CreerUtilisateurForm.Designer.cs.
/// </summary>
public partial class CreerUtilisateurForm : DialogueBase
{
    // Identifiant du compte créé : LoginForm le recopie dans la zone « Identifiant »
    public string LoginCree { get; private set; } = "";

    /// <summary>Constructeur utilisé par le concepteur de Visual Studio.</summary>
    public CreerUtilisateurForm() : this(false) { }

    public CreerUtilisateurForm(bool premierAdministrateur)
    {
        InitializeComponent();
        DefinirIcone(Theme.IcoPersonne);

        if (premierAdministrateur)
        {
            // Premier compte : titre adapté et rôle ADMIN imposé (liste grisée)
            Text = LibelleTitre.Text = "Compte administrateur";
            LibelleSousTitre.Text = "Premier compte de l'application (rôle ADMIN)";
            _role.SelectedItem = "ADMIN";
            _role.Enabled = false;
        }
        else
        {
            // Rôle proposé par défaut
            _role.SelectedItem = "TRESORIER";
        }
        _btnCreer.Click += (_, _) => Creer();
    }

    // Clic sur « Créer » : contrôles de saisie puis enregistrement dans la base
    private void Creer()
    {
        if (_nom.Text.Trim() == "" || _prenoms.Text.Trim() == "" || _login.Text.Trim() == "")
        {
            Ui.Info("Le nom, les prénoms et l'identifiant sont obligatoires.");
            return;
        }
        if (_mdp.Text != _confirmation.Text)
        {
            Ui.Info("Les deux mots de passe ne sont pas identiques.");
            return;
        }
        try
        {
            // La longueur minimale du mot de passe et l'unicité de l'identifiant sont vérifiées par la base
            Depot.CreerUtilisateur(_nom.Text.Trim(), _prenoms.Text.Trim(), _login.Text.Trim(), _mdp.Text,
                _role.SelectedItem!.ToString()!, _email.Text);
            LoginCree = _login.Text.Trim();
            Ui.Info($"Utilisateur « {LoginCree} » créé.");
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { Ui.Erreur(ex); }
    }
}
