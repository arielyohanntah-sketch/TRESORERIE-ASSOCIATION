// =====================================================================================
//  Views/VueBase.cs — MODÈLE COMMUN DE TOUS LES ÉCRANS DU CENTRE
// -------------------------------------------------------------------------------------
//  Chaque écran (TableauDeBordVue, MouvementsVue, ComptesVue…) hérite de VueBase et
//  peut redéfinir (« override ») :
//      Titre        texte ajouté au titre de la fenêtre (« Trésorerie Asso — Titre »)
//      Outils()     boutons à afficher dans la barre d'outils du haut pour cet écran
//      Actualiser() recharge les données depuis la base (appelé à l'ouverture et par F5)
//
//  Outils disponibles pour les vues :
//      Principale   la fenêtre principale (pour appeler Naviguer, Notifier, NouveauVirement…)
//      Executer(…)  lance une action en affichant proprement les erreurs, puis actualise
//
//  Ce fichier n'a PAS de .Designer.cs : il ne contient aucun contrôle.
// =====================================================================================

using SLN_TRESORERIE_ASSO.Forms;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Views;

/// <summary>
/// Écran affiché dans la zone centrale de la fenêtre principale.
/// Chaque écran a sa mise en page dans son fichier .Designer.cs (modifiable dans le concepteur)
/// et fournit ses propres boutons de barre d'outils (méthode Outils).
/// </summary>
public class VueBase : UserControl
{
    public virtual string Titre => "";

    public VueBase()
    {
        // L'écran remplit toute la zone centrale, sur le fond gris clair de l'application
        Dock = DockStyle.Fill;
        BackColor = Theme.Fond;
        Padding = new Padding(10);
        Font = Theme.Police;
    }

    /// <summary>Boutons de la barre d'outils pour cet écran.</summary>
    public virtual IEnumerable<ToolStripItem> Outils() => Array.Empty<ToolStripItem>();

    /// <summary>Recharge les données depuis la base.</summary>
    public virtual void Actualiser() { }

    // Fenêtre qui contient cet écran (null dans le concepteur de Visual Studio, d'où les « ?. »)
    protected MainForm? Principale => FindForm() as MainForm;

    /// <summary>Exécute une action en affichant les erreurs MySQL proprement, puis rafraîchit.</summary>
    protected void Executer(Action action, bool actualiserApres = true)
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            action();
            if (actualiserApres)
            {
                // Recharge l'écran et met à jour les compteurs de la barre d'état
                Actualiser();
                Principale?.RafraichirStatut();
            }
        }
        catch (Exception ex)
        {
            Ui.Erreur(ex);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // Petite barre verticale de séparation entre deux groupes de boutons
    protected static ToolStripSeparator Separateur() => new();
}
