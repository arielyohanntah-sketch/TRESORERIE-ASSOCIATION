// =====================================================================================
//  Forms/DialogueBase.cs — MODÈLE COMMUN DES PETITES FENÊTRES
// -------------------------------------------------------------------------------------
//  LoginForm, MotDePasseForm, MotifForm, CreerUtilisateurForm et VirementForm
//  « héritent » de cette fenêtre (« : DialogueBase ») : elles récupèrent donc
//  automatiquement les zones suivantes, visibles dans leur concepteur :
//      Entete           bandeau bleu du haut
//        Icone            image à gauche du bandeau (choisie avec DefinirIcone)
//        LibelleTitre     grand titre blanc
//        LibelleSousTitre petite phrase sous le titre
//      Corps            zone blanche du milieu : on y place les champs du formulaire
//      Pied             barre grise du bas : on y place les boutons (de droite à gauche)
//
//  ➜ Changer la couleur du bandeau ici (dans le concepteur de DialogueBase) la change
//    dans toutes les fenêtres filles. Recompilez (Générer) pour voir le résultat.
// =====================================================================================

using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Forms;

/// <summary>
/// Modèle commun des boîtes de dialogue : bandeau bleu en haut (icône + titre),
/// zone de contenu (Corps) et barre de boutons en bas (Pied).
/// La mise en page est dans DialogueBase.Designer.cs ; les fenêtres filles héritent de ces zones
/// et ajoutent leurs contrôles dans Corps et leurs boutons dans Pied (depuis le concepteur).
/// </summary>
public partial class DialogueBase : Form
{
    public DialogueBase()
    {
        // Crée les contrôles définis dans DialogueBase.Designer.cs
        InitializeComponent();
    }

    /// <summary>Place l'icône du bandeau (dessinée avec la police Segoe MDL2 Assets).</summary>
    protected void DefinirIcone(string glyphe, int tailleLogique = 36)
    {
        Icone.Image = Theme.Glyphe(glyphe, Color.White, tailleLogique);
    }
}
