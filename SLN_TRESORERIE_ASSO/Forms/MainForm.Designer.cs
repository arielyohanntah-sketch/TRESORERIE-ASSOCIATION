// =====================================================================================
//  Forms/MainForm.Designer.cs — MISE EN PAGE DE MainForm (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Barre d'outils _outils (haut), menu _arbre (gauche) séparé par _separation, zone _contenu (centre), barre d'état (bas).
//
//  ➜ Pour modifier l'apparence : clic droit sur MainForm.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans MainForm.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans MainForm.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Forms
{
    partial class MainForm
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
            this._outils = new System.Windows.Forms.ToolStrip();
            this._arbre = new System.Windows.Forms.TreeView();
            this._separation = new System.Windows.Forms.Panel();
            this._contenu = new System.Windows.Forms.Panel();
            this._statut = new System.Windows.Forms.StatusStrip();
            this._lblUtilisateur = new System.Windows.Forms.ToolStripStatusLabel();
            this._lblExercice = new System.Windows.Forms.ToolStripStatusLabel();
            this._lblBrouillons = new System.Windows.Forms.ToolStripStatusLabel();
            this._lblRessort = new System.Windows.Forms.ToolStripStatusLabel();
            this._lblBase = new System.Windows.Forms.ToolStripStatusLabel();
            this._statut.SuspendLayout();
            this.SuspendLayout();
            //
            // _outils
            //
            this._outils.AutoSize = false;
            this._outils.BackColor = System.Drawing.Color.FromArgb(247, 247, 247);
            this._outils.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._outils.Location = new System.Drawing.Point(0, 0);
            this._outils.Name = "_outils";
            this._outils.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this._outils.Size = new System.Drawing.Size(1280, 44);
            this._outils.TabIndex = 0;
            this._outils.Text = "Barre d'outils";
            //
            // _arbre
            //
            this._arbre.BackColor = System.Drawing.Color.White;
            this._arbre.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._arbre.Dock = System.Windows.Forms.DockStyle.Left;
            this._arbre.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this._arbre.FullRowSelect = true;
            this._arbre.HideSelection = false;
            this._arbre.Indent = 16;
            this._arbre.ItemHeight = 28;
            this._arbre.Location = new System.Drawing.Point(0, 44);
            this._arbre.Name = "_arbre";
            this._arbre.ShowLines = false;
            this._arbre.Size = new System.Drawing.Size(235, 710);
            this._arbre.TabIndex = 1;
            //
            // _separation
            //
            this._separation.BackColor = System.Drawing.Color.FromArgb(218, 218, 218);
            this._separation.Dock = System.Windows.Forms.DockStyle.Left;
            this._separation.Location = new System.Drawing.Point(235, 44);
            this._separation.Name = "_separation";
            this._separation.Size = new System.Drawing.Size(1, 710);
            this._separation.TabIndex = 2;
            //
            // _contenu
            //
            this._contenu.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this._contenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this._contenu.Location = new System.Drawing.Point(236, 44);
            this._contenu.Name = "_contenu";
            this._contenu.Size = new System.Drawing.Size(1044, 710);
            this._contenu.TabIndex = 3;
            //
            // _statut
            //
            this._statut.BackColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._statut.ForeColor = System.Drawing.Color.White;
            this._statut.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._lblUtilisateur,
            this._lblExercice,
            this._lblBrouillons,
            this._lblRessort,
            this._lblBase});
            this._statut.Location = new System.Drawing.Point(0, 754);
            this._statut.Name = "_statut";
            this._statut.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this._statut.Size = new System.Drawing.Size(1280, 26);
            this._statut.SizingGrip = false;
            this._statut.TabIndex = 4;
            //
            // _lblUtilisateur
            //
            this._lblUtilisateur.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._lblUtilisateur.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this._lblUtilisateur.ForeColor = System.Drawing.Color.White;
            this._lblUtilisateur.Name = "_lblUtilisateur";
            this._lblUtilisateur.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._lblUtilisateur.Text = "Connecté : Prénom Nom (Rôle)";
            //
            // _lblExercice
            //
            this._lblExercice.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._lblExercice.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this._lblExercice.ForeColor = System.Drawing.Color.White;
            this._lblExercice.Name = "_lblExercice";
            this._lblExercice.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._lblExercice.Text = "Exercice 2026 — OUVERT";
            //
            // _lblBrouillons
            //
            this._lblBrouillons.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._lblBrouillons.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this._lblBrouillons.ForeColor = System.Drawing.Color.White;
            this._lblBrouillons.Name = "_lblBrouillons";
            this._lblBrouillons.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._lblBrouillons.Text = "Aucune opération à valider";
            //
            // _lblRessort
            //
            this._lblRessort.Name = "_lblRessort";
            this._lblRessort.Spring = true;
            //
            // _lblBase
            //
            this._lblBase.ForeColor = System.Drawing.Color.White;
            this._lblBase.Name = "_lblBase";
            this._lblBase.Text = "tresorerie_asso @ localhost";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.ClientSize = new System.Drawing.Size(1280, 780);
            this.Controls.Add(this._contenu);
            this.Controls.Add(this._separation);
            this.Controls.Add(this._arbre);
            this.Controls.Add(this._statut);
            this.Controls.Add(this._outils);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1000, 640);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trésorerie Asso";
            this._statut.ResumeLayout(false);
            this._statut.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip _outils;
        private System.Windows.Forms.TreeView _arbre;
        private System.Windows.Forms.Panel _separation;
        private System.Windows.Forms.Panel _contenu;
        private System.Windows.Forms.StatusStrip _statut;
        private System.Windows.Forms.ToolStripStatusLabel _lblUtilisateur;
        private System.Windows.Forms.ToolStripStatusLabel _lblExercice;
        private System.Windows.Forms.ToolStripStatusLabel _lblBrouillons;
        private System.Windows.Forms.ToolStripStatusLabel _lblRessort;
        private System.Windows.Forms.ToolStripStatusLabel _lblBase;
    }
}
