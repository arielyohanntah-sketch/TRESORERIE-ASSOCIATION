// =====================================================================================
//  Data/Connexion.cs — PARAMÈTRES DU SERVEUR MySQL
// -------------------------------------------------------------------------------------
//  C'est le SEUL fichier à modifier quand on change d'ordinateur ou de serveur.
//  Ces constantes sont lues par Data/Db.cs pour construire la chaîne de connexion.
//    Serveur     : nom ou adresse IP de la machine où tourne MySQL ("localhost" = ce PC)
//    Port        : 3306 par défaut avec WampServer (vérifiez dans phpMyAdmin si besoin)
//    Base        : nom de la base créée par le script tresorerie_asso_v2.sql
//    Utilisateur : compte MySQL (souvent "root" avec WampServer)
//    MotDePasse  : se règle dans Data/Connexion.local.cs (voir Connexion.local.exemple.cs)
//
//  Ce fichier peut être publié : il ne contient pas le mot de passe.
//    Chaque membre du groupe met son mot de passe dans son propre Connexion.local.cs.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Data;

/// <summary>
/// Paramètres de connexion au serveur MySQL (WampServer).
/// ➜ Mettez ici les mêmes informations que pour vous connecter à phpMyAdmin.
/// </summary>
public static partial class Connexion
{
    public const string Serveur = "localhost";
    public const int Port = 3306;
    public const string Base = "tresorerie_asso";
    public const string Utilisateur = "root";

    // Le mot de passe N'EST PAS dans ce fichier (qui est publié sur GitHub) :
    // il est lu dans Data/Connexion.local.cs, fichier personnel exclu de Git.
    // S'il n'existe pas, le mot de passe est vide "" (réglage par défaut de WampServer).
    public static string MotDePasse
    {
        get
        {
            string motDePasse = "";
            LireMotDePasse(ref motDePasse);
            return motDePasse;
        }
    }

    // Méthode « partielle » : son contenu est écrit dans Connexion.local.cs.
    // Si ce fichier est absent, C# ignore simplement l'appel (pas d'erreur de compilation).
    static partial void LireMotDePasse(ref string motDePasse);
}
