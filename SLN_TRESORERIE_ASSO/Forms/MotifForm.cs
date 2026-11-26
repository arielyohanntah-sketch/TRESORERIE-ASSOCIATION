// =====================================================================================
//  Forms/MotifForm.cs — DEMANDE DU MOTIF D'ANNULATION
// -------------------------------------------------------------------------------------
//  Petite fenêtre réutilisée pour annuler un mouvement ou un virement.
//  Exemple d'utilisation :
//      using var f = new MotifForm("Annuler la pièce DEP-2026-00012", "Le motif est obligatoire");
//      if (f.ShowDialog(this) == DialogResult.OK) { ... f.Motif ... }
// =====================================================================================

using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Forms;

/// <summary>Demande le motif obligatoire d'une annulation (mise en page : MotifForm.Designer.cs).</summary>
public partial class MotifForm : DialogueBase
{
    // Texte saisi, sans espaces au début ni à la fin
    public string Motif => _motif.Text.Trim();

    /// <summary>Constructeur utilisé par le concepteur de Visual Studio.</summary>
    public MotifForm() : this("Annuler la pièce", "Le motif est obligatoire") { }

    public MotifForm(string titre, string description)
    {
        InitializeComponent();
        DefinirIcone(Theme.IcoAnnuler);
        // Titre de la fenêtre et du bandeau, puis sous-titre
        Text = LibelleTitre.Text = titre;
        LibelleSousTitre.Text = description;

        _btnConfirmer.Click += (_, _) =>
        {
            // Au moins 3 caractères pour éviter un motif vide ou « x »
            if (Motif.Length < 3)
            {
                Ui.Info("Indiquez le motif de l'annulation.");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        };
    }
}
