// =====================================================================================
//  Views/TableVue.cs — ÉDITEUR GÉNÉRIQUE DES TABLES DE PARAMÉTRAGE
// -------------------------------------------------------------------------------------
//  Un seul écran pour modifier six tables : compte, categorie, tiers, centre_analytique,
//  budget_centre et exercice (voir les Noeud(... new TableVue(...)) dans MainForm).
//
//  Comment ça marche (méthode Actualiser) :
//    1. lit la liste des colonnes de la table dans information_schema (catalogue MySQL)
//    2. crée une colonne de grille adaptée à chacune :
//         clé étrangère (ex. id_centre) → liste déroulante « code — libellé »
//         ENUM (ex. sens)               → liste déroulante des valeurs autorisées
//         booléen (ex. actif)           → case à cocher
//         autre                         → zone de texte
//    3. l'utilisateur modifie / ajoute (dernière ligne vide) / supprime des lignes,
//       puis « Enregistrer » envoie tout d'un coup à MySQL (MySqlCommandBuilder).
//
//  ➜ Pour changer le titre d'une colonne, ajoutez-la dans le dictionnaire NomsColonnes.
//  ➜ Ajouter une nouvelle table de paramétrage = une ligne Noeud(...) dans MainForm.
// =====================================================================================

using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using MySqlConnector;
using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Views;

/// <summary>
/// Éditeur générique d'une table de paramétrage (comptes, catégories, tiers, centres, budgets, exercices).
/// Les colonnes sont lues dans information_schema : listes déroulantes pour les ENUM et les clés étrangères,
/// cases à cocher pour les booléens. Les modifications sont enregistrées d'un coup avec « Enregistrer ».
/// </summary>
/// <remarks>Mise en page : TableVue.Designer.cs (les colonnes, elles, sont créées à l'exécution selon la table).</remarks>
public partial class TableVue : VueBase
{
    // Nom de colonne dans la base → titre affiché dans la grille (sinon le nom brut est affiché)
    private static readonly Dictionary<string, string> NomsColonnes = new()
    {
        ["code"] = "Code", ["libelle"] = "Libellé", ["sens"] = "Sens", ["actif"] = "Actif",
        ["description"] = "Description", ["id_centre_parent"] = "Centre parent", ["responsable"] = "Responsable",
        ["type_compte"] = "Type", ["etablissement"] = "Établissement", ["numero_compte"] = "N° de compte",
        ["solde_initial"] = "Solde initial", ["decouvert_autorise"] = "Découvert autorisé", ["date_ouverture"] = "Date d'ouverture",
        ["nom"] = "Nom", ["type_tiers"] = "Type", ["telephone"] = "Téléphone", ["email"] = "E-mail", ["adresse"] = "Adresse",
        ["id_centre"] = "Centre analytique", ["id_exercice"] = "Exercice", ["recettes_prevues"] = "Recettes prévues",
        ["depenses_prevues"] = "Dépenses prévues", ["date_debut"] = "Début", ["date_fin"] = "Fin", ["statut"] = "Statut",
        ["date_cloture"] = "Clôturé le"
    };

    // Nom de la table MySQL éditée et titre de l'écran
    private readonly string _table;
    private readonly string _titre;
    // L'adaptateur lit les données ET sait renvoyer les modifications à MySQL
    private MySqlDataAdapter? _adapter;
    // Copie locale des lignes : les modifications restent ici jusqu'à « Enregistrer »
    private DataTable? _donnees;
    // Colonne identifiant (ex. id_compte), cachée dans la grille
    private string _clePrimaire = "";

    public override string Titre => _titre;

    /// <summary>Constructeur utilisé par le concepteur de Visual Studio.</summary>
    public TableVue() : this("categorie", "Catégories de recettes et de dépenses") { }

    public TableVue(string table, string titre)
    {
        InitializeComponent();
        _table = table;
        _titre = titre;
        _entete.Text = titre;
        // Les colonnes sont créées par Actualiser(), pas automatiquement
        _grille.AutoGenerateColumns = false;
        Theme.StyleGrille(_grille, lectureSeule: false, ajusterColonnes: false);
        // On modifie une cellule en tapant directement dedans (ou avec F2)
        _grille.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
        _grille.SelectionMode = DataGridViewSelectionMode.CellSelect;
    }

    public override IEnumerable<ToolStripItem> Outils()
    {
        var outils = new List<ToolStripItem>
        {
            Theme.Outil("Enregistrer", Theme.IcoEnregistrer, Theme.Accent, (_, _) => Enregistrer()),
            Theme.Outil("Supprimer la ligne", Theme.IcoSupprimer, Theme.Rouge, (_, _) => SupprimerLigne()),
            Theme.Outil("Annuler les modifications", Theme.IcoAnnuler, Theme.Gris, (_, _) => Executer(Actualiser, false))
        };
        // Bouton supplémentaire seulement pour la table des exercices
        if (_table == "exercice" && Session.Peut("CLOTURER"))
        {
            outils.Add(Separateur());
            outils.Add(Theme.Outil("Clôturer l'exercice", Theme.IcoCadenas, Theme.Orange, (_, _) => Cloturer()));
        }
        outils.Add(Separateur());
        outils.Add(Theme.Outil("Exporter (Excel)", Theme.IcoExporter, Theme.Vert, (_, _) => Ui.ExporterCsv(_grille, _table)));
        return outils;
    }

    public override void Actualiser()
    {
        // 1) Description des colonnes de la table (nom, type, valeur par défaut…)
        var colonnes = Db.Table(@"
SELECT COLUMN_NAME, COLUMN_TYPE, DATA_TYPE, COLUMN_DEFAULT, IS_NULLABLE, EXTRA
  FROM information_schema.COLUMNS
 WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @t
 ORDER BY ORDINAL_POSITION", ("@t", _table));
        // 2) Clés étrangères : quelle colonne pointe vers quelle table
        var cles = Db.Table(@"
SELECT COLUMN_NAME, REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME
  FROM information_schema.KEY_COLUMN_USAGE
 WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @t AND REFERENCED_TABLE_NAME IS NOT NULL", ("@t", _table));
        var etrangeres = cles.Rows.Cast<DataRow>().ToDictionary(
            r => r["COLUMN_NAME"].ToString()!, r => (Table: r["REFERENCED_TABLE_NAME"].ToString()!, Colonne: r["REFERENCED_COLUMN_NAME"].ToString()!));

        // Les colonnes remplies automatiquement par MySQL (CURRENT_TIMESTAMP) ne sont pas chargées.
        var retenues = colonnes.Rows.Cast<DataRow>()
            .Where(r => !string.Equals(r["COLUMN_DEFAULT"]?.ToString(), "CURRENT_TIMESTAMP", StringComparison.OrdinalIgnoreCase))
            .ToList();
        // Clé primaire = colonne auto_increment (ou, à défaut, la première colonne id_…)
        _clePrimaire = retenues.First(r => r["EXTRA"].ToString()!.Contains("auto_increment")
                                           || r["COLUMN_NAME"].ToString()!.StartsWith("id_"))["COLUMN_NAME"].ToString()!;

        // 3) Lecture des lignes ; la connexion est ensuite détachée (elle sera rouverte pour enregistrer)
        var select = $"SELECT {string.Join(", ", retenues.Select(r => $"`{r["COLUMN_NAME"]}`"))} FROM `{_table}` ORDER BY `{_clePrimaire}`";
        using (var cn = Db.Ouvrir())
        {
            _adapter = new MySqlDataAdapter(select, cn);
            _donnees = new DataTable(_table);
            _adapter.Fill(_donnees);
            _adapter.SelectCommand!.Connection = null;
        }

        // 4) Création d'une colonne de grille par colonne de la table
        _grille.DataSource = null;
        _grille.Columns.Clear();
        foreach (var r in retenues)
        {
            string nom = r["COLUMN_NAME"].ToString()!;
            string typeSql = r["COLUMN_TYPE"].ToString()!;
            string dataType = r["DATA_TYPE"].ToString()!;
            var colonneDonnees = _donnees.Columns[nom]!;
            AppliquerDefaut(colonneDonnees, r["COLUMN_DEFAULT"] as string);

            // Type de colonne de grille selon le type SQL
            DataGridViewColumn col;
            if (etrangeres.TryGetValue(nom, out var fk))
                col = ColonneListe(ChargerReference(fk.Table, fk.Colonne, r["IS_NULLABLE"].ToString() == "YES"));
            else if (dataType == "enum")
                col = ColonneEnum(typeSql);
            else if (colonneDonnees.DataType == typeof(bool))
                col = new DataGridViewCheckBoxColumn { FillWeight = 40 };
            else
                col = new DataGridViewTextBoxColumn();

            // Liaison avec la colonne de données, titre, formats d'affichage
            col.DataPropertyName = nom;
            col.Name = nom;
            col.HeaderText = NomsColonnes.TryGetValue(nom, out var joli) ? joli : nom;
            col.Visible = nom != _clePrimaire;
            if (col is DataGridViewTextBoxColumn && dataType is "decimal" or "int" or "smallint")
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                if (dataType == "decimal") col.DefaultCellStyle.Format = "N0";
            }
            if (dataType == "date") col.DefaultCellStyle.Format = "dd/MM/yyyy";
            if (dataType == "datetime") col.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            // Le statut d'un exercice ne change que par le bouton « Clôturer l'exercice »
            if (_table == "exercice" && nom is "statut" or "date_cloture") col.ReadOnly = true;
            _grille.Columns.Add(col);
        }
        _grille.DataSource = _donnees;
    }

    // Recopie la valeur par défaut MySQL : une nouvelle ligne est ainsi pré-remplie (ex. actif = coché)
    private static void AppliquerDefaut(DataColumn c, string? defaut)
    {
        if (defaut == null) return;
        try
        {
            if (c.DataType == typeof(bool)) c.DefaultValue = defaut == "1";
            else c.DefaultValue = Convert.ChangeType(defaut, c.DataType, CultureInfo.InvariantCulture);
        }
        catch
        {
            // valeur par défaut non convertible : MySQL la gérera
        }
    }

    // Liste déroulante des valeurs d'un ENUM : enum('CAISSE','BANQUE') → CAISSE, BANQUE
    private static DataGridViewComboBoxColumn ColonneEnum(string typeSql)
    {
        var col = new DataGridViewComboBoxColumn { FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox };
        foreach (Match m in Regex.Matches(typeSql, "'((?:[^']|'')*)'"))
            col.Items.Add(m.Groups[1].Value.Replace("''", "'"));
        return col;
    }

    // Liste déroulante alimentée par une table (affiche « lib », enregistre « id »)
    private static DataGridViewComboBoxColumn ColonneListe(DataTable source) => new()
    {
        DataSource = source,
        DisplayMember = "lib",
        ValueMember = "id",
        FlatStyle = FlatStyle.Flat,
        DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
    };

    /// <summary>Liste de choix pour une clé étrangère : « code — libellé », « libellé » ou « nom ».</summary>
    private static DataTable ChargerReference(string table, string colonneId, bool nullable)
    {
        var cols = Db.Table("SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @t", ("@t", table))
            .Rows.Cast<DataRow>().Select(r => r[0].ToString()!).ToHashSet();
        string affichage = cols.Contains("code") && cols.Contains("libelle") ? "CONCAT(code, ' — ', libelle)"
            : cols.Contains("libelle") ? "libelle"
            : cols.Contains("nom") ? "nom"
            : $"CAST(`{colonneId}` AS CHAR)";
        var dt = Db.Table($"SELECT `{colonneId}` AS id, {affichage} AS lib FROM `{table}` ORDER BY lib");
        if (nullable)
        {
            var vide = dt.NewRow();
            vide["id"] = DBNull.Value;
            vide["lib"] = "(aucun)";
            dt.Rows.InsertAt(vide, 0);
        }
        return dt;
    }

    // Bouton « Enregistrer » : envoie à MySQL tous les ajouts, modifications et suppressions
    private void Enregistrer()
    {
        if (_adapter == null || _donnees == null) return;
        // Valide la cellule en cours de saisie
        _grille.EndEdit();
        if (BindingContext?[_donnees] is CurrencyManager cm) cm.EndCurrentEdit();
        if (_donnees.GetChanges() == null)
        {
            Ui.Info("Aucune modification à enregistrer.");
            return;
        }
        Executer(() =>
        {
            Db.VerifierDroit("PARAMETRER");
            using var cn = Db.Ouvrir();
            _adapter.SelectCommand!.Connection = cn;
            // Le CommandBuilder fabrique tout seul les requêtes INSERT / UPDATE / DELETE
            using var builder = new MySqlCommandBuilder(_adapter) { ConflictOption = ConflictOption.OverwriteChanges };
            _adapter.InsertCommand = builder.GetInsertCommand();
            _adapter.UpdateCommand = builder.GetUpdateCommand();
            _adapter.DeleteCommand = builder.GetDeleteCommand();
            int n = _adapter.Update(_donnees);
            _adapter.SelectCommand.Connection = null;
            Principale?.RafraichirStatut();
            Principale?.Notifier("Modifications enregistrées", $"{n} ligne(s) mise(s) à jour dans « {_titre} ».");
        });
    }

    // Bouton « Supprimer la ligne » : retire la ligne de la grille (effacée dans MySQL au prochain Enregistrer)
    private void SupprimerLigne()
    {
        var ligne = _grille.CurrentRow;
        if (ligne == null || ligne.IsNewRow) { Ui.Info("Sélectionnez une ligne."); return; }
        if (!Ui.Confirmer("Supprimer cette ligne ?\nLa suppression sera faite dans la base au prochain « Enregistrer ».")) return;
        _grille.Rows.Remove(ligne);
    }

    // Bouton « Clôturer l'exercice » (table exercice uniquement, droit CLOTURER)
    private void Cloturer()
    {
        var ligne = _grille.CurrentRow;
        if (ligne == null || ligne.IsNewRow || ligne.Cells[_clePrimaire].Value is null or DBNull)
        {
            Ui.Info("Sélectionnez un exercice enregistré.");
            return;
        }
        int id = Convert.ToInt32(ligne.Cells[_clePrimaire].Value);
        if (!Ui.Confirmer($"Clôturer l'exercice « {ligne.Cells["libelle"].Value} » ?\n\nPlus aucune opération ne pourra être saisie, modifiée ou annulée sur cette période.")) return;
        bool ok = false;
        Executer(() => { Depot.CloturerExercice(id); ok = true; });
        if (ok) Principale?.Notifier("Exercice clôturé", $"L'exercice « {ligne.Cells["libelle"].Value} » est maintenant fermé.");
    }
}
