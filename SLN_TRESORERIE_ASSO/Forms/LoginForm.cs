// =====================================================================================
//  Forms/LoginForm.cs — ÉCRAN DE CONNEXION
// -------------------------------------------------------------------------------------
//  Contrôles (créés dans LoginForm.Designer.cs, à modifier dans le concepteur) :
//      _login, _motDePasse   zones de saisie
//      _afficher             case « Afficher le mot de passe »
//      _btnConnecter         bouton « Se connecter » (aussi déclenché par Entrée)
//      _erreur               bandeau rouge des messages d'erreur (caché au départ)
//      _premierAdmin, _creerAdmin  message + lien visibles seulement si la base n'a
//                            encore aucun utilisateur (création du 1er administrateur)
//      _serveur              rappel de la base et du serveur utilisés
//
//  Si la connexion réussit : DialogResult = OK → Program.cs ouvre la fenêtre principale.
// =====================================================================================

using MySqlConnector;
using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Forms;

/// <summary>
/// Connexion d'un utilisateur via la procédure sp_utilisateur_connecter.
/// La mise en page est dans LoginForm.Designer.cs (modifiable dans le concepteur).
/// </summary>
public partial class LoginForm : DialogueBase
{
    public LoginForm()
    {
        InitializeComponent();
        // Icône « personne » dans le bandeau (taille 48 au lieu de 36 par défaut)
        DefinirIcone(Theme.IcoCompte, 48);

        // Branchement des événements (ce qui se passe quand on clique / coche)
        _afficher.CheckedChanged += (_, _) => _motDePasse.UseSystemPasswordChar = !_afficher.Checked;
        _btnConnecter.Click += (_, _) => SeConnecter();
        _creerAdmin.LinkClicked += (_, _) =>
        {
            // Création du tout premier compte (administrateur) quand la base est vide
            using var f = new CreerUtilisateurForm(premierAdministrateur: true);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                _login.Text = f.LoginCree;
                _motDePasse.Focus();
                Actualiser();
            }
        };
        Shown += (_, _) => { Actualiser(); _login.Focus(); };
    }

    // Met à jour le texte du serveur et affiche/cache le lien « créer le premier administrateur »
    private void Actualiser()
    {
        _serveur.Text = $"Base : {Connexion.Base} sur {Connexion.Serveur}:{Connexion.Port}";
        try
        {
            bool aucun = Depot.AucunUtilisateur();
            _premierAdmin.Visible = aucun;
            _creerAdmin.Visible = aucun;
        }
        catch (Exception ex)
        {
            AfficherErreur("Base inaccessible : " + ex.Message);
        }
    }

    // Clic sur « Se connecter »
    private void SeConnecter()
    {
        _erreur.Visible = false;
        if (string.IsNullOrWhiteSpace(_login.Text) || _motDePasse.Text.Length == 0)
        {
            AfficherErreur("Saisissez votre identifiant et votre mot de passe.");
            return;
        }
        // Curseur « sablier » pendant la vérification
        Cursor = Cursors.WaitCursor;
        try
        {
            Depot.Connecter(_login.Text.Trim(), _motDePasse.Text);
            // Succès : on ferme la fenêtre avec le résultat OK
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (MySqlException ex) when (ex.Number == 1644)   // erreur SIGNAL de la procédure
        {
            // Message envoyé par la procédure sp_utilisateur_connecter (mauvais mot de passe, compte bloqué…)
            string message = ex.Message;
            if (message.StartsWith("Identifiant ou mot de passe"))
            {
                // On ajoute le nombre d'essais restants avant le blocage
                int reste = Depot.TentativesRestantes(_login.Text.Trim());
                if (reste is > 0 and < 5) message += $"\nEncore {reste} essai(s) avant le blocage du compte.";
                if (reste == 0) message = "Compte bloqué après 5 tentatives échouées : contactez un administrateur.";
            }
            AfficherErreur(message);
            _motDePasse.Clear();
            _motDePasse.Focus();
        }
        catch (Exception ex)
        {
            AfficherErreur(ex.Message);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // Affiche un message dans le bandeau rouge
    private void AfficherErreur(string message)
    {
        _erreur.Text = message;
        _erreur.Visible = true;
    }
}
