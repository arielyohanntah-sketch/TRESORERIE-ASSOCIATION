// =====================================================================================
//  Forms/MotDePasseForm.cs — CHANGER SON MOT DE PASSE
// -------------------------------------------------------------------------------------
//  Ouverte depuis le menu de l'utilisateur (en haut à droite de la fenêtre principale).
//  Contrôles : _ancien, _nouveau, _confirmation (zones de saisie) et _btnEnregistrer.
//  Règle : 8 caractères minimum (vérifié dans Depot.ChangerMotDePasse).
// =====================================================================================

using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Forms;

/// <summary>Changement du mot de passe de l'utilisateur connecté (mise en page : MotDePasseForm.Designer.cs).</summary>
public partial class MotDePasseForm : DialogueBase
{
    public MotDePasseForm()
    {
        InitializeComponent();
        DefinirIcone(Theme.IcoCadenas);
        // Sous-titre personnalisé avec le nom de l'utilisateur connecté
        if (Session.Connecte) LibelleSousTitre.Text = $"Compte de {Session.NomComplet} ({Session.Login})";

        _btnEnregistrer.Click += (_, _) =>
        {
            // Les deux saisies du nouveau mot de passe doivent être identiques
            if (_nouveau.Text != _confirmation.Text) { Ui.Info("Les deux nouveaux mots de passe ne sont pas identiques."); return; }
            try
            {
                Depot.ChangerMotDePasse(_ancien.Text, _nouveau.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex) { Ui.Erreur(ex); }
        };
    }
}
