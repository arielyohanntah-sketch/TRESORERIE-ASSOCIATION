// =====================================================================================
//  Data/Db.cs — ACCÈS BAS NIVEAU À MySQL
// -------------------------------------------------------------------------------------
//  Petites fonctions réutilisables pour parler à la base. On ne les appelle presque
//  jamais directement depuis les écrans : on passe par Data/Depot.cs.
//
//    Ouvrir()          ouvre une connexion (à fermer avec « using »)
//    Table(sql, …)     exécute un SELECT et renvoie un DataTable (pour remplir une grille)
//    Scalaire(sql, …)  exécute une requête qui renvoie UNE seule valeur (COUNT, SUM…)
//    Executer(sql, …)  exécute INSERT / UPDATE / DELETE / CALL et renvoie le nb de lignes
//
//  Les paramètres sont passés sous forme de paires ("@nom", valeur), par exemple :
//      Db.Table("SELECT * FROM compte WHERE id_compte = @id", ("@id", 3));
//  ➜ Ne jamais coller une valeur saisie par l'utilisateur directement dans le texte SQL
//    (risque d'injection SQL) : toujours utiliser un paramètre @….
// =====================================================================================

using System.Data;
using MySqlConnector;

namespace SLN_TRESORERIE_ASSO.Data;

/// <summary>
/// Accès bas niveau à MySQL.
/// Chaque connexion ouverte reçoit la variable de session @id_utilisateur_courant :
/// c'est elle qu'utilisent les procédures, les triggers et le journal d'audit de la base.
/// </summary>
public static class Db
{
    /// <summary>Chaîne de connexion construite à partir de Data/Connexion.cs.</summary>
    public static string ChaineConnexion { get; set; } = new MySqlConnectionStringBuilder
    {
        Server = Connexion.Serveur,
        Port = Connexion.Port,
        Database = Connexion.Base,
        UserID = Connexion.Utilisateur,
        Password = Connexion.MotDePasse,
        // utf8mb4 : accents et caractères spéciaux correctement enregistrés
        CharacterSet = "utf8mb4",
        AllowUserVariables = true,   // indispensable pour SET @id_utilisateur_courant
        // Temps maximum (en secondes) pour se connecter, puis pour exécuter une requête
        ConnectionTimeout = 8,
        DefaultCommandTimeout = 30
    }.ConnectionString;

    /// <summary>
    /// Ouvre une connexion et indique à MySQL qui est connecté (pour les triggers et l'audit).
    /// À utiliser avec « using var cn = Db.Ouvrir(); » pour que la connexion soit refermée.
    /// </summary>
    public static MySqlConnection Ouvrir()
    {
        var cn = new MySqlConnection(ChaineConnexion);
        cn.Open();
        if (Session.IdUtilisateur is int id)
        {
            // Variable de session MySQL lue par les triggers (champ « fait par » du journal d'audit)
            using var cmd = new MySqlCommand("SET @id_utilisateur_courant = @p_id", cn);
            cmd.Parameters.AddWithValue("@p_id", id);
            cmd.ExecuteNonQuery();
        }
        return cn;
    }

    /// <summary>Teste la connexion et renvoie la version du serveur MySQL.</summary>
    public static string TesterConnexion()
    {
        using var cn = new MySqlConnection(ChaineConnexion);
        cn.Open();
        return cn.ServerVersion;
    }

    /// <summary>
    /// Prépare une commande SQL avec ses paramètres sur une connexion déjà ouverte
    /// (et éventuellement dans une transaction). Les valeurs null deviennent NULL en SQL.
    /// </summary>
    public static MySqlCommand Commande(MySqlConnection cn, string sql, (string Nom, object? Valeur)[] parametres,
        MySqlTransaction? tx = null)
    {
        var cmd = new MySqlCommand(sql, cn, tx);
        foreach (var (nom, valeur) in parametres)
            cmd.Parameters.AddWithValue(nom, valeur ?? DBNull.Value);
        return cmd;
    }

    /// <summary>Exécute un SELECT et renvoie toutes les lignes dans un DataTable.</summary>
    public static DataTable Table(string sql, params (string, object?)[] parametres)
    {
        using var cn = Ouvrir();
        using var cmd = Commande(cn, sql, parametres);
        using var da = new MySqlDataAdapter(cmd);
        var dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    /// <summary>Exécute une requête et renvoie la première colonne de la première ligne (ou null).</summary>
    public static object? Scalaire(string sql, params (string, object?)[] parametres)
    {
        using var cn = Ouvrir();
        using var cmd = Commande(cn, sql, parametres);
        var r = cmd.ExecuteScalar();
        return r is DBNull ? null : r;
    }

    /// <summary>Exécute INSERT / UPDATE / DELETE / CALL ; renvoie le nombre de lignes touchées.</summary>
    public static int Executer(string sql, params (string, object?)[] parametres)
    {
        using var cn = Ouvrir();
        using var cmd = Commande(cn, sql, parametres);
        return cmd.ExecuteNonQuery();
    }

    /// <summary>Appelle sp_verifier_droit : lève une erreur MySQL si le rôle n'a pas le droit.</summary>
    public static void VerifierDroit(string action) => Executer("CALL sp_verifier_droit(@a)", ("@a", action));

    /// <summary>Demande à la base (fonction fn_a_le_droit) si l'utilisateur connecté a ce droit.</summary>
    public static bool ADroit(string action) =>
        Convert.ToInt32(Scalaire("SELECT fn_a_le_droit(@a)", ("@a", action)) ?? 0) == 1;
}
