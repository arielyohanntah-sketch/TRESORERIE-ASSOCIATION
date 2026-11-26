// =====================================================================================
//  Program.cs — POINT D'ENTRÉE DE L'APPLICATION
// -------------------------------------------------------------------------------------
//  C'est la première chose exécutée quand on lance l'application (F5 dans Visual Studio).
//  Ordre des écrans :
//      1. SplashForm   : écran de chargement qui teste la connexion à MySQL
//      2. LoginForm    : saisie de l'identifiant et du mot de passe
//      3. MainForm     : fenêtre principale (menu à gauche + écrans au centre)
//  Quand l'utilisateur clique sur « Se déconnecter », on revient à l'étape 2.
//
//  Pour modifier :
//    - la police par défaut de toute l'application → Theme.Police (UI/Theme.cs)
//    - les paramètres du serveur MySQL            → Data/Connexion.cs
// =====================================================================================

using System.Globalization;
using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.Forms;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO;

internal static class Program
{
    /// <summary>
    /// Démarrage : écran de chargement → connexion → fenêtre principale.
    /// « Se déconnecter » ramène à l'écran de connexion.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // Culture française : dates au format jj/mm/aaaa, virgule décimale,
        // espace comme séparateur de milliers (ex. « 1 250 000 FCFA »).
        var fr = new CultureInfo("fr-FR");
        CultureInfo.DefaultThreadCurrentCulture = fr;
        CultureInfo.DefaultThreadCurrentUICulture = fr;
        Thread.CurrentThread.CurrentCulture = fr;
        Thread.CurrentThread.CurrentUICulture = fr;

        // Réglages standards de WinForms (haute résolution, styles visuels Windows…)
        ApplicationConfiguration.Initialize();
        // Police utilisée par tous les contrôles qui n'en définissent pas une autre
        Application.SetDefaultFont(Theme.Police);

        // 1. Écran de chargement : vérifie la connexion à MySQL (paramètres dans Data/Connexion.cs)
        using (var splash = new SplashForm())
        {
            splash.ShowDialog();
            // Si le test de connexion a échoué, on affiche le message et on quitte l'application
            if (splash.Erreur != null)
            {
                MessageBox.Show(
                    "Impossible d'ouvrir la base de la trésorerie :\n\n" + splash.Erreur +
                    "\n\nVérifiez que WampServer est démarré et les paramètres du fichier Data\\Connexion.cs.",
                    "Trésorerie Asso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        // 2. Connexion puis fenêtre principale (boucle pour « Se déconnecter »)
        while (true)
        {
            using (var login = new LoginForm())
            {
                // L'utilisateur a fermé la fenêtre de connexion sans se connecter → on quitte
                if (login.ShowDialog() != DialogResult.OK) return;
            }

            // À ce stade, Session contient l'utilisateur connecté et ses droits
            var principale = new MainForm();
            Application.Run(principale);
            // Fenêtre fermée par la croix → on quitte ; par « Se déconnecter » → on recommence la boucle
            if (!principale.Deconnexion) return;
            Session.Fermer();
        }
    }
}
