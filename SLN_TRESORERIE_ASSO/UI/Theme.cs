// =====================================================================================
//  UI/Theme.cs — CHARTE GRAPHIQUE (couleurs, polices, icônes, style des grilles)
// -------------------------------------------------------------------------------------
//  Toutes les couleurs et polices de l'application sont définies ICI, une seule fois.
//  ➜ Pour changer le bleu de toute l'application, modifiez « Accent » ci-dessous.
//    Color.FromArgb(rouge, vert, bleu) : chaque valeur va de 0 à 255.
//
//  Contenu :
//    - couleurs (Accent, Nuit, Or, Vert, Rouge…) et polices (Police, PoliceGras…)
//    - icônes : codes de caractères de la police Windows « Segoe MDL2 Assets »
//      (liste complète : chercher « Segoe MDL2 Assets icons » sur learn.microsoft.com)
//    - Px() : adapte une taille à l'agrandissement de l'écran (100 %, 125 %, 150 %…)
//    - boutons et boutons de barre d'outils (Outil)
//    - StyleGrille / AjusterColonnes / ColorerOperations : apparence des DataGridView
//    - Surligner : effet de ligne colorée qui s'estompe après une validation
// =====================================================================================

using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace SLN_TRESORERIE_ASSO.UI;

/// <summary>Couleurs, polices et mise en forme communes à toute l'application.</summary>
public static class Theme
{
    // Couleurs principales
    // Accent = bleu des boutons et liens ; Nuit = bleu foncé des titres ; Or = touche « trésorerie »
    public static readonly Color Accent = Color.FromArgb(0, 103, 192);
    public static readonly Color AccentFonce = Color.FromArgb(0, 74, 140);
    public static readonly Color Nuit = Color.FromArgb(11, 61, 102);
    public static readonly Color NuitFonce = Color.FromArgb(6, 36, 64);
    public static readonly Color Or = Color.FromArgb(232, 179, 61);
    public static readonly Color OrFonce = Color.FromArgb(176, 128, 32);

    // Fonds et bordures
    public static readonly Color Fond = Color.FromArgb(240, 240, 240);
    public static readonly Color FondBarre = Color.FromArgb(247, 247, 247);
    public static readonly Color Bordure = Color.FromArgb(218, 218, 218);
    public static readonly Color Selection = Color.FromArgb(204, 232, 255);
    public static readonly Color EnTeteGrille = Color.FromArgb(243, 243, 243);
    public static readonly Color LigneAlternee = Color.FromArgb(250, 250, 250);
    public static readonly Color TexteSecondaire = Color.FromArgb(74, 74, 74);

    // Couleurs de sens : vert = recette / validé, rouge = dépense / erreur,
    // orange = brouillon / attention, gris = annulé. Les versions « Clair » servent de fond.
    public static readonly Color Vert = Color.FromArgb(26, 107, 58);
    public static readonly Color VertClair = Color.FromArgb(227, 242, 231);
    public static readonly Color Rouge = Color.FromArgb(180, 35, 24);
    public static readonly Color RougeClair = Color.FromArgb(253, 231, 233);
    public static readonly Color Orange = Color.FromArgb(138, 75, 0);
    public static readonly Color OrangeClair = Color.FromArgb(255, 240, 217);
    public static readonly Color Gris = Color.FromArgb(90, 90, 90);
    public static readonly Color GrisClair = Color.FromArgb(237, 237, 237);
    public static readonly Color BleuClair = Color.FromArgb(229, 241, 251);

    // Polices (taille en points). Police = police par défaut de toute l'application (voir Program.cs).
    public static readonly Font Police = new("Segoe UI", 9f);
    public static readonly Font PoliceGras = new("Segoe UI", 9f, FontStyle.Bold);
    public static readonly Font PoliceTitre = new("Segoe UI Semibold", 15f);
    public static readonly Font PoliceSousTitre = new("Segoe UI Semibold", 11f);
    public static readonly Font PoliceMono = new("Consolas", 8.5f);

    // Icônes de la police Windows « Segoe MDL2 Assets »
    // Chaque constante contient UN caractère spécial (invisible ici, c'est normal) qui s'affiche
    // comme une icône. Utilisation : Theme.Glyphe(Theme.IcoAjouter, Theme.Vert) → image 20×20.
    public const string IcoAjouter = "";
    public const string IcoValider = "";
    public const string IcoAnnuler = "";
    public const string IcoSupprimer = "";
    public const string IcoModifier = "";
    public const string IcoActualiser = "";
    public const string IcoVirement = "";
    public const string IcoExporter = "";
    public const string IcoEnregistrer = "";
    public const string IcoAccueil = "";
    public const string IcoListe = "";
    public const string IcoCompte = "";
    public const string IcoEtiquette = "";
    public const string IcoPersonnes = "";
    public const string IcoPersonne = "";
    public const string IcoCentre = "";
    public const string IcoCalendrier = "";
    public const string IcoBouclier = "";
    public const string IcoGraphique = "";
    public const string IcoParametres = "";
    public const string IcoCadenas = "";
    public const string IcoDeverrouiller = "";
    public const string IcoRecette = "";
    public const string IcoDepense = "";
    public const string IcoTrombone = "";
    public const string IcoDossier = "";
    public const string IcoImprimer = "";   // imprimante (écrit avec son code pour rester lisible)

    // Les icônes déjà dessinées sont gardées en mémoire pour ne pas les redessiner à chaque fois
    private static readonly Dictionary<(string, int, int), Bitmap> CacheGlyphes = new();

    // Calculé une seule fois au premier appel de Echelle
    private static float? _echelle;

    /// <summary>Facteur d'agrandissement de l'écran (1 à 100 %, 1,5 à 150 %…).</summary>
    public static float Echelle
    {
        get
        {
            if (_echelle == null)
            {
                try
                {
                    using var g = Graphics.FromHwnd(IntPtr.Zero);
                    _echelle = g.DpiX / 96f;
                }
                catch (Exception)
                {
                    _echelle = 1f;   // par sécurité (concepteur de Visual Studio, session sans écran…)
                }
            }
            return _echelle.Value;
        }
    }

    /// <summary>Convertit une taille prévue à 100 % en pixels réels de l'écran.</summary>
    public static int Px(int valeur) => (int)Math.Round(valeur * Echelle);

    /// <summary>Dessine une icône de la police Segoe MDL2 Assets (taille donnée à 100 %, adaptée à l'écran).</summary>
    public static Bitmap Glyphe(string glyphe, Color couleur, int tailleLogique = 20)
    {
        var cle = (glyphe, couleur.ToArgb(), tailleLogique);
        if (CacheGlyphes.TryGetValue(cle, out var deja)) return deja;

        int taille = Px(tailleLogique);
        var bmp = new Bitmap(taille, taille);
        using (var g = Graphics.FromImage(bmp))
        using (var police = new Font("Segoe MDL2 Assets", taille * 0.62f, GraphicsUnit.Pixel))
        using (var pinceau = new SolidBrush(couleur))
        {
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(glyphe, police, pinceau, new RectangleF(0, 0, taille, taille), format);
        }
        CacheGlyphes[cle] = bmp;
        return bmp;
    }

    /// <summary>Formate un montant sans décimales avec séparateur de milliers : 1250000 → « 1 250 000 ».</summary>
    public static string Montant(decimal d) => d.ToString("N0");

    // ------------------------------------------------------------------ boutons

    /// <summary>Crée un bouton bleu (action principale : Enregistrer, Valider…).</summary>
    public static Button BoutonPrincipal(string texte, int largeur = 150) => new()
    {
        Text = texte,
        Width = largeur,
        Height = 32,
        FlatStyle = FlatStyle.Flat,
        BackColor = Accent,
        ForeColor = Color.White,
        Font = PoliceGras,
        Cursor = Cursors.Hand,
        FlatAppearance = { BorderColor = Accent, MouseOverBackColor = AccentFonce, MouseDownBackColor = Nuit }
    };

    /// <summary>Crée un bouton blanc à bordure grise (action secondaire : Annuler, Fermer…).</summary>
    public static Button BoutonSecondaire(string texte, int largeur = 120) => new()
    {
        Text = texte,
        Width = largeur,
        Height = 32,
        FlatStyle = FlatStyle.Flat,
        BackColor = Color.FromArgb(253, 253, 253),
        ForeColor = Color.FromArgb(27, 27, 27),
        Cursor = Cursors.Hand,
        FlatAppearance = { BorderColor = Color.FromArgb(168, 168, 168), MouseOverBackColor = Color.FromArgb(232, 240, 250) }
    };

    /// <summary>Donne à un bouton existant (créé dans le concepteur) le style du bouton principal bleu.</summary>
    public static void StylePrincipal(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.BackColor = Accent;
        b.ForeColor = Color.White;
        b.Font = PoliceGras;
        b.FlatAppearance.BorderColor = Accent;
        b.FlatAppearance.MouseOverBackColor = AccentFonce;
        b.FlatAppearance.MouseDownBackColor = Nuit;
    }

    /// <summary>
    /// Crée un bouton de la barre d'outils du haut (icône + texte). Les vues renvoient leurs boutons
    /// dans leur méthode Outils() ; MainForm les place dans la barre quand la vue s'affiche.
    /// Exemple : Theme.Outil("Actualiser", Theme.IcoActualiser, Theme.Gris, (_, _) => Actualiser())
    /// </summary>
    public static ToolStripButton Outil(string texte, string glyphe, Color couleur, EventHandler clic)
    {
        var b = new ToolStripButton(texte, Glyphe(glyphe, couleur), clic)
        {
            DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
            ImageScaling = ToolStripItemImageScaling.None,
            Padding = new Padding(4, 2, 4, 2)
        };
        return b;
    }

    // ------------------------------------------------------------------ grilles

    /// <summary>
    /// Applique le style commun à une grille : en-têtes gris, lignes alternées, sélection de ligne entière…
    /// lectureSeule = false pour une grille modifiable (ajout/suppression de lignes).
    /// ajusterColonnes = cache les colonnes id_… et formate montants/dates à chaque chargement.
    /// Appelée dans le constructeur de chaque vue, juste après InitializeComponent().
    /// </summary>
    public static void StyleGrille(DataGridView g, bool lectureSeule = true, bool ajusterColonnes = true)
    {
        g.BorderStyle = BorderStyle.FixedSingle;
        g.BackgroundColor = Color.White;
        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersDefaultCellStyle.BackColor = EnTeteGrille;
        g.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(27, 27, 27);
        g.ColumnHeadersDefaultCellStyle.Font = PoliceGras;
        g.ColumnHeadersDefaultCellStyle.SelectionBackColor = EnTeteGrille;
        g.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = 30;
        g.RowHeadersVisible = false;
        g.DefaultCellStyle.Font = Police;
        g.DefaultCellStyle.SelectionBackColor = Selection;
        g.DefaultCellStyle.SelectionForeColor = Color.Black;
        g.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
        g.AlternatingRowsDefaultCellStyle.BackColor = LigneAlternee;
        g.RowTemplate.Height = 27;
        g.GridColor = Color.FromArgb(230, 230, 230);
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.MultiSelect = false;
        g.AllowUserToResizeRows = false;
        g.AllowUserToAddRows = !lectureSeule;
        g.AllowUserToDeleteRows = !lectureSeule;
        g.ReadOnly = lectureSeule;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // Anti-scintillement
        typeof(DataGridView).GetProperty("DoubleBuffered",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(g, true);

        // DataBindingComplete = événement déclenché chaque fois que la grille reçoit de nouvelles données
        if (ajusterColonnes) g.DataBindingComplete += (_, _) => AjusterColonnes(g);
        // Valeur invalide tapée dans une cellule (ex. lettres dans un montant) : message au lieu d'un plantage
        g.DataError += (_, e) =>
        {
            e.ThrowException = false;
            MessageBox.Show("Valeur incorrecte : " + e.Exception?.Message, "Saisie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        };
    }

    /// <summary>Masque les colonnes techniques et formate montants et dates.</summary>
    public static void AjusterColonnes(DataGridView g)
    {
        foreach (DataGridViewColumn c in g.Columns)
        {
            var nom = c.DataPropertyName.Length > 0 ? c.DataPropertyName : c.Name;
            // Colonnes techniques cachées : id_…, type_mouvement, report
            if (nom.StartsWith("id_") || nom is "type_mouvement" or "report")
            {
                c.Visible = false;
                continue;
            }
            // Mise en forme selon le type de la colonne. FillWeight = largeur relative
            // (une colonne à 70 est un peu plus étroite qu'une colonne à 100).
            var type = c.ValueType;
            if (type == typeof(decimal) || type == typeof(double))
            {
                // Montants : sans décimales, alignés à droite
                c.DefaultCellStyle.Format = "N0";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.FillWeight = 70;
            }
            else if (type == typeof(DateTime))
            {
                // Dates : avec l'heure pour « Dernière connexion » et pour les grilles marquées Tag = "datetime"
                bool avecHeure = nom.Contains("connexion", StringComparison.OrdinalIgnoreCase) || nom == "Date" && g.Tag as string == "datetime";
                c.DefaultCellStyle.Format = avecHeure ? "dd/MM/yyyy HH:mm" : "dd/MM/yyyy";
                c.FillWeight = 55;
                c.MinimumWidth = Px(avecHeure ? 130 : 92);
            }
            else if (type == typeof(bool))
            {
                c.FillWeight = 35;
            }
            // Réglages particuliers selon le titre de la colonne
            if (type == typeof(decimal)) c.MinimumWidth = Px(95);
            if (nom == "N° pièce")
            {
                c.DefaultCellStyle.Font = PoliceMono;
                c.FillWeight = 75;
                c.MinimumWidth = Px(125);
            }
            if (nom == "Saisi par") c.MinimumWidth = Px(115);
            if (nom == "Statut")
            {
                c.FillWeight = 55;
                c.MinimumWidth = Px(100);
            }
        }
    }

    /// <summary>
    /// Ajoute les couleurs : statut (pastille), montant vert/rouge selon le sens.
    /// La grille doit contenir une colonne « Statut » et, pour le sens, « type_mouvement ».
    /// </summary>
    public static void ColorerOperations(DataGridView g)
    {
        // CellFormatting est appelé pour chaque cellule au moment de l'afficher :
        // on peut y changer sa couleur, sa police ou le texte affiché (sans toucher aux données).
        g.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var col = g.Columns[e.ColumnIndex];
            var row = g.Rows[e.RowIndex];
            string statut = g.Columns.Contains("Statut") ? row.Cells["Statut"].Value?.ToString() ?? "" : "";

            if (col.Name == "Statut")
            {
                // Pastille de statut : couleur de fond + couleur de texte
                var (fond, texte) = statut switch
                {
                    "VALIDE" => (VertClair, Vert),
                    "BROUILLON" => (OrangeClair, Orange),
                    "ANNULE" => (GrisClair, Gris),
                    _ => (Color.Empty, Color.Empty)
                };
                if (fond != Color.Empty)
                {
                    e.CellStyle!.BackColor = fond;
                    e.CellStyle.ForeColor = texte;
                    e.CellStyle.SelectionBackColor = fond;
                    e.CellStyle.SelectionForeColor = texte;
                    e.CellStyle.Font = PoliceGras;
                }
            }
            else if (col.Name == "Montant" && e.Value is decimal mt)
            {
                // Montant : vert avec « + » pour une recette, rouge avec « − » pour une dépense, gris si annulé
                bool recette = !g.Columns.Contains("type_mouvement") || row.Cells["type_mouvement"].Value?.ToString() == "RECETTE";
                if (statut == "ANNULE")
                {
                    e.CellStyle!.ForeColor = Gris;
                }
                else if (g.Columns.Contains("type_mouvement"))
                {
                    e.CellStyle!.ForeColor = recette ? Vert : Rouge;
                    e.CellStyle.SelectionForeColor = recette ? Vert : Rouge;
                }
                if (g.Columns.Contains("type_mouvement"))
                {
                    e.Value = (recette ? "+ " : "− ") + Montant(mt);
                    e.FormattingApplied = true;
                }
            }
        };
    }

    /// <summary>
    /// Met en évidence une ligne (par son identifiant) avec une couleur qui s'estompe doucement :
    /// utilisé après une validation ou une annulation.
    /// </summary>
    public static void Surligner(DataGridView g, string colonneId, int id, Color couleur)
    {
        // Recherche de la ligne dont la colonne colonneId vaut id
        if (!g.Columns.Contains(colonneId)) return;
        var ligne = g.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Cells[colonneId].Value is not null and not DBNull
                                                                        && Convert.ToInt32(r.Cells[colonneId].Value) == id);
        if (ligne == null) return;
        g.ClearSelection();
        g.FirstDisplayedScrollingRowIndex = Math.Max(0, ligne.Index - 2);

        // Animation : 45 étapes de 30 ms (≈ 1,4 s). Pour un effet plus long, augmentez « etapes ».
        int etape = 0;
        const int etapes = 45;
        var timer = new System.Windows.Forms.Timer { Interval = 30 };
        timer.Tick += (_, _) =>
        {
            etape++;
            if (g.IsDisposed || ligne.DataGridView == null || etape > etapes)
            {
                timer.Stop();
                timer.Dispose();
                if (ligne.DataGridView != null) ligne.DefaultCellStyle.BackColor = Color.Empty;
                return;
            }
            float t = etape < 12 ? 0 : (etape - 12) / (float)(etapes - 12);   // reste vif un instant puis s'estompe
            // Mélange progressif entre la couleur de départ et la couleur normale de la ligne
            Color fin = ligne.Index % 2 == 1 ? LigneAlternee : Color.White;
            ligne.DefaultCellStyle.BackColor = Color.FromArgb(
                (int)(couleur.R + (fin.R - couleur.R) * t),
                (int)(couleur.G + (fin.G - couleur.G) * t),
                (int)(couleur.B + (fin.B - couleur.B) * t));
        };
        timer.Start();
    }

    // ------------------------------------------------------------------ dessin

    /// <summary>Forme de rectangle à coins arrondis (utilisée pour dessiner les cartes, pastilles, barres).</summary>
    public static GraphicsPath Arrondi(RectangleF r, float rayon)
    {
        var p = new GraphicsPath();
        float d = rayon * 2;
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }
}
