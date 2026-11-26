// =====================================================================================
//  Forms/SplashForm.cs — ÉCRAN DE CHARGEMENT (premier écran affiché)
// -------------------------------------------------------------------------------------
//  Déroulement (méthode Travailler) :
//      12 %  ouverture (simple pause pour l'animation)
//      38 %  test de connexion au serveur MySQL         → Db.TesterConnexion()
//      62 %  vérification que le script SQL est importé  → Depot.VerifierStructure()
//      85 %  comptage des comptes                        → Depot.CompterComptes()
//     100 %  terminé : la fenêtre se ferme, Program.cs ouvre LoginForm
//  En cas d'erreur, la propriété Erreur est remplie et Program.cs affiche le message.
//
//  ➜ Pour raccourcir l'écran de chargement : diminuez les durées (en millisecondes)
//    passées à Etape(...) et la valeur 3200 dans Animer().
// =====================================================================================

using System.Diagnostics;
using SLN_TRESORERIE_ASSO.Data;

namespace SLN_TRESORERIE_ASSO.Forms;

/// <summary>
/// Écran de chargement : des piles de pièces d'or se remplissent pendant que
/// l'application vérifie la connexion à MySQL et la structure de la base.
/// La mise en page est dans SplashForm.Designer.cs, le dessin dans UI/DessinChargement.cs.
/// </summary>
public partial class SplashForm : Form
{
    // Minuterie qui fait avancer l'animation (~60 images/seconde)
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 16 };
    // Mesure le temps écoulé depuis l'ouverture (durée minimale d'affichage)
    private readonly Stopwatch _chrono = Stopwatch.StartNew();
    // Progression affichée (avance doucement) et progression à atteindre (fixée par chaque étape)
    private float _progresAffiche;
    private float _progresCible;
    private bool _termine;

    /// <summary>Message d'erreur si la connexion a échoué (null si tout va bien).</summary>
    public string? Erreur { get; private set; }

    public SplashForm()
    {
        InitializeComponent();
        _dessin.Progres = 0;
        _dessin.Message = "Ouverture du coffre-fort…";

        _timer.Tick += (_, _) => Animer();
        // Shown = la fenêtre vient d'apparaître : on lance l'animation puis les vérifications
        Shown += async (_, _) =>
        {
            _timer.Start();
            await Travailler();
        };
        FormClosed += (_, _) => _timer.Dispose();
    }

    // Enchaîne les étapes de vérification (le travail se fait en arrière-plan pour ne pas figer l'animation)
    private async Task Travailler()
    {
        try
        {
            await Etape(0.12f, "Ouverture du coffre-fort…", 600);
            await Etape(0.38f, $"Connexion au serveur MySQL ({Connexion.Serveur}:{Connexion.Port})…", 500,
                () => Db.TesterConnexion());
            await Etape(0.62f, $"Vérification de la base « {Connexion.Base} »…", 500, Depot.VerifierStructure);
            await Etape(0.85f, "Comptage des caisses et des comptes…", 500, () => Depot.CompterComptes());
            await Etape(1f, "Le coffre est ouvert. Bienvenue !", 400);
            _termine = true;
        }
        catch (Exception ex)
        {
            // Message clair pour les deux erreurs les plus fréquentes :
            //   1045 = mauvais mot de passe MySQL, 1049 = base inexistante (script non importé)
            Erreur = ex is MySqlConnector.MySqlException { Number: 1045 }
                ? "Accès refusé par MySQL : l'utilisateur ou le mot de passe du fichier Data\\Connexion.cs est incorrect " +
                  "(mettez le même que dans phpMyAdmin)."
                : ex is MySqlConnector.MySqlException { Number: 1049 }
                    ? $"La base « {Connexion.Base} » n'existe pas. Importez le script tresorerie_asso_v2.sql dans phpMyAdmin."
                    : ex.Message;
            // On fige la barre, on affiche l'échec un court instant puis on ferme
            _dessin.Message = "Échec de l'ouverture du coffre";
            _progresCible = _progresAffiche;
            _dessin.Invalidate();
            await Task.Delay(700);
            _timer.Stop();
            DialogResult = DialogResult.Abort;
            Close();
        }
    }

    /// <summary>
    /// Une étape : affiche le message, exécute le travail en arrière-plan, attend au moins
    /// dureeMini millisecondes (pour que l'animation soit visible), puis fixe la progression cible.
    /// </summary>
    private async Task Etape(float cible, string message, int dureeMini, Action? travail = null)
    {
        _dessin.Message = message;
        var tache = Task.Run(() => travail?.Invoke());
        await Task.WhenAll(tache, Task.Delay(dureeMini));
        _progresCible = cible;
    }

    // Appelée ~60 fois par seconde : rapproche la barre de la cible (7 % de l'écart à chaque fois)
    private void Animer()
    {
        _progresAffiche += (_progresCible - _progresAffiche) * 0.07f;
        if (Math.Abs(_progresCible - _progresAffiche) < 0.002f) _progresAffiche = _progresCible;
        _dessin.Progres = _progresAffiche;
        _dessin.Invalidate();

        // Fermeture quand tout est fini ET que l'écran est resté au moins 3,2 secondes
        if (_termine && _progresAffiche >= 0.999f && _chrono.ElapsedMilliseconds > 3200)
        {
            _timer.Stop();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
