// =====================================================================================
//  Data/Depot.cs — TOUTES LES REQUÊTES SQL DE L'APPLICATION
// -------------------------------------------------------------------------------------
//  Les écrans (Forms et Views) ne contiennent pas de SQL : ils appellent les fonctions
//  de cette classe. Pour changer ce qu'affiche une grille, c'est donc ICI qu'il faut
//  modifier la requête correspondante.
//
//  Organisation (cherchez les lignes « // ---- ») :
//    démarrage        : VerifierStructure, CompterComptes
//    utilisateurs     : Connecter, ChangerMotDePasse, CreerUtilisateur, Utilisateurs…
//    listes de choix  : Comptes, Categories, Tiers, Centres, Exercices (pour les ComboBox)
//    espace personnel : Taches (tableau de bord), SixDerniersMois (graphique)
//    mouvements       : Mouvements, ChargerMouvement, EnregistrerMouvement, Valider…
//    virements        : Virements, CreerVirement, AnnulerVirement
//    comptes          : SoldesComptes, Releve
//    budget           : BudgetVsRealise
//    exercices/audit  : CloturerExercice, JournalAudit
//
//  Astuce : « AS `Libellé` » dans un SELECT donne le titre de la colonne affiché dans la
//  grille. Les colonnes dont le nom commence par id_ sont cachées automatiquement
//  (voir Theme.StyleGrille).
//
//  Les règles importantes (solde suffisant, numérotation des pièces, exercice clôturé…)
//  sont contrôlées par les TRIGGERS de la base : si une règle est violée, MySQL renvoie
//  une erreur que Ui.Erreur affiche en clair.
// =====================================================================================

using System.Data;
using MySqlConnector;

namespace SLN_TRESORERIE_ASSO.Data;

/// <summary>Une ligne de répartition analytique : quelle part du montant va à quel centre (table ventilation).</summary>
public sealed class LigneVentilation
{
    public int IdCentre { get; set; }
    public decimal Montant { get; set; }
}

/// <summary>Une pièce justificative jointe à un mouvement : facture, reçu… (table piece_justificative).</summary>
public sealed class LignePiece
{
    // FACTURE, RECU, BON_DE_CAISSE, RELEVE, CONTRAT ou AUTRE (valeurs autorisées par la base)
    public string TypePiece { get; set; } = "AUTRE";
    public string? Reference { get; set; }
    public DateTime DatePiece { get; set; } = DateTime.Today;
    public decimal? Montant { get; set; }
    public string? CheminFichier { get; set; }
}

/// <summary>
/// Un mouvement (recette ou dépense) tel qu'il est affiché et saisi dans SaisieMouvementForm.
/// Correspond à une ligne de la table mouvement + ses ventilations + ses pièces.
/// </summary>
public sealed class MouvementSaisie
{
    // null = nouveau mouvement pas encore enregistré
    public int? Id { get; set; }
    // BROUILLON → VALIDE → (éventuellement) ANNULE
    public string Statut { get; set; } = "BROUILLON";
    public string? NumeroPiece { get; set; }
    public string Type { get; set; } = "DEPENSE";
    public DateTime Date { get; set; } = DateTime.Today;
    public int IdCompte { get; set; }
    public int IdCategorie { get; set; }
    public int? IdTiers { get; set; }
    public decimal Montant { get; set; }
    public string ModePaiement { get; set; } = "ESPECES";
    public string? ReferenceExterne { get; set; }
    public string Libelle { get; set; } = "";
    // Personne qui a saisi le mouvement (id_utilisateur_saisie) et date de saisie
    public int? IdAuteur { get; set; }
    public string NomAuteur { get; set; } = "";
    public DateTime? DateSaisie { get; set; }

    /// <summary>Un brouillon ne peut être modifié que par la personne qui l'a saisi (ou un administrateur).</summary>
    public bool ModifiableParMoi =>
        Statut == "BROUILLON" && Session.Peut("SAISIR") && (Id == null || IdAuteur == Session.IdUtilisateur || Session.EstAdmin);

    // Lignes des onglets « Ventilation analytique » et « Pièces justificatives »
    public List<LigneVentilation> Ventilations { get; } = new();
    public List<LignePiece> Pieces { get; } = new();
}

/// <summary>Toutes les requêtes métier de l'application.</summary>
public static class Depot
{
    // ------------------------------------------------------------------ démarrage

    /// <summary>Vérifie que la base contient bien la version complète du script (procédures, vue...).</summary>
    public static void VerifierStructure()
    {
        var n = Convert.ToInt32(Db.Scalaire(
            "SELECT COUNT(*) FROM information_schema.ROUTINES " +
            "WHERE ROUTINE_SCHEMA = DATABASE() AND ROUTINE_NAME IN ('sp_utilisateur_connecter','sp_utilisateur_creer','fn_solde_compte')"));
        if (n < 3)
            throw new InvalidOperationException(
                "La base est incomplète : les procédures sp_utilisateur_connecter / sp_utilisateur_creer sont absentes.\n" +
                "Importez le fichier tresorerie_asso_v2.sql dans phpMyAdmin.");
    }

    /// <summary>Nombre de comptes de trésorerie (affiché sur l'écran de chargement).</summary>
    public static int CompterComptes() => Convert.ToInt32(Db.Scalaire("SELECT COUNT(*) FROM compte"));

    // ------------------------------------------------------------------ utilisateurs

    /// <summary>Vrai si la table utilisateur est vide : l'écran de connexion propose alors de créer le 1er administrateur.</summary>
    public static bool AucunUtilisateur() => Convert.ToInt64(Db.Scalaire("SELECT COUNT(*) FROM utilisateur")) == 0;

    /// <summary>
    /// Vérifie l'identifiant et le mot de passe (procédure sp_utilisateur_connecter, qui compte aussi
    /// les échecs et bloque le compte après 5 essais), puis remplit Session avec l'utilisateur et ses droits.
    /// Lève une erreur si la connexion est refusée.
    /// </summary>
    public static void Connecter(string login, string motDePasse)
    {
        Session.Fermer();
        // 1) Vérification du mot de passe par la base : renvoie une ligne avec l'utilisateur
        using var cn = Db.Ouvrir();
        using var cmd = Db.Commande(cn, "CALL sp_utilisateur_connecter(@l, @m)", new (string, object?)[] { ("@l", login), ("@m", motDePasse) });
        int id;
        string nom, prenoms, log, role;
        using (var r = cmd.ExecuteReader())
        {
            if (!r.Read()) throw new InvalidOperationException("Connexion refusée.");
            id = Convert.ToInt32(r["id_utilisateur"]);
            nom = r.GetString("nom");
            prenoms = r.GetString("prenoms");
            log = r.GetString("login");
            role = r.GetString("role");
        }

        // 2) Droits du rôle, tels que définis dans la base (fn_utilisateur_a_le_droit)
        //    Une seule requête qui renvoie une colonne 0/1 par action.
        var actions = new[] { "SAISIR", "VALIDER", "OPERER", "PARAMETRER", "CLOTURER", "UTILISATEURS", "CONSULTER" };
        var droits = new List<string>();
        using (var c2 = Db.Commande(cn, "SELECT " + string.Join(", ", actions.Select(a => $"fn_utilisateur_a_le_droit(@id, '{a}') AS {a}")),
                   new (string, object?)[] { ("@id", id) }))
        using (var r2 = c2.ExecuteReader())
        {
            r2.Read();
            foreach (var a in actions)
                if (Convert.ToInt32(r2[a]) == 1) droits.Add(a);
        }
        // 3) On mémorise l'utilisateur connecté pour le reste de l'application
        Session.Ouvrir(id, nom, prenoms, log, role, droits);
    }

    /// <summary>Change le mot de passe de l'utilisateur connecté (vérifie l'ancien).</summary>
    public static void ChangerMotDePasse(string ancien, string nouveau)
    {
        if (nouveau.Length < 8) throw new InvalidOperationException("Le nouveau mot de passe doit contenir au moins 8 caractères.");
        // Nouveau « sel » aléatoire : le mot de passe n'est jamais stocké en clair, seulement son empreinte (hash)
        var sel = Convert.ToHexString(System.Security.Cryptography.RandomNumberGenerator.GetBytes(16)).ToLowerInvariant();
        int n = Db.Executer(@"
UPDATE utilisateur
   SET sel = @sel, mot_de_passe_hash = fn_hash_mot_de_passe(@sel, @nouveau)
 WHERE id_utilisateur = @id AND mot_de_passe_hash = fn_hash_mot_de_passe(sel, @ancien)",
            ("@sel", sel), ("@nouveau", nouveau), ("@ancien", ancien), ("@id", Session.IdUtilisateur));
        // Aucune ligne modifiée = la condition sur l'ancien mot de passe n'était pas remplie
        if (n == 0) throw new InvalidOperationException("L'ancien mot de passe est incorrect.");
    }

    /// <summary>Nombre d'essais restants avant blocage (5 maximum) ; -1 si l'identifiant n'existe pas.</summary>
    public static int TentativesRestantes(string login)
    {
        var v = Db.Scalaire("SELECT tentatives_echouees FROM utilisateur WHERE login = @l", ("@l", login));
        return v == null ? -1 : Math.Max(0, 5 - Convert.ToInt32(v));
    }

    /// <summary>Crée un utilisateur via la procédure sp_utilisateur_creer (qui calcule le hash du mot de passe).</summary>
    public static void CreerUtilisateur(string nom, string prenoms, string login, string motDePasse, string role, string? email) =>
        Db.Executer("CALL sp_utilisateur_creer(@n, @p, @l, @m, @r, @e)",
            ("@n", nom), ("@p", prenoms), ("@l", login), ("@m", motDePasse), ("@r", role),
            ("@e", string.IsNullOrWhiteSpace(email) ? null : email.Trim()));

    /// <summary>Liste des utilisateurs pour l'écran « Utilisateurs » (réservé à l'administrateur).</summary>
    public static DataTable Utilisateurs() => Db.Table(
        "SELECT id_utilisateur, nom AS `Nom`, prenoms AS `Prénoms`, login AS `Identifiant`, role AS `Rôle`, " +
        "email AS `E-mail`, telephone AS `Téléphone`, actif AS `Actif`, tentatives_echouees AS `Échecs`, " +
        "derniere_connexion AS `Dernière connexion` FROM utilisateur ORDER BY nom, prenoms");

    /// <summary>Remet à zéro le compteur d'échecs d'un compte bloqué.</summary>
    public static void DebloquerUtilisateur(int id) => Db.Executer("CALL sp_utilisateur_debloquer(@id)", ("@id", id));

    /// <summary>Active ou désactive un compte utilisateur (un compte désactivé ne peut plus se connecter).</summary>
    public static void BasculerActif(int id)
    {
        Db.VerifierDroit("UTILISATEURS");
        if (id == Session.IdUtilisateur) throw new InvalidOperationException("Vous ne pouvez pas désactiver votre propre compte.");
        Db.Executer("UPDATE utilisateur SET actif = NOT actif WHERE id_utilisateur = @id", ("@id", id));
    }

    // ------------------------------------------------------------------ listes de choix
    // Ces fonctions renvoient toujours deux colonnes « id » et « lib » : c'est ce qu'attend
    // Ui.Liste() pour remplir une ComboBox (ValueMember = id, DisplayMember = lib).

    /// <summary>Comptes actifs avec leur solde. avecTous = ajoute « Tous les comptes » en tête (pour les filtres).</summary>
    public static DataTable Comptes(bool avecTous = false)
    {
        var dt = Db.Table(
            "SELECT id_compte AS id, CONCAT(code, ' — ', libelle) AS lib, fn_solde_compte(id_compte, NULL) AS solde, " +
            "decouvert_autorise AS decouvert FROM compte WHERE actif = 1 ORDER BY code");
        if (avecTous) AjouterLigneTous(dt, "Tous les comptes");
        return dt;
    }

    /// <summary>Catégories actives. sens = "RECETTE" ou "DEPENSE" pour filtrer, null pour toutes.</summary>
    public static DataTable Categories(string? sens)
    {
        return Db.Table(
            "SELECT id_categorie AS id, CONCAT(code, ' — ', libelle) AS lib FROM categorie " +
            "WHERE actif = 1 AND (@s IS NULL OR sens = @s) ORDER BY code", ("@s", sens));
    }

    /// <summary>Tiers actifs (fournisseurs, membres, partenaires) précédés de « (aucun) ».</summary>
    public static DataTable Tiers()
    {
        var dt = Db.Table("SELECT id_tiers AS id, nom AS lib FROM tiers WHERE actif = 1 ORDER BY nom");
        AjouterLigneTous(dt, "(aucun)");
        return dt;
    }

    /// <summary>Centres analytiques actifs (projets / activités de l'association).</summary>
    public static DataTable Centres() => Db.Table(
        "SELECT id_centre AS id, CONCAT(code, ' — ', libelle) AS lib FROM centre_analytique WHERE actif = 1 ORDER BY code");

    /// <summary>Exercices comptables, du plus récent au plus ancien, avec « (ouvert) » / « (clôturé) ».</summary>
    public static DataTable Exercices() => Db.Table(
        "SELECT id_exercice AS id, CONCAT(libelle, IF(statut = 'CLOTURE', ' (clôturé)', ' (ouvert)')) AS lib, " +
        "date_debut, date_fin, statut FROM exercice ORDER BY date_debut DESC");

    // Ajoute en première position une ligne sans id (« Tous », « (aucun) »…)
    private static void AjouterLigneTous(DataTable dt, string libelle)
    {
        var r = dt.NewRow();
        r["id"] = DBNull.Value;
        r["lib"] = libelle;
        dt.Rows.InsertAt(r, 0);
    }

    /// <summary>Exercice qui contient la date du jour (libellé, statut) ou null.</summary>
    public static (string Libelle, string Statut)? ExerciceCourant()
    {
        var dt = Db.Table("SELECT libelle, statut FROM exercice WHERE CURDATE() BETWEEN date_debut AND date_fin LIMIT 1");
        if (dt.Rows.Count == 0) return null;
        return (dt.Rows[0]["libelle"].ToString()!, dt.Rows[0]["statut"].ToString()!);
    }

    /// <summary>Brouillons en attente : tous (pour un valideur) ou seulement les miens.</summary>
    public static int NombreBrouillons(bool seulementLesMiens = false) =>
        Convert.ToInt32(Db.Scalaire(
            "SELECT COUNT(*) FROM mouvement WHERE statut = 'BROUILLON' AND (@moi IS NULL OR id_utilisateur_saisie = @moi)",
            ("@moi", seulementLesMiens ? Session.IdUtilisateur : null)));

    // ------------------------------------------------------------------ espace personnel

    /// <summary>
    /// Règle de confidentialité : un brouillon n'est visible que par son auteur,
    /// ou par ceux qui ont le droit de le valider.
    /// </summary>
    private const string FiltreVisibilite =
        "(m.statut <> 'BROUILLON' OR m.id_utilisateur_saisie = @moi OR @valideur = 1)";

    // Valeurs des paramètres @moi et @valideur utilisés par FiltreVisibilite
    private static (string, object?)[] ParamsVisibilite() =>
        new (string, object?)[] { ("@moi", Session.IdUtilisateur), ("@valideur", Session.Peut("VALIDER") ? 1 : 0) };

    /// <summary>Chiffres qui alimentent « Mes tâches » (TableauDeBordVue choisit lesquels afficher selon le rôle).</summary>
    public sealed class CompteursTaches
    {
        public int MesBrouillons, AValider, ExercicesACloturer, Comptes, Categories, Centres, UtilisateursBloques,
            SansJustificatif, AnnulationsDuMois, MesSaisiesDuMois;
        public bool ExerciceCourant;
    }

    /// <summary>Compteurs utilisés pour la liste « Mes tâches » du tableau de bord.</summary>
    public static CompteursTaches Taches()
    {
        var dt = Db.Table(@"
SELECT
  (SELECT COUNT(*) FROM mouvement WHERE statut = 'BROUILLON' AND id_utilisateur_saisie = @moi) AS mes_brouillons,
  (SELECT COUNT(*) FROM mouvement WHERE statut = 'BROUILLON') AS a_valider,
  (SELECT COUNT(*) FROM exercice WHERE statut = 'OUVERT' AND date_fin < CURDATE()) AS a_cloturer,
  (SELECT COUNT(*) FROM exercice WHERE CURDATE() BETWEEN date_debut AND date_fin) AS ex_courant,
  (SELECT COUNT(*) FROM compte WHERE actif = 1) AS comptes,
  (SELECT COUNT(*) FROM categorie WHERE actif = 1) AS categories,
  (SELECT COUNT(*) FROM centre_analytique WHERE actif = 1) AS centres,
  (SELECT COUNT(*) FROM utilisateur WHERE tentatives_echouees >= 5) AS bloques,
  (SELECT COUNT(*) FROM mouvement m WHERE m.statut = 'VALIDE'
      AND NOT EXISTS (SELECT 1 FROM piece_justificative p WHERE p.id_mouvement = m.id_mouvement)) AS sans_piece,
  (SELECT COUNT(*) FROM mouvement WHERE statut = 'ANNULE'
      AND annule_at >= DATE_FORMAT(CURDATE(), '%Y-%m-01')) AS annulations,
  (SELECT COUNT(*) FROM mouvement WHERE id_utilisateur_saisie = @moi
      AND created_at >= DATE_FORMAT(CURDATE(), '%Y-%m-01')) AS mes_saisies", ("@moi", Session.IdUtilisateur));
        var r = dt.Rows[0];
        // Petite fonction locale : lit une colonne de la ligne et la convertit en entier
        int I(string c) => Convert.ToInt32(r[c]);
        return new CompteursTaches
        {
            MesBrouillons = I("mes_brouillons"), AValider = I("a_valider"), ExercicesACloturer = I("a_cloturer"),
            ExerciceCourant = I("ex_courant") > 0, Comptes = I("comptes"), Categories = I("categories"), Centres = I("centres"),
            UtilisateursBloques = I("bloques"), SansJustificatif = I("sans_piece"), AnnulationsDuMois = I("annulations"),
            MesSaisiesDuMois = I("mes_saisies")
        };
    }

    /// <summary>Recettes et dépenses validées des 6 derniers mois (pour le graphique).</summary>
    public static List<(DateTime Mois, decimal Recettes, decimal Depenses)> SixDerniersMois()
    {
        var debut = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-5);
        var dt = Db.Table(@"
SELECT YEAR(date_mouvement) AS a, MONTH(date_mouvement) AS m,
       SUM(IF(type_mouvement = 'RECETTE', montant, 0)) AS r, SUM(IF(type_mouvement = 'DEPENSE', montant, 0)) AS d
  FROM mouvement WHERE statut = 'VALIDE' AND date_mouvement >= @debut
 GROUP BY YEAR(date_mouvement), MONTH(date_mouvement)", ("@debut", debut));
        // On construit toujours 6 mois, même ceux sans opération (montants à 0)
        var liste = new List<(DateTime, decimal, decimal)>();
        for (int i = 0; i < 6; i++)
        {
            var mois = debut.AddMonths(i);
            var ligne = dt.Rows.Cast<DataRow>().FirstOrDefault(x => Convert.ToInt32(x["a"]) == mois.Year && Convert.ToInt32(x["m"]) == mois.Month);
            liste.Add((mois, ligne == null ? 0 : Convert.ToDecimal(ligne["r"]), ligne == null ? 0 : Convert.ToDecimal(ligne["d"])));
        }
        return liste;
    }

    /// <summary>Numéro de pièce attribué par le trigger à la validation (ex. « DEP-2026-00012 », « REC-… »).</summary>
    public static string? NumeroPiece(int idMouvement) =>
        Db.Scalaire("SELECT numero_piece FROM mouvement WHERE id_mouvement = @id", ("@id", idMouvement))?.ToString();

    // ------------------------------------------------------------------ mouvements

    /// <summary>
    /// Liste des mouvements de l'écran « Opérations », filtrée par période, compte, type, statut et texte recherché.
    /// Un paramètre à null signifie « pas de filtre ». seulementMesSaisies = écran « Mes saisies ».
    /// Les brouillons des autres utilisateurs sont exclus sauf pour les valideurs (FiltreVisibilite).
    /// </summary>
    public static DataTable Mouvements(DateTime du, DateTime au, object? idCompte, string? type, string? statut, string recherche,
        bool seulementMesSaisies = false)
    {
        var p = new List<(string, object?)>
        {
            ("@du", du.Date), ("@au", au.Date), ("@cpt", idCompte is DBNull ? null : idCompte), ("@type", type), ("@st", statut),
            ("@q", recherche.Trim()), ("@ql", "%" + recherche.Trim() + "%"), ("@seulmoi", seulementMesSaisies ? 1 : 0)
        };
        p.AddRange(ParamsVisibilite());
        return Db.Table(@"
SELECT m.id_mouvement, m.type_mouvement,
       COALESCE(m.numero_piece, '—') AS `N° pièce`, m.date_mouvement AS `Date`,
       IF(m.type_mouvement = 'RECETTE', 'Recette', 'Dépense') AS `Type`,
       m.libelle AS `Libellé`, c.libelle AS `Catégorie`, cp.libelle AS `Compte`, t.nom AS `Tiers`,
       m.montant AS `Montant`, m.statut AS `Statut`, CONCAT(u.prenoms, ' ', u.nom) AS `Saisi par`
  FROM mouvement m
  JOIN categorie c  ON c.id_categorie = m.id_categorie
  JOIN compte cp    ON cp.id_compte = m.id_compte
  JOIN utilisateur u ON u.id_utilisateur = m.id_utilisateur_saisie
  LEFT JOIN tiers t ON t.id_tiers = m.id_tiers
 WHERE m.date_mouvement BETWEEN @du AND @au
   AND (@cpt IS NULL OR m.id_compte = @cpt)
   AND (@type IS NULL OR m.type_mouvement = @type)
   AND (@st IS NULL OR m.statut = @st)
   AND (@q = '' OR m.libelle LIKE @ql OR m.numero_piece LIKE @ql OR t.nom LIKE @ql)
   AND (@seulmoi = 0 OR m.id_utilisateur_saisie = @moi)
   AND " + FiltreVisibilite + @"
 ORDER BY m.date_mouvement DESC, m.id_mouvement DESC", p.ToArray());
    }

    /// <summary>Les dernières opérations saisies (grille du bas du tableau de bord).</summary>
    public static DataTable DerniersMouvements(int nombre) => Db.Table(@"
SELECT m.id_mouvement, m.type_mouvement, m.date_mouvement AS `Date`, COALESCE(m.numero_piece, '—') AS `N° pièce`,
       m.libelle AS `Libellé`, c.libelle AS `Catégorie`, cp.libelle AS `Compte`, m.montant AS `Montant`, m.statut AS `Statut`
  FROM mouvement m JOIN categorie c ON c.id_categorie = m.id_categorie JOIN compte cp ON cp.id_compte = m.id_compte
 WHERE " + FiltreVisibilite + @"
 ORDER BY m.created_at DESC, m.id_mouvement DESC LIMIT " + nombre, ParamsVisibilite());

    /// <summary>Total des recettes et des dépenses validées du mois en cours (cartes du tableau de bord).</summary>
    public static (decimal Recettes, decimal Depenses) TotauxMois()
    {
        var dt = Db.Table(@"
SELECT COALESCE(SUM(IF(type_mouvement = 'RECETTE', montant, 0)), 0) AS r,
       COALESCE(SUM(IF(type_mouvement = 'DEPENSE', montant, 0)), 0) AS d
  FROM mouvement
 WHERE statut = 'VALIDE' AND YEAR(date_mouvement) = YEAR(CURDATE()) AND MONTH(date_mouvement) = MONTH(CURDATE())");
        return (Convert.ToDecimal(dt.Rows[0]["r"]), Convert.ToDecimal(dt.Rows[0]["d"]));
    }

    /// <summary>
    /// Lit un mouvement complet (avec ventilations et pièces) pour l'ouvrir dans SaisieMouvementForm.
    /// Refuse d'ouvrir le brouillon privé d'un autre utilisateur.
    /// </summary>
    public static MouvementSaisie ChargerMouvement(int id)
    {
        var prm = new List<(string, object?)> { ("@id", id) };
        prm.AddRange(ParamsVisibilite());
        var dt = Db.Table(@"SELECT m.*, CONCAT(u.prenoms, ' ', u.nom) AS auteur FROM mouvement m
                             JOIN utilisateur u ON u.id_utilisateur = m.id_utilisateur_saisie
                            WHERE m.id_mouvement = @id AND " + FiltreVisibilite, prm.ToArray());
        if (dt.Rows.Count == 0) throw new InvalidOperationException("Ce mouvement n'existe pas ou est un brouillon privé d'un autre utilisateur.");
        var r = dt.Rows[0];
        var m = new MouvementSaisie
        {
            Id = id,
            Statut = r["statut"].ToString()!,
            NumeroPiece = r["numero_piece"] as string,
            Type = r["type_mouvement"].ToString()!,
            Date = Convert.ToDateTime(r["date_mouvement"]),
            IdCompte = Convert.ToInt32(r["id_compte"]),
            IdCategorie = Convert.ToInt32(r["id_categorie"]),
            IdTiers = r["id_tiers"] is DBNull ? null : Convert.ToInt32(r["id_tiers"]),
            Montant = Convert.ToDecimal(r["montant"]),
            ModePaiement = r["mode_paiement"].ToString()!,
            ReferenceExterne = r["reference_externe"] as string,
            Libelle = r["libelle"].ToString()!,
            IdAuteur = Convert.ToInt32(r["id_utilisateur_saisie"]),
            NomAuteur = r["auteur"].ToString()!,
            DateSaisie = Convert.ToDateTime(r["created_at"])
        };
        // Lignes de ventilation analytique, puis pièces justificatives
        foreach (DataRow v in Db.Table("SELECT id_centre, montant FROM ventilation WHERE id_mouvement = @id ORDER BY id_ventilation", ("@id", id)).Rows)
            m.Ventilations.Add(new LigneVentilation { IdCentre = Convert.ToInt32(v["id_centre"]), Montant = Convert.ToDecimal(v["montant"]) });
        foreach (DataRow p in Db.Table("SELECT * FROM piece_justificative WHERE id_mouvement = @id ORDER BY id_justificatif", ("@id", id)).Rows)
            m.Pieces.Add(new LignePiece
            {
                TypePiece = p["type_piece"].ToString()!,
                Reference = p["reference"] as string,
                DatePiece = Convert.ToDateTime(p["date_piece"]),
                Montant = p["montant"] is DBNull ? null : Convert.ToDecimal(p["montant"]),
                CheminFichier = p["chemin_fichier"] as string
            });
        return m;
    }

    /// <summary>
    /// Enregistre un mouvement (création ou modification d'un brouillon) avec sa ventilation et ses pièces,
    /// dans une seule transaction. Si <paramref name="valider"/> est vrai, le mouvement est ensuite validé :
    /// le trigger de la base contrôle le solde et attribue le numéro de pièce.
    /// </summary>
    public static int EnregistrerMouvement(MouvementSaisie m, bool valider)
    {
        // La base vérifie elle-même les droits (une erreur est levée si le rôle ne les a pas)
        Db.VerifierDroit("SAISIR");
        if (valider) Db.VerifierDroit("VALIDER");

        // Transaction : soit tout est enregistré (mouvement + ventilations + pièces), soit rien
        using var cn = Db.Ouvrir();
        using var tx = cn.BeginTransaction();
        try
        {
            var champs = new (string, object?)[]
            {
                ("@type", m.Type), ("@date", m.Date.Date), ("@cpt", m.IdCompte), ("@cat", m.IdCategorie), ("@tiers", m.IdTiers),
                ("@mt", m.Montant), ("@mode", m.ModePaiement), ("@ref", Vide(m.ReferenceExterne)), ("@lib", m.Libelle.Trim()),
                ("@user", Session.IdUtilisateur), ("@id", m.Id), ("@admin", Session.EstAdmin ? 1 : 0)
            };
            int id;
            if (m.Id is null)
            {
                // Nouveau mouvement : toujours créé en BROUILLON, il sera validé plus bas si demandé
                using var cmd = Db.Commande(cn, @"
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement,
                       reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES (@type, @date, @cpt, @cat, @tiers, @mt, @mode, @ref, @lib, 'BROUILLON', @user)", champs, tx);
                cmd.ExecuteNonQuery();
                id = (int)cmd.LastInsertedId;
            }
            else
            {
                // Modification d'un brouillon existant : seulement par son auteur (ou un administrateur)
                id = m.Id.Value;
                using var cmd = Db.Commande(cn, @"
UPDATE mouvement SET type_mouvement = @type, date_mouvement = @date, id_compte = @cpt, id_categorie = @cat,
       id_tiers = @tiers, montant = @mt, mode_paiement = @mode, reference_externe = @ref, libelle = @lib
 WHERE id_mouvement = @id AND statut = 'BROUILLON' AND (id_utilisateur_saisie = @user OR @admin = 1)", champs, tx);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException("Seul l'auteur d'un brouillon peut le modifier.");
                // On efface les anciennes lignes de ventilation et de pièces ; elles sont réinsérées juste après
                using var del1 = Db.Commande(cn, "DELETE FROM ventilation WHERE id_mouvement = @id", new (string, object?)[] { ("@id", id) }, tx);
                del1.ExecuteNonQuery();
                using var del2 = Db.Commande(cn, "DELETE FROM piece_justificative WHERE id_mouvement = @id", new (string, object?)[] { ("@id", id) }, tx);
                del2.ExecuteNonQuery();
            }

            foreach (var v in m.Ventilations)
            {
                using var cmd = Db.Commande(cn, "INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@id, @c, @mt)",
                    new (string, object?)[] { ("@id", id), ("@c", v.IdCentre), ("@mt", v.Montant) }, tx);
                cmd.ExecuteNonQuery();
            }

            foreach (var p in m.Pieces)
            {
                using var cmd = Db.Commande(cn, @"
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant, chemin_fichier)
VALUES (@id, @t, @r, @d, @mt, @f)",
                    new (string, object?)[] { ("@id", id), ("@t", p.TypePiece), ("@r", Vide(p.Reference)), ("@d", p.DatePiece.Date), ("@mt", p.Montant), ("@f", Vide(p.CheminFichier)) }, tx);
                cmd.ExecuteNonQuery();
            }

            if (valider)
            {
                // Passage en VALIDE : le trigger contrôle le solde du compte et donne le numéro de pièce
                using var cmd = Db.Commande(cn, "UPDATE mouvement SET statut = 'VALIDE' WHERE id_mouvement = @id",
                    new (string, object?)[] { ("@id", id) }, tx);
                cmd.ExecuteNonQuery();
            }

            tx.Commit();
            return id;
        }
        catch
        {
            // Une erreur (règle de la base non respectée…) → on annule tout ce qui a été fait
            tx.Rollback();
            throw;
        }
    }

    /// <summary>Valide un brouillon depuis la liste des opérations (droit VALIDER).</summary>
    public static void ValiderMouvement(int id)
    {
        Db.VerifierDroit("VALIDER");
        Db.Executer("UPDATE mouvement SET statut = 'VALIDE' WHERE id_mouvement = @id AND statut = 'BROUILLON'", ("@id", id));
    }

    /// <summary>Annule un mouvement validé (on ne supprime jamais une opération validée : on l'annule avec un motif).</summary>
    public static void AnnulerMouvement(int id, string motif)
    {
        Db.VerifierDroit("VALIDER");
        Db.Executer("UPDATE mouvement SET statut = 'ANNULE', motif_annulation = @m WHERE id_mouvement = @id", ("@id", id), ("@m", motif));
    }

    /// <summary>Supprime un brouillon (seulement par son auteur ou un administrateur ; la base refuse pour un mouvement validé).</summary>
    public static void SupprimerBrouillon(int id)
    {
        Db.VerifierDroit("SAISIR");
        var auteur = Db.Scalaire("SELECT id_utilisateur_saisie FROM mouvement WHERE id_mouvement = @id", ("@id", id));
        if (auteur != null && Convert.ToInt32(auteur) != Session.IdUtilisateur && !Session.EstAdmin)
            throw new InvalidOperationException("Seul l'auteur d'un brouillon peut le supprimer.");
        Db.Executer("DELETE FROM mouvement WHERE id_mouvement = @id", ("@id", id));
    }

    // ------------------------------------------------------------------ virements

    /// <summary>Liste de tous les virements entre comptes (écran « Virements »).</summary>
    public static DataTable Virements() => Db.Table(@"
SELECT v.id_virement, v.numero_piece AS `N° pièce`, v.date_virement AS `Date`, s.libelle AS `De`, d.libelle AS `Vers`,
       v.libelle AS `Libellé`, v.montant AS `Montant`, v.statut AS `Statut`, v.motif_annulation AS `Motif d'annulation`
  FROM virement v JOIN compte s ON s.id_compte = v.id_compte_source JOIN compte d ON d.id_compte = v.id_compte_dest
 ORDER BY v.date_virement DESC, v.id_virement DESC");

    /// <summary>Crée un virement d'un compte vers un autre (le trigger vérifie le solde du compte source).</summary>
    public static void CreerVirement(DateTime date, int source, int destination, decimal montant, string libelle)
    {
        Db.VerifierDroit("OPERER");
        Db.Executer(@"
INSERT INTO virement (date_virement, id_compte_source, id_compte_dest, montant, libelle, id_utilisateur_saisie)
VALUES (@d, @s, @dst, @mt, @lib, @u)",
            ("@d", date.Date), ("@s", source), ("@dst", destination), ("@mt", montant), ("@lib", libelle.Trim()), ("@u", Session.IdUtilisateur));
    }

    /// <summary>Annule un virement avec un motif obligatoire.</summary>
    public static void AnnulerVirement(int id, string motif)
    {
        Db.VerifierDroit("OPERER");
        Db.Executer("UPDATE virement SET statut = 'ANNULE', motif_annulation = @m WHERE id_virement = @id", ("@id", id), ("@m", motif));
    }

    // ------------------------------------------------------------------ comptes

    /// <summary>Tous les comptes avec leur solde actuel (calculé par la fonction fn_solde_compte).</summary>
    public static DataTable SoldesComptes() => Db.Table(@"
SELECT id_compte, code AS `Code`, libelle AS `Libellé`, type_compte AS `Type`, etablissement AS `Établissement`,
       numero_compte AS `N° de compte`, solde_initial AS `Solde initial`, decouvert_autorise AS `Découvert`,
       fn_solde_compte(id_compte, NULL) AS `Solde actuel`, actif AS `Actif`
  FROM compte ORDER BY actif DESC, code");

    /// <summary>Relevé d'un compte : solde de départ, opérations validées (mouvements et virements), solde courant.</summary>
    public static DataTable Releve(int idCompte, DateTime du, DateTime au)
    {
        // Solde la veille du début de la période
        var soldeDepart = Convert.ToDecimal(Db.Scalaire("SELECT fn_solde_compte(@c, @d)", ("@c", idCompte), ("@d", du.Date.AddDays(-1))) ?? 0m);
        // Mouvements ET virements validés du compte, réunis (UNION ALL) et triés par date
        var lignes = Db.Table(@"
SELECT d, num, lib, entree, sortie FROM (
  SELECT date_mouvement AS d, numero_piece AS num, libelle AS lib,
         IF(type_mouvement = 'RECETTE', montant, 0) AS entree, IF(type_mouvement = 'DEPENSE', montant, 0) AS sortie,
         created_at AS c, id_mouvement AS k
    FROM mouvement WHERE id_compte = @c AND statut = 'VALIDE' AND date_mouvement BETWEEN @du AND @au
  UNION ALL
  SELECT date_virement, numero_piece, CONCAT('Virement : ', libelle),
         IF(id_compte_dest = @c, montant, 0), IF(id_compte_source = @c, montant, 0), created_at, id_virement
    FROM virement WHERE (id_compte_source = @c OR id_compte_dest = @c) AND statut = 'VALIDE' AND date_virement BETWEEN @du AND @au
) x ORDER BY d, c, k", ("@c", idCompte), ("@du", du.Date), ("@au", au.Date));

        // Tableau affiché : la colonne « report » (cachée) marque les lignes de solde en gras
        var dt = new DataTable();
        dt.Columns.Add("Date", typeof(DateTime));
        dt.Columns.Add("N° pièce", typeof(string));
        dt.Columns.Add("Libellé", typeof(string));
        dt.Columns.Add("Entrée", typeof(decimal));
        dt.Columns.Add("Sortie", typeof(decimal));
        dt.Columns.Add("Solde", typeof(decimal));
        dt.Columns.Add("report", typeof(bool));

        dt.Rows.Add(du.Date.AddDays(-1), "", $"Solde au {du.AddDays(-1):dd/MM/yyyy}", DBNull.Value, DBNull.Value, soldeDepart, true);
        // Solde cumulé ligne après ligne
        decimal solde = soldeDepart, totalIn = 0, totalOut = 0;
        foreach (DataRow r in lignes.Rows)
        {
            var e = Convert.ToDecimal(r["entree"]);
            var s = Convert.ToDecimal(r["sortie"]);
            solde += e - s; totalIn += e; totalOut += s;
            dt.Rows.Add(r["d"], r["num"], r["lib"], e == 0 ? DBNull.Value : e, s == 0 ? DBNull.Value : s, solde, false);
        }
        dt.Rows.Add(au.Date, "", $"Solde au {au:dd/MM/yyyy}", totalIn, totalOut, solde, true);
        return dt;
    }

    // ------------------------------------------------------------------ budget

    /// <summary>
    /// Pour chaque centre analytique : recettes/dépenses prévues (budget_centre) et réalisées
    /// (somme des ventilations) sur l'exercice, jusqu'à la date « au ».
    /// Colonnes renvoyées : rp/rr = recettes prévues/réalisées, dp/dr = dépenses prévues/réalisées.
    /// </summary>
    public static DataTable BudgetVsRealise(int idExercice, DateTime au, bool avecBrouillons)
    {
        return Db.Table(@"
SELECT ca.code, CONCAT(ca.code, ' — ', ca.libelle) AS centre,
       COALESCE(b.recettes_prevues, 0) AS rp,
       COALESCE((SELECT SUM(v.montant) FROM ventilation v JOIN mouvement m ON m.id_mouvement = v.id_mouvement
                  WHERE v.id_centre = ca.id_centre AND m.type_mouvement = 'RECETTE' AND m.id_exercice = @ex
                    AND m.date_mouvement <= @au AND FIND_IN_SET(m.statut, @st)), 0) AS rr,
       COALESCE(b.depenses_prevues, 0) AS dp,
       COALESCE((SELECT SUM(v.montant) FROM ventilation v JOIN mouvement m ON m.id_mouvement = v.id_mouvement
                  WHERE v.id_centre = ca.id_centre AND m.type_mouvement = 'DEPENSE' AND m.id_exercice = @ex
                    AND m.date_mouvement <= @au AND FIND_IN_SET(m.statut, @st)), 0) AS dr
  FROM centre_analytique ca
  LEFT JOIN budget_centre b ON b.id_centre = ca.id_centre AND b.id_exercice = @ex
 WHERE ca.actif = 1
 ORDER BY ca.code",
            ("@ex", idExercice), ("@au", au.Date), ("@st", avecBrouillons ? "VALIDE,BROUILLON" : "VALIDE"));
    }

    // ------------------------------------------------------------------ exercices / audit

    /// <summary>Clôture un exercice : plus aucune opération ne pourra y être ajoutée ou modifiée (contrôlé par les triggers).</summary>
    public static void CloturerExercice(int idExercice)
    {
        Db.VerifierDroit("CLOTURER");
        var brouillons = Convert.ToInt32(Db.Scalaire(
            "SELECT COUNT(*) FROM mouvement WHERE id_exercice = @e AND statut = 'BROUILLON'", ("@e", idExercice)));
        if (brouillons > 0)
            throw new InvalidOperationException($"Impossible de clôturer : {brouillons} brouillon(s) restent à valider ou à supprimer sur cet exercice.");
        Db.Executer("UPDATE exercice SET statut = 'CLOTURE', date_cloture = NOW() WHERE id_exercice = @e", ("@e", idExercice));
    }

    /// <summary>Journal d'audit complet, ou seulement mes propres actions (« Mon activité »).</summary>
    public static DataTable JournalAudit(bool seulementMoi = false) => Db.Table(@"
SELECT a.date_action AS `Date`, a.table_cible AS `Table`, a.id_enregistrement AS `N°`, a.action AS `Action`,
       CONCAT(u.prenoms, ' ', u.nom) AS `Utilisateur`, a.utilisateur_sql AS `Compte MySQL`, CAST(a.details AS CHAR) AS `Détails`
  FROM journal_audit a LEFT JOIN utilisateur u ON u.id_utilisateur = a.id_utilisateur
 WHERE (@moi IS NULL OR a.id_utilisateur = @moi)
 ORDER BY a.id_audit DESC LIMIT 1000", ("@moi", seulementMoi ? Session.IdUtilisateur : null));

    // Texte vide ou espaces → null (enregistré comme NULL dans la base)
    private static string? Vide(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}
