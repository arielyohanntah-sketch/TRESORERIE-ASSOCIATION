// =====================================================================================
//  Views/UtilisateursVue.Designer.cs — MISE EN PAGE DE UtilisateursVue (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Titre, grille des utilisateurs et texte d'aide en bas.
//
//  ➜ Pour modifier l'apparence : clic droit sur UtilisateursVue.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans UtilisateursVue.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans UtilisateursVue.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Views
{
    partial class UtilisateursVue
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
            _aide = new Label();
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
            _entete.TabIndex = 2;
            _entete.Text = "Utilisateurs de l'application";
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
            _grille.Size = new Size(1208, 494);
            _grille.TabIndex = 0;
            // 
            // _aide
            // 
            _aide.Dock = DockStyle.Bottom;
            _aide.ForeColor = Color.FromArgb(74, 74, 74);
            _aide.Location = new Point(15, 566);
            _aide.Margin = new Padding(4, 0, 4, 0);
            _aide.Name = "_aide";
            _aide.Size = new Size(1208, 39);
            _aide.TabIndex = 1;
            _aide.Text = "Un compte est bloqué après 5 tentatives de connexion échouées : sélectionnez-le puis « Débloquer ».";
            _aide.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // UtilisateursVue
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(_grille);
            Controls.Add(_aide);
            Controls.Add(_entete);
            Margin = new Padding(4);
            Name = "UtilisateursVue";
            Padding = new Padding(15);
            Size = new Size(1238, 620);
            ((System.ComponentModel.ISupportInitialize)_grille).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _entete;
        private System.Windows.Forms.DataGridView _grille;
        private System.Windows.Forms.Label _aide;
    }
}
