// =====================================================================================
//  Views/AuditVue.Designer.cs — MISE EN PAGE DE AuditVue (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Titre _entete et grille _grille du journal d'audit.
//
//  ➜ Pour modifier l'apparence : clic droit sur AuditVue.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans AuditVue.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans AuditVue.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Views
{
    partial class AuditVue
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
            _grille = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)_grille).BeginInit();
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
            _entete.Text = "Journal d'audit (1000 dernières opérations)";
            _entete.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _grille
            // 
            _grille.BackgroundColor = Color.White;
            _grille.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _grille.Dock = DockStyle.Fill;
            _grille.Location = new Point(15, 72);
            _grille.Margin = new Padding(4);
            _grille.Name = "_grille";
            _grille.RowHeadersVisible = false;
            _grille.RowHeadersWidth = 62;
            _grille.Size = new Size(1208, 573);
            _grille.TabIndex = 0;
            // 
            // AuditVue
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(_grille);
            Controls.Add(_entete);
            Margin = new Padding(4);
            Name = "AuditVue";
            Padding = new Padding(15);
            Size = new Size(1238, 660);
            ((System.ComponentModel.ISupportInitialize)_grille).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _entete;
        private System.Windows.Forms.DataGridView _grille;
    }
}
