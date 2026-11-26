// =====================================================================================
//  Views/MouvementsVue.Designer.cs — MISE EN PAGE DE MouvementsVue (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Titre, bandeau d'information, barre de filtres, grille des opérations et ligne des totaux.
//  La barre de filtres (_filtres) est un FlowLayoutPanel : chaque filtre (libellé + champ)
//  est dans son propre petit panneau (_pDu, _pAu…) et passe à la ligne suivante quand la
//  fenêtre est trop étroite, au lieu d'être coupé.
//
//  ➜ Pour modifier l'apparence : clic droit sur MouvementsVue.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans MouvementsVue.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans MouvementsVue.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Views
{
    partial class MouvementsVue
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        private void InitializeComponent()
        {
            _info = new Label();
            _entete = new Label();
            _filtres = new FlowLayoutPanel();
            _pDu = new FlowLayoutPanel();
            _lblDu = new Label();
            _du = new DateTimePicker();
            _pAu = new FlowLayoutPanel();
            _lblAu = new Label();
            _au = new DateTimePicker();
            _pCompte = new FlowLayoutPanel();
            _lblCompte = new Label();
            _compte = new ComboBox();
            _pType = new FlowLayoutPanel();
            _lblType = new Label();
            _type = new ComboBox();
            _pStatut = new FlowLayoutPanel();
            _lblStatut = new Label();
            _statut = new ComboBox();
            _pRecherche = new FlowLayoutPanel();
            _lblRecherche = new Label();
            _recherche = new TextBox();
            _pBoutons = new FlowLayoutPanel();
            _btnFiltrer = new Button();
            _btnEffacer = new Button();
            _marge = new Panel();
            _grille = new DataGridView();
            _totaux = new Label();
            _filtres.SuspendLayout();
            _pDu.SuspendLayout();
            _pAu.SuspendLayout();
            _pCompte.SuspendLayout();
            _pType.SuspendLayout();
            _pStatut.SuspendLayout();
            _pRecherche.SuspendLayout();
            _pBoutons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grille).BeginInit();
            SuspendLayout();
            //
            // _info
            //
            _info.BackColor = Color.FromArgb(229, 241, 251);
            _info.BorderStyle = BorderStyle.FixedSingle;
            _info.Dock = DockStyle.Top;
            _info.ForeColor = Color.FromArgb(11, 61, 102);
            _info.Location = new Point(15, 15);
            _info.Margin = new Padding(4, 0, 4, 0);
            _info.Name = "_info";
            _info.Padding = new Padding(15, 0, 15, 0);
            _info.Size = new Size(1208, 50);
            _info.TabIndex = 5;
            _info.Text = "Espace personnel : seules les opérations que vous avez saisies apparaissent ici. Vos brouillons ne sont visibles que par vous et par les valideurs.";
            _info.TextAlign = ContentAlignment.MiddleLeft;
            _info.Visible = false;
            //
            // _entete
            //
            _entete.Dock = DockStyle.Top;
            _entete.Font = new Font("Segoe UI Semibold", 15F);
            _entete.ForeColor = Color.FromArgb(11, 61, 102);
            _entete.Location = new Point(15, 65);
            _entete.Margin = new Padding(4, 0, 4, 0);
            _entete.Name = "_entete";
            _entete.Size = new Size(1208, 57);
            _entete.TabIndex = 4;
            _entete.Text = "Mouvements";
            _entete.TextAlign = ContentAlignment.MiddleLeft;
            //
            // _filtres
            //
            _filtres.AutoSize = true;
            _filtres.BackColor = Color.White;
            _filtres.Controls.Add(_pDu);
            _filtres.Controls.Add(_pAu);
            _filtres.Controls.Add(_pCompte);
            _filtres.Controls.Add(_pType);
            _filtres.Controls.Add(_pStatut);
            _filtres.Controls.Add(_pRecherche);
            _filtres.Controls.Add(_pBoutons);
            _filtres.Dock = DockStyle.Top;
            _filtres.Location = new Point(15, 122);
            _filtres.Margin = new Padding(0);
            _filtres.Name = "_filtres";
            _filtres.Padding = new Padding(15, 9, 6, 0);
            _filtres.Size = new Size(1208, 186);
            _filtres.TabIndex = 3;
            //
            // _pDu
            //
            _pDu.AutoSize = true;
            _pDu.Controls.Add(_lblDu);
            _pDu.Controls.Add(_du);
            _pDu.FlowDirection = FlowDirection.TopDown;
            _pDu.Location = new Point(15, 9);
            _pDu.Margin = new Padding(0, 0, 21, 12);
            _pDu.Name = "_pDu";
            _pDu.Size = new Size(163, 59);
            _pDu.TabIndex = 0;
            _pDu.WrapContents = false;
            //
            // _lblDu
            //
            _lblDu.AutoSize = true;
            _lblDu.Location = new Point(0, 0);
            _lblDu.Margin = new Padding(0, 0, 0, 3);
            _lblDu.Name = "_lblDu";
            _lblDu.Size = new Size(35, 25);
            _lblDu.TabIndex = 0;
            _lblDu.Text = "Du";
            //
            // _du
            //
            _du.Format = DateTimePickerFormat.Short;
            _du.Location = new Point(0, 28);
            _du.Margin = new Padding(0);
            _du.Name = "_du";
            _du.Size = new Size(163, 31);
            _du.TabIndex = 1;
            //
            // _pAu
            //
            _pAu.AutoSize = true;
            _pAu.Controls.Add(_lblAu);
            _pAu.Controls.Add(_au);
            _pAu.FlowDirection = FlowDirection.TopDown;
            _pAu.Location = new Point(199, 9);
            _pAu.Margin = new Padding(0, 0, 21, 12);
            _pAu.Name = "_pAu";
            _pAu.Size = new Size(163, 59);
            _pAu.TabIndex = 1;
            _pAu.WrapContents = false;
            //
            // _lblAu
            //
            _lblAu.AutoSize = true;
            _lblAu.Location = new Point(0, 0);
            _lblAu.Margin = new Padding(0, 0, 0, 3);
            _lblAu.Name = "_lblAu";
            _lblAu.Size = new Size(34, 25);
            _lblAu.TabIndex = 0;
            _lblAu.Text = "Au";
            //
            // _au
            //
            _au.Format = DateTimePickerFormat.Short;
            _au.Location = new Point(0, 28);
            _au.Margin = new Padding(0);
            _au.Name = "_au";
            _au.Size = new Size(163, 31);
            _au.TabIndex = 1;
            //
            // _pCompte
            //
            _pCompte.AutoSize = true;
            _pCompte.Controls.Add(_lblCompte);
            _pCompte.Controls.Add(_compte);
            _pCompte.FlowDirection = FlowDirection.TopDown;
            _pCompte.Location = new Point(383, 9);
            _pCompte.Margin = new Padding(0, 0, 21, 12);
            _pCompte.Name = "_pCompte";
            _pCompte.Size = new Size(283, 61);
            _pCompte.TabIndex = 2;
            _pCompte.WrapContents = false;
            //
            // _lblCompte
            //
            _lblCompte.AutoSize = true;
            _lblCompte.Location = new Point(0, 0);
            _lblCompte.Margin = new Padding(0, 0, 0, 3);
            _lblCompte.Name = "_lblCompte";
            _lblCompte.Size = new Size(73, 25);
            _lblCompte.TabIndex = 0;
            _lblCompte.Text = "Compte";
            //
            // _compte
            //
            _compte.DropDownStyle = ComboBoxStyle.DropDownList;
            _compte.Location = new Point(0, 28);
            _compte.Margin = new Padding(0);
            _compte.Name = "_compte";
            _compte.Size = new Size(283, 33);
            _compte.TabIndex = 1;
            //
            // _pType
            //
            _pType.AutoSize = true;
            _pType.Controls.Add(_lblType);
            _pType.Controls.Add(_type);
            _pType.FlowDirection = FlowDirection.TopDown;
            _pType.Location = new Point(687, 9);
            _pType.Margin = new Padding(0, 0, 21, 12);
            _pType.Name = "_pType";
            _pType.Size = new Size(163, 61);
            _pType.TabIndex = 3;
            _pType.WrapContents = false;
            //
            // _lblType
            //
            _lblType.AutoSize = true;
            _lblType.Location = new Point(0, 0);
            _lblType.Margin = new Padding(0, 0, 0, 3);
            _lblType.Name = "_lblType";
            _lblType.Size = new Size(49, 25);
            _lblType.TabIndex = 0;
            _lblType.Text = "Type";
            //
            // _type
            //
            _type.DropDownStyle = ComboBoxStyle.DropDownList;
            _type.Items.AddRange(new object[] { "Tous", "Recettes", "Dépenses" });
            _type.Location = new Point(0, 28);
            _type.Margin = new Padding(0);
            _type.Name = "_type";
            _type.Size = new Size(163, 33);
            _type.TabIndex = 1;
            //
            // _pStatut
            //
            _pStatut.AutoSize = true;
            _pStatut.Controls.Add(_lblStatut);
            _pStatut.Controls.Add(_statut);
            _pStatut.FlowDirection = FlowDirection.TopDown;
            _pStatut.Location = new Point(871, 9);
            _pStatut.Margin = new Padding(0, 0, 21, 12);
            _pStatut.Name = "_pStatut";
            _pStatut.Size = new Size(178, 61);
            _pStatut.TabIndex = 4;
            _pStatut.WrapContents = false;
            //
            // _lblStatut
            //
            _lblStatut.AutoSize = true;
            _lblStatut.Location = new Point(0, 0);
            _lblStatut.Margin = new Padding(0, 0, 0, 3);
            _lblStatut.Name = "_lblStatut";
            _lblStatut.Size = new Size(59, 25);
            _lblStatut.TabIndex = 0;
            _lblStatut.Text = "Statut";
            //
            // _statut
            //
            _statut.DropDownStyle = ComboBoxStyle.DropDownList;
            _statut.Items.AddRange(new object[] { "Tous", "Brouillons", "Validés", "Annulés" });
            _statut.Location = new Point(0, 28);
            _statut.Margin = new Padding(0);
            _statut.Name = "_statut";
            _statut.Size = new Size(178, 33);
            _statut.TabIndex = 1;
            //
            // _pRecherche
            //
            _pRecherche.AutoSize = true;
            _pRecherche.Controls.Add(_lblRecherche);
            _pRecherche.Controls.Add(_recherche);
            _pRecherche.FlowDirection = FlowDirection.TopDown;
            _pRecherche.Location = new Point(15, 82);
            _pRecherche.Margin = new Padding(0, 0, 21, 12);
            _pRecherche.Name = "_pRecherche";
            _pRecherche.Size = new Size(375, 59);
            _pRecherche.TabIndex = 5;
            _pRecherche.WrapContents = false;
            //
            // _lblRecherche
            //
            _lblRecherche.AutoSize = true;
            _lblRecherche.Location = new Point(0, 0);
            _lblRecherche.Margin = new Padding(0, 0, 0, 3);
            _lblRecherche.Name = "_lblRecherche";
            _lblRecherche.Size = new Size(102, 25);
            _lblRecherche.TabIndex = 0;
            _lblRecherche.Text = "Rechercher";
            //
            // _recherche
            //
            _recherche.Location = new Point(0, 28);
            _recherche.Margin = new Padding(0);
            _recherche.Name = "_recherche";
            _recherche.PlaceholderText = "Libellé, n° de pièce, tiers, montant…";
            _recherche.Size = new Size(375, 31);
            _recherche.TabIndex = 1;
            //
            // _pBoutons
            //
            _pBoutons.AutoSize = true;
            _pBoutons.Controls.Add(_btnFiltrer);
            _pBoutons.Controls.Add(_btnEffacer);
            _pBoutons.Location = new Point(411, 82);
            _pBoutons.Margin = new Padding(0, 22, 0, 12);
            _pBoutons.Name = "_pBoutons";
            _pBoutons.Size = new Size(345, 45);
            _pBoutons.TabIndex = 6;
            _pBoutons.WrapContents = false;
            //
            // _btnFiltrer
            //
            _btnFiltrer.BackColor = Color.FromArgb(0, 103, 192);
            _btnFiltrer.Cursor = Cursors.Hand;
            _btnFiltrer.FlatAppearance.BorderColor = Color.FromArgb(0, 103, 192);
            _btnFiltrer.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 74, 140);
            _btnFiltrer.FlatStyle = FlatStyle.Flat;
            _btnFiltrer.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnFiltrer.ForeColor = Color.White;
            _btnFiltrer.Location = new Point(0, 0);
            _btnFiltrer.Margin = new Padding(0, 0, 12, 0);
            _btnFiltrer.Name = "_btnFiltrer";
            _btnFiltrer.Size = new Size(135, 45);
            _btnFiltrer.TabIndex = 0;
            _btnFiltrer.Text = "Filtrer";
            _btnFiltrer.UseVisualStyleBackColor = false;
            //
            // _btnEffacer
            //
            _btnEffacer.BackColor = Color.FromArgb(253, 253, 253);
            _btnEffacer.Cursor = Cursors.Hand;
            _btnEffacer.FlatAppearance.BorderColor = Color.FromArgb(168, 168, 168);
            _btnEffacer.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 240, 250);
            _btnEffacer.FlatStyle = FlatStyle.Flat;
            _btnEffacer.Location = new Point(147, 0);
            _btnEffacer.Margin = new Padding(0);
            _btnEffacer.Name = "_btnEffacer";
            _btnEffacer.Size = new Size(198, 45);
            _btnEffacer.TabIndex = 1;
            _btnEffacer.Text = "Effacer les filtres";
            _btnEffacer.UseVisualStyleBackColor = false;
            //
            // _marge
            //
            _marge.Dock = DockStyle.Top;
            _marge.Location = new Point(15, 308);
            _marge.Margin = new Padding(4);
            _marge.Name = "_marge";
            _marge.Size = new Size(1208, 12);
            _marge.TabIndex = 2;
            //
            // _grille
            //
            _grille.BackgroundColor = Color.White;
            _grille.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _grille.Dock = DockStyle.Fill;
            _grille.Location = new Point(15, 320);
            _grille.Margin = new Padding(4);
            _grille.Name = "_grille";
            _grille.RowHeadersVisible = false;
            _grille.RowHeadersWidth = 62;
            _grille.Size = new Size(1208, 197);
            _grille.TabIndex = 0;
            //
            // _totaux
            //
            _totaux.Dock = DockStyle.Bottom;
            _totaux.Location = new Point(15, 517);
            _totaux.Margin = new Padding(4, 0, 4, 0);
            _totaux.Name = "_totaux";
            _totaux.Size = new Size(1208, 48);
            _totaux.TabIndex = 1;
            _totaux.Text = "0 mouvement(s)      Recettes : + 0      Dépenses : − 0      Solde de la période : 0 FCFA";
            _totaux.TextAlign = ContentAlignment.MiddleRight;
            //
            // MouvementsVue
            //
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(_grille);
            Controls.Add(_totaux);
            Controls.Add(_marge);
            Controls.Add(_filtres);
            Controls.Add(_entete);
            Controls.Add(_info);
            Margin = new Padding(4);
            Name = "MouvementsVue";
            Padding = new Padding(15);
            Size = new Size(1238, 580);
            _filtres.ResumeLayout(false);
            _filtres.PerformLayout();
            _pDu.ResumeLayout(false);
            _pDu.PerformLayout();
            _pAu.ResumeLayout(false);
            _pAu.PerformLayout();
            _pCompte.ResumeLayout(false);
            _pCompte.PerformLayout();
            _pType.ResumeLayout(false);
            _pType.PerformLayout();
            _pStatut.ResumeLayout(false);
            _pStatut.PerformLayout();
            _pRecherche.ResumeLayout(false);
            _pRecherche.PerformLayout();
            _pBoutons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_grille).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _info;
        private System.Windows.Forms.Label _entete;
        private System.Windows.Forms.FlowLayoutPanel _filtres;
        private System.Windows.Forms.FlowLayoutPanel _pDu;
        private System.Windows.Forms.Label _lblDu;
        private System.Windows.Forms.DateTimePicker _du;
        private System.Windows.Forms.FlowLayoutPanel _pAu;
        private System.Windows.Forms.Label _lblAu;
        private System.Windows.Forms.DateTimePicker _au;
        private System.Windows.Forms.FlowLayoutPanel _pCompte;
        private System.Windows.Forms.Label _lblCompte;
        private System.Windows.Forms.ComboBox _compte;
        private System.Windows.Forms.FlowLayoutPanel _pType;
        private System.Windows.Forms.Label _lblType;
        private System.Windows.Forms.ComboBox _type;
        private System.Windows.Forms.FlowLayoutPanel _pStatut;
        private System.Windows.Forms.Label _lblStatut;
        private System.Windows.Forms.ComboBox _statut;
        private System.Windows.Forms.FlowLayoutPanel _pRecherche;
        private System.Windows.Forms.Label _lblRecherche;
        private System.Windows.Forms.TextBox _recherche;
        private System.Windows.Forms.FlowLayoutPanel _pBoutons;
        private System.Windows.Forms.Button _btnFiltrer;
        private System.Windows.Forms.Button _btnEffacer;
        private System.Windows.Forms.Panel _marge;
        private System.Windows.Forms.DataGridView _grille;
        private System.Windows.Forms.Label _totaux;
    }
}
