// =====================================================================================
//  Forms/ApercuRecuForm.cs — APERÇU, IMPRESSION ET PDF D'UN REÇU
// -------------------------------------------------------------------------------------
//  Ouverte par « Imprimer le reçu » (liste des opérations ou fenêtre d'une opération
//  validée). Utilisation : ApercuRecuForm.Afficher(this, idMouvement);
//    - Imprimer…          : choix de l'imprimante (fenêtre Windows) puis impression ;
//    - Enregistrer en PDF : crée un fichier PDF avec l'imprimante « Microsoft Print to PDF »
//                           fournie avec Windows 10 et 11.
//  Le dessin de la page est fait par UI/ImpressionRecu.cs.
// =====================================================================================

using System.Drawing.Printing;
using SLN_TRESORERIE_ASSO.Data;
using SLN_TRESORERIE_ASSO.UI;

namespace SLN_TRESORERIE_ASSO.Forms;

/// <summary>Aperçu d'un reçu avant impression (mise en page : ApercuRecuForm.Designer.cs).</summary>
public partial class ApercuRecuForm : Form
{
    // Imprimante PDF intégrée à Windows
    private const string ImprimantePdf = "Microsoft Print to PDF";

    private readonly RecuMouvement? _recu;
    private readonly PrintDocument? _document;

    /// <summary>Constructeur utilisé par le concepteur de Visual Studio.</summary>
    public ApercuRecuForm() : this(null) { }

    public ApercuRecuForm(RecuMouvement? recu)
    {
        InitializeComponent();
        _recu = recu;

        // Icônes des boutons et taille adaptée à l'écran
        _outils.Renderer = new RenduBarres();
        _btnImprimer.Image = Theme.Glyphe(Theme.IcoImprimer, Theme.Nuit);
        _btnPdf.Image = Theme.Glyphe(Theme.IcoEnregistrer, Theme.Rouge);
        _btnFermer.Image = Theme.Glyphe(Theme.IcoAnnuler, Theme.Gris);
        foreach (ToolStripItem b in _outils.Items) b.ImageScaling = ToolStripItemImageScaling.None;
        Size = new Size(Math.Min(Theme.Px(820), Screen.FromControl(this).WorkingArea.Width),
                        Math.Min(Theme.Px(900), Screen.FromControl(this).WorkingArea.Height));

        if (recu != null)
        {
            Text = $"{(recu.Recette ? "Reçu" : "Bon de dépense")} {recu.NumeroPiece}";
            _document = ImpressionRecu.Document(recu);
            _apercu.Document = _document;
        }

        _btnImprimer.Click += (_, _) => Imprimer();
        _btnPdf.Click += (_, _) => EnregistrerPdf();
        _btnFermer.Click += (_, _) => Close();
    }

    /// <summary>Charge le mouvement et ouvre l'aperçu de son reçu (affiche l'erreur si impossible).</summary>
    public static void Afficher(IWin32Window? proprietaire, int idMouvement)
    {
        try
        {
            var recu = Depot.ChargerRecu(idMouvement);
            if (recu.Statut == "BROUILLON")
            {
                Ui.Info("Le reçu n'est disponible qu'une fois l'opération validée (il porte le numéro de pièce).");
                return;
            }
            using var f = new ApercuRecuForm(recu);
            f.ShowDialog(proprietaire);
        }
        catch (Exception ex)
        {
            Ui.Erreur(ex, "Reçu");
        }
    }

    // Bouton « Imprimer… » : fenêtre Windows de choix de l'imprimante, puis impression
    private void Imprimer()
    {
        if (_document == null) return;
        using var dlg = new PrintDialog { Document = _document, UseEXDialog = true, AllowSomePages = false };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            _document.PrinterSettings.PrintToFile = false;
            _document.Print();
        }
        catch (Exception ex) { Ui.Erreur(ex, "Impression"); }
    }

    // Bouton « Enregistrer en PDF » : impression vers un fichier avec Microsoft Print to PDF
    private void EnregistrerPdf()
    {
        if (_document == null || _recu == null) return;
        if (!PrinterSettings.InstalledPrinters.Cast<string>().Contains(ImprimantePdf))
        {
            Ui.Info("L'imprimante « Microsoft Print to PDF » n'est pas installée sur ce PC.\n" +
                    "Utilisez « Imprimer… » et choisissez une autre imprimante PDF.");
            return;
        }
        using var dlg = new SaveFileDialog
        {
            Filter = "Document PDF|*.pdf",
            FileName = $"{(_recu.Recette ? "Recu" : "Bon_depense")}_{_recu.NumeroPiece}.pdf"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        var ancienne = _document.PrinterSettings.PrinterName;
        try
        {
            _document.PrinterSettings.PrinterName = ImprimantePdf;
            _document.PrinterSettings.PrintToFile = true;
            _document.PrinterSettings.PrintFileName = dlg.FileName;
            _document.Print();
            Ui.Info("Reçu enregistré :\n" + dlg.FileName);
        }
        catch (Exception ex) { Ui.Erreur(ex, "PDF"); }
        finally
        {
            // On remet l'imprimante habituelle pour le bouton « Imprimer… »
            _document.PrinterSettings.PrintToFile = false;
            _document.PrinterSettings.PrinterName = ancienne;
        }
    }
}
