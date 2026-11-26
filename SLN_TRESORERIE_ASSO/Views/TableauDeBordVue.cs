// =====================================================================================
//  Views/TableauDeBordVue.cs — TABLEAU DE BORD (écran d'accueil après la connexion)
// -------------------------------------------------------------------------------------
//  De haut en bas (contrôles de TableauDeBordVue.Designer.cs) :
//    1. _bandeau      « Bienvenue, Rôle Prénom Nom » (UI/Graphiques.cs → BandeauBienvenue)
//    2. _cartes       une carte par compte actif + la trésorerie totale
//    3. _milieu       trois colonnes :
//         _cadreTaches     « Mes tâches » (_taches) : liste qui dépend du RÔLE + raccourcis
//         _cadreGraphique  histogramme des 6 derniers mois (_graphique)
//         _indicateurs     trois tuiles : recettes du mois, dépenses du mois, mes saisies
//    4. _grille       dernières opérations (double-clic pour ouvrir)
//
//  Les cartes, tâches et tuiles sont créées ici en code (méthodes Carte, LigneTache,
//  Tuile) car leur nombre dépend des données ; on ne les voit donc pas dans le concepteur.
//  ➜ Pour ajouter une tâche : une ligne « Tache(...) » dans RemplirTaches, avec le
//    compteur correspondant ajouté dans Depot.Taches().
// =====================================================================================

using System.Data;
using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Views;

/// <summary>
/// Accueil personnalisé : bienvenue (rôle, prénom, nom), mes tâches selon mon rôle,
/// soldes des comptes, graphique des 6 derniers mois et dernières opérations.
/// </summary>
/// <remarks>
/// Mise en page : TableauDeBordVue.Designer.cs. Les cartes, les tâches et les indicateurs
/// sont créés à l'exécution, car ils dépendent des comptes et du rôle de l'utilisateur.
/// </remarks>
public partial class TableauDeBordVue : VueBase
{
    public override string Titre => "Tableau de bord";

    public TableauDeBordVue()
    {
        InitializeComponent();
        Theme.StyleGrille(_grille);
        Theme.ColorerOperations(_grille);
        // Double-clic sur une dernière opération → ouverture dans la fenêtre de saisie
        _grille.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0) Principale?.OuvrirMouvement(Convert.ToInt32(_grille.Rows[e.RowIndex].Cells["id_mouvement"].Value));
        };
        // Fine bordure grise autour des cadres « Mes tâches » et « Graphique »
        foreach (var cadre in new[] { _cadreTaches, _cadreGraphique })
        {
            var c = cadre;
            c.Paint += (_, e) =>
            {
                using var p = new Pen(Theme.Bordure);
                e.Graphics.DrawRectangle(p, 0, 0, c.Width - 1, c.Height - 1);
            };
        }
    }

    public override IEnumerable<ToolStripItem> Outils()
    {
        var outils = new List<ToolStripItem>();
        if (Session.Peut("SAISIR"))
        {
            outils.Add(Theme.Outil("Nouvelle recette", Theme.IcoRecette, Theme.Vert, (_, _) => Principale?.NouvelleOperation("RECETTE")));
            outils.Add(Theme.Outil("Nouvelle dépense", Theme.IcoDepense, Theme.Rouge, (_, _) => Principale?.NouvelleOperation("DEPENSE")));
        }
        if (Session.Peut("OPERER"))
            outils.Add(Theme.Outil("Virement", Theme.IcoVirement, Theme.Accent, (_, _) => Principale?.NouveauVirement()));
        if (outils.Count > 0) outils.Add(Separateur());
        // Action vide : Executer recharge ensuite l'écran (Actualiser) et la barre d'état
        outils.Add(Theme.Outil("Actualiser", Theme.IcoActualiser, Theme.Gris, (_, _) => Executer(() => { })));
        return outils;
    }

    // Recharge tout le tableau de bord (appelé à l'ouverture, par F5 et après chaque opération)
    public override void Actualiser()
    {
        // 1. Bienvenue
        var exercice = Depot.ExerciceCourant();
        string salut = DateTime.Now.Hour < 18 ? "Bonjour" : "Bonsoir";
        _bandeau.Ligne1 = $"Bienvenue, {Session.RoleAffiche} {Session.NomComplet}";
        _bandeau.Ligne2 = Session.DescriptionRole;
        _bandeau.Ligne3 = $"{salut} ! Nous sommes le {DateTime.Today:dddd d MMMM yyyy}" +
                          (exercice is { } ex ? $"  ·  Exercice {ex.Libelle} ({ex.Statut.ToLower()})" : "  ·  Aucun exercice ouvert pour aujourd'hui");
        _bandeau.Animer();

        // 2. Soldes des comptes
        var comptes = Depot.SoldesComptes();
        // SuspendLayout / ResumeLayout : évite que l'écran se redessine à chaque carte ajoutée
        _cartes.SuspendLayout();
        _cartes.Controls.Clear();
        var actifs = comptes.Rows.Cast<DataRow>().Where(r => Convert.ToBoolean(r["Actif"])).ToList();
        decimal total = actifs.Sum(r => Convert.ToDecimal(r["Solde actuel"]));
        _cartes.Controls.Add(Carte("Trésorerie totale", Theme.Montant(total) + " FCFA", $"{actifs.Count} compte(s) actif(s)",
            Theme.Nuit, Color.White, Theme.Or));
        // Une carte par compte : bande verte = caisse, bleue = banque, dorée = mobile money
        foreach (var r in actifs)
        {
            var type = r["Type"].ToString();
            var bande = type switch { "CAISSE" => Theme.Vert, "BANQUE" => Theme.Accent, _ => Theme.Or };
            var solde = Convert.ToDecimal(r["Solde actuel"]);
            _cartes.Controls.Add(Carte(r["Libellé"].ToString()!, Theme.Montant(solde),
                $"{TypeAffiche(type)} · découvert {Theme.Montant(Convert.ToDecimal(r["Découvert"]))}",
                Color.White, solde < 0 ? Theme.Rouge : Color.FromArgb(27, 27, 27), bande));
        }
        if (actifs.Count == 0)
            _cartes.Controls.Add(Carte("Aucun compte", "—", "Créez la caisse et les comptes dans Paramétrage", Color.White, Theme.Gris, Theme.Gris));
        _cartes.ResumeLayout();

        // 3. Mes tâches
        var t = Depot.Taches();
        RemplirTaches(t, exercice != null);

        // 4. Graphique et indicateurs du mois
        _graphique.Definir(Depot.SixDerniersMois());
        var (recettes, depenses) = Depot.TotauxMois();
        _indicateurs.SuspendLayout();
        _indicateurs.Controls.Clear();
        string mois = DateTime.Today.ToString("MMMM yyyy");
        _indicateurs.Controls.Add(Tuile("Recettes du mois", "+ " + Theme.Montant(recettes), mois, Theme.VertClair, Theme.Vert));
        _indicateurs.Controls.Add(Tuile("Dépenses du mois", "− " + Theme.Montant(depenses), mois, Theme.RougeClair, Theme.Rouge));
        // 3e tuile : personnelle pour ceux qui saisissent, sinon résultat du mois
        if (Session.Peut("SAISIR"))
            _indicateurs.Controls.Add(Tuile("Mes saisies du mois", t.MesSaisiesDuMois.ToString(), "opération(s) saisie(s) par moi", Theme.BleuClair, Theme.Nuit));
        else
            _indicateurs.Controls.Add(Tuile("Résultat du mois", Theme.Montant(recettes - depenses), "recettes − dépenses", Theme.BleuClair, Theme.Nuit));
        _indicateurs.ResumeLayout();

        // 5. Dernières opérations (mes brouillons privés inclus, pas ceux des autres)
        _grille.DataSource = Depot.DerniersMouvements(12);
    }

    // ------------------------------------------------------------------ mes tâches

    /// <summary>
    /// Construit la liste « Mes tâches » : chaque tâche n'apparaît que si le rôle a le droit
    /// correspondant ET s'il y a quelque chose à faire. Cliquer une tâche ouvre l'écran concerné.
    /// </summary>
    private void RemplirTaches(Depot.CompteursTaches t, bool exerciceCourant)
    {
        _taches.SuspendLayout();
        _taches.Controls.Clear();
        int urgentes = 0;

        // Fonction locale : ajoute une tâche (en gras) et la compte
        void Tache(string texte, Color couleur, string glyphe, Action action)
        {
            urgentes++;
            _taches.Controls.Add(LigneTache(texte, couleur, glyphe, action, gras: true));
        }

        // Valideurs (trésorier, président…) : brouillons à valider ; saisie : mes brouillons à terminer
        if (Session.Peut("VALIDER") && t.AValider > 0)
            Tache($"{t.AValider} opération(s) en attente de validation", Theme.Vert, Theme.IcoValider, () => Principale?.Naviguer("a_valider"));
        if (Session.Peut("SAISIR") && t.MesBrouillons > 0)
            Tache($"Terminer mes brouillons ({t.MesBrouillons})",Theme.Orange, Theme.IcoModifier, () => Principale?.Naviguer("mes_brouillons"));
        // Paramétrage manquant (exercice, comptes, catégories, centres)
        if (Session.Peut("PARAMETRER"))
        {
            if (!exerciceCourant) Tache($"Créer l'exercice {DateTime.Today.Year}", Theme.Orange, Theme.IcoCalendrier, () => Principale?.Naviguer("param_exercice"));
            if (t.Comptes == 0) Tache("Créer la caisse et les comptes", Theme.Orange, Theme.IcoCompte, () => Principale?.Naviguer("param_compte"));
            if (t.Categories == 0) Tache("Créer les catégories de recettes et de dépenses", Theme.Orange, Theme.IcoEtiquette, () => Principale?.Naviguer("param_categorie"));
            if (t.Centres == 0) Tache("Créer les centres analytiques", Theme.Orange, Theme.IcoCentre, () => Principale?.Naviguer("param_centre"));
        }
        if (Session.Peut("CLOTURER") && t.ExercicesACloturer > 0)
            Tache($"{t.ExercicesACloturer} exercice(s) terminé(s) à clôturer", Theme.Rouge, Theme.IcoCadenas, () => Principale?.Naviguer("param_exercice"));
        if (Session.Peut("UTILISATEURS") && t.UtilisateursBloques > 0)
            Tache($"Débloquer {t.UtilisateursBloques} compte(s) utilisateur", Theme.Rouge, Theme.IcoDeverrouiller, () => Principale?.Naviguer("utilisateurs"));
        // Points de contrôle propres au commissaire aux comptes
        if (Session.Role == "COMMISSAIRE_COMPTES")
        {
            if (t.SansJustificatif > 0)
                Tache($"{t.SansJustificatif} opération(s) validée(s) sans pièce justificative", Theme.Orange, Theme.IcoTrombone, () => Principale?.Naviguer("mouvements"));
            if (t.AnnulationsDuMois > 0)
                Tache($"{t.AnnulationsDuMois} annulation(s) ce mois à contrôler", Theme.Orange, Theme.IcoBouclier, () => Principale?.Naviguer("audit"));
        }

        // Rien à faire : message vert
        if (urgentes == 0)
            _taches.Controls.Add(new Label
            {
                Text = "Aucune tâche en attente : tout est à jour.",
                AutoSize = true,
                ForeColor = Theme.Vert,
                Font = Theme.PoliceGras,
                Margin = new Padding(Theme.Px(6), Theme.Px(6), 0, Theme.Px(6))
            });

        // Raccourcis selon le rôle
        _taches.Controls.Add(new Label
        {
            Text = "Raccourcis",
            AutoSize = true,
            ForeColor = Theme.TexteSecondaire,
            Margin = new Padding(Theme.Px(6), Theme.Px(8), 0, Theme.Px(2))
        });
        if (Session.Peut("SAISIR"))
        {
            _taches.Controls.Add(LigneTache("Saisir une recette", Theme.Vert, Theme.IcoRecette, () => Principale?.NouvelleOperation("RECETTE")));
            _taches.Controls.Add(LigneTache("Saisir une dépense", Theme.Rouge, Theme.IcoDepense, () => Principale?.NouvelleOperation("DEPENSE")));
        }
        if (Session.Peut("OPERER"))
            _taches.Controls.Add(LigneTache("Faire un virement entre comptes", Theme.Accent, Theme.IcoVirement, () => Principale?.NouveauVirement()));
        _taches.Controls.Add(LigneTache("Consulter les comptes et soldes", Theme.OrFonce, Theme.IcoCompte, () => Principale?.Naviguer("comptes")));
        _taches.Controls.Add(LigneTache("Suivre le budget", Theme.Accent, Theme.IcoGraphique, () => Principale?.Naviguer("budget")));
        _taches.ResumeLayout();
    }

    // Une ligne cliquable de « Mes tâches » : bouton plat avec icône, qui exécute « action » au clic
    private Button LigneTache(string texte, Color couleur, string glyphe, Action action, bool gras = false)
    {
        var b = new Button
        {
            Text = "  " + texte,
            Image = Theme.Glyphe(glyphe, couleur, 16),
            ImageAlign = ContentAlignment.MiddleLeft,
            TextAlign = ContentAlignment.MiddleLeft,
            TextImageRelation = TextImageRelation.ImageBeforeText,
            FlatStyle = FlatStyle.Flat,
            Font = gras ? Theme.PoliceGras : Theme.Police,
            ForeColor = gras ? couleur : Color.FromArgb(27, 27, 27),
            BackColor = Color.White,
            Height = Theme.Px(26),
            Width = Math.Max(Theme.Px(200), _taches.ClientSize.Width - Theme.Px(24)),
            Margin = new Padding(Theme.Px(2), 0, 0, Theme.Px(1)),
            Padding = new Padding(Theme.Px(4), 0, 0, 0),
            Cursor = Cursors.Hand,
            FlatAppearance = { BorderSize = 0, MouseOverBackColor = Theme.Selection, MouseDownBackColor = Color.FromArgb(170, 210, 245) }
        };
        b.Click += (_, _) => action();
        return b;
    }

    // ------------------------------------------------------------------ éléments graphiques

    // Tuile d'indicateur (colonne de droite) : titre, grande valeur, note, bande de couleur à gauche
    private Panel Tuile(string titre, string valeur, string note, Color fond, Color couleur)
    {
        int largeur = Math.Max(Theme.Px(160), _indicateurs.ClientSize.Width - Theme.Px(4));
        var p = new Panel
        {
            Size = new Size(largeur, Theme.Px(70)),
            BackColor = fond,
            Margin = new Padding(0, 0, 0, Theme.Px(7)),
            Padding = new Padding(Theme.Px(12), Theme.Px(6), Theme.Px(6), Theme.Px(4))
        };
        // Avec Dock = Top, le DERNIER ajouté se place en HAUT : on ajoute donc note, valeur, puis titre
        p.Controls.Add(new Label { Text = note, Dock = DockStyle.Top, Height = Theme.Px(18), ForeColor = Theme.TexteSecondaire, AutoEllipsis = true });
        p.Controls.Add(new Label { Text = valeur, Dock = DockStyle.Top, Height = Theme.Px(26), Font = new Font("Segoe UI Semibold", 13f), ForeColor = couleur, AutoEllipsis = true });
        p.Controls.Add(new Label { Text = titre, Dock = DockStyle.Top, Height = Theme.Px(18), ForeColor = couleur, AutoEllipsis = true });
        p.Paint += (_, e) =>
        {
            using var b = new SolidBrush(couleur);
            e.Graphics.FillRectangle(b, 0, 0, Theme.Px(4), p.Height);
        };
        return p;
    }

    // Code du type de compte → texte lisible
    private static string TypeAffiche(string? type) => type switch
    {
        "CAISSE" => "Caisse",
        "BANQUE" => "Banque",
        "MOBILE_MONEY" => "Mobile Money",
        _ => type ?? ""
    };

    // Carte de solde (ligne du haut) : 232 × 96, bande de couleur en haut ; texte clair si le fond est sombre
    private static Panel Carte(string titre, string valeur, string note, Color fond, Color couleurValeur, Color bande)
    {
        var carte = new Panel
        {
            Size = new Size(Theme.Px(232), Theme.Px(96)),
            BackColor = fond,
            Margin = new Padding(0, 0, Theme.Px(10), Theme.Px(8)),
            Padding = new Padding(Theme.Px(14), Theme.Px(10), Theme.Px(10), Theme.Px(6))
        };
        bool sombre = fond.GetBrightness() < 0.4f;
        var secondaire = sombre ? Color.FromArgb(200, 220, 240) : Theme.TexteSecondaire;
        carte.Controls.Add(new Label { Text = note, Dock = DockStyle.Top, Height = Theme.Px(22), ForeColor = secondaire, AutoEllipsis = true });
        carte.Controls.Add(new Label { Text = valeur, Dock = DockStyle.Top, Height = Theme.Px(34), Font = new Font("Segoe UI Semibold", 15f), ForeColor = couleurValeur, AutoEllipsis = true });
        carte.Controls.Add(new Label { Text = titre, Dock = DockStyle.Top, Height = Theme.Px(22), ForeColor = secondaire, AutoEllipsis = true });
        carte.Paint += (_, e) =>
        {
            using var b = new SolidBrush(bande);
            e.Graphics.FillRectangle(b, 0, 0, carte.Width, Theme.Px(4));
            if (!sombre)
            {
                using var p = new Pen(Theme.Bordure);
                e.Graphics.DrawRectangle(p, 0, 0, carte.Width - 1, carte.Height - 1);
            }
        };
        return carte;
    }
}
