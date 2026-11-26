// =====================================================================================
//  Data/Session.cs — UTILISATEUR CONNECTÉ
// -------------------------------------------------------------------------------------
//  Garde en mémoire, pendant toute l'utilisation de l'application, qui est connecté
//  (id, nom, rôle) et ce qu'il a le droit de faire.
//    - Rempli par Depot.Connecter() quand la connexion réussit (Session.Ouvrir).
//    - Vidé par Program.cs quand on clique sur « Se déconnecter » (Session.Fermer).
//  Partout dans le code on écrit par exemple :
//      if (Session.Peut("VALIDER")) { ... }   pour afficher ou cacher un bouton.
//
//  Pour ajouter un nouveau rôle : ajoutez-le dans la base (table des rôles et droits),
//  puis ajoutez une ligne dans RoleAffiche et DescriptionRole ci-dessous.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Data;

/// <summary>Utilisateur connecté à l'application et ses droits.</summary>
public static class Session
{
    // ---- Informations sur l'utilisateur (lecture seule en dehors de cette classe) ----
    // IdUtilisateur est null tant que personne n'est connecté.
    public static int? IdUtilisateur { get; private set; }
    public static string Nom { get; private set; } = "";
    public static string Prenoms { get; private set; } = "";
    public static string Login { get; private set; } = "";
    // Code du rôle tel qu'il est stocké dans la base : ADMIN, PRESIDENT, TRESORIER…
    public static string Role { get; private set; } = "";

    /// <summary>Actions autorisées pour le rôle, lues dans la base (fn_utilisateur_a_le_droit).</summary>
    private static readonly HashSet<string> Droits = new();

    // Vrai si un utilisateur est connecté
    public static bool Connecte => IdUtilisateur.HasValue;

    // « Prénoms Nom », utilisé dans le bandeau de bienvenue et la barre d'état
    public static string NomComplet => $"{Prenoms} {Nom}".Trim();

    public static bool EstAdmin => Role == "ADMIN";

    /// <summary>Le journal d'audit est réservé aux rôles de contrôle.</summary>
    public static bool PeutVoirAudit => Role is "ADMIN" or "PRESIDENT" or "COMMISSAIRE_COMPTES";

    /// <summary>SAISIR, VALIDER, OPERER, PARAMETRER, CLOTURER, UTILISATEURS ou CONSULTER.</summary>
    public static bool Peut(string action) => Droits.Contains(action);

    // Nom du rôle « lisible » affiché à l'écran (le code technique reste dans Role)
    public static string RoleAffiche => Role switch
    {
        "ADMIN" => "Administrateur",
        "PRESIDENT" => "Président",
        "TRESORIER" => "Trésorier",
        "TRESORIER_ADJOINT" => "Trésorier adjoint",
        "COMMISSAIRE_COMPTES" => "Commissaire aux comptes",
        "MEMBRE_BUREAU" => "Membre du bureau",
        _ => Role
    };

    // Phrase affichée sous « Bienvenue » dans le tableau de bord
    public static string DescriptionRole => Role switch
    {
        "ADMIN" => "Vous avez accès à toutes les fonctions et vous gérez les comptes des utilisateurs.",
        "PRESIDENT" => "Vous validez les opérations, supervisez la trésorerie et clôturez les exercices.",
        "TRESORIER" => "Vous saisissez et validez les opérations, faites les virements et tenez le paramétrage.",
        "TRESORIER_ADJOINT" => "Vous saisissez les opérations et les virements ; la validation revient au trésorier ou au président.",
        "COMMISSAIRE_COMPTES" => "Vous contrôlez les comptes : opérations, rapports et journal d'audit, en consultation.",
        "MEMBRE_BUREAU" => "Vous consultez les soldes, les opérations validées et le suivi du budget.",
        _ => ""
    };

    /// <summary>Enregistre l'utilisateur qui vient de se connecter (appelé par Depot.Connecter).</summary>
    public static void Ouvrir(int id, string nom, string prenoms, string login, string role, IEnumerable<string> droits)
    {
        IdUtilisateur = id;
        Nom = nom;
        Prenoms = prenoms;
        Login = login;
        Role = role;
        // On remplace l'ancienne liste de droits par celle du nouvel utilisateur
        Droits.Clear();
        foreach (var d in droits) Droits.Add(d);
    }

    /// <summary>Oublie l'utilisateur (déconnexion).</summary>
    public static void Fermer()
    {
        IdUtilisateur = null;
        Nom = Prenoms = Login = Role = "";
        Droits.Clear();
    }
}
