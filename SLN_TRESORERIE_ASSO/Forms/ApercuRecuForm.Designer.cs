// =====================================================================================
//  Forms/ApercuRecuForm.Designer.cs — MISE EN PAGE DE ApercuRecuForm (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Barre d'outils _outils (Imprimer…, Enregistrer en PDF, Fermer) et aperçu _apercu
//  (PrintPreviewControl) qui montre la page telle qu'elle sortira de l'imprimante.
//
//  ➜ Pour modifier l'apparence : clic droit sur ApercuRecuForm.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur. Le code « logique »
//    va dans ApercuRecuForm.cs ; le dessin du reçu est dans UI/ImpressionRecu.cs.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Forms
{
    partial class ApercuRecuForm
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
            _outils = new ToolStrip();
            _btnImprimer = new ToolStripButton();
            _btnPdf = new ToolStripButton();
            _separateur = new ToolStripSeparator();
            _btnFermer = new ToolStripButton();
            _aide = new ToolStripLabel();
            _apercu = new PrintPreviewControl();
            _outils.SuspendLayout();
            SuspendLayout();
            //
            // _outils
            //
            _outils.GripStyle = ToolStripGripStyle.Hidden;
            _outils.Items.AddRange(new ToolStripItem[] { _btnImprimer, _btnPdf, _separateur, _btnFermer, _aide });
            _outils.Location = new Point(0, 0);
            _outils.Name = "_outils";
            _outils.Padding = new Padding(8, 4, 8, 4);
            _outils.Size = new Size(784, 33);
            _outils.TabIndex = 0;
            //
            // _btnImprimer
            //
            _btnImprimer.Name = "_btnImprimer";
            _btnImprimer.Size = new Size(68, 22);
            _btnImprimer.Text = "Imprimer…";
            _btnImprimer.ToolTipText = "Choisir l'imprimante et imprimer le reçu";
            //
            // _btnPdf
            //
            _btnPdf.Name = "_btnPdf";
            _btnPdf.Size = new Size(112, 22);
            _btnPdf.Text = "Enregistrer en PDF";
            _btnPdf.ToolTipText = "Créer un fichier PDF du reçu (à envoyer par e-mail ou WhatsApp)";
            //
            // _separateur
            //
            _separateur.Name = "_separateur";
            _separateur.Size = new Size(6, 25);
            //
            // _btnFermer
            //
            _btnFermer.Name = "_btnFermer";
            _btnFermer.Size = new Size(49, 22);
            _btnFermer.Text = "Fermer";
            //
            // _aide
            //
            _aide.Alignment = ToolStripItemAlignment.Right;
            _aide.ForeColor = Color.FromArgb(74, 74, 74);
            _aide.Name = "_aide";
            _aide.Size = new Size(240, 22);
            _aide.Text = "Deux exemplaires par page A4 : découpez au pointillé";
            //
            // _apercu
            //
            _apercu.AutoZoom = true;
            _apercu.BackColor = Color.FromArgb(240, 240, 240);
            _apercu.Dock = DockStyle.Fill;
            _apercu.Location = new Point(0, 33);
            _apercu.Name = "_apercu";
            _apercu.Size = new Size(784, 828);
            _apercu.TabIndex = 1;
            _apercu.UseAntiAlias = true;
            //
            // ApercuRecuForm
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(784, 861);
            Controls.Add(_apercu);
            Controls.Add(_outils);
            MinimizeBox = false;
            Name = "ApercuRecuForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Aperçu du reçu";
            _outils.ResumeLayout(false);
            _outils.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip _outils;
        private System.Windows.Forms.ToolStripButton _btnImprimer;
        private System.Windows.Forms.ToolStripButton _btnPdf;
        private System.Windows.Forms.ToolStripSeparator _separateur;
        private System.Windows.Forms.ToolStripButton _btnFermer;
        private System.Windows.Forms.ToolStripLabel _aide;
        private System.Windows.Forms.PrintPreviewControl _apercu;
    }
}
