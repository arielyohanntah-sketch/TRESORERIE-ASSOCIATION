// =====================================================================================
//  Forms/MainForm.cs — FENÊTRE PRINCIPALE
// -------------------------------------------------------------------------------------
//  Disposition (créée dans MainForm.Designer.cs) :
//      _outils     barre d'outils du haut : boutons de l'écran affiché + menu utilisateur
//      _arbre      menu de navigation à gauche (TreeView)
//      _contenu    zone centrale où s'affiche l'écran choisi (une « Vue » du dossier Views)
//      barre d'état en bas : _lblUtilisateur, _lblExercice, _lblBrouillons, _lblBase
//
//  Fonctionnement :
//    - ConstruireArbre() crée les entrées du menu SELON LES DROITS du rôle connecté.
//      Chaque entrée a une « clé » (ex. "comptes") et une fonction qui crée sa vue.
//    - Cliquer une entrée → Afficher(vue) : la vue remplace la précédente au centre,
//      ses boutons (vue.Outils()) remplacent ceux de la barre d'outils, puis
//      vue.Actualiser() charge les données.
//    - Naviguer("cle") permet à une vue d'en ouvrir une autre (ex. carte du tableau de bord).
//
//  ➜ AJOUTER UN ÉCRAN : créez la vue dans Views (en copiant une vue simple comme
//    VirementsVue), puis ajoutez une ligne Noeud(...) dans ConstruireArbre().
//
//  Raccourcis : Ctrl+R recette, Ctrl+D dépense, Ctrl+T virement, F5 actualiser.
// =====================================================================================

using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;
using SLN_TRESORERIE_ASSO.Views;

namespace SLN_TRESORERIE_ASSO.Forms;

/// <summary>
/// Fenêtre principale : barre d'outils (actions de l'écran + menu de l'utilisateur à droite),
/// arborescence à gauche (une seule navigation, adaptée au rôle), écran au centre, barre d'état en bas.
/// La mise en page est dans MainForm.Designer.cs ; l'arborescence est remplie ici selon les droits.
/// </summary>
public partial class MainForm : Form
{
    // Vrai si l'utilisateur a choisi « Se déconnecter » (Program.cs réaffiche alors l'écran de connexion)
    public bool Deconnexion { get; private set; }

    // Menu déroulant « Prénom Nom · Rôle » à droite de la barre d'outils
    private readonly ToolStripDropDownButton _menuUtilisateur = new();
    // Entrées du menu de gauche, retrouvées par leur clé ("accueil", "comptes"…)
    private readonly Dictionary<string, TreeNode> _noeuds = new();
    // Écran affiché actuellement au centre
    private VueBase? _vue;

    public MainForm()
    {
        InitializeComponent();
        // Icône de la fenêtre (barre de titre et barre des tâches) et style des barres
        Icon = Icon.FromHandle(Theme.Glyphe(Theme.IcoCompte, Theme.Accent, 32).GetHicon());
        ToolStripManager.Renderer = new RenduBarres();
        _outils.ImageScalingSize = new Size(Theme.Px(20), Theme.Px(20));

        ConstruireMenuUtilisateur();
        ConstruireArbre();
        // Clic sur le compteur de brouillons de la barre d'état → ouvre la liste correspondante
        _lblBrouillons.Click += (_, _) => Naviguer(Session.Peut("VALIDER") ? "a_valider" : "mes_brouillons");

        // À l'ouverture : barre d'état à jour et affichage du tableau de bord
        Load += (_, _) =>
        {
            RafraichirStatut();
            Naviguer("accueil");
        };
    }

    // ------------------------------------------------------------------ construction

    /// <summary>Menu de l'utilisateur connecté, toujours à droite de la barre d'outils.</summary>
    private void ConstruireMenuUtilisateur()
    {
        _menuUtilisateur.Text = $"{Session.NomComplet} · {Session.RoleAffiche}";
        _menuUtilisateur.Image = Theme.Glyphe(Theme.IcoPersonne, Theme.Nuit, 20);
        _menuUtilisateur.Alignment = ToolStripItemAlignment.Right;
        _menuUtilisateur.ImageScaling = ToolStripItemImageScaling.None;
        _menuUtilisateur.Font = Theme.PoliceGras;
        // Éléments du menu : DropDownItems.Add(texte, icône, action au clic)
        _menuUtilisateur.DropDownItems.Add("Changer mon mot de passe…", Theme.Glyphe(Theme.IcoCadenas, Theme.Gris, 16), (_, _) =>
        {
            using var f = new MotDePasseForm();
            if (f.ShowDialog(this) == DialogResult.OK)
                Notifier("Mot de passe modifié", "Utilisez-le dès votre prochaine connexion.");
        });
        _menuUtilisateur.DropDownItems.Add("À propos", Theme.Glyphe("", Theme.Gris, 16), (_, _) => Ui.Info(
            "Trésorerie Asso — version 1.0\nGestion de la trésorerie d'une association.\n\n" +
            $"Base : {Connexion.Base} sur {Connexion.Serveur}:{Connexion.Port}\n\n" +
            "Raccourcis : Ctrl+R nouvelle recette · Ctrl+D nouvelle dépense · Ctrl+T virement", "À propos"));
        _menuUtilisateur.DropDownItems.Add(new ToolStripSeparator());
        _menuUtilisateur.DropDownItems.Add("Se déconnecter", Theme.Glyphe("", Theme.Rouge, 16), (_, _) =>
        {
            Deconnexion = true;
            Close();
        });
        _menuUtilisateur.DropDownItems.Add("Quitter l'application", null, (_, _) => Close());
    }

    /// <summary>Navigation unique, construite selon les droits du rôle connecté.</summary>
    private void ConstruireArbre()
    {
        // Liste des petites icônes affichées devant chaque entrée
        var images = new ImageList { ImageSize = new Size(Theme.Px(18), Theme.Px(18)), ColorDepth = ColorDepth.Depth32Bit };
        _arbre.ImageList = images;

        // Fonction locale qui ajoute une entrée au menu :
        //   parent  : où l'ajouter (racine ou sous une autre entrée)
        //   cle     : nom unique utilisé par Naviguer("cle")
        //   texte   : texte affiché ; glyphe/couleur : icône
        //   fabrique: fonction qui crée l'écran (null = simple titre de groupe sans écran)
        TreeNode Noeud(TreeNodeCollection parent, string cle, string texte, string glyphe, Color couleur, Func<VueBase>? fabrique)
        {
            images.Images.Add(cle, Theme.Glyphe(glyphe, couleur, 18));
            var n = new TreeNode(texte) { Name = cle, Tag = fabrique, ImageKey = cle, SelectedImageKey = cle };
            parent.Add(n);
            _noeuds[cle] = n;
            return n;
        }

        var racine = _arbre.Nodes;
        Noeud(racine, "accueil", "Tableau de bord", Theme.IcoAccueil, Theme.Accent, () => new TableauDeBordVue());

        // Mon espace : ce qui ne concerne que moi
        var espace = Noeud(racine, "espace", "Mon espace", Theme.IcoPersonne, Theme.OrFonce, null);
        if (Session.Peut("SAISIR"))
        {
            Noeud(espace.Nodes, "mes_brouillons", "Mes brouillons", Theme.IcoModifier, Theme.Orange, () => new MouvementsVue(null, "BROUILLON", mesSaisies: true));
            Noeud(espace.Nodes, "mes_saisies", "Mes saisies", Theme.IcoListe, Theme.Nuit, () => new MouvementsVue(null, null, mesSaisies: true));
        }
        if (Session.Peut("VALIDER"))
            Noeud(espace.Nodes, "a_valider", "À valider", Theme.IcoValider, Theme.Vert, () => new MouvementsVue(null, "BROUILLON"));
        Noeud(espace.Nodes, "mon_activite", "Mon activité", Theme.IcoBouclier, Theme.Gris, () => new AuditVue(seulementMoi: true));

        // Trésorerie (consultation pour tous)
        var mvts = Noeud(racine, "mouvements", "Mouvements", Theme.IcoListe, Theme.Nuit, () => new MouvementsVue(null, null));
        Noeud(mvts.Nodes, "recettes", "Recettes", Theme.IcoRecette, Theme.Vert, () => new MouvementsVue("RECETTE", null));
        Noeud(mvts.Nodes, "depenses", "Dépenses", Theme.IcoDepense, Theme.Rouge, () => new MouvementsVue("DEPENSE", null));
        Noeud(racine, "virements", "Virements", Theme.IcoVirement, Theme.Accent, () => new VirementsVue());
        Noeud(racine, "comptes", "Comptes et soldes", Theme.IcoCompte, Theme.OrFonce, () => new ComptesVue());
        Noeud(racine, "budget", "Budget vs réalisé", Theme.IcoGraphique, Theme.Accent, () => new BudgetVue());

        // Paramétrage : seulement pour les rôles qui ont le droit PARAMETRER
        if (Session.Peut("PARAMETRER"))
        {
            var param = Noeud(racine, "param", "Paramétrage", Theme.IcoParametres, Theme.Gris, null);
            Noeud(param.Nodes, "param_compte", "Fiches des comptes", Theme.IcoCompte, Theme.Gris, () => new TableVue("compte", "Fiches des comptes"));
            Noeud(param.Nodes, "param_categorie", "Catégories", Theme.IcoEtiquette, Theme.Gris, () => new TableVue("categorie", "Catégories de recettes et de dépenses"));
            Noeud(param.Nodes, "param_tiers", "Tiers", Theme.IcoPersonnes, Theme.Gris, () => new TableVue("tiers", "Tiers (membres, fournisseurs, donateurs…)"));
            Noeud(param.Nodes, "param_centre", "Centres analytiques", Theme.IcoCentre, Theme.Gris, () => new TableVue("centre_analytique", "Centres analytiques"));
            Noeud(param.Nodes, "param_budget", "Budgets prévisionnels", Theme.IcoGraphique, Theme.Gris, () => new TableVue("budget_centre", "Budgets prévisionnels par centre et par exercice"));
            Noeud(param.Nodes, "param_exercice", "Exercices", Theme.IcoCalendrier, Theme.Gris, () => new TableVue("exercice", "Exercices comptables"));
        }

        // Contrôle et administration
        if (Session.PeutVoirAudit)
            Noeud(racine, "audit", "Journal d'audit", Theme.IcoBouclier, Theme.Nuit, () => new AuditVue());
        if (Session.Peut("UTILISATEURS"))
            Noeud(racine, "utilisateurs", "Utilisateurs", Theme.IcoPersonnes, Theme.Nuit, () => new UtilisateursVue());

        // Toutes les branches ouvertes au démarrage
        _arbre.ExpandAll();

        // Dessin personnalisé des entrées (DrawMode = OwnerDrawText dans le concepteur) :
        // fond bleu clair pour l'entrée sélectionnée, titres de groupe en gras
        _arbre.DrawNode += (_, e) =>
        {
            if (e.Node == null) return;
            bool selectionne = (e.State & TreeNodeStates.Selected) != 0;
            var zone = new Rectangle(e.Bounds.X, e.Bounds.Y, _arbre.ClientSize.Width - e.Bounds.X, e.Bounds.Height);
            using (var fond = new SolidBrush(selectionne ? Theme.Selection : _arbre.BackColor))
                e.Graphics.FillRectangle(fond, zone);
            var police = e.Node.Parent == null && e.Node.Nodes.Count > 0 ? Theme.PoliceGras : Theme.Police;
            TextRenderer.DrawText(e.Graphics, e.Node.Text, police, zone,
                selectionne ? Theme.Nuit : Color.FromArgb(27, 27, 27), TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        };
        // Sélection d'une entrée → on crée son écran et on l'affiche
        _arbre.AfterSelect += (_, e) =>
        {
            if (e.Node?.Tag is Func<VueBase> fabrique) Afficher(fabrique());
        };
    }

    // ------------------------------------------------------------------ navigation

    /// <summary>Ouvre l'écran correspondant à une clé du menu (ex. "a_valider"). Sans effet si le rôle n'y a pas accès.</summary>
    public void Naviguer(string cle)
    {
        if (!_noeuds.TryGetValue(cle, out var noeud)) return;
        // Entrée déjà sélectionnée : on recrée l'écran (sinon AfterSelect ne se déclencherait pas)
        if (_arbre.SelectedNode == noeud && noeud.Tag is Func<VueBase> f) Afficher(f());
        else _arbre.SelectedNode = noeud;
    }

    /// <summary>Vrai si l'entrée existe dans le menu de ce rôle (utilisé par le tableau de bord pour ses liens).</summary>
    public bool PeutNaviguer(string cle) => _noeuds.ContainsKey(cle);

    // Remplace l'écran du centre par « vue »
    private void Afficher(VueBase vue)
    {
        // 1) Échange des écrans ; l'ancien est libéré de la mémoire (Dispose)
        var ancienne = _vue;
        _vue = vue;
        _contenu.SuspendLayout();
        _contenu.Controls.Clear();
        _contenu.Controls.Add(vue);
        _contenu.ResumeLayout();
        ancienne?.Dispose();

        // 2) Boutons propres à cet écran + menu utilisateur, et titre de la fenêtre
        _outils.Items.Clear();
        _outils.Items.AddRange(vue.Outils().ToArray());
        _outils.Items.Add(_menuUtilisateur);
        Text = string.IsNullOrEmpty(vue.Titre) ? "Trésorerie Asso" : $"Trésorerie Asso — {vue.Titre}";

        // 3) Chargement des données de l'écran
        try
        {
            Cursor = Cursors.WaitCursor;
            vue.Actualiser();
        }
        catch (Exception ex)
        {
            Ui.Erreur(ex, "Chargement impossible");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>Met à jour la barre d'état (utilisateur, exercice, compteur de brouillons) et les compteurs du menu.</summary>
    public void RafraichirStatut()
    {
        _lblUtilisateur.Text = $"Connecté : {Session.NomComplet} ({Session.RoleAffiche})";
        _lblBase.Text = $"{Connexion.Base} @ {Connexion.Serveur} · {DateTime.Today:dd/MM/yyyy}";
        try
        {
            var ex = Depot.ExerciceCourant();
            _lblExercice.Text = ex is { } e ? $"Exercice {e.Libelle} — {e.Statut}" : "Aucun exercice ne couvre la date du jour";
            // Fond orange = attention (pas d'exercice en cours, ou exercice clôturé)
            _lblExercice.BackColor = ex is null || ex.Value.Statut == "CLOTURE" ? Theme.Orange : Theme.Accent;

            // Compteur personnel : opérations à valider (valideurs) ou mes brouillons (saisie)
            if (Session.Peut("VALIDER"))
            {
                int n = Depot.NombreBrouillons();
                _lblBrouillons.Text = n == 0 ? "Aucune opération à valider" : $"{n} opération(s) à valider";
                _lblBrouillons.Visible = true;
                MajNoeud("a_valider", "À valider", n);
            }
            else if (Session.Peut("SAISIR"))
            {
                int n = Depot.NombreBrouillons(seulementLesMiens: true);
                _lblBrouillons.Text = n == 0 ? "Aucun brouillon en cours" : $"{n} brouillon(s) en cours";
                _lblBrouillons.Visible = true;
            }
            else
            {
                _lblBrouillons.Visible = false;
            }
            if (Session.Peut("SAISIR")) MajNoeud("mes_brouillons", "Mes brouillons", Depot.NombreBrouillons(seulementLesMiens: true));
        }
        catch
        {
            _lblExercice.Text = "Base inaccessible";
        }
    }

    // Ajoute le nombre entre parenthèses au texte d'une entrée du menu : « À valider (3) »
    private void MajNoeud(string cle, string texte, int nombre)
    {
        if (_noeuds.TryGetValue(cle, out var n)) n.Text = nombre > 0 ? $"{texte} ({nombre})" : texte;
    }

    /// <summary>Raccourcis clavier.</summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        // Pour ajouter un raccourci : une nouvelle ligne « case Keys.Control | Keys.X: ...; return true; »
        switch (keyData)
        {
            case Keys.Control | Keys.R: NouvelleOperation("RECETTE"); return true;
            case Keys.Control | Keys.D: NouvelleOperation("DEPENSE"); return true;
            case Keys.Control | Keys.T: NouveauVirement(); return true;
            case Keys.F5: ApresModification(); return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    // ------------------------------------------------------------------ actions globales

    // Ces méthodes sont publiques : les vues les appellent via « Principale?.NouvelleOperation(...) ».

    /// <summary>Affiche une notification animée en bas à droite.</summary>
    public void Notifier(string titre, string message, TypeNotification type = TypeNotification.Succes) =>
        Notification.Afficher(this, titre, message, type);

    /// <summary>Ouvre la saisie d'une nouvelle recette ("RECETTE") ou dépense ("DEPENSE").</summary>
    public void NouvelleOperation(string type)
    {
        if (!Session.Peut("SAISIR"))
        {
            Ui.Info($"Votre rôle ({Session.RoleAffiche}) ne permet pas de saisir des opérations.");
            return;
        }
        using var f = new SaisieMouvementForm(type);
        if (f.ShowDialog(this) == DialogResult.OK) ApresModification(f.Resultat, f.IdEnregistre);
    }

    /// <summary>Ouvre un mouvement existant (double-clic dans une liste) en consultation ou en modification.</summary>
    public void OuvrirMouvement(int id)
    {
        // Le chargement peut échouer (ex. brouillon privé d'un autre) : on affiche l'erreur et on s'arrête
        SaisieMouvementForm f;
        try { f = new SaisieMouvementForm(id); }
        catch (Exception ex) { Ui.Erreur(ex); return; }
        using (f)
        {
            if (f.ShowDialog(this) == DialogResult.OK) ApresModification(f.Resultat, f.IdEnregistre);
        }
    }

    /// <summary>Ouvre la fenêtre de nouveau virement (droit OPERER).</summary>
    public void NouveauVirement()
    {
        if (!Session.Peut("OPERER"))
        {
            Ui.Info($"Votre rôle ({Session.RoleAffiche}) ne permet pas de faire des virements.");
            return;
        }
        using var f = new VirementForm();
        if (f.ShowDialog(this) == DialogResult.OK) ApresModification(f.Resultat, null);
    }

    /// <summary>Rafraîchit l'écran et la barre d'état, puis affiche la notification du résultat.</summary>
    public void ApresModification((string Titre, string Message, TypeNotification Type)? resultat = null, int? idMouvement = null)
    {
        try { _vue?.Actualiser(); } catch (Exception ex) { Ui.Erreur(ex); }
        RafraichirStatut();
        if (resultat is { } r)
        {
            Notifier(r.Titre, r.Message, r.Type);
            // Effet visuel : la ligne concernée s'allume puis s'estompe dans la liste
            if (idMouvement is int id && _vue is MouvementsVue mv) mv.Surligner(id, r.Type);
        }
    }
}
