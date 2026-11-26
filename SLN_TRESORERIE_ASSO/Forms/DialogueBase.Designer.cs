// =====================================================================================
//  Forms/DialogueBase.Designer.cs — MISE EN PAGE DE DialogueBase (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Bandeau bleu (Entete, Icone, LibelleTitre, LibelleSousTitre), zone Corps et barre de boutons Pied, hérités par toutes les petites fenêtres.
//
//  ➜ Pour modifier l'apparence : clic droit sur DialogueBase.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans DialogueBase.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans DialogueBase.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Forms
{
    partial class DialogueBase
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
            this.Entete = new System.Windows.Forms.Panel();
            this.Icone = new System.Windows.Forms.PictureBox();
            this.LibelleTitre = new System.Windows.Forms.Label();
            this.LibelleSousTitre = new System.Windows.Forms.Label();
            this.Corps = new System.Windows.Forms.Panel();
            this.Pied = new System.Windows.Forms.FlowLayoutPanel();
            this.Entete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Icone)).BeginInit();
            this.SuspendLayout();
            //
            // Entete
            //
            this.Entete.BackColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this.Entete.Controls.Add(this.Icone);
            this.Entete.Controls.Add(this.LibelleTitre);
            this.Entete.Controls.Add(this.LibelleSousTitre);
            this.Entete.Dock = System.Windows.Forms.DockStyle.Top;
            this.Entete.Location = new System.Drawing.Point(0, 0);
            this.Entete.Name = "Entete";
            this.Entete.Size = new System.Drawing.Size(460, 66);
            this.Entete.TabIndex = 2;
            //
            // Icone
            //
            this.Icone.Location = new System.Drawing.Point(22, 15);
            this.Icone.Name = "Icone";
            this.Icone.Size = new System.Drawing.Size(36, 36);
            this.Icone.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.Icone.TabIndex = 0;
            this.Icone.TabStop = false;
            //
            // LibelleTitre
            //
            this.LibelleTitre.AutoSize = true;
            this.LibelleTitre.BackColor = System.Drawing.Color.Transparent;
            this.LibelleTitre.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.LibelleTitre.ForeColor = System.Drawing.Color.White;
            this.LibelleTitre.Location = new System.Drawing.Point(70, 11);
            this.LibelleTitre.Name = "LibelleTitre";
            this.LibelleTitre.Size = new System.Drawing.Size(40, 20);
            this.LibelleTitre.TabIndex = 1;
            this.LibelleTitre.Text = "Titre";
            //
            // LibelleSousTitre
            //
            this.LibelleSousTitre.AutoSize = true;
            this.LibelleSousTitre.BackColor = System.Drawing.Color.Transparent;
            this.LibelleSousTitre.ForeColor = System.Drawing.Color.FromArgb(215, 232, 250);
            this.LibelleSousTitre.Location = new System.Drawing.Point(72, 36);
            this.LibelleSousTitre.Name = "LibelleSousTitre";
            this.LibelleSousTitre.Size = new System.Drawing.Size(70, 15);
            this.LibelleSousTitre.TabIndex = 2;
            this.LibelleSousTitre.Text = "Sous-titre";
            //
            // Corps
            //
            this.Corps.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.Corps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Corps.Location = new System.Drawing.Point(0, 66);
            this.Corps.Name = "Corps";
            this.Corps.Padding = new System.Windows.Forms.Padding(22, 16, 22, 8);
            this.Corps.Size = new System.Drawing.Size(460, 176);
            this.Corps.TabIndex = 0;
            //
            // Pied
            //
            this.Pied.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            this.Pied.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Pied.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.Pied.Location = new System.Drawing.Point(0, 242);
            this.Pied.Name = "Pied";
            this.Pied.Padding = new System.Windows.Forms.Padding(16, 13, 10, 0);
            this.Pied.Size = new System.Drawing.Size(460, 58);
            this.Pied.TabIndex = 1;
            //
            // DialogueBase
            //
            this.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.ClientSize = new System.Drawing.Size(460, 300);
            this.Controls.Add(this.Corps);
            this.Controls.Add(this.Pied);
            this.Controls.Add(this.Entete);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogueBase";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DialogueBase";
            this.Entete.ResumeLayout(false);
            this.Entete.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Icone)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        protected System.Windows.Forms.Panel Entete;
        protected System.Windows.Forms.PictureBox Icone;
        protected System.Windows.Forms.Label LibelleTitre;
        protected System.Windows.Forms.Label LibelleSousTitre;
        protected System.Windows.Forms.Panel Corps;
        protected System.Windows.Forms.FlowLayoutPanel Pied;
    }
}
