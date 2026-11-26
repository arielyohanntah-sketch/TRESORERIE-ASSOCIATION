// =====================================================================================
//  Forms/LoginForm.Designer.cs — MISE EN PAGE DE LoginForm (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Identifiant, mot de passe, case Afficher, bandeau d'erreur, lien « premier administrateur », bouton Se connecter.
//
//  ➜ Pour modifier l'apparence : clic droit sur LoginForm.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans LoginForm.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans LoginForm.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Forms
{
    partial class LoginForm
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
            this._lblIdentifiant = new System.Windows.Forms.Label();
            this._login = new System.Windows.Forms.TextBox();
            this._lblMotDePasse = new System.Windows.Forms.Label();
            this._motDePasse = new System.Windows.Forms.TextBox();
            this._afficher = new System.Windows.Forms.CheckBox();
            this._erreur = new System.Windows.Forms.Label();
            this._premierAdmin = new System.Windows.Forms.Label();
            this._creerAdmin = new System.Windows.Forms.LinkLabel();
            this._serveur = new System.Windows.Forms.Label();
            this._btnQuitter = new System.Windows.Forms.Button();
            this._btnConnecter = new System.Windows.Forms.Button();
            this.Entete.SuspendLayout();
            this.Corps.SuspendLayout();
            this.Pied.SuspendLayout();
            this.SuspendLayout();
            //
            // Entete
            //
            this.Entete.Size = new System.Drawing.Size(460, 96);
            //
            // Icone
            //
            this.Icone.Location = new System.Drawing.Point(22, 24);
            this.Icone.Size = new System.Drawing.Size(48, 48);
            //
            // LibelleTitre
            //
            this.LibelleTitre.Font = new System.Drawing.Font("Segoe UI Semibold", 17F);
            this.LibelleTitre.Location = new System.Drawing.Point(82, 18);
            this.LibelleTitre.Text = "Trésorerie Asso";
            //
            // LibelleSousTitre
            //
            this.LibelleSousTitre.Location = new System.Drawing.Point(85, 56);
            this.LibelleSousTitre.Text = "Connectez-vous pour accéder à la trésorerie";
            //
            // Corps
            //
            this.Corps.Controls.Add(this._lblIdentifiant);
            this.Corps.Controls.Add(this._login);
            this.Corps.Controls.Add(this._lblMotDePasse);
            this.Corps.Controls.Add(this._motDePasse);
            this.Corps.Controls.Add(this._afficher);
            this.Corps.Controls.Add(this._erreur);
            this.Corps.Controls.Add(this._premierAdmin);
            this.Corps.Controls.Add(this._creerAdmin);
            this.Corps.Controls.Add(this._serveur);
            //
            // _lblIdentifiant
            //
            this._lblIdentifiant.AutoSize = true;
            this._lblIdentifiant.Location = new System.Drawing.Point(32, 16);
            this._lblIdentifiant.Name = "_lblIdentifiant";
            this._lblIdentifiant.Size = new System.Drawing.Size(66, 15);
            this._lblIdentifiant.TabIndex = 0;
            this._lblIdentifiant.Text = "Identifiant";
            //
            // _login
            //
            this._login.Location = new System.Drawing.Point(32, 36);
            this._login.Name = "_login";
            this._login.Size = new System.Drawing.Size(396, 23);
            this._login.TabIndex = 1;
            //
            // _lblMotDePasse
            //
            this._lblMotDePasse.AutoSize = true;
            this._lblMotDePasse.Location = new System.Drawing.Point(32, 72);
            this._lblMotDePasse.Name = "_lblMotDePasse";
            this._lblMotDePasse.Size = new System.Drawing.Size(77, 15);
            this._lblMotDePasse.TabIndex = 2;
            this._lblMotDePasse.Text = "Mot de passe";
            //
            // _motDePasse
            //
            this._motDePasse.Location = new System.Drawing.Point(32, 92);
            this._motDePasse.Name = "_motDePasse";
            this._motDePasse.Size = new System.Drawing.Size(396, 23);
            this._motDePasse.TabIndex = 3;
            this._motDePasse.UseSystemPasswordChar = true;
            //
            // _afficher
            //
            this._afficher.AutoSize = true;
            this._afficher.Location = new System.Drawing.Point(32, 126);
            this._afficher.Name = "_afficher";
            this._afficher.Size = new System.Drawing.Size(160, 19);
            this._afficher.TabIndex = 4;
            this._afficher.Text = "Afficher le mot de passe";
            //
            // _erreur
            //
            this._erreur.BackColor = System.Drawing.Color.FromArgb(253, 231, 233);
            this._erreur.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._erreur.ForeColor = System.Drawing.Color.FromArgb(142, 27, 36);
            this._erreur.Location = new System.Drawing.Point(32, 156);
            this._erreur.Name = "_erreur";
            this._erreur.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this._erreur.Size = new System.Drawing.Size(396, 46);
            this._erreur.TabIndex = 5;
            this._erreur.Text = "Message d'erreur (affiché seulement en cas d'échec)";
            this._erreur.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._erreur.Visible = false;
            //
            // _premierAdmin
            //
            this._premierAdmin.BackColor = System.Drawing.Color.FromArgb(229, 241, 251);
            this._premierAdmin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._premierAdmin.ForeColor = System.Drawing.Color.FromArgb(11, 61, 102);
            this._premierAdmin.Location = new System.Drawing.Point(32, 210);
            this._premierAdmin.Name = "_premierAdmin";
            this._premierAdmin.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this._premierAdmin.Size = new System.Drawing.Size(396, 40);
            this._premierAdmin.TabIndex = 6;
            this._premierAdmin.Text = "Aucun utilisateur n'existe encore. Commencez par créer le compte administrateur.";
            this._premierAdmin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._premierAdmin.Visible = false;
            //
            // _creerAdmin
            //
            this._creerAdmin.AutoSize = true;
            this._creerAdmin.Location = new System.Drawing.Point(32, 256);
            this._creerAdmin.Name = "_creerAdmin";
            this._creerAdmin.Size = new System.Drawing.Size(185, 15);
            this._creerAdmin.TabIndex = 7;
            this._creerAdmin.TabStop = true;
            this._creerAdmin.Text = "Créer le compte administrateur…";
            this._creerAdmin.Visible = false;
            //
            // _serveur
            //
            this._serveur.AutoSize = true;
            this._serveur.ForeColor = System.Drawing.Color.FromArgb(74, 74, 74);
            this._serveur.Location = new System.Drawing.Point(32, 286);
            this._serveur.Name = "_serveur";
            this._serveur.Size = new System.Drawing.Size(220, 15);
            this._serveur.TabIndex = 8;
            this._serveur.Text = "Base : tresorerie_asso sur localhost:3306";
            //
            // Pied
            //
            this.Pied.Controls.Add(this._btnQuitter);
            this.Pied.Controls.Add(this._btnConnecter);
            //
            // _btnQuitter
            //
            this._btnQuitter.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this._btnQuitter.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnQuitter.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnQuitter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 168, 168);
            this._btnQuitter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(232, 240, 250);
            this._btnQuitter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnQuitter.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnQuitter.Name = "_btnQuitter";
            this._btnQuitter.Size = new System.Drawing.Size(120, 32);
            this._btnQuitter.TabIndex = 10;
            this._btnQuitter.Text = "Quitter";
            this._btnQuitter.UseVisualStyleBackColor = false;
            //
            // _btnConnecter
            //
            this._btnConnecter.BackColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnConnecter.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnConnecter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnConnecter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 74, 140);
            this._btnConnecter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnConnecter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnConnecter.ForeColor = System.Drawing.Color.White;
            this._btnConnecter.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnConnecter.Name = "_btnConnecter";
            this._btnConnecter.Size = new System.Drawing.Size(130, 32);
            this._btnConnecter.TabIndex = 9;
            this._btnConnecter.Text = "Se connecter";
            this._btnConnecter.UseVisualStyleBackColor = false;
            //
            // LoginForm
            //
            this.AcceptButton = this._btnConnecter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this._btnQuitter;
            this.ClientSize = new System.Drawing.Size(460, 470);
            this.Name = "LoginForm";
            this.ShowInTaskbar = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connexion — Trésorerie Asso";
            this.Entete.ResumeLayout(false);
            this.Entete.PerformLayout();
            this.Corps.ResumeLayout(false);
            this.Corps.PerformLayout();
            this.Pied.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _lblIdentifiant;
        private System.Windows.Forms.TextBox _login;
        private System.Windows.Forms.Label _lblMotDePasse;
        private System.Windows.Forms.TextBox _motDePasse;
        private System.Windows.Forms.CheckBox _afficher;
        private System.Windows.Forms.Label _erreur;
        private System.Windows.Forms.Label _premierAdmin;
        private System.Windows.Forms.LinkLabel _creerAdmin;
        private System.Windows.Forms.Label _serveur;
        private System.Windows.Forms.Button _btnQuitter;
        private System.Windows.Forms.Button _btnConnecter;
    }
}
