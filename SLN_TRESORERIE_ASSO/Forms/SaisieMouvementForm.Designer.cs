// =====================================================================================
//  Forms/SaisieMouvementForm.Designer.cs — MISE EN PAGE DE SaisieMouvementForm (fichier géré par le concepteur)
// -------------------------------------------------------------------------------------
//  Bandeau de statut, champs de l'opération, onglets Ventilation analytique / Pièces justificatives, boutons du pied.
//
//  ➜ Pour modifier l'apparence : clic droit sur SaisieMouvementForm.cs > « Concepteur de vues »
//    (ou Maj+F7), puis changez les contrôles dans la fenêtre Propriétés.
//  ⚠ Ne tapez pas de code à la main dans InitializeComponent() : Visual Studio
//    réécrit cette méthode à chaque enregistrement du concepteur (vos ajouts et
//    commentaires y seraient perdus). Le code « logique » va dans SaisieMouvementForm.cs.
//  Les noms des contrôles (_grille, _btnValider…) sont utilisés dans SaisieMouvementForm.cs :
//  si vous en renommez un, faites-le depuis la propriété (Name) du concepteur.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Forms
{
    partial class SaisieMouvementForm
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
            System.Windows.Forms.DataGridViewCellStyle styleMontantV = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle styleDate = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle styleMontantP = new System.Windows.Forms.DataGridViewCellStyle();
            this._bandeau = new System.Windows.Forms.Label();
            this._grpOperation = new System.Windows.Forms.GroupBox();
            this._lblType = new System.Windows.Forms.Label();
            this._recette = new System.Windows.Forms.RadioButton();
            this._depense = new System.Windows.Forms.RadioButton();
            this._lblDate = new System.Windows.Forms.Label();
            this._date = new System.Windows.Forms.DateTimePicker();
            this._lblMode = new System.Windows.Forms.Label();
            this._mode = new System.Windows.Forms.ComboBox();
            this._lblCompte = new System.Windows.Forms.Label();
            this._compte = new System.Windows.Forms.ComboBox();
            this._lblCategorie = new System.Windows.Forms.Label();
            this._categorie = new System.Windows.Forms.ComboBox();
            this._lblTiers = new System.Windows.Forms.Label();
            this._tiers = new System.Windows.Forms.ComboBox();
            this._lblMontant = new System.Windows.Forms.Label();
            this._montant = new System.Windows.Forms.NumericUpDown();
            this._lblReference = new System.Windows.Forms.Label();
            this._reference = new System.Windows.Forms.TextBox();
            this._lblLibelle = new System.Windows.Forms.Label();
            this._libelle = new System.Windows.Forms.TextBox();
            this._soldes = new System.Windows.Forms.Label();
            this._onglets = new System.Windows.Forms.TabControl();
            this._pageVentilation = new System.Windows.Forms.TabPage();
            this._ventilation = new System.Windows.Forms.DataGridView();
            this._colCentre = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this._colMontantV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._panelVentilation = new System.Windows.Forms.Panel();
            this._btnReste = new System.Windows.Forms.Button();
            this._btnSupprimerV = new System.Windows.Forms.Button();
            this._totalVentile = new System.Windows.Forms.Label();
            this._resteVentile = new System.Windows.Forms.Label();
            this._pagePieces = new System.Windows.Forms.TabPage();
            this._pieces = new System.Windows.Forms.DataGridView();
            this._colType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this._colReference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colMontantP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colChemin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._panelPieces = new System.Windows.Forms.Panel();
            this._btnJoindre = new System.Windows.Forms.Button();
            this._btnOuvrir = new System.Windows.Forms.Button();
            this._btnSupprimerP = new System.Windows.Forms.Button();
            this._btnAnnuler = new System.Windows.Forms.Button();
            this._btnValider = new System.Windows.Forms.Button();
            this._btnBrouillon = new System.Windows.Forms.Button();
            this._btnFermer = new System.Windows.Forms.Button();
            this._btnValiderBrouillon = new System.Windows.Forms.Button();
            this._btnRecu = new System.Windows.Forms.Button();
            this._lblObligatoire = new System.Windows.Forms.Label();
            this.Entete.SuspendLayout();
            this.Corps.SuspendLayout();
            this.Pied.SuspendLayout();
            this._grpOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._montant)).BeginInit();
            this._onglets.SuspendLayout();
            this._pageVentilation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ventilation)).BeginInit();
            this._panelVentilation.SuspendLayout();
            this._pagePieces.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pieces)).BeginInit();
            this._panelPieces.SuspendLayout();
            this.SuspendLayout();
            //
            // LibelleTitre
            //
            this.LibelleTitre.Text = "Nouvelle dépense";
            //
            // LibelleSousTitre
            //
            this.LibelleSousTitre.Text = "Saisissez l'opération puis enregistrez-la en brouillon ou validez-la";
            //
            // Corps
            //
            this.Corps.Controls.Add(this._bandeau);
            this.Corps.Controls.Add(this._grpOperation);
            this.Corps.Controls.Add(this._onglets);
            //
            // _bandeau
            //
            this._bandeau.BackColor = System.Drawing.Color.FromArgb(255, 240, 217);
            this._bandeau.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._bandeau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._bandeau.ForeColor = System.Drawing.Color.FromArgb(138, 75, 0);
            this._bandeau.Location = new System.Drawing.Point(22, 10);
            this._bandeau.Name = "_bandeau";
            this._bandeau.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this._bandeau.Size = new System.Drawing.Size(876, 32);
            this._bandeau.TabIndex = 0;
            this._bandeau.Text = "BROUILLON — le numéro de pièce sera attribué automatiquement à la validation.";
            this._bandeau.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // _grpOperation
            //
            this._grpOperation.Controls.Add(this._lblType);
            this._grpOperation.Controls.Add(this._recette);
            this._grpOperation.Controls.Add(this._depense);
            this._grpOperation.Controls.Add(this._lblDate);
            this._grpOperation.Controls.Add(this._date);
            this._grpOperation.Controls.Add(this._lblMode);
            this._grpOperation.Controls.Add(this._mode);
            this._grpOperation.Controls.Add(this._lblCompte);
            this._grpOperation.Controls.Add(this._compte);
            this._grpOperation.Controls.Add(this._lblCategorie);
            this._grpOperation.Controls.Add(this._categorie);
            this._grpOperation.Controls.Add(this._lblTiers);
            this._grpOperation.Controls.Add(this._tiers);
            this._grpOperation.Controls.Add(this._lblMontant);
            this._grpOperation.Controls.Add(this._montant);
            this._grpOperation.Controls.Add(this._lblReference);
            this._grpOperation.Controls.Add(this._reference);
            this._grpOperation.Controls.Add(this._lblLibelle);
            this._grpOperation.Controls.Add(this._libelle);
            this._grpOperation.Controls.Add(this._soldes);
            this._grpOperation.Location = new System.Drawing.Point(22, 50);
            this._grpOperation.Name = "_grpOperation";
            this._grpOperation.Size = new System.Drawing.Size(876, 268);
            this._grpOperation.TabIndex = 1;
            this._grpOperation.TabStop = false;
            this._grpOperation.Text = "Opération";
            //
            // _lblType
            //
            this._lblType.AutoSize = true;
            this._lblType.Location = new System.Drawing.Point(16, 22);
            this._lblType.Name = "_lblType";
            this._lblType.Text = "Type *";
            //
            // _recette
            //
            this._recette.AutoSize = true;
            this._recette.Location = new System.Drawing.Point(16, 44);
            this._recette.Name = "_recette";
            this._recette.TabIndex = 0;
            this._recette.Text = "Recette";
            //
            // _depense
            //
            this._depense.AutoSize = true;
            this._depense.Checked = true;
            this._depense.Location = new System.Drawing.Point(100, 44);
            this._depense.Name = "_depense";
            this._depense.TabIndex = 1;
            this._depense.TabStop = true;
            this._depense.Text = "Dépense";
            //
            // _lblDate
            //
            this._lblDate.AutoSize = true;
            this._lblDate.Location = new System.Drawing.Point(306, 22);
            this._lblDate.Name = "_lblDate";
            this._lblDate.Text = "Date *";
            //
            // _date
            //
            this._date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._date.Location = new System.Drawing.Point(306, 42);
            this._date.Name = "_date";
            this._date.Size = new System.Drawing.Size(150, 23);
            this._date.TabIndex = 2;
            //
            // _lblMode
            //
            this._lblMode.AutoSize = true;
            this._lblMode.Location = new System.Drawing.Point(596, 22);
            this._lblMode.Name = "_lblMode";
            this._lblMode.Text = "Mode de paiement *";
            //
            // _mode
            //
            this._mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._mode.Items.AddRange(new object[] {
            "Espèces",
            "Chèque",
            "Virement bancaire",
            "Mobile Money",
            "Carte bancaire"});
            this._mode.Location = new System.Drawing.Point(596, 42);
            this._mode.Name = "_mode";
            this._mode.Size = new System.Drawing.Size(262, 23);
            this._mode.TabIndex = 3;
            //
            // _lblCompte
            //
            this._lblCompte.AutoSize = true;
            this._lblCompte.Location = new System.Drawing.Point(16, 76);
            this._lblCompte.Name = "_lblCompte";
            this._lblCompte.Text = "Compte *";
            //
            // _compte
            //
            this._compte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._compte.Location = new System.Drawing.Point(16, 96);
            this._compte.Name = "_compte";
            this._compte.Size = new System.Drawing.Size(270, 23);
            this._compte.TabIndex = 4;
            //
            // _lblCategorie
            //
            this._lblCategorie.AutoSize = true;
            this._lblCategorie.Location = new System.Drawing.Point(306, 76);
            this._lblCategorie.Name = "_lblCategorie";
            this._lblCategorie.Text = "Catégorie *";
            //
            // _categorie
            //
            this._categorie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._categorie.Location = new System.Drawing.Point(306, 96);
            this._categorie.Name = "_categorie";
            this._categorie.Size = new System.Drawing.Size(270, 23);
            this._categorie.TabIndex = 5;
            //
            // _lblTiers
            //
            this._lblTiers.AutoSize = true;
            this._lblTiers.Location = new System.Drawing.Point(596, 76);
            this._lblTiers.Name = "_lblTiers";
            this._lblTiers.Text = "Tiers";
            //
            // _tiers
            //
            this._tiers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._tiers.Location = new System.Drawing.Point(596, 96);
            this._tiers.Name = "_tiers";
            this._tiers.Size = new System.Drawing.Size(262, 23);
            this._tiers.TabIndex = 6;
            //
            // _lblMontant
            //
            this._lblMontant.AutoSize = true;
            this._lblMontant.Location = new System.Drawing.Point(16, 130);
            this._lblMontant.Name = "_lblMontant";
            this._lblMontant.Text = "Montant (FCFA) *";
            //
            // _montant
            //
            this._montant.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._montant.Location = new System.Drawing.Point(16, 150);
            this._montant.Maximum = new decimal(new int[] { -727379969, 232, 0, 0 });
            this._montant.Name = "_montant";
            this._montant.Size = new System.Drawing.Size(180, 23);
            this._montant.TabIndex = 7;
            this._montant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._montant.ThousandsSeparator = true;
            //
            // _lblReference
            //
            this._lblReference.AutoSize = true;
            this._lblReference.Location = new System.Drawing.Point(306, 130);
            this._lblReference.Name = "_lblReference";
            this._lblReference.Text = "Référence externe (n° de reçu, de chèque, de transaction…)";
            //
            // _reference
            //
            this._reference.Location = new System.Drawing.Point(306, 150);
            this._reference.MaxLength = 50;
            this._reference.Name = "_reference";
            this._reference.Size = new System.Drawing.Size(552, 23);
            this._reference.TabIndex = 8;
            //
            // _lblLibelle
            //
            this._lblLibelle.AutoSize = true;
            this._lblLibelle.Location = new System.Drawing.Point(16, 184);
            this._lblLibelle.Name = "_lblLibelle";
            this._lblLibelle.Text = "Libellé *";
            //
            // _libelle
            //
            this._libelle.Location = new System.Drawing.Point(16, 204);
            this._libelle.MaxLength = 255;
            this._libelle.Name = "_libelle";
            this._libelle.Size = new System.Drawing.Size(842, 23);
            this._libelle.TabIndex = 9;
            //
            // _soldes
            //
            this._soldes.AutoSize = true;
            this._soldes.ForeColor = System.Drawing.Color.FromArgb(74, 74, 74);
            this._soldes.Location = new System.Drawing.Point(16, 238);
            this._soldes.Name = "_soldes";
            this._soldes.Text = "Solde du compte : 0        Après l'opération : 0        Découvert autorisé : 0";
            //
            // _onglets
            //
            this._onglets.Controls.Add(this._pageVentilation);
            this._onglets.Controls.Add(this._pagePieces);
            this._onglets.Location = new System.Drawing.Point(22, 326);
            this._onglets.Name = "_onglets";
            this._onglets.Padding = new System.Drawing.Point(14, 4);
            this._onglets.SelectedIndex = 0;
            this._onglets.Size = new System.Drawing.Size(876, 222);
            this._onglets.TabIndex = 2;
            //
            // _pageVentilation
            //
            this._pageVentilation.BackColor = System.Drawing.Color.White;
            this._pageVentilation.Controls.Add(this._ventilation);
            this._pageVentilation.Controls.Add(this._panelVentilation);
            this._pageVentilation.Name = "_pageVentilation";
            this._pageVentilation.Padding = new System.Windows.Forms.Padding(8);
            this._pageVentilation.TabIndex = 0;
            this._pageVentilation.Text = "Ventilation analytique";
            //
            // _ventilation
            //
            this._ventilation.BackgroundColor = System.Drawing.Color.White;
            this._ventilation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._ventilation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colCentre,
            this._colMontantV});
            this._ventilation.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ventilation.Name = "_ventilation";
            this._ventilation.RowHeadersVisible = false;
            this._ventilation.TabIndex = 0;
            //
            // _colCentre
            //
            this._colCentre.FillWeight = 200F;
            this._colCentre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._colCentre.HeaderText = "Centre analytique";
            this._colCentre.Name = "_colCentre";
            //
            // _colMontantV
            //
            styleMontantV.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            styleMontantV.Format = "N0";
            this._colMontantV.DefaultCellStyle = styleMontantV;
            this._colMontantV.HeaderText = "Montant";
            this._colMontantV.Name = "_colMontantV";
            //
            // _panelVentilation
            //
            this._panelVentilation.Controls.Add(this._btnReste);
            this._panelVentilation.Controls.Add(this._btnSupprimerV);
            this._panelVentilation.Controls.Add(this._totalVentile);
            this._panelVentilation.Controls.Add(this._resteVentile);
            this._panelVentilation.Dock = System.Windows.Forms.DockStyle.Right;
            this._panelVentilation.Name = "_panelVentilation";
            this._panelVentilation.Size = new System.Drawing.Size(190, 178);
            this._panelVentilation.TabIndex = 1;
            //
            // _btnReste
            //
            this._btnReste.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this._btnReste.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 168, 168);
            this._btnReste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnReste.Location = new System.Drawing.Point(10, 0);
            this._btnReste.Name = "_btnReste";
            this._btnReste.Size = new System.Drawing.Size(170, 32);
            this._btnReste.TabIndex = 0;
            this._btnReste.Text = "Mettre le reste ici";
            this._btnReste.UseVisualStyleBackColor = false;
            //
            // _btnSupprimerV
            //
            this._btnSupprimerV.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this._btnSupprimerV.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 168, 168);
            this._btnSupprimerV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnSupprimerV.Location = new System.Drawing.Point(10, 38);
            this._btnSupprimerV.Name = "_btnSupprimerV";
            this._btnSupprimerV.Size = new System.Drawing.Size(170, 32);
            this._btnSupprimerV.TabIndex = 1;
            this._btnSupprimerV.Text = "Supprimer la ligne";
            this._btnSupprimerV.UseVisualStyleBackColor = false;
            //
            // _totalVentile
            //
            this._totalVentile.AutoSize = true;
            this._totalVentile.Location = new System.Drawing.Point(10, 84);
            this._totalVentile.Name = "_totalVentile";
            this._totalVentile.Text = "Ventilé : 0 / 0";
            //
            // _resteVentile
            //
            this._resteVentile.AutoSize = true;
            this._resteVentile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._resteVentile.Location = new System.Drawing.Point(10, 106);
            this._resteVentile.Name = "_resteVentile";
            this._resteVentile.Text = "Reste à ventiler : 0";
            //
            // _pagePieces
            //
            this._pagePieces.BackColor = System.Drawing.Color.White;
            this._pagePieces.Controls.Add(this._pieces);
            this._pagePieces.Controls.Add(this._panelPieces);
            this._pagePieces.Name = "_pagePieces";
            this._pagePieces.Padding = new System.Windows.Forms.Padding(8);
            this._pagePieces.TabIndex = 1;
            this._pagePieces.Text = "Pièces justificatives";
            //
            // _pieces
            //
            this._pieces.BackgroundColor = System.Drawing.Color.White;
            this._pieces.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._pieces.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colType,
            this._colReference,
            this._colDate,
            this._colMontantP,
            this._colChemin});
            this._pieces.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pieces.Name = "_pieces";
            this._pieces.RowHeadersVisible = false;
            this._pieces.TabIndex = 0;
            //
            // _colType
            //
            this._colType.FillWeight = 80F;
            this._colType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._colType.HeaderText = "Type";
            this._colType.Items.AddRange(new object[] {
            "FACTURE",
            "RECU",
            "BON_DE_CAISSE",
            "RELEVE",
            "CONTRAT",
            "AUTRE"});
            this._colType.Name = "_colType";
            //
            // _colReference
            //
            this._colReference.FillWeight = 80F;
            this._colReference.HeaderText = "Référence";
            this._colReference.Name = "_colReference";
            //
            // _colDate
            //
            styleDate.Format = "dd/MM/yyyy";
            this._colDate.DefaultCellStyle = styleDate;
            this._colDate.FillWeight = 60F;
            this._colDate.HeaderText = "Date";
            this._colDate.Name = "_colDate";
            //
            // _colMontantP
            //
            styleMontantP.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            styleMontantP.Format = "N0";
            this._colMontantP.DefaultCellStyle = styleMontantP;
            this._colMontantP.FillWeight = 60F;
            this._colMontantP.HeaderText = "Montant";
            this._colMontantP.Name = "_colMontantP";
            //
            // _colChemin
            //
            this._colChemin.FillWeight = 200F;
            this._colChemin.HeaderText = "Fichier";
            this._colChemin.Name = "_colChemin";
            this._colChemin.ReadOnly = true;
            //
            // _panelPieces
            //
            this._panelPieces.Controls.Add(this._btnJoindre);
            this._panelPieces.Controls.Add(this._btnOuvrir);
            this._panelPieces.Controls.Add(this._btnSupprimerP);
            this._panelPieces.Dock = System.Windows.Forms.DockStyle.Right;
            this._panelPieces.Name = "_panelPieces";
            this._panelPieces.Size = new System.Drawing.Size(190, 178);
            this._panelPieces.TabIndex = 1;
            //
            // _btnJoindre
            //
            this._btnJoindre.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this._btnJoindre.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 168, 168);
            this._btnJoindre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnJoindre.Location = new System.Drawing.Point(10, 0);
            this._btnJoindre.Name = "_btnJoindre";
            this._btnJoindre.Size = new System.Drawing.Size(170, 32);
            this._btnJoindre.TabIndex = 0;
            this._btnJoindre.Text = "Joindre un fichier…";
            this._btnJoindre.UseVisualStyleBackColor = false;
            //
            // _btnOuvrir
            //
            this._btnOuvrir.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this._btnOuvrir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 168, 168);
            this._btnOuvrir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnOuvrir.Location = new System.Drawing.Point(10, 38);
            this._btnOuvrir.Name = "_btnOuvrir";
            this._btnOuvrir.Size = new System.Drawing.Size(170, 32);
            this._btnOuvrir.TabIndex = 1;
            this._btnOuvrir.Text = "Ouvrir le fichier";
            this._btnOuvrir.UseVisualStyleBackColor = false;
            //
            // _btnSupprimerP
            //
            this._btnSupprimerP.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this._btnSupprimerP.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 168, 168);
            this._btnSupprimerP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnSupprimerP.Location = new System.Drawing.Point(10, 76);
            this._btnSupprimerP.Name = "_btnSupprimerP";
            this._btnSupprimerP.Size = new System.Drawing.Size(170, 32);
            this._btnSupprimerP.TabIndex = 2;
            this._btnSupprimerP.Text = "Supprimer la pièce";
            this._btnSupprimerP.UseVisualStyleBackColor = false;
            //
            // Pied
            //
            this.Pied.Controls.Add(this._btnAnnuler);
            this.Pied.Controls.Add(this._btnValider);
            this.Pied.Controls.Add(this._btnBrouillon);
            this.Pied.Controls.Add(this._btnFermer);
            this.Pied.Controls.Add(this._btnRecu);
            this.Pied.Controls.Add(this._btnValiderBrouillon);
            this.Pied.Controls.Add(this._lblObligatoire);
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
            this._btnAnnuler.TabIndex = 12;
            this._btnAnnuler.Text = "Annuler";
            this._btnAnnuler.UseVisualStyleBackColor = false;
            //
            // _btnValider
            //
            this._btnValider.BackColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnValider.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnValider.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnValider.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 74, 140);
            this._btnValider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnValider.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnValider.ForeColor = System.Drawing.Color.White;
            this._btnValider.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnValider.Name = "_btnValider";
            this._btnValider.Size = new System.Drawing.Size(170, 32);
            this._btnValider.TabIndex = 11;
            this._btnValider.Text = "Enregistrer et valider";
            this._btnValider.UseVisualStyleBackColor = false;
            //
            // _btnBrouillon
            //
            this._btnBrouillon.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this._btnBrouillon.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnBrouillon.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 168, 168);
            this._btnBrouillon.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(232, 240, 250);
            this._btnBrouillon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnBrouillon.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnBrouillon.Name = "_btnBrouillon";
            this._btnBrouillon.Size = new System.Drawing.Size(170, 32);
            this._btnBrouillon.TabIndex = 10;
            this._btnBrouillon.Text = "Enregistrer en brouillon";
            this._btnBrouillon.UseVisualStyleBackColor = false;
            //
            // _btnFermer
            //
            this._btnFermer.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this._btnFermer.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnFermer.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnFermer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 168, 168);
            this._btnFermer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnFermer.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnFermer.Name = "_btnFermer";
            this._btnFermer.Size = new System.Drawing.Size(110, 32);
            this._btnFermer.TabIndex = 13;
            this._btnFermer.Text = "Fermer";
            this._btnFermer.UseVisualStyleBackColor = false;
            this._btnFermer.Visible = false;
            //
            // _btnValiderBrouillon
            //
            this._btnValiderBrouillon.BackColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnValiderBrouillon.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnValiderBrouillon.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 103, 192);
            this._btnValiderBrouillon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnValiderBrouillon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnValiderBrouillon.ForeColor = System.Drawing.Color.White;
            this._btnValiderBrouillon.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnValiderBrouillon.Name = "_btnValiderBrouillon";
            this._btnValiderBrouillon.Size = new System.Drawing.Size(170, 32);
            this._btnValiderBrouillon.TabIndex = 14;
            this._btnValiderBrouillon.Text = "Valider ce brouillon";
            this._btnValiderBrouillon.UseVisualStyleBackColor = false;
            this._btnValiderBrouillon.Visible = false;
            //
            // _btnRecu
            //
            this._btnRecu.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this._btnRecu.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnRecu.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(168, 168, 168);
            this._btnRecu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnRecu.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._btnRecu.Name = "_btnRecu";
            this._btnRecu.Size = new System.Drawing.Size(150, 32);
            this._btnRecu.TabIndex = 15;
            this._btnRecu.Text = "Imprimer le reçu";
            this._btnRecu.UseVisualStyleBackColor = false;
            this._btnRecu.Visible = false;
            //
            // _lblObligatoire
            //
            this._lblObligatoire.AutoSize = true;
            this._lblObligatoire.ForeColor = System.Drawing.Color.FromArgb(74, 74, 74);
            this._lblObligatoire.Margin = new System.Windows.Forms.Padding(0, 8, 80, 0);
            this._lblObligatoire.Name = "_lblObligatoire";
            this._lblObligatoire.Text = "* champs obligatoires";
            //
            // SaisieMouvementForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this._btnAnnuler;
            this.ClientSize = new System.Drawing.Size(920, 680);
            this.Name = "SaisieMouvementForm";
            this.Text = "Nouvelle dépense";
            this.Entete.ResumeLayout(false);
            this.Entete.PerformLayout();
            this._grpOperation.ResumeLayout(false);
            this._grpOperation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._montant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ventilation)).EndInit();
            this._panelVentilation.ResumeLayout(false);
            this._panelVentilation.PerformLayout();
            this._pageVentilation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._pieces)).EndInit();
            this._panelPieces.ResumeLayout(false);
            this._pagePieces.ResumeLayout(false);
            this._onglets.ResumeLayout(false);
            this.Corps.ResumeLayout(false);
            this.Pied.ResumeLayout(false);
            this.Pied.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _bandeau;
        private System.Windows.Forms.GroupBox _grpOperation;
        private System.Windows.Forms.Label _lblType;
        private System.Windows.Forms.RadioButton _recette;
        private System.Windows.Forms.RadioButton _depense;
        private System.Windows.Forms.Label _lblDate;
        private System.Windows.Forms.DateTimePicker _date;
        private System.Windows.Forms.Label _lblMode;
        private System.Windows.Forms.ComboBox _mode;
        private System.Windows.Forms.Label _lblCompte;
        private System.Windows.Forms.ComboBox _compte;
        private System.Windows.Forms.Label _lblCategorie;
        private System.Windows.Forms.ComboBox _categorie;
        private System.Windows.Forms.Label _lblTiers;
        private System.Windows.Forms.ComboBox _tiers;
        private System.Windows.Forms.Label _lblMontant;
        private System.Windows.Forms.NumericUpDown _montant;
        private System.Windows.Forms.Label _lblReference;
        private System.Windows.Forms.TextBox _reference;
        private System.Windows.Forms.Label _lblLibelle;
        private System.Windows.Forms.TextBox _libelle;
        private System.Windows.Forms.Label _soldes;
        private System.Windows.Forms.TabControl _onglets;
        private System.Windows.Forms.TabPage _pageVentilation;
        private System.Windows.Forms.DataGridView _ventilation;
        private System.Windows.Forms.DataGridViewComboBoxColumn _colCentre;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colMontantV;
        private System.Windows.Forms.Panel _panelVentilation;
        private System.Windows.Forms.Button _btnReste;
        private System.Windows.Forms.Button _btnSupprimerV;
        private System.Windows.Forms.Label _totalVentile;
        private System.Windows.Forms.Label _resteVentile;
        private System.Windows.Forms.TabPage _pagePieces;
        private System.Windows.Forms.DataGridView _pieces;
        private System.Windows.Forms.DataGridViewComboBoxColumn _colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colReference;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colMontantP;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colChemin;
        private System.Windows.Forms.Panel _panelPieces;
        private System.Windows.Forms.Button _btnJoindre;
        private System.Windows.Forms.Button _btnOuvrir;
        private System.Windows.Forms.Button _btnSupprimerP;
        private System.Windows.Forms.Button _btnAnnuler;
        private System.Windows.Forms.Button _btnValider;
        private System.Windows.Forms.Button _btnBrouillon;
        private System.Windows.Forms.Button _btnFermer;
        private System.Windows.Forms.Button _btnValiderBrouillon;
        private System.Windows.Forms.Button _btnRecu;
        private System.Windows.Forms.Label _lblObligatoire;
    }
}
