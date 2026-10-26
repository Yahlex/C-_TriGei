using ClosedXML.Excel;
using Tri_Gei.Modeles;
//using TriGei.Modeles;

namespace Tri_Gei.Fichiers;

/// <summary>
/// Lit un fichier d'inventaire GEI (.xlsx) et le transforme en liste de LigneGei.
/// Transposition de la boucle xlsDonnée du WINDEV.
/// </summary>
public static class LecteurExcel
{
    public static List<LigneGei> Lire(string cheminFichier)
    {
        if (!File.Exists(cheminFichier))
        {
            throw new FileNotFoundException("Fichier introuvable.", cheminFichier);
        }

        // Ouverture en lecture seule, même si le fichier est déjà ouvert dans Excel
        using var flux = new FileStream(cheminFichier, FileMode.Open,
                                        FileAccess.Read, FileShare.ReadWrite);
        using var classeur = new XLWorkbook(flux);
        IXLWorksheet feuille = classeur.Worksheet(1);

        int derniereLigne = feuille.LastRowUsed()?.RowNumber() ?? 0;

        // 1. Recherche de l'en-tête : première ligne dont la colonne A n'est pas vide
        int numLigne = 1;
        while (numLigne <= derniereLigne && LireCellule(feuille, numLigne, 1) == "")
        {
            numLigne++;
        }

        if (numLigne > derniereLigne)
        {
            throw new InvalidDataException("Aucune donnée trouvée dans le fichier.");
        }

        numLigne++; // on saute la ligne d'en-tête

        // 2. Lecture des données jusqu'à la première ligne dont la colonne A est vide
        var lignes = new List<LigneGei>();

        while (numLigne <= derniereLigne && LireCellule(feuille, numLigne, 1) != "")
        {
            lignes.Add(new LigneGei
            {
                NumLigne = numLigne,
                Allee = LireCellule(feuille, numLigne, 1),
                Face = LireCellule(feuille, numLigne, 2),
                Colonne = LireCellule(feuille, numLigne, 3),
                Niveau = LireCellule(feuille, numLigne, 4),
                Emplacement = LireCellule(feuille, numLigne, 5),
                Support = LireCellule(feuille, numLigne, 6),
                Article = LireCellule(feuille, numLigne, 7),
                Libelle = LireCellule(feuille, numLigne, 8),
                Quantite = LireCellule(feuille, numLigne, 9)
            });

            numLigne++;
        }

        return lignes;
    }

    /// <summary>
    /// Équivalent de xlsDonnée(id, ligne, colonne) : renvoie le texte affiché dans la cellule.
    /// </summary>
    private static string LireCellule(IXLWorksheet feuille, int ligne, int colonne)
    {
        return feuille.Cell(ligne, colonne).GetFormattedString();
    }
}