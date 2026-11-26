// =====================================================================================
//  Data/Association.cs — COORDONNÉES DE L'ASSOCIATION (imprimées sur les reçus)
// -------------------------------------------------------------------------------------
//  ➜ Remplacez ces textes par ceux de votre association : ils apparaissent en haut de
//    chaque reçu et de chaque bon de dépense (UI/ImpressionRecu.cs).
//    Laissez une valeur vide ("") pour ne pas afficher la ligne correspondante.
// =====================================================================================

namespace SLN_TRESORERIE_ASSO.Data;

public static class Association
{
    // Nom complet, en gros en haut du reçu
    public const string Nom = "Nom de votre association";

    // Ligne sous le nom : statut légal, n° de déclaration ou devise (facultatif)
    public const string Mention = "Association déclarée — régie par la loi sur les associations";

    // Adresse du siège
    public const string Adresse = "Adresse du siège, Ville";

    // Téléphone et e-mail (affichés sur la même ligne)
    public const string Telephone = "Tél. : 00 00 00 00 00";
    public const string Email = "contact@association.org";

    // Ville utilisée dans « Fait à …, le … » en bas du reçu
    public const string Ville = "Ville";

    // Monnaie écrite en toutes lettres et en abrégé
    public const string MonnaieLettres = "francs CFA";
    public const string Monnaie = "FCFA";
}
