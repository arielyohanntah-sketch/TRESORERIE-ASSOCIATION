// =====================================================================================
//  Forms/CreerUtilisateurForm.Designer.cs — MISE EN PAGE DE CreerUtilisateurForm (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Champs Nom, Prénoms, Identifiant, E-mail, Mot de passe, Confirmation, Rôle (Corps) et bouton Créer (Pied).
//
//  ➜ Pour modifier l'apparence : clic droit sur CreerUtilisateurForm.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans CreerUtilisateurForm.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans CreerUtilisateurForm.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Forms
{
    partial class CreerUtilisateurForm
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
            this._lblNom = new System.Windows.Forms.Label();
            this._nom = new System.Windows.Forms.TextBox();
            this._lblPrenoms = new System.Windows.Forms.Label();
            this._prenoms = new System.Windows.Forms.TextBox();
            this._lblLogin = new System.Windows.Forms.Label();
            this._login = new System.Windows.Forms.TextBox();
            this._lblRole = new System.Windows.Forms.Label();
            this._role = new System.Windows.Forms.ComboBox();
            this._lblMdp = new System.Windows.Forms.Label();
            this._mdp = new System.Windows.Forms.TextBox();
            this._lblConfirmation = new System.Windows.Forms.Label();
            this._confirmation = new System.Windows.Forms.TextBox();
            this._lblEmail = new System.Windows.Forms.Label();
            this._email = new System.Windows.Forms.TextBox();
            this._btnAnnuler = new System.Windows.Forms.Button();
            this._btnCreer = new System.Windows.Forms.Button();
            this.Entete.SuspendLayout();
            this.Corps.SuspendLayout();
            this.Pied.SuspendLayout();
            this.SuspendLayout();
            //
            // LibelleTitre
            //
            this.LibelleTitre.Text = "Nouvel utilisateur";
            //
            // LibelleSousTitre
            //
            this.LibelleSousTitre.Text = "Accès à l'application selon le rôle choisi";
            //
            // Corps
            //
            this.Corps.Controls.Add(this._lblNom);
            this.Corps.Controls.Add(this._nom);
            this.Corps.Controls.Add(this._lblPrenoms);
            this.Corps.Controls.Add(this._prenoms);
            this.Corps.Controls.Add(this._lblLogin);
            this.Corps.Controls.Add(this._login);
            this.Corps.Controls.Add(this._lblRole);
            this.Corps.Controls.Add(this._role);
            this.Corps.Controls.Add(this._lblMdp);
            this.Corps.Controls.Add(this._mdp);
            this.Corps.Controls.Add(this._lblConfirmation);
            this.Corps.Controls.Add(this._confirmation);
            this.Corps.Controls.Add(this._lblEmail);
            this.Corps.Controls.Add(this._email);
            //
            // _lblNom
            //
            this._lblNom.AutoSize = true;
            this._lblNom.Location = new System.Drawing.Point(24, 14);
            this._lblNom.Name = "_lblNom";
            this._lblNom.Text = "Nom *";
            //
            // _nom
            //
            this._nom.Location = new System.Drawing.Point(24, 34);
            this._nom.Name = "_nom";
            this._nom.Size = new System.Drawing.Size(196, 23);
            this._nom.TabIndex = 0;
            //
            // _lblPrenoms
            //
            this._lblPrenoms.AutoSize = true;
            this._lblPrenoms.Location = new System.Drawing.Point(240, 14);
            this._lblPrenoms.Name = "_lblPrenoms";
            this._lblPrenoms.Text = "Prénoms *";
            //
            // _prenoms
            //
            this._prenoms.Location = new System.Drawing.Point(240, 34);
            this._prenoms.Name = "_prenoms";
            this._prenoms.Size = new System.Drawing.Size(196, 23);
            this._prenoms.TabIndex = 1;
            //
            // _lblLogin
            //
            this._lblLogin.AutoSize = true;
            this._lblLogin.Location = new System.Drawing.Point(24, 70);
            this._lblLogin.Name = "_lblLogin";
            this._lblLogin.Text = "Identifiant de connexion *";
            //
            // _login
            //
            this._login.Location = new System.Drawing.Point(24, 90);
            this._login.Name = "_login";
            this._login.Size = new System.Drawing.Size(196, 23);
            this._login.TabIndex = 2;
            //
            // _lblRole
            //
            this._lblRole.AutoSize = true;
            this._lblRole.Location = new System.Drawing.Point(240, 70);
            this._lblRole.Name = "_lblRole";
            this._lblRole.Text = "Rôle *";
            //
            // _role
            //
            this._role.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._role.Items.AddRange(new object[] {
            "ADMIN",
            "PRESIDENT",
            "TRESORIER",
            "TRESORIER_ADJOINT",
            "COMMISSAIRE_COMPTES",
            "MEMBRE_BUREAU"});
            this._role.Location = new System.Drawing.Point(240, 90);
            this._role.Name = "_role";
            this._role.Size = new System.Drawing.Size(196, 23);
            this._role.TabIndex = 3;
            //
            // _lblMdp
            //
            this._lblMdp.AutoSize = true;
            this._lblMdp.Location = new System.Drawing.Point(24, 126);
            this._lblMdp.Name = "_lblMdp";
            this._lblMdp.Text = "Mot de passe * (8 caractères min.)";
            //
            // _mdp
            //
            this._mdp.Location = new System.Drawing.Point(24, 146);
            this._mdp.Name = "_mdp";
            this._mdp.Size = new System.Drawing.Size(196, 23);
            this._mdp.TabIndex = 4;
            this._mdp.UseSystemPasswordChar = true;
            //
            // _lblConfirmation
            //
            this._lblConfirmation.AutoSize = true;
            this._lblConfirmation.Location = new System.Drawing.Point(240, 126);
            this._lblConfirmation.Name = "_lblConfirmation";
            this._lblConfirmation.Text = "Confirmation *";
            //
            // _confirmation
            //
            this._confirmation.Location = new System.Drawing.Point(240, 146);
            this._confirmation.Name = "_confirmation";
            this._confirmation.Size = new System.Drawing.Size(196, 23);
            this._confirmation.TabIndex = 5;
            this._confirmation.UseSystemPasswordChar = true;
            //
            // _lblEmail
            //
            this._lblEmail.AutoSize = true;
            this._lblEmail.Location = new System.Drawing.Point(24, 182);
            this._lblEmail.Name = "_lblEmail";
            this._lblEmail.Text = "E-mail";
            //
            // _email
            //
            this._email.Location = new System.Drawing.Point(24, 202);
            this._email.Name = "_email";
            this._email.Size = new System.Drawing.Size(412, 23);
            this._email.TabIndex = 6;
            //
            // Pied
            //
            this.Pied.Controls.Add(this._btnAnnuler);
            this.Pied.Controls.Add(this._btnCreer);
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
            this._btnAnnuler.TabIndex = 8;
            this._btnAnnuler.Text = "Annuler";
            this._btnAnnuler.UseVisualStyleBackColor = false;
            //
            // _btnCreer
            //
            this._btnCreer.BackColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnCreer.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnCreer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnCreer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 74, 140);
            this._btnCreer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnCreer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnCreer.ForeColor = System.Drawing.Color.White;
            this._btnCreer.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnCreer.Name = "_btnCreer";
            this._btnCreer.Size = new System.Drawing.Size(150, 32);
            this._btnCreer.TabIndex = 7;
            this._btnCreer.Text = "Créer l'utilisateur";
            this._btnCreer.UseVisualStyleBackColor = false;
            //
            // CreerUtilisateurForm
            //
            this.AcceptButton = this._btnCreer;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this._btnAnnuler;
            this.ClientSize = new System.Drawing.Size(460, 370);
            this.Name = "CreerUtilisateurForm";
            this.Text = "Nouvel utilisateur";
            this.Entete.ResumeLayout(false);
            this.Entete.PerformLayout();
            this.Corps.ResumeLayout(false);
            this.Corps.PerformLayout();
            this.Pied.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _lblNom;
        private System.Windows.Forms.TextBox _nom;
        private System.Windows.Forms.Label _lblPrenoms;
        private System.Windows.Forms.TextBox _prenoms;
        private System.Windows.Forms.Label _lblLogin;
        private System.Windows.Forms.TextBox _login;
        private System.Windows.Forms.Label _lblRole;
        private System.Windows.Forms.ComboBox _role;
        private System.Windows.Forms.Label _lblMdp;
        private System.Windows.Forms.TextBox _mdp;
        private System.Windows.Forms.Label _lblConfirmation;
        private System.Windows.Forms.TextBox _confirmation;
        private System.Windows.Forms.Label _lblEmail;
        private System.Windows.Forms.TextBox _email;
        private System.Windows.Forms.Button _btnAnnuler;
        private System.Windows.Forms.Button _btnCreer;
    }
}
