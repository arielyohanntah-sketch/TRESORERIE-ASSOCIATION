// =====================================================================================
//  Views/TableauDeBordVue.Designer.cs — MISE EN PAGE DE TableauDeBordVue (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Bandeau de bienvenue, cartes des comptes, zone du milieu (tâches / graphique / indicateurs), dernières opérations.
//
//  ➜ Pour modifier l'apparence : clic droit sur TableauDeBordVue.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans TableauDeBordVue.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans TableauDeBordVue.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Views
{
    partial class TableauDeBordVue
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
            _bandeau = new SLN_TRESORERIE_ASSO.UI.BandeauBienvenue();
            _cartes = new FlowLayoutPanel();
            _milieu = new TableLayoutPanel();
            _cadreTaches = new Panel();
            _taches = new FlowLayoutPanel();
            _lblTaches = new Label();
            _cadreGraphique = new Panel();
            _graphique = new SLN_TRESORERIE_ASSO.UI.GraphiqueMois();
            _lblGraphique = new Label();
            _indicateurs = new FlowLayoutPanel();
            _lblDernieres = new Label();
            _grille = new DataGridView();
            _milieu.SuspendLayout();
            _cadreTaches.SuspendLayout();
            _cadreGraphique.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grille).BeginInit();
            SuspendLayout();
            // 
            // _bandeau
            // 
            _bandeau.Dock = DockStyle.Top;
            _bandeau.Location = new Point(15, 15);
            _bandeau.Margin = new Padding(4);
            _bandeau.Name = "_bandeau";
            _bandeau.Size = new Size(1208, 156);
            _bandeau.TabIndex = 4;
            // 
            // _cartes
            // 
            _cartes.AutoSize = true;
            _cartes.Dock = DockStyle.Top;
            _cartes.Location = new Point(15, 171);
            _cartes.Margin = new Padding(4);
            _cartes.MinimumSize = new Size(0, 60);
            _cartes.Name = "_cartes";
            _cartes.Padding = new Padding(0, 15, 0, 0);
            _cartes.Size = new Size(1208, 60);
            _cartes.TabIndex = 3;
            // 
            // _milieu
            // 
            _milieu.ColumnCount = 3;
            _milieu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37F));
            _milieu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 41F));
            _milieu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            _milieu.Controls.Add(_cadreTaches, 0, 0);
            _milieu.Controls.Add(_cadreGraphique, 1, 0);
            _milieu.Controls.Add(_indicateurs, 2, 0);
            _milieu.Dock = DockStyle.Top;
            _milieu.Location = new Point(15, 231);
            _milieu.Margin = new Padding(4);
            _milieu.Name = "_milieu";
            _milieu.Padding = new Padding(0, 6, 0, 0);
            _milieu.RowCount = 1;
            _milieu.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _milieu.Size = new Size(1208, 387);
            _milieu.TabIndex = 2;
            // 
            // _cadreTaches
            // 
            _cadreTaches.BackColor = Color.White;
            _cadreTaches.Controls.Add(_taches);
            _cadreTaches.Controls.Add(_lblTaches);
            _cadreTaches.Dock = DockStyle.Fill;
            _cadreTaches.Location = new Point(0, 6);
            _cadreTaches.Margin = new Padding(0, 0, 12, 0);
            _cadreTaches.Name = "_cadreTaches";
            _cadreTaches.Padding = new Padding(18, 9, 18, 12);
            _cadreTaches.Size = new Size(434, 381);
            _cadreTaches.TabIndex = 0;
            // 
            // _taches
            // 
            _taches.AutoScroll = true;
            _taches.BackColor = Color.White;
            _taches.Dock = DockStyle.Fill;
            _taches.FlowDirection = FlowDirection.TopDown;
            _taches.Location = new Point(18, 48);
            _taches.Margin = new Padding(4);
            _taches.Name = "_taches";
            _taches.Size = new Size(398, 321);
            _taches.TabIndex = 1;
            _taches.WrapContents = false;
            // 
            // _lblTaches
            // 
            _lblTaches.Dock = DockStyle.Top;
            _lblTaches.Font = new Font("Segoe UI Semibold", 11F);
            _lblTaches.ForeColor = Color.FromArgb(11, 61, 102);
            _lblTaches.Location = new Point(18, 9);
            _lblTaches.Margin = new Padding(4, 0, 4, 0);
            _lblTaches.Name = "_lblTaches";
            _lblTaches.Size = new Size(398, 39);
            _lblTaches.TabIndex = 0;
            _lblTaches.Text = "Mes tâches";
            // 
            // _cadreGraphique
            // 
            _cadreGraphique.BackColor = Color.White;
            _cadreGraphique.Controls.Add(_graphique);
            _cadreGraphique.Controls.Add(_lblGraphique);
            _cadreGraphique.Dock = DockStyle.Fill;
            _cadreGraphique.Location = new Point(446, 6);
            _cadreGraphique.Margin = new Padding(0, 0, 12, 0);
            _cadreGraphique.Name = "_cadreGraphique";
            _cadreGraphique.Padding = new Padding(18, 9, 18, 12);
            _cadreGraphique.Size = new Size(483, 381);
            _cadreGraphique.TabIndex = 1;
            // 
            // _graphique
            // 
            _graphique.BackColor = Color.White;
            _graphique.Dock = DockStyle.Fill;
            _graphique.Location = new Point(18, 48);
            _graphique.Margin = new Padding(4);
            _graphique.Name = "_graphique";
            _graphique.Size = new Size(447, 321);
            _graphique.TabIndex = 1;
            // 
            // _lblGraphique
            // 
            _lblGraphique.Dock = DockStyle.Top;
            _lblGraphique.Font = new Font("Segoe UI Semibold", 11F);
            _lblGraphique.ForeColor = Color.FromArgb(11, 61, 102);
            _lblGraphique.Location = new Point(18, 9);
            _lblGraphique.Margin = new Padding(4, 0, 4, 0);
            _lblGraphique.Name = "_lblGraphique";
            _lblGraphique.Size = new Size(447, 39);
            _lblGraphique.TabIndex = 0;
            _lblGraphique.Text = "Recettes et dépenses validées — 6 derniers mois";
            // 
            // _indicateurs
            // 
            _indicateurs.Dock = DockStyle.Fill;
            _indicateurs.FlowDirection = FlowDirection.TopDown;
            _indicateurs.Location = new Point(950, 6);
            _indicateurs.Margin = new Padding(9, 0, 0, 0);
            _indicateurs.Name = "_indicateurs";
            _indicateurs.Size = new Size(258, 381);
            _indicateurs.TabIndex = 2;
            _indicateurs.WrapContents = false;
            // 
            // _lblDernieres
            // 
            _lblDernieres.Dock = DockStyle.Top;
            _lblDernieres.Font = new Font("Segoe UI Semibold", 11F);
            _lblDernieres.ForeColor = Color.FromArgb(11, 61, 102);
            _lblDernieres.Location = new Point(15, 618);
            _lblDernieres.Margin = new Padding(4, 0, 4, 0);
            _lblDernieres.Name = "_lblDernieres";
            _lblDernieres.Size = new Size(1208, 45);
            _lblDernieres.TabIndex = 1;
            _lblDernieres.Text = "Dernières opérations";
            _lblDernieres.TextAlign = ContentAlignment.BottomLeft;
            // 
            // _grille
            // 
            _grille.BackgroundColor = Color.White;
            _grille.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _grille.Dock = DockStyle.Fill;
            _grille.Location = new Point(15, 663);
            _grille.Margin = new Padding(4);
            _grille.Name = "_grille";
            _grille.RowHeadersVisible = false;
            _grille.RowHeadersWidth = 62;
            _grille.Size = new Size(1208, 0);
            _grille.TabIndex = 0;
            // 
            // TableauDeBordVue
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(_grille);
            Controls.Add(_lblDernieres);
            Controls.Add(_milieu);
            Controls.Add(_cartes);
            Controls.Add(_bandeau);
            Margin = new Padding(4);
            Name = "TableauDeBordVue";
            Padding = new Padding(15);
            Size = new Size(1238, 620);
            _milieu.ResumeLayout(false);
            _cadreTaches.ResumeLayout(false);
            _cadreGraphique.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_grille).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SLN_TRESORERIE_ASSO.UI.BandeauBienvenue _bandeau;
        private System.Windows.Forms.FlowLayoutPanel _cartes;
        private System.Windows.Forms.TableLayoutPanel _milieu;
        private System.Windows.Forms.Panel _cadreTaches;
        private System.Windows.Forms.FlowLayoutPanel _taches;
        private System.Windows.Forms.Label _lblTaches;
        private System.Windows.Forms.Panel _cadreGraphique;
        private SLN_TRESORERIE_ASSO.UI.GraphiqueMois _graphique;
        private System.Windows.Forms.Label _lblGraphique;
        private System.Windows.Forms.FlowLayoutPanel _indicateurs;
        private System.Windows.Forms.Label _lblDernieres;
        private System.Windows.Forms.DataGridView _grille;
    }
}
