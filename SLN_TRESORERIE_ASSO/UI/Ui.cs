// =====================================================================================
//  UI/Ui.cs — PETITS OUTILS D'INTERFACE
// -------------------------------------------------------------------------------------
//    Erreur(ex)          affiche une erreur en français clair (traduit les codes MySQL)
//    Info / Confirmer    boîte de message simple / question Oui-Non
//    Liste(...)          crée une ComboBox remplie avec un DataTable (colonnes id / lib)
//    IdSelectionne(...)  renvoie l'id choisi dans une ComboBox (ou null)
//    ExporterCsv(...)    exporte une grille vers un fichier .csv ouvrable dans Excel
//
//  Pour ajouter la traduction d'une autre erreur MySQL : ajoutez une ligne dans le
//  « switch » de Erreur avec son numéro (liste des numéros : documentation MySQL
//  « Server Error Message Reference »).
// =====================================================================================

using System.Text;
using MySqlConnector;

namespace SLN_TRESORERIE_ASSO.UI;

/// <summary>Petits utilitaires d'interface : messages, champs de formulaire, export CSV.</summary>
public static class Ui
{
    /// <summary>
    /// Affiche une erreur à l'utilisateur. Les erreurs levées par les triggers (SIGNAL '45000')
    /// contiennent déjà un message en français écrit dans le script SQL : il est affiché tel quel.
    /// </summary>
    public static void Erreur(Exception ex, string titre = "Opération refusée")
    {
        string message = ex switch
        {
            // Serveur arrêté ou injoignable
            MySqlException { ErrorCode: MySqlErrorCode.UnableToConnectToHost } =>
                "Le serveur MySQL ne répond pas. Vérifiez que WampServer est démarré.",
            // 1045 : mauvais utilisateur / mot de passe MySQL
            MySqlException { Number: 1045 } =>
                "Accès refusé par MySQL : utilisateur ou mot de passe incorrect dans Data\\Connexion.cs.",
            // 1451 : clé étrangère (la ligne est utilisée par une autre table)
            MySqlException { Number: 1451 } =>
                "Suppression impossible : cet élément est utilisé ailleurs (mouvements, budgets...).",
            // 1062 : doublon sur une colonne UNIQUE
            MySqlException { Number: 1062 } =>
                "Cette valeur existe déjà (code, identifiant ou e-mail en double).\n\n" + ex.Message,
            // Tous les autres cas : message d'origine
            _ => ex.Message
        };
        MessageBox.Show(message, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>Message d'information avec un bouton OK.</summary>
    public static void Info(string message, string titre = "Trésorerie Asso") =>
        MessageBox.Show(message, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);

    /// <summary>Pose une question Oui / Non ; renvoie vrai si l'utilisateur répond Oui.</summary>
    public static bool Confirmer(string message, string titre = "Confirmation") =>
        MessageBox.Show(message, titre, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

    /// <summary>Un libellé au-dessus d'un contrôle, prêt à mettre dans une grille de formulaire.</summary>
    public static Control Champ(string libelle, Control controle, int largeur = 0)
    {
        // Petit panneau vertical : le libellé en haut, le contrôle en dessous
        var panneau = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            Margin = new Padding(0, 0, 14, 8),
            Padding = Padding.Empty
        };
        panneau.Controls.Add(new Label { Text = libelle, AutoSize = true, Margin = new Padding(0, 0, 0, 3) });
        controle.Margin = Padding.Empty;
        if (largeur > 0) controle.Width = largeur;
        panneau.Controls.Add(controle);
        return panneau;
    }

    /// <summary>
    /// Crée une liste déroulante non modifiable au clavier. Si une source est donnée
    /// (DataTable de Depot.Comptes, Depot.Categories…), elle affiche « lib » et renvoie « id ».
    /// </summary>
    public static ComboBox Liste(object? source = null, int largeur = 200)
    {
        var c = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = largeur };
        if (source != null)
        {
            c.DisplayMember = "lib";   // texte affiché
            c.ValueMember = "id";      // valeur renvoyée par SelectedValue
            c.DataSource = source;
        }
        return c;
    }

    /// <summary>Liste déroulante de textes fixes ; le premier est sélectionné.</summary>
    public static ComboBox ListeTextes(int largeur, params string[] valeurs)
    {
        var c = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = largeur };
        c.Items.AddRange(valeurs);
        if (valeurs.Length > 0) c.SelectedIndex = 0;
        return c;
    }

    /// <summary>Id de l'élément choisi dans la ComboBox, ou null (rien choisi, ou ligne « Tous » / « (aucun) »).</summary>
    public static int? IdSelectionne(ComboBox c) =>
        c.SelectedValue is null or DBNull ? null : Convert.ToInt32(c.SelectedValue);

    /// <summary>Bandeau d'information coloré (info, alerte, erreur).</summary>
    public static Label Bandeau(string texte, Color fond, Color bordure, Color couleurTexte) => new()
    {
        Text = texte,
        AutoSize = false,
        Height = 34,
        Dock = DockStyle.Top,
        BackColor = fond,
        ForeColor = couleurTexte,
        Padding = new Padding(10, 0, 10, 0),
        TextAlign = ContentAlignment.MiddleLeft,
        BorderStyle = BorderStyle.FixedSingle
    };

    /// <summary>Exporte les colonnes visibles d'une grille en CSV (ouvrable dans Excel).</summary>
    public static void ExporterCsv(DataGridView g, string nomParDefaut)
    {
        // Fenêtre « Enregistrer sous » ; nom proposé : nom_AAAAMMJJ.csv
        using var dlg = new SaveFileDialog
        {
            Filter = "Fichier CSV (Excel)|*.csv",
            FileName = $"{nomParDefaut}_{DateTime.Now:yyyyMMdd}.csv"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        // Seulement les colonnes visibles, dans l'ordre affiché ; séparateur « ; » (Excel en français)
        var colonnes = g.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(";", colonnes.Select(c => Csv(c.HeaderText))));
        foreach (DataGridViewRow r in g.Rows)
        {
            if (r.IsNewRow) continue;
            // FormattedValue = valeur telle qu'affichée (montants avec espaces, dates jj/mm/aaaa)
            sb.AppendLine(string.Join(";", colonnes.Select(c => Csv(r.Cells[c.Index].FormattedValue?.ToString() ?? ""))));
        }
        // UTF-8 avec BOM : Excel affiche ainsi correctement les accents
        File.WriteAllText(dlg.FileName, sb.ToString(), new UTF8Encoding(true));
        Info("Export terminé :\n" + dlg.FileName);
    }

    // Met une valeur entre guillemets si elle contient « ; », un guillemet ou un retour à la ligne
    private static string Csv(string s) =>
        s.Contains(';') || s.Contains('"') || s.Contains('\n') ? "\"" + s.Replace("\"", "\"\"") + "\"" : s;
}
