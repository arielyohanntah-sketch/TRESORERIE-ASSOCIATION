// =====================================================================================
//  Forms/MotifForm.Designer.cs — MISE EN PAGE DE MotifForm (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Zone de texte du motif d'annulation et bouton Confirmer.
//
//  ➜ Pour modifier l'apparence : clic droit sur MotifForm.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans MotifForm.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans MotifForm.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Forms
{
    partial class MotifForm
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

        #region Code généré par le Concepteur Windows Form

        private void InitializeComponent()
        {
            this._lblMotif = new System.Windows.Forms.Label();
            this._motif = new System.Windows.Forms.TextBox();
            this._btnFermer = new System.Windows.Forms.Button();
            this._btnConfirmer = new System.Windows.Forms.Button();
            this.Entete.SuspendLayout();
            this.Corps.SuspendLayout();
            this.Pied.SuspendLayout();
            this.SuspendLayout();
            //
            // LibelleTitre
            //
            this.LibelleTitre.Text = "Annuler la pièce";
            //
            // LibelleSousTitre
            //
            this.LibelleSousTitre.Text = "Le motif est obligatoire";
            //
            // Corps
            //
            this.Corps.Controls.Add(this._lblMotif);
            this.Corps.Controls.Add(this._motif);
            //
            // _lblMotif
            //
            this._lblMotif.AutoSize = true;
            this._lblMotif.Location = new System.Drawing.Point(24, 12);
            this._lblMotif.Name = "_lblMotif";
            this._lblMotif.Text = "Motif de l'annulation *";
            //
            // _motif
            //
            this._motif.Location = new System.Drawing.Point(24, 34);
            this._motif.MaxLength = 255;
            this._motif.Multiline = true;
            this._motif.Name = "_motif";
            this._motif.Size = new System.Drawing.Size(432, 96);
            this._motif.TabIndex = 0;
            //
            // Pied
            //
            this.Pied.Controls.Add(this._btnFermer);
            this.Pied.Controls.Add(this._btnConfirmer);
            //
            // _btnFermer
            //
            this._btnFermer.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this._btnFermer.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnFermer.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnFermer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 168, 168);
            this._btnFermer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(232, 240, 250);
            this._btnFermer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnFermer.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnFermer.Name = "_btnFermer";
            this._btnFermer.Size = new System.Drawing.Size(120, 32);
            this._btnFermer.TabIndex = 2;
            this._btnFermer.Text = "Fermer";
            this._btnFermer.UseVisualStyleBackColor = false;
            //
            // _btnConfirmer
            //
            this._btnConfirmer.BackColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnConfirmer.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnConfirmer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnConfirmer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 74, 140);
            this._btnConfirmer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnConfirmer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnConfirmer.ForeColor = System.Drawing.Color.White;
            this._btnConfirmer.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnConfirmer.Name = "_btnConfirmer";
            this._btnConfirmer.Size = new System.Drawing.Size(170, 32);
            this._btnConfirmer.TabIndex = 1;
            this._btnConfirmer.Text = "Confirmer l'annulation";
            this._btnConfirmer.UseVisualStyleBackColor = false;
            //
            // MotifForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this._btnFermer;
            this.ClientSize = new System.Drawing.Size(480, 270);
            this.Name = "MotifForm";
            this.Text = "Annuler la pièce";
            this.Entete.ResumeLayout(false);
            this.Entete.PerformLayout();
            this.Corps.ResumeLayout(false);
            this.Corps.PerformLayout();
            this.Pied.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _lblMotif;
        private System.Windows.Forms.TextBox _motif;
        private System.Windows.Forms.Button _btnFermer;
        private System.Windows.Forms.Button _btnConfirmer;
    }
}
