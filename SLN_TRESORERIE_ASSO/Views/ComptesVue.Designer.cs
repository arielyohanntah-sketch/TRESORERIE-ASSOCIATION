// =====================================================================================
//  Views/ComptesVue.Designer.cs — MISE EN PAGE DE ComptesVue (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Grille des comptes en haut, total, puis cadre du relevé avec sa période et sa grille.
//
//  ➜ Pour modifier l'apparence : clic droit sur ComptesVue.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans ComptesVue.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans ComptesVue.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Views
{
    partial class ComptesVue
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
            _entete = new Label();
            _disposition = new TableLayoutPanel();
            _haut = new Panel();
            _comptes = new DataGridView();
            _total = new Label();
            _cadreReleve = new GroupBox();
            _releve = new DataGridView();
            _note = new Label();
            _filtresReleve = new Panel();
            _lblDu = new Label();
            _du = new DateTimePicker();
            _lblAu = new Label();
            _au = new DateTimePicker();
            _btnAfficher = new Button();
            _disposition.SuspendLayout();
            _haut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_comptes).BeginInit();
            _cadreReleve.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_releve).BeginInit();
            _filtresReleve.SuspendLayout();
            SuspendLayout();
            // 
            // _entete
            // 
            _entete.Dock = DockStyle.Top;
            _entete.Font = new Font("Segoe UI Semibold", 15F);
            _entete.ForeColor = Color.FromArgb(11, 61, 102);
            _entete.Location = new Point(15, 15);
            _entete.Margin = new Padding(4, 0, 4, 0);
            _entete.Name = "_entete";
            _entete.Size = new Size(1208, 57);
            _entete.TabIndex = 1;
            _entete.Text = "Comptes et soldes";
            _entete.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _disposition
            // 
            _disposition.ColumnCount = 1;
            _disposition.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _disposition.Controls.Add(_haut, 0, 0);
            _disposition.Controls.Add(_cadreReleve, 0, 1);
            _disposition.Dock = DockStyle.Fill;
            _disposition.Location = new Point(15, 72);
            _disposition.Margin = new Padding(4);
            _disposition.Name = "_disposition";
            _disposition.RowCount = 2;
            _disposition.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            _disposition.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            _disposition.Size = new Size(1208, 533);
            _disposition.TabIndex = 0;
            // 
            // _haut
            // 
            _haut.Controls.Add(_comptes);
            _haut.Controls.Add(_total);
            _haut.Dock = DockStyle.Fill;
            _haut.Location = new Point(0, 0);
            _haut.Margin = new Padding(0, 0, 0, 12);
            _haut.Name = "_haut";
            _haut.Size = new Size(1208, 201);
            _haut.TabIndex = 0;
            // 
            // _comptes
            // 
            _comptes.BackgroundColor = Color.White;
            _comptes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _comptes.Dock = DockStyle.Fill;
            _comptes.Location = new Point(0, 0);
            _comptes.Margin = new Padding(4);
            _comptes.Name = "_comptes";
            _comptes.RowHeadersVisible = false;
            _comptes.RowHeadersWidth = 62;
            _comptes.Size = new Size(1208, 159);
            _comptes.TabIndex = 0;
            // 
            // _total
            // 
            _total.Dock = DockStyle.Bottom;
            _total.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _total.Location = new Point(0, 159);
            _total.Margin = new Padding(4, 0, 4, 0);
            _total.Name = "_total";
            _total.Size = new Size(1208, 42);
            _total.TabIndex = 1;
            _total.Text = "Trésorerie totale (comptes actifs) : 0 FCFA";
            _total.TextAlign = ContentAlignment.MiddleRight;
            // 
            // _cadreReleve
            // 
            _cadreReleve.Controls.Add(_releve);
            _cadreReleve.Controls.Add(_note);
            _cadreReleve.Controls.Add(_filtresReleve);
            _cadreReleve.Dock = DockStyle.Fill;
            _cadreReleve.Location = new Point(0, 213);
            _cadreReleve.Margin = new Padding(0);
            _cadreReleve.Name = "_cadreReleve";
            _cadreReleve.Padding = new Padding(15, 9, 15, 12);
            _cadreReleve.Size = new Size(1208, 320);
            _cadreReleve.TabIndex = 1;
            _cadreReleve.TabStop = false;
            _cadreReleve.Text = "Relevé";
            // 
            // _releve
            // 
            _releve.BackgroundColor = Color.White;
            _releve.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _releve.Dock = DockStyle.Fill;
            _releve.Location = new Point(15, 114);
            _releve.Margin = new Padding(4);
            _releve.Name = "_releve";
            _releve.RowHeadersVisible = false;
            _releve.RowHeadersWidth = 62;
            _releve.Size = new Size(1178, 158);
            _releve.TabIndex = 0;
            // 
            // _note
            // 
            _note.Dock = DockStyle.Bottom;
            _note.ForeColor = Color.FromArgb(74, 74, 74);
            _note.Location = new Point(15, 272);
            _note.Margin = new Padding(4, 0, 4, 0);
            _note.Name = "_note";
            _note.Size = new Size(1178, 36);
            _note.TabIndex = 1;
            _note.Text = "Seuls les mouvements et virements validés apparaissent dans le relevé.";
            _note.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _filtresReleve
            // 
            _filtresReleve.Controls.Add(_lblDu);
            _filtresReleve.Controls.Add(_du);
            _filtresReleve.Controls.Add(_lblAu);
            _filtresReleve.Controls.Add(_au);
            _filtresReleve.Controls.Add(_btnAfficher);
            _filtresReleve.Dock = DockStyle.Top;
            _filtresReleve.Location = new Point(15, 33);
            _filtresReleve.Margin = new Padding(4);
            _filtresReleve.Name = "_filtresReleve";
            _filtresReleve.Size = new Size(1178, 81);
            _filtresReleve.TabIndex = 2;
            // 
            // _lblDu
            // 
            _lblDu.AutoSize = true;
            _lblDu.Location = new Point(3, 6);
            _lblDu.Margin = new Padding(4, 0, 4, 0);
            _lblDu.Name = "_lblDu";
            _lblDu.Size = new Size(35, 25);
            _lblDu.TabIndex = 0;
            _lblDu.Text = "Du";
            // 
            // _du
            // 
            _du.Format = DateTimePickerFormat.Short;
            _du.Location = new Point(3, 36);
            _du.Margin = new Padding(4);
            _du.Name = "_du";
            _du.Size = new Size(163, 31);
            _du.TabIndex = 0;
            // 
            // _lblAu
            // 
            _lblAu.AutoSize = true;
            _lblAu.Location = new Point(189, 6);
            _lblAu.Margin = new Padding(4, 0, 4, 0);
            _lblAu.Name = "_lblAu";
            _lblAu.Size = new Size(34, 25);
            _lblAu.TabIndex = 1;
            _lblAu.Text = "Au";
            // 
            // _au
            // 
            _au.Format = DateTimePickerFormat.Short;
            _au.Location = new Point(189, 36);
            _au.Margin = new Padding(4);
            _au.Name = "_au";
            _au.Size = new Size(163, 31);
            _au.TabIndex = 1;
            // 
            // _btnAfficher
            // 
            _btnAfficher.BackColor = Color.FromArgb(253, 253, 253);
            _btnAfficher.Cursor = Cursors.Hand;
            _btnAfficher.FlatAppearance.BorderColor = Color.FromArgb(168, 168, 168);
            _btnAfficher.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 240, 250);
            _btnAfficher.FlatStyle = FlatStyle.Flat;
            _btnAfficher.Location = new Point(378, 30);
            _btnAfficher.Margin = new Padding(4);
            _btnAfficher.Name = "_btnAfficher";
            _btnAfficher.Size = new Size(135, 45);
            _btnAfficher.TabIndex = 2;
            _btnAfficher.Text = "Afficher";
            _btnAfficher.UseVisualStyleBackColor = false;
            // 
            // ComptesVue
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(_disposition);
            Controls.Add(_entete);
            Margin = new Padding(4);
            Name = "ComptesVue";
            Padding = new Padding(15);
            Size = new Size(1238, 620);
            _disposition.ResumeLayout(false);
            _haut.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_comptes).EndInit();
            _cadreReleve.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_releve).EndInit();
            _filtresReleve.ResumeLayout(false);
            _filtresReleve.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _entete;
        private System.Windows.Forms.TableLayoutPanel _disposition;
        private System.Windows.Forms.Panel _haut;
        private System.Windows.Forms.DataGridView _comptes;
        private System.Windows.Forms.Label _total;
        private System.Windows.Forms.GroupBox _cadreReleve;
        private System.Windows.Forms.DataGridView _releve;
        private System.Windows.Forms.Label _note;
        private System.Windows.Forms.Panel _filtresReleve;
        private System.Windows.Forms.Label _lblDu;
        private System.Windows.Forms.DateTimePicker _du;
        private System.Windows.Forms.Label _lblAu;
        private System.Windows.Forms.DateTimePicker _au;
        private System.Windows.Forms.Button _btnAfficher;
    }
}
