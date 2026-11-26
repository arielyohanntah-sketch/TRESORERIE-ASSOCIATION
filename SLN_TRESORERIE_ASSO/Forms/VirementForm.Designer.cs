// =====================================================================================
//  Forms/VirementForm.Designer.cs — MISE EN PAGE DE VirementForm (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Comptes source et destination avec leurs soldes, montant, date, libellé, bouton Enregistrer.
//
//  ➜ Pour modifier l'apparence : clic droit sur VirementForm.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans VirementForm.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans VirementForm.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Forms
{
    partial class VirementForm
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
            this._grpSource = new System.Windows.Forms.GroupBox();
            this._source = new System.Windows.Forms.ComboBox();
            this._soldeSource = new System.Windows.Forms.Label();
            this._apresSource = new System.Windows.Forms.Label();
            this._fleche = new System.Windows.Forms.PictureBox();
            this._grpDestination = new System.Windows.Forms.GroupBox();
            this._destination = new System.Windows.Forms.ComboBox();
            this._soldeDest = new System.Windows.Forms.Label();
            this._apresDest = new System.Windows.Forms.Label();
            this._lblDate = new System.Windows.Forms.Label();
            this._date = new System.Windows.Forms.DateTimePicker();
            this._lblMontant = new System.Windows.Forms.Label();
            this._montant = new System.Windows.Forms.NumericUpDown();
            this._lblLibelle = new System.Windows.Forms.Label();
            this._libelle = new System.Windows.Forms.TextBox();
            this._info = new System.Windows.Forms.Label();
            this._btnAnnuler = new System.Windows.Forms.Button();
            this._btnEnregistrer = new System.Windows.Forms.Button();
            this.Entete.SuspendLayout();
            this.Corps.SuspendLayout();
            this.Pied.SuspendLayout();
            this._grpSource.SuspendLayout();
            this._grpDestination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._fleche)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._montant)).BeginInit();
            this.SuspendLayout();
            //
            // LibelleTitre
            //
            this.LibelleTitre.Text = "Virement entre comptes";
            //
            // LibelleSousTitre
            //
            this.LibelleSousTitre.Text = "Transfert d'argent d'un compte de l'association vers un autre";
            //
            // Corps
            //
            this.Corps.Controls.Add(this._grpSource);
            this.Corps.Controls.Add(this._fleche);
            this.Corps.Controls.Add(this._grpDestination);
            this.Corps.Controls.Add(this._lblDate);
            this.Corps.Controls.Add(this._date);
            this.Corps.Controls.Add(this._lblMontant);
            this.Corps.Controls.Add(this._montant);
            this.Corps.Controls.Add(this._lblLibelle);
            this.Corps.Controls.Add(this._libelle);
            this.Corps.Controls.Add(this._info);
            //
            // _grpSource
            //
            this._grpSource.Controls.Add(this._source);
            this._grpSource.Controls.Add(this._soldeSource);
            this._grpSource.Controls.Add(this._apresSource);
            this._grpSource.Location = new System.Drawing.Point(22, 12);
            this._grpSource.Name = "_grpSource";
            this._grpSource.Size = new System.Drawing.Size(270, 118);
            this._grpSource.TabIndex = 0;
            this._grpSource.TabStop = false;
            this._grpSource.Text = "Compte source";
            //
            // _source
            //
            this._source.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._source.Location = new System.Drawing.Point(12, 26);
            this._source.Name = "_source";
            this._source.Size = new System.Drawing.Size(246, 23);
            this._source.TabIndex = 0;
            //
            // _soldeSource
            //
            this._soldeSource.AutoSize = true;
            this._soldeSource.Location = new System.Drawing.Point(12, 60);
            this._soldeSource.Name = "_soldeSource";
            this._soldeSource.Text = "Solde actuel : 0";
            //
            // _apresSource
            //
            this._apresSource.AutoSize = true;
            this._apresSource.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._apresSource.Location = new System.Drawing.Point(12, 84);
            this._apresSource.Name = "_apresSource";
            this._apresSource.Text = "Après virement : 0";
            //
            // _fleche
            //
            this._fleche.Location = new System.Drawing.Point(302, 52);
            this._fleche.Name = "_fleche";
            this._fleche.Size = new System.Drawing.Size(48, 40);
            this._fleche.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this._fleche.TabIndex = 1;
            this._fleche.TabStop = false;
            //
            // _grpDestination
            //
            this._grpDestination.Controls.Add(this._destination);
            this._grpDestination.Controls.Add(this._soldeDest);
            this._grpDestination.Controls.Add(this._apresDest);
            this._grpDestination.Location = new System.Drawing.Point(362, 12);
            this._grpDestination.Name = "_grpDestination";
            this._grpDestination.Size = new System.Drawing.Size(276, 118);
            this._grpDestination.TabIndex = 2;
            this._grpDestination.TabStop = false;
            this._grpDestination.Text = "Compte destination";
            //
            // _destination
            //
            this._destination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._destination.Location = new System.Drawing.Point(12, 26);
            this._destination.Name = "_destination";
            this._destination.Size = new System.Drawing.Size(252, 23);
            this._destination.TabIndex = 0;
            //
            // _soldeDest
            //
            this._soldeDest.AutoSize = true;
            this._soldeDest.Location = new System.Drawing.Point(12, 60);
            this._soldeDest.Name = "_soldeDest";
            this._soldeDest.Text = "Solde actuel : 0";
            //
            // _apresDest
            //
            this._apresDest.AutoSize = true;
            this._apresDest.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._apresDest.Location = new System.Drawing.Point(12, 84);
            this._apresDest.Name = "_apresDest";
            this._apresDest.Text = "Après virement : 0";
            //
            // _lblDate
            //
            this._lblDate.AutoSize = true;
            this._lblDate.Location = new System.Drawing.Point(22, 146);
            this._lblDate.Name = "_lblDate";
            this._lblDate.Text = "Date *";
            //
            // _date
            //
            this._date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._date.Location = new System.Drawing.Point(22, 166);
            this._date.Name = "_date";
            this._date.Size = new System.Drawing.Size(140, 23);
            this._date.TabIndex = 3;
            //
            // _lblMontant
            //
            this._lblMontant.AutoSize = true;
            this._lblMontant.Location = new System.Drawing.Point(180, 146);
            this._lblMontant.Name = "_lblMontant";
            this._lblMontant.Text = "Montant (FCFA) *";
            //
            // _montant
            //
            this._montant.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._montant.Location = new System.Drawing.Point(180, 166);
            this._montant.Maximum = new decimal(new int[] { -727379969, 232, 0, 0 });
            this._montant.Name = "_montant";
            this._montant.Size = new System.Drawing.Size(170, 23);
            this._montant.TabIndex = 4;
            this._montant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._montant.ThousandsSeparator = true;
            //
            // _lblLibelle
            //
            this._lblLibelle.AutoSize = true;
            this._lblLibelle.Location = new System.Drawing.Point(22, 202);
            this._lblLibelle.Name = "_lblLibelle";
            this._lblLibelle.Text = "Libellé *";
            //
            // _libelle
            //
            this._libelle.Location = new System.Drawing.Point(22, 222);
            this._libelle.MaxLength = 255;
            this._libelle.Name = "_libelle";
            this._libelle.Size = new System.Drawing.Size(616, 23);
            this._libelle.TabIndex = 5;
            //
            // _info
            //
            this._info.BackColor = System.Drawing.Color.FromArgb(229, 241, 251);
            this._info.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._info.ForeColor = System.Drawing.Color.FromArgb(11, 61, 102);
            this._info.Location = new System.Drawing.Point(22, 268);
            this._info.Name = "_info";
            this._info.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this._info.Size = new System.Drawing.Size(616, 44);
            this._info.TabIndex = 6;
            this._info.Text = "Le virement est enregistré directement comme validé (n° VIR-…). Pour le corriger, il faudra l'annuler avec un motif.";
            this._info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this._btnAnnuler.TabIndex = 8;
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
            this._btnEnregistrer.Size = new System.Drawing.Size(180, 32);
            this._btnEnregistrer.TabIndex = 7;
            this._btnEnregistrer.Text = "Enregistrer le virement";
            this._btnEnregistrer.UseVisualStyleBackColor = false;
            //
            // VirementForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this._btnAnnuler;
            this.ClientSize = new System.Drawing.Size(660, 470);
            this.Name = "VirementForm";
            this.Text = "Virement entre comptes";
            this.Entete.ResumeLayout(false);
            this.Entete.PerformLayout();
            this._grpSource.ResumeLayout(false);
            this._grpSource.PerformLayout();
            this._grpDestination.ResumeLayout(false);
            this._grpDestination.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._fleche)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._montant)).EndInit();
            this.Corps.ResumeLayout(false);
            this.Corps.PerformLayout();
            this.Pied.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox _grpSource;
        private System.Windows.Forms.ComboBox _source;
        private System.Windows.Forms.Label _soldeSource;
        private System.Windows.Forms.Label _apresSource;
        private System.Windows.Forms.PictureBox _fleche;
        private System.Windows.Forms.GroupBox _grpDestination;
        private System.Windows.Forms.ComboBox _destination;
        private System.Windows.Forms.Label _soldeDest;
        private System.Windows.Forms.Label _apresDest;
        private System.Windows.Forms.Label _lblDate;
        private System.Windows.Forms.DateTimePicker _date;
        private System.Windows.Forms.Label _lblMontant;
        private System.Windows.Forms.NumericUpDown _montant;
        private System.Windows.Forms.Label _lblLibelle;
        private System.Windows.Forms.TextBox _libelle;
        private System.Windows.Forms.Label _info;
        private System.Windows.Forms.Button _btnAnnuler;
        private System.Windows.Forms.Button _btnEnregistrer;
    }
}
