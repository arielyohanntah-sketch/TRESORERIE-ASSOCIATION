// =====================================================================================
//  Forms/MotDePasseForm.Designer.cs — MISE EN PAGE DE MotDePasseForm (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Ancien mot de passe, nouveau, confirmation et bouton Enregistrer.
//
//  ➜ Pour modifier l'apparence : clic droit sur MotDePasseForm.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans MotDePasseForm.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans MotDePasseForm.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Forms
{
    partial class MotDePasseForm
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
            this._lblAncien = new System.Windows.Forms.Label();
            this._ancien = new System.Windows.Forms.TextBox();
            this._lblNouveau = new System.Windows.Forms.Label();
            this._nouveau = new System.Windows.Forms.TextBox();
            this._lblConfirmation = new System.Windows.Forms.Label();
            this._confirmation = new System.Windows.Forms.TextBox();
            this._btnAnnuler = new System.Windows.Forms.Button();
            this._btnEnregistrer = new System.Windows.Forms.Button();
            this.Entete.SuspendLayout();
            this.Corps.SuspendLayout();
            this.Pied.SuspendLayout();
            this.SuspendLayout();
            //
            // LibelleTitre
            //
            this.LibelleTitre.Text = "Changer mon mot de passe";
            //
            // LibelleSousTitre
            //
            this.LibelleSousTitre.Text = "Compte de l'utilisateur connecté";
            //
            // Corps
            //
            this.Corps.Controls.Add(this._lblAncien);
            this.Corps.Controls.Add(this._ancien);
            this.Corps.Controls.Add(this._lblNouveau);
            this.Corps.Controls.Add(this._nouveau);
            this.Corps.Controls.Add(this._lblConfirmation);
            this.Corps.Controls.Add(this._confirmation);
            //
            // _lblAncien
            //
            this._lblAncien.AutoSize = true;
            this._lblAncien.Location = new System.Drawing.Point(24, 14);
            this._lblAncien.Name = "_lblAncien";
            this._lblAncien.Text = "Mot de passe actuel";
            //
            // _ancien
            //
            this._ancien.Location = new System.Drawing.Point(24, 34);
            this._ancien.Name = "_ancien";
            this._ancien.Size = new System.Drawing.Size(340, 23);
            this._ancien.TabIndex = 0;
            this._ancien.UseSystemPasswordChar = true;
            //
            // _lblNouveau
            //
            this._lblNouveau.AutoSize = true;
            this._lblNouveau.Location = new System.Drawing.Point(24, 68);
            this._lblNouveau.Name = "_lblNouveau";
            this._lblNouveau.Text = "Nouveau mot de passe (8 caractères minimum)";
            //
            // _nouveau
            //
            this._nouveau.Location = new System.Drawing.Point(24, 88);
            this._nouveau.Name = "_nouveau";
            this._nouveau.Size = new System.Drawing.Size(340, 23);
            this._nouveau.TabIndex = 1;
            this._nouveau.UseSystemPasswordChar = true;
            //
            // _lblConfirmation
            //
            this._lblConfirmation.AutoSize = true;
            this._lblConfirmation.Location = new System.Drawing.Point(24, 122);
            this._lblConfirmation.Name = "_lblConfirmation";
            this._lblConfirmation.Text = "Confirmation du nouveau mot de passe";
            //
            // _confirmation
            //
            this._confirmation.Location = new System.Drawing.Point(24, 142);
            this._confirmation.Name = "_confirmation";
            this._confirmation.Size = new System.Drawing.Size(340, 23);
            this._confirmation.TabIndex = 2;
            this._confirmation.UseSystemPasswordChar = true;
            //
            // Pied
            //
            this.Pied.Controls.Add(this._btnAnnuler);
            this.Pied.Controls.Add(this._btnEnregistrer);
            //
            // _btnAnnuler
            //
            this._btnAnnuler.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this._btnAnnuler.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnAnnuler.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 168, 168);
            this._btnAnnuler.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(232, 240, 250);
            this._btnAnnuler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnAnnuler.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnAnnuler.Name = "_btnAnnuler";
            this._btnAnnuler.Size = new System.Drawing.Size(120, 32);
            this._btnAnnuler.TabIndex = 4;
            this._btnAnnuler.Text = "Annuler";
            this._btnAnnuler.UseVisualStyleBackColor = false;
            //
            // _btnEnregistrer
            //
            this._btnEnregistrer.BackColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnEnregistrer.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnEnregistrer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnEnregistrer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 74, 140);
            this._btnEnregistrer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnEnregistrer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnEnregistrer.ForeColor = System.Drawing.Color.White;
            this._btnEnregistrer.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnEnregistrer.Name = "_btnEnregistrer";
            this._btnEnregistrer.Size = new System.Drawing.Size(120, 32);
            this._btnEnregistrer.TabIndex = 3;
            this._btnEnregistrer.Text = "Enregistrer";
            this._btnEnregistrer.UseVisualStyleBackColor = false;
            //
            // MotDePasseForm
            //
            this.AcceptButton = this._btnEnregistrer;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this._btnAnnuler;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Name = "MotDePasseForm";
            this.Text = "Changer mon mot de passe";
            this.Entete.ResumeLayout(false);
            this.Entete.PerformLayout();
            this.Corps.ResumeLayout(false);
            this.Corps.PerformLayout();
            this.Pied.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _lblAncien;
        private System.Windows.Forms.TextBox _ancien;
        private System.Windows.Forms.Label _lblNouveau;
        private System.Windows.Forms.TextBox _nouveau;
        private System.Windows.Forms.Label _lblConfirmation;
        private System.Windows.Forms.TextBox _confirmation;
        private System.Windows.Forms.Button _btnAnnuler;
        private System.Windows.Forms.Button _btnEnregistrer;
    }
}
