// =====================================================================================
//  Views/BudgetVue.Designer.cs — MISE EN PAGE DE BudgetVue (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Cadre des paramètres du rapport (_parametres) puis « feuille » blanche du rapport (_feuille).
//
//  ➜ Pour modifier l'apparence : clic droit sur BudgetVue.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans BudgetVue.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans BudgetVue.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Views
{
    partial class BudgetVue
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
            _parametres = new GroupBox();
            _lblExercice = new Label();
            _exercice = new ComboBox();
            _lblAu = new Label();
            _au = new DateTimePicker();
            _brouillons = new CheckBox();
            _btnGenerer = new Button();
            _marge = new Panel();
            _feuille = new Panel();
            _grille = new DataGridView();
            _alerte = new Label();
            _sousTitre = new Label();
            _titreRapport = new Label();
            _parametres.SuspendLayout();
            _feuille.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grille).BeginInit();
            SuspendLayout();
            // 
            // _parametres
            // 
            _parametres.Controls.Add(_lblExercice);
            _parametres.Controls.Add(_exercice);
            _parametres.Controls.Add(_lblAu);
            _parametres.Controls.Add(_au);
            _parametres.Controls.Add(_brouillons);
            _parametres.Controls.Add(_btnGenerer);
            _parametres.Dock = DockStyle.Top;
            _parametres.Location = new Point(15, 15);
            _parametres.Margin = new Padding(4);
            _parametres.Name = "_parametres";
            _parametres.Padding = new Padding(4);
            _parametres.Size = new Size(1208, 117);
            _parametres.TabIndex = 2;
            _parametres.TabStop = false;
            _parametres.Text = "Paramètres du rapport";
            // 
            // _lblExercice
            // 
            _lblExercice.AutoSize = true;
            _lblExercice.Location = new Point(18, 30);
            _lblExercice.Margin = new Padding(4, 0, 4, 0);
            _lblExercice.Name = "_lblExercice";
            _lblExercice.Size = new Size(73, 25);
            _lblExercice.TabIndex = 0;
            _lblExercice.Text = "Exercice";
            // 
            // _exercice
            // 
            _exercice.DropDownStyle = ComboBoxStyle.DropDownList;
            _exercice.Location = new Point(18, 60);
            _exercice.Margin = new Padding(4);
            _exercice.Name = "_exercice";
            _exercice.Size = new Size(253, 33);
            _exercice.TabIndex = 0;
            // 
            // _lblAu
            // 
            _lblAu.AutoSize = true;
            _lblAu.Location = new Point(294, 30);
            _lblAu.Margin = new Padding(4, 0, 4, 0);
            _lblAu.Name = "_lblAu";
            _lblAu.Size = new Size(84, 25);
            _lblAu.TabIndex = 1;
            _lblAu.Text = "Arrêté au";
            // 
            // _au
            // 
            _au.Format = DateTimePickerFormat.Short;
            _au.Location = new Point(294, 60);
            _au.Margin = new Padding(4);
            _au.Name = "_au";
            _au.Size = new Size(163, 31);
            _au.TabIndex = 1;
            // 
            // _brouillons
            // 
            _brouillons.AutoSize = true;
            _brouillons.Location = new Point(486, 63);
            _brouillons.Margin = new Padding(4);
            _brouillons.Name = "_brouillons";
            _brouillons.Size = new Size(200, 29);
            _brouillons.TabIndex = 2;
            _brouillons.Text = "Inclure les brouillons";
            // 
            // _btnGenerer
            // 
            _btnGenerer.BackColor = Color.FromArgb(0, 103, 192);
            _btnGenerer.Cursor = Cursors.Hand;
            _btnGenerer.FlatAppearance.BorderColor = Color.FromArgb(0, 103, 192);
            _btnGenerer.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 74, 140);
            _btnGenerer.FlatStyle = FlatStyle.Flat;
            _btnGenerer.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnGenerer.ForeColor = Color.White;
            _btnGenerer.Location = new Point(732, 54);
            _btnGenerer.Margin = new Padding(4);
            _btnGenerer.Name = "_btnGenerer";
            _btnGenerer.Size = new Size(150, 45);
            _btnGenerer.TabIndex = 3;
            _btnGenerer.Text = "Générer";
            _btnGenerer.UseVisualStyleBackColor = false;
            // 
            // _marge
            // 
            _marge.Dock = DockStyle.Top;
            _marge.Location = new Point(15, 132);
            _marge.Margin = new Padding(4);
            _marge.Name = "_marge";
            _marge.Size = new Size(1208, 12);
            _marge.TabIndex = 1;
            // 
            // _feuille
            // 
            _feuille.BackColor = Color.White;
            _feuille.Controls.Add(_grille);
            _feuille.Controls.Add(_alerte);
            _feuille.Controls.Add(_sousTitre);
            _feuille.Controls.Add(_titreRapport);
            _feuille.Dock = DockStyle.Fill;
            _feuille.Location = new Point(15, 144);
            _feuille.Margin = new Padding(4);
            _feuille.Name = "_feuille";
            _feuille.Padding = new Padding(27, 18, 27, 18);
            _feuille.Size = new Size(1208, 501);
            _feuille.TabIndex = 0;
            // 
            // _grille
            // 
            _grille.BackgroundColor = Color.White;
            _grille.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _grille.Dock = DockStyle.Fill;
            _grille.Location = new Point(27, 96);
            _grille.Margin = new Padding(4);
            _grille.Name = "_grille";
            _grille.RowHeadersVisible = false;
            _grille.RowHeadersWidth = 62;
            _grille.Size = new Size(1154, 337);
            _grille.TabIndex = 0;
            // 
            // _alerte
            // 
            _alerte.BackColor = Color.FromArgb(253, 231, 233);
            _alerte.BorderStyle = BorderStyle.FixedSingle;
            _alerte.Dock = DockStyle.Bottom;
            _alerte.ForeColor = Color.FromArgb(142, 27, 36);
            _alerte.Location = new Point(27, 433);
            _alerte.Margin = new Padding(4, 0, 4, 0);
            _alerte.Name = "_alerte";
            _alerte.Padding = new Padding(15, 0, 15, 0);
            _alerte.Size = new Size(1154, 50);
            _alerte.TabIndex = 1;
            _alerte.Text = "Alerte de dépassement de budget (affichée seulement en cas de dépassement)";
            _alerte.TextAlign = ContentAlignment.MiddleLeft;
            _alerte.Visible = false;
            // 
            // _sousTitre
            // 
            _sousTitre.Dock = DockStyle.Top;
            _sousTitre.ForeColor = Color.FromArgb(74, 74, 74);
            _sousTitre.Location = new Point(27, 63);
            _sousTitre.Margin = new Padding(4, 0, 4, 0);
            _sousTitre.Name = "_sousTitre";
            _sousTitre.Size = new Size(1154, 33);
            _sousTitre.TabIndex = 2;
            _sousTitre.Text = "Exercice · arrêté au · mouvements validés uniquement · montants en FCFA";
            // 
            // _titreRapport
            // 
            _titreRapport.Dock = DockStyle.Top;
            _titreRapport.Font = new Font("Segoe UI Semibold", 11F);
            _titreRapport.ForeColor = Color.FromArgb(11, 61, 102);
            _titreRapport.Location = new Point(27, 18);
            _titreRapport.Margin = new Padding(4, 0, 4, 0);
            _titreRapport.Name = "_titreRapport";
            _titreRapport.Size = new Size(1154, 45);
            _titreRapport.TabIndex = 3;
            _titreRapport.Text = "Budget prévisionnel et réalisé par centre analytique";
            // 
            // BudgetVue
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(_feuille);
            Controls.Add(_marge);
            Controls.Add(_parametres);
            Margin = new Padding(4);
            Name = "BudgetVue";
            Padding = new Padding(15);
            Size = new Size(1238, 660);
            _parametres.ResumeLayout(false);
            _parametres.PerformLayout();
            _feuille.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_grille).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox _parametres;
        private System.Windows.Forms.Label _lblExercice;
        private System.Windows.Forms.ComboBox _exercice;
        private System.Windows.Forms.Label _lblAu;
        private System.Windows.Forms.DateTimePicker _au;
        private System.Windows.Forms.CheckBox _brouillons;
        private System.Windows.Forms.Button _btnGenerer;
        private System.Windows.Forms.Panel _marge;
        private System.Windows.Forms.Panel _feuille;
        private System.Windows.Forms.DataGridView _grille;
        private System.Windows.Forms.Label _alerte;
        private System.Windows.Forms.Label _sousTitre;
        private System.Windows.Forms.Label _titreRapport;
    }
}
