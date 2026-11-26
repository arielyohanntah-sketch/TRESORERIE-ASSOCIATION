// =====================================================================================
//  Forms/SplashForm.Designer.cs — MISE EN PAGE DE SplashForm (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Fenêtre sans bordure contenant seulement le contrôle _dessin (UI/DessinChargement.cs).
//
//  ➜ Pour modifier l'apparence : clic droit sur SplashForm.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans SplashForm.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans SplashForm.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Forms
{
    partial class SplashForm
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
            this._dessin = new SLN_TRESORERIE_ASSO.UI.DessinChargement();
            this.SuspendLayout();
            //
            // _dessin
            //
            this._dessin.BackColor = System.Drawing.Color.FromArgb(11, 61, 102);
            this._dessin.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dessin.Location = new System.Drawing.Point(0, 0);
            this._dessin.Name = "_dessin";
            this._dessin.Size = new System.Drawing.Size(640, 380);
            this._dessin.TabIndex = 0;
            //
            // SplashForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(11, 61, 102);
            this.ClientSize = new System.Drawing.Size(640, 380);
            this.Controls.Add(this._dessin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trésorerie Asso";
            this.ResumeLayout(false);
        }

        #endregion

        private SLN_TRESORERIE_ASSO.UI.DessinChargement _dessin;
    }
}
