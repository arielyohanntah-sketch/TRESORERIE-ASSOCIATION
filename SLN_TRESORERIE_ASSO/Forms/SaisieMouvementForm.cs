// =====================================================================================
//  Forms/SaisieMouvementForm.cs — SAISIE / CONSULTATION D'UNE RECETTE OU D'UNE DÉPENSE
// -------------------------------------------------------------------------------------
//  Fenêtre la plus complète de l'application. Contrôles (SaisieMouvementForm.Designer.cs) :
//    _bandeau        bandeau de statut (BROUILLON orange / VALIDÉ vert / ANNULÉ gris)
//    _recette/_depense, _date, _mode, _compte, _categorie, _tiers, _montant,
//    _reference, _libelle            champs de l'opération
//    _soldes         solde du compte avant / après l'opération
//    Onglet « Ventilation analytique » : grille _ventilation (colonnes _colCentre, _colMontantV),
//                    _totalVentile, _resteVentile, boutons _btnReste et _btnSupprimerV
//    Onglet « Pièces justificatives »  : grille _pieces (colonnes _colType, _colReference,
//                    _colDate, _colMontantP, _colChemin), boutons _btnJoindre, _btnOuvrir, _btnSupprimerP
//    Pied : _btnValider, _btnBrouillon, _btnAnnuler, _btnFermer, _btnValiderBrouillon
//
//  Trois situations :
//    1. Nouvelle opération           → tout est modifiable
//    2. Mon brouillon                → modifiable (seul l'auteur peut le modifier)
//    3. Opération validée/annulée, ou brouillon d'un autre → lecture seule
//       (un valideur voit alors le bouton « Valider ce brouillon »)
//
//  Ce que fait « Valider » : Depot.EnregistrerMouvement(_m, true) → la base contrôle
//  le solde et attribue le numéro de pièce ; la fenêtre se ferme et MainForm affiche
//  la notification contenue dans Resultat.
// =====================================================================================

using System.Data;
using System.Diagnostics;
using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Forms;

/// <summary>
/// Saisie ou consultation d'une recette / dépense : champs de l'opération,
/// ventilation analytique et pièces justificatives.
/// La mise en page est dans SaisieMouvementForm.Designer.cs (modifiable dans le concepteur).
/// </summary>
public partial class SaisieMouvementForm : DialogueBase
{
    /// <summary>Codes des modes de paiement, dans le même ordre que la liste déroulante du concepteur.</summary>
    private static readonly string[] Modes = { "ESPECES", "CHEQUE", "VIREMENT", "MOBILE_MONEY", "CARTE" };

    // Le mouvement affiché (nouveau ou chargé depuis la base)
    private readonly MouvementSaisie _m;
    // Vrai si l'utilisateur ne peut que consulter
    private readonly bool _lectureSeule;
    // Liste des centres analytiques (colonne « Centre » de la ventilation)
    private DataTable _centres = new();

    /// <summary>Message de notification à afficher après la fermeture (titre, texte, type).</summary>
    public (string Titre, string Message, TypeNotification Type)? Resultat { get; private set; }
    // Id du mouvement enregistré (pour le surligner dans la liste)
    public int? IdEnregistre { get; private set; }
    // Vrai après la validation d'une recette : la fenêtre principale propose d'imprimer le reçu
    public bool ProposerRecu { get; private set; }

    /// <summary>Constructeur utilisé par le concepteur de Visual Studio.</summary>
    public SaisieMouvementForm() : this(new MouvementSaisie()) { }

    /// <summary>Nouvelle opération (« RECETTE » ou « DEPENSE »).</summary>
    public SaisieMouvementForm(string type) : this(new MouvementSaisie { Type = type }) { }

    /// <summary>Ouverture d'une opération existante (modifiable seulement par l'auteur d'un brouillon).</summary>
    public SaisieMouvementForm(int id) : this(Depot.ChargerMouvement(id)) { }

    // Constructeur commun aux trois précédents
    private SaisieMouvementForm(MouvementSaisie m)
    {
        InitializeComponent();
        _m = m;
        _lectureSeule = !m.ModifiableParMoi;

        // Bandeau du haut
        DefinirIcone(m.Type == "RECETTE" ? Theme.IcoRecette : Theme.IcoDepense);
        Text = LibelleTitre.Text = m.Id == null
            ? (m.Type == "RECETTE" ? "Nouvelle recette" : "Nouvelle dépense")
            : $"Mouvement {m.NumeroPiece ?? "(brouillon)"}";
        LibelleSousTitre.Text = m.Id == null
            ? "Saisissez l'opération puis enregistrez-la en brouillon ou validez-la"
            : $"Saisi par {m.NomAuteur} le {m.DateSaisie:dd/MM/yyyy à HH:mm}" + (m.ModifiableParMoi ? " · encore modifiable" : " · consultation seulement");

        // Bandeau de statut
        var (texte, fond, couleur) = m.Statut switch
        {
            "VALIDE" => ($"VALIDÉ — pièce n° {m.NumeroPiece}. Pour corriger, annulez la pièce puis ressaisissez-la.", Theme.VertClair, Theme.Vert),
            "ANNULE" => ($"ANNULÉ — pièce n° {m.NumeroPiece}.", Theme.GrisClair, Theme.Gris),
            _ => ("BROUILLON — le numéro de pièce sera attribué automatiquement à la validation.", Theme.OrangeClair, Theme.Orange)
        };
        _bandeau.Text = texte;
        _bandeau.BackColor = fond;
        _bandeau.ForeColor = couleur;

        // Grilles (ajusterColonnes: false car leurs colonnes sont définies dans le concepteur)
        Theme.StyleGrille(_ventilation, lectureSeule: _lectureSeule, ajusterColonnes: false);
        _ventilation.SelectionMode = DataGridViewSelectionMode.CellSelect;
        _colMontantV.ValueType = typeof(decimal);
        Theme.StyleGrille(_pieces, lectureSeule: _lectureSeule, ajusterColonnes: false);
        // Les pièces s'ajoutent avec le bouton « Joindre un fichier », pas par une ligne vide
        _pieces.AllowUserToAddRows = false;
        _pieces.SelectionMode = DataGridViewSelectionMode.CellSelect;
        _colDate.ValueType = typeof(DateTime);
        _colMontantP.ValueType = typeof(decimal);
        // Recalcul du « reste à ventiler » à chaque modification
        _ventilation.CellValueChanged += (_, _) => MajVentilation();
        _ventilation.RowsRemoved += (_, _) => MajVentilation();

        // Boutons selon le rôle et le statut
        _btnReste.Enabled = _btnSupprimerV.Enabled = _btnJoindre.Enabled = _btnSupprimerP.Enabled = !_lectureSeule;
        if (_lectureSeule)
        {
            // Consultation : seul le bouton « Fermer » reste
            _btnAnnuler.Visible = _btnValider.Visible = _btnBrouillon.Visible = _lblObligatoire.Visible = false;
            _btnFermer.Visible = true;
            CancelButton = _btnFermer;
            // Un valideur peut valider le brouillon d'un autre sans pouvoir le modifier
            _btnValiderBrouillon.Visible = m.Statut == "BROUILLON" && m.Id != null && Session.Peut("VALIDER");
            // Opération validée (ou annulée) : on peut imprimer (ou réimprimer) son reçu
            _btnRecu.Image = Theme.Glyphe(Theme.IcoImprimer, Theme.Nuit, 16);
            _btnRecu.TextImageRelation = TextImageRelation.ImageBeforeText;
            _btnRecu.Visible = m.Id != null && m.Statut is "VALIDE" or "ANNULE";
        }
        else
        {
            // « Valider » seulement pour les rôles qui ont ce droit ; sinon le brouillon devient le bouton principal
            _btnValider.Visible = Session.Peut("VALIDER");
            if (!Session.Peut("VALIDER"))
            {
                _btnBrouillon.Text = "Enregistrer (à faire valider)";
                _btnBrouillon.Width = _btnBrouillon.Width * 200 / 170;
                Theme.StylePrincipal(_btnBrouillon);
            }
        }

        // Branchement des clics sur les boutons
        _btnValider.Click += (_, _) => Enregistrer(true);
        _btnBrouillon.Click += (_, _) => Enregistrer(false);
        _btnValiderBrouillon.Click += (_, _) => { if (_m.Id is int id) ValiderBrouillon(id); };
        _btnRecu.Click += (_, _) => { if (_m.Id is int id) ApercuRecuForm.Afficher(this, id); };
        _btnReste.Click += (_, _) => MettreLeReste();
        _btnSupprimerV.Click += (_, _) =>
        {
            if (_ventilation.CurrentRow is { IsNewRow: false } r) _ventilation.Rows.Remove(r);
        };
        _btnJoindre.Click += (_, _) => JoindreFichier();
        _btnOuvrir.Click += (_, _) => OuvrirFichier();
        _btnSupprimerP.Click += (_, _) =>
        {
            if (_pieces.CurrentRow != null) _pieces.Rows.Remove(_pieces.CurrentRow);
        };

        Load += (_, _) => Charger();
    }

    // ------------------------------------------------------------------ chargement

    // Remplit les listes déroulantes puis recopie les valeurs du mouvement dans les champs
    private void Charger()
    {
        try
        {
            // Listes : comptes, tiers et centres analytiques
            _compte.DisplayMember = "lib";
            _compte.ValueMember = "id";
            _compte.DataSource = Depot.Comptes();
            _tiers.DisplayMember = "lib";
            _tiers.ValueMember = "id";
            _tiers.DataSource = Depot.Tiers();
            _centres = Depot.Centres();
            _colCentre.DataSource = _centres;
            _colCentre.DisplayMember = "lib";
            _colCentre.ValueMember = "id";

            // Recette ou dépense : la liste des catégories change selon le choix
            _recette.Checked = _m.Type == "RECETTE";
            _depense.Checked = _m.Type != "RECETTE";
            ChargerCategories();
            _recette.CheckedChanged += (_, _) => ChargerCategories();

            _date.Value = _m.Date;
            _mode.SelectedIndex = Math.Max(0, Array.IndexOf(Modes, _m.ModePaiement));
            if (_m.Id != null)
            {
                // Mouvement existant : on remplit tous les champs et les deux grilles
                SelectionnerId(_compte, _m.IdCompte);
                SelectionnerId(_categorie, _m.IdCategorie);
                if (_m.IdTiers is int t) SelectionnerId(_tiers, t);
                _montant.Value = Math.Min(_montant.Maximum, _m.Montant);
                _reference.Text = _m.ReferenceExterne;
                _libelle.Text = _m.Libelle;
                foreach (var v in _m.Ventilations)
                    _ventilation.Rows.Add(Convert.ToUInt32(v.IdCentre), v.Montant);
                foreach (var p in _m.Pieces)
                    _pieces.Rows.Add(p.TypePiece, p.Reference ?? "", p.DatePiece, p.Montant.HasValue ? p.Montant.Value : DBNull.Value, p.CheminFichier ?? "");
            }

            // Recalcul des soldes et de la ventilation quand le compte ou le montant change
            _compte.SelectedIndexChanged += (_, _) => MajSoldes();
            _montant.ValueChanged += (_, _) => { MajSoldes(); MajVentilation(); };
            MajSoldes();
            MajVentilation();

            // Lecture seule : tous les champs sont grisés
            if (_lectureSeule)
                foreach (Control c in new Control[] { _recette, _depense, _date, _mode, _compte, _categorie, _tiers, _montant, _reference, _libelle })
                    c.Enabled = false;
            if (_compte.Items.Count == 0)
                Ui.Info("Aucun compte actif : créez d'abord une caisse ou un compte dans Paramétrage > Fiches des comptes.");
        }
        catch (Exception ex)
        {
            Ui.Erreur(ex, "Chargement impossible");
        }
    }

    // Recharge les catégories du sens choisi (recette ou dépense) en gardant la sélection si possible
    private void ChargerCategories()
    {
        var ancienne = Ui.IdSelectionne(_categorie);
        _categorie.DisplayMember = "lib";
        _categorie.ValueMember = "id";
        _categorie.DataSource = Depot.Categories(_recette.Checked ? "RECETTE" : "DEPENSE");
        if (ancienne is int id) SelectionnerId(_categorie, id);
        if (_m.Id == null) Text = LibelleTitre.Text = _recette.Checked ? "Nouvelle recette" : "Nouvelle dépense";
    }

    // Sélectionne dans la ComboBox la ligne dont l'id vaut « id »
    private static void SelectionnerId(ComboBox c, int id)
    {
        if (c.DataSource is not DataTable dt) return;
        foreach (DataRow r in dt.Rows)
            if (r["id"] is not DBNull && Convert.ToInt32(r["id"]) == id)
            {
                c.SelectedValue = r["id"];
                return;
            }
    }

    // Affiche le solde du compte choisi et le solde après l'opération (en rouge si découvert dépassé)
    private void MajSoldes()
    {
        if (_compte.SelectedItem is not DataRowView r)
        {
            _soldes.Text = "";
            return;
        }
        decimal solde = Convert.ToDecimal(r["solde"]);
        decimal decouvert = Convert.ToDecimal(r["decouvert"]);
        decimal apres = _lectureSeule ? solde : solde + (_recette.Checked ? _montant.Value : -_montant.Value);
        _soldes.Text = _lectureSeule
            ? $"Solde actuel du compte : {Theme.Montant(solde)}"
            : $"Solde du compte : {Theme.Montant(solde)}        Après l'opération : {Theme.Montant(apres)}        Découvert autorisé : {Theme.Montant(decouvert)}";
        _soldes.ForeColor = apres < -decouvert ? Theme.Rouge : Theme.TexteSecondaire;
    }

    // Somme des montants saisis dans la grille de ventilation
    private decimal TotalVentile() => _ventilation.Rows.Cast<DataGridViewRow>()
        .Where(r => !r.IsNewRow && r.Cells[_colMontantV.Index].Value is not null and not DBNull)
        .Sum(r => Convert.ToDecimal(r.Cells[_colMontantV.Index].Value));

    // Met à jour « Ventilé : x / y » et « Reste à ventiler » (vert = tout ventilé, rouge = trop ventilé)
    private void MajVentilation()
    {
        decimal total = TotalVentile();
        decimal reste = _montant.Value - total;
        _totalVentile.Text = $"Ventilé : {Theme.Montant(total)} / {Theme.Montant(_montant.Value)}";
        _resteVentile.Text = $"Reste à ventiler : {Theme.Montant(reste)}";
        _resteVentile.ForeColor = reste < 0 ? Theme.Rouge : reste == 0 ? Theme.Vert : Theme.Orange;
    }

    // Bouton « Mettre le reste » : ajoute sur la ligne sélectionnée le montant qui reste à ventiler
    private void MettreLeReste()
    {
        // Termine la saisie en cours dans la cellule avant de calculer
        _ventilation.EndEdit();
        var ligne = _ventilation.CurrentRow;
        if (ligne == null || ligne.IsNewRow || ligne.Cells[_colCentre.Index].Value is null or DBNull)
        {
            Ui.Info("Choisissez d'abord un centre analytique sur une ligne, puis cliquez sur ce bouton.");
            return;
        }
        var cellule = ligne.Cells[_colMontantV.Index];
        decimal actuel = cellule.Value is null or DBNull ? 0 : Convert.ToDecimal(cellule.Value);
        decimal reste = _montant.Value - TotalVentile();
        cellule.Value = Math.Max(0, actuel + reste);
    }

    // Bouton « Joindre un fichier » : ajoute une ligne de pièce avec le chemin du fichier choisi
    private void JoindreFichier()
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Choisir une pièce justificative",
            Filter = "Documents (PDF, images)|*.pdf;*.jpg;*.jpeg;*.png|Tous les fichiers|*.*"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        // Type deviné d'après le nom du fichier (modifiable ensuite dans la grille)
        var type = Path.GetFileName(dlg.FileName).Contains("facture", StringComparison.OrdinalIgnoreCase) ? "FACTURE" : "RECU";
        // Remarque : seul le CHEMIN est enregistré dans la base, pas le fichier lui-même
        _pieces.Rows.Add(type, Path.GetFileNameWithoutExtension(dlg.FileName), DateTime.Today, DBNull.Value, dlg.FileName);
    }

    // Bouton « Ouvrir » : ouvre le fichier de la pièce sélectionnée avec le programme par défaut de Windows
    private void OuvrirFichier()
    {
        var chemin = _pieces.CurrentRow?.Cells[_colChemin.Index].Value?.ToString();
        if (string.IsNullOrEmpty(chemin) || !File.Exists(chemin))
        {
            Ui.Info("Fichier introuvable sur ce poste.");
            return;
        }
        Process.Start(new ProcessStartInfo(chemin) { UseShellExecute = true });
    }

    // ------------------------------------------------------------------ enregistrement

    // Bouton « Valider ce brouillon » (valideur qui consulte le brouillon d'un autre)
    private void ValiderBrouillon(int id)
    {
        if (!Ui.Confirmer($"Valider ce brouillon saisi par {_m.NomAuteur} ?\n\nIl ne pourra plus être modifié, seulement annulé.")) return;
        try
        {
            Depot.ValiderMouvement(id);
            IdEnregistre = id;
            ProposerRecu = _m.Type == "RECETTE";
            Resultat = ("Opération validée", $"Pièce n° {Depot.NumeroPiece(id)} · saisie par {_m.NomAuteur}", TypeNotification.Succes);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { Ui.Erreur(ex); }
    }

    /// <summary>Boutons « Valider » (valider = true) et « Enregistrer en brouillon » (valider = false).</summary>
    private void Enregistrer(bool valider)
    {
        // Termine la saisie en cours dans les grilles
        _ventilation.EndEdit();
        _pieces.EndEdit();

        // 1) Contrôles des champs obligatoires
        if (Ui.IdSelectionne(_compte) is not int compte) { Ui.Info("Choisissez le compte."); return; }
        if (Ui.IdSelectionne(_categorie) is not int categorie) { Ui.Info("Choisissez la catégorie."); return; }
        if (_montant.Value <= 0) { Ui.Info("Le montant doit être supérieur à zéro."); return; }
        if (string.IsNullOrWhiteSpace(_libelle.Text)) { Ui.Info("Le libellé est obligatoire."); return; }

        // 2) Recopie des champs dans l'objet _m
        _m.Type = _recette.Checked ? "RECETTE" : "DEPENSE";
        _m.Date = _date.Value.Date;
        _m.IdCompte = compte;
        _m.IdCategorie = categorie;
        _m.IdTiers = Ui.IdSelectionne(_tiers);
        _m.Montant = _montant.Value;
        _m.ModePaiement = Modes[Math.Max(0, _mode.SelectedIndex)];
        _m.ReferenceExterne = _reference.Text;
        _m.Libelle = _libelle.Text;

        // 3) Lignes de ventilation : on ignore les lignes vides, on refuse les lignes incomplètes et les doublons
        _m.Ventilations.Clear();
        foreach (DataGridViewRow r in _ventilation.Rows)
        {
            if (r.IsNewRow) continue;
            var centre = r.Cells[_colCentre.Index].Value;
            var montant = r.Cells[_colMontantV.Index].Value;
            bool aCentre = centre is not null and not DBNull;
            bool aMontant = montant is not null and not DBNull;
            if (!aCentre && !aMontant) continue;
            if (!aCentre || !aMontant || Convert.ToDecimal(montant) <= 0)
            {
                Ui.Info("Chaque ligne de ventilation doit avoir un centre et un montant positif.");
                return;
            }
            int idCentre = Convert.ToInt32(centre);
            if (_m.Ventilations.Any(v => v.IdCentre == idCentre))
            {
                Ui.Info("Un même centre analytique apparaît deux fois dans la ventilation.");
                return;
            }
            _m.Ventilations.Add(new LigneVentilation { IdCentre = idCentre, Montant = Convert.ToDecimal(montant) });
        }
        if (_m.Ventilations.Sum(v => v.Montant) > _m.Montant)
        {
            Ui.Info("Le total ventilé dépasse le montant du mouvement.");
            return;
        }

        // 4) Pièces justificatives
        _m.Pieces.Clear();
        foreach (DataGridViewRow r in _pieces.Rows)
        {
            _m.Pieces.Add(new LignePiece
            {
                TypePiece = r.Cells[_colType.Index].Value?.ToString() ?? "AUTRE",
                Reference = r.Cells[_colReference.Index].Value?.ToString(),
                DatePiece = r.Cells[_colDate.Index].Value is DateTime d ? d : DateTime.Today,
                Montant = r.Cells[_colMontantP.Index].Value is decimal mt ? mt : null,
                CheminFichier = r.Cells[_colChemin.Index].Value?.ToString()
            });
        }

        // 5) Confirmation avant validation (une pièce validée n'est plus modifiable)
        if (valider && !Ui.Confirmer("Valider ce mouvement ?\n\nUn numéro de pièce lui sera attribué et il ne pourra plus être modifié, seulement annulé."))
            return;

        // 6) Enregistrement dans la base, puis message de notification pour la fenêtre principale
        try
        {
            Cursor = Cursors.WaitCursor;
            int id = Depot.EnregistrerMouvement(_m, valider);
            IdEnregistre = id;
            ProposerRecu = valider && _m.Type == "RECETTE";
            string nature = _m.Type == "RECETTE" ? "Recette" : "Dépense";
            Resultat = valider
                ? ($"{nature} validée", $"Pièce n° {Depot.NumeroPiece(id)} · {Theme.Montant(_m.Montant)} FCFA", TypeNotification.Succes)
                : ($"{nature} enregistrée en brouillon", Session.Peut("VALIDER")
                    ? "Vous pourrez la valider plus tard depuis « À valider »."
                    : "Elle apparaît dans « Mes brouillons » en attendant la validation.", TypeNotification.Info);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            // Erreur de la base (solde insuffisant, exercice clôturé…) : la fenêtre reste ouverte
            Ui.Erreur(ex);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}
