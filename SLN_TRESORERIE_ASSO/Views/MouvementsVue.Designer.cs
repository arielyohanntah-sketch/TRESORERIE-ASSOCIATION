// =====================================================================================
//  Views/MouvementsVue.Designer.cs — MISE EN PAGE DE MouvementsVue (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Titre, bandeau d'information, barre de filtres, grille des opérations et ligne des totaux.
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
            _filtres = new GroupBox();
            _lblDu = new Label();
            _du = new DateTimePicker();
            _lblAu = new Label();
            _au = new DateTimePicker();
            _lblCompte = new Label();
            _compte = new ComboBox();
            _lblType = new Label();
            _type = new ComboBox();
            _lblStatut = new Label();
            _statut = new ComboBox();
            _lblRecherche = new Label();
            _recherche = new TextBox();
            _btnFiltrer = new Button();
            _marge = new Panel();
            _grille = new DataGridView();
            _totaux = new Label();
            _filtres.SuspendLayout();
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
            _filtres.Controls.Add(_lblDu);
            _filtres.Controls.Add(_du);
            _filtres.Controls.Add(_lblAu);
            _filtres.Controls.Add(_au);
            _filtres.Controls.Add(_lblCompte);
            _filtres.Controls.Add(_compte);
            _filtres.Controls.Add(_lblType);
            _filtres.Controls.Add(_type);
            _filtres.Controls.Add(_lblStatut);
            _filtres.Controls.Add(_statut);
            _filtres.Controls.Add(_lblRecherche);
            _filtres.Controls.Add(_recherche);
            _filtres.Controls.Add(_btnFiltrer);
            _filtres.Dock = DockStyle.Top;
            _filtres.Location = new Point(15, 122);
            _filtres.Margin = new Padding(4);
            _filtres.Name = "_filtres";
            _filtres.Padding = new Padding(4);
            _filtres.Size = new Size(1208, 117);
            _filtres.TabIndex = 3;
            _filtres.TabStop = false;
            _filtres.Text = "Filtres";
            // 
            // _lblDu
            // 
            _lblDu.AutoSize = true;
            _lblDu.Location = new Point(18, 30);
            _lblDu.Margin = new Padding(4, 0, 4, 0);
            _lblDu.Name = "_lblDu";
            _lblDu.Size = new Size(35, 25);
            _lblDu.TabIndex = 0;
            _lblDu.Text = "Du";
            // 
            // _du
            // 
            _du.Format = DateTimePickerFormat.Short;
            _du.Location = new Point(18, 60);
            _du.Margin = new Padding(4);
            _du.Name = "_du";
            _du.Size = new Size(163, 31);
            _du.TabIndex = 0;
            // 
            // _lblAu
            // 
            _lblAu.AutoSize = true;
            _lblAu.Location = new Point(201, 30);
            _lblAu.Margin = new Padding(4, 0, 4, 0);
            _lblAu.Name = "_lblAu";
            _lblAu.Size = new Size(34, 25);
            _lblAu.TabIndex = 1;
            _lblAu.Text = "Au";
            // 
            // _au
            // 
            _au.Format = DateTimePickerFormat.Short;
            _au.Location = new Point(201, 60);
            _au.Margin = new Padding(4);
            _au.Name = "_au";
            _au.Size = new Size(163, 31);
            _au.TabIndex = 1;
            // 
            // _lblCompte
            // 
            _lblCompte.AutoSize = true;
            _lblCompte.Location = new Point(384, 30);
            _lblCompte.Margin = new Padding(4, 0, 4, 0);
            _lblCompte.Name = "_lblCompte";
            _lblCompte.Size = new Size(76, 25);
            _lblCompte.TabIndex = 2;
            _lblCompte.Text = "Compte";
            // 
            // _compte
            // 
            _compte.DropDownStyle = ComboBoxStyle.DropDownList;
            _compte.Location = new Point(384, 60);
            _compte.Margin = new Padding(4);
            _compte.Name = "_compte";
            _compte.Size = new Size(283, 33);
            _compte.TabIndex = 2;
            // 
            // _lblType
            // 
            _lblType.AutoSize = true;
            _lblType.Location = new Point(687, 30);
            _lblType.Margin = new Padding(4, 0, 4, 0);
            _lblType.Name = "_lblType";
            _lblType.Size = new Size(49, 25);
            _lblType.TabIndex = 3;
            _lblType.Text = "Type";
            // 
            // _type
            // 
            _type.DropDownStyle = ComboBoxStyle.DropDownList;
            _type.Items.AddRange(new object[] { "Tous", "Recettes", "Dépenses" });
            _type.Location = new Point(687, 60);
            _type.Margin = new Padding(4);
            _type.Name = "_type";
            _type.Size = new Size(163, 33);
            _type.TabIndex = 3;
            // 
            // _lblStatut
            // 
            _lblStatut.AutoSize = true;
            _lblStatut.Location = new Point(870, 30);
            _lblStatut.Margin = new Padding(4, 0, 4, 0);
            _lblStatut.Name = "_lblStatut";
            _lblStatut.Size = new Size(58, 25);
            _lblStatut.TabIndex = 4;
            _lblStatut.Text = "Statut";
            // 
            // _statut
            // 
            _statut.DropDownStyle = ComboBoxStyle.DropDownList;
            _statut.Items.AddRange(new object[] { "Tous", "Brouillons", "Validés", "Annulés" });
            _statut.Location = new Point(870, 60);
            _statut.Margin = new Padding(4);
            _statut.Name = "_statut";
            _statut.Size = new Size(178, 33);
            _statut.TabIndex = 4;
            // 
            // _lblRecherche
            // 
            _lblRecherche.AutoSize = true;
            _lblRecherche.Location = new Point(1068, 30);
            _lblRecherche.Margin = new Padding(4, 0, 4, 0);
            _lblRecherche.Name = "_lblRecherche";
            _lblRecherche.Size = new Size(97, 25);
            _lblRecherche.TabIndex = 5;
            _lblRecherche.Text = "Rechercher";
            // 
            // _recherche
            // 
            _recherche.Location = new Point(1068, 60);
            _recherche.Margin = new Padding(4);
            _recherche.Name = "_recherche";
            _recherche.PlaceholderText = "Libellé, n° de pièce, tiers…";
            _recherche.Size = new Size(328, 31);
            _recherche.TabIndex = 5;
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
            _btnFiltrer.Location = new Point(1416, 54);
            _btnFiltrer.Margin = new Padding(4);
            _btnFiltrer.Name = "_btnFiltrer";
            _btnFiltrer.Size = new Size(135, 45);
            _btnFiltrer.TabIndex = 6;
            _btnFiltrer.Text = "Filtrer";
            _btnFiltrer.UseVisualStyleBackColor = false;
            // 
            // _marge
            // 
            _marge.Dock = DockStyle.Top;
            _marge.Location = new Point(15, 239);
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
            _grille.Location = new Point(15, 251);
            _grille.Margin = new Padding(4);
            _grille.Name = "_grille";
            _grille.RowHeadersVisible = false;
            _grille.RowHeadersWidth = 62;
            _grille.Size = new Size(1208, 266);
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
            ((System.ComponentModel.ISupportInitialize)_grille).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _info;
        private System.Windows.Forms.Label _entete;
        private System.Windows.Forms.GroupBox _filtres;
        private System.Windows.Forms.Label _lblDu;
        private System.Windows.Forms.DateTimePicker _du;
        private System.Windows.Forms.Label _lblAu;
        private System.Windows.Forms.DateTimePicker _au;
        private System.Windows.Forms.Label _lblCompte;
        private System.Windows.Forms.ComboBox _compte;
        private System.Windows.Forms.Label _lblType;
        private System.Windows.Forms.ComboBox _type;
        private System.Windows.Forms.Label _lblStatut;
        private System.Windows.Forms.ComboBox _statut;
        private System.Windows.Forms.Label _lblRecherche;
        private System.Windows.Forms.TextBox _recherche;
        private System.Windows.Forms.Button _btnFiltrer;
        private System.Windows.Forms.Panel _marge;
        private System.Windows.Forms.DataGridView _grille;
        private System.Windows.Forms.Label _totaux;
    }
}
