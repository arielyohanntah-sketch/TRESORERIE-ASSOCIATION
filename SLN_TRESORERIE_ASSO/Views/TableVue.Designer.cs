// =====================================================================================
//  Views/TableVue.Designer.cs — MISE EN PAGE DE TableVue (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Titre, texte d'aide et grille éditable (ses colonnes sont créées à l'exécution selon la table).
//
//  ➜ Pour modifier l'apparence : clic droit sur TableVue.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans TableVue.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans TableVue.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Views
{
    partial class TableVue
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
            this._entete = new System.Windows.Forms.Label();
            this._grille = new System.Windows.Forms.DataGridView();
            this._aide = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._grille)).BeginInit();
            this.SuspendLayout();
            //
            // _entete
            //
            this._entete.Dock = System.Windows.Forms.DockStyle.Top;
            this._entete.Font = new System.Drawing.Font("Segoe UI Semibold", 15F);
            this._entete.ForeColor = System.Drawing.Color.FromArgb(11, 61, 102);
            this._entete.Name = "_entete";
            this._entete.Size = new System.Drawing.Size(980, 38);
            this._entete.TabIndex = 2;
            this._entete.Text = "Titre de la table";
            this._entete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // _grille
            //
            this._grille.BackgroundColor = System.Drawing.Color.White;
            this._grille.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grille.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grille.Name = "_grille";
            this._grille.RowHeadersVisible = false;
            this._grille.TabIndex = 0;
            //
            // _aide
            //
            this._aide.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._aide.ForeColor = System.Drawing.Color.FromArgb(74, 74, 74);
            this._aide.Name = "_aide";
            this._aide.Size = new System.Drawing.Size(980, 26);
            this._aide.TabIndex = 1;
            this._aide.Text = "Modifiez directement dans la grille, ajoutez une ligne sur la dernière ligne vide, puis cliquez sur « Enregistrer ».";
            this._aide.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // TableVue
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._grille);
            this.Controls.Add(this._aide);
            this.Controls.Add(this._entete);
            this.Name = "TableVue";
            this.Size = new System.Drawing.Size(1000, 640);
            ((System.ComponentModel.ISupportInitialize)(this._grille)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _entete;
        private System.Windows.Forms.DataGridView _grille;
        private System.Windows.Forms.Label _aide;
    }
}
