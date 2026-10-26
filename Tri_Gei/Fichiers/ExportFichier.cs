using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Text;
using Tri_Gei.Modeles;

namespace Tri_Gei.Fichiers
{
     public static class ExportFichier
    {
        /// <summary>
        /// Construit le chemin du fichier résultat :
        /// C:\Reflex\A06.xlsx  →  C:\Reflex\A06_res.xlsx
        /// </summary>
        public static string ConstruireCheminResultat(string cheminSource)
        {
            string dossier = "";
            string? dossierSource = Path.GetDirectoryName(cheminSource);

            if (dossierSource != null)
            {
                dossier = dossierSource;
            }

            string nomSansExtension = Path.GetFileNameWithoutExtension(cheminSource);
            string extension = Path.GetExtension(cheminSource);

            return Path.Combine(dossier, nomSansExtension + "_res" + extension);
        }

        /// <summary>
        /// Écrit les lignes dans un nouveau fichier Excel.
        /// </summary>
        public static void Ecrire(string cheminFichier, List<LigneGei> lignes)
        {
            using XLWorkbook classeur = new XLWorkbook();
            IXLWorksheet feuille = classeur.Worksheets.Add("A");

            // Toutes les colonnes au format Texte : on garde les zéros (001, 0011900...)
            feuille.Columns(1, 9).Style.NumberFormat.Format = "@";

            // Ligne 1 : en-tête (mêmes titres que le _res WINDEV)
            feuille.Cell(1, 1).Value = "Allee";
            feuille.Cell(1, 2).Value = "Face";
            feuille.Cell(1, 3).Value = "Colonne";
            feuille.Cell(1, 4).Value = "Niveau";
            feuille.Cell(1, 5).Value = "Emplacement";
            feuille.Cell(1, 6).Value = "Support";
            feuille.Cell(1, 7).Value = "Article";
            feuille.Cell(1, 8).Value = "Libellé";
            feuille.Cell(1, 9).Value = "Quantité";

            // Lignes suivantes : les données, dans l'ordre reçu
            int numLigne = 2;

            foreach (LigneGei ligne in lignes)
            {
                feuille.Cell(numLigne, 1).Value = ligne.Allee;
                feuille.Cell(numLigne, 2).Value = ligne.Face;
                feuille.Cell(numLigne, 3).Value = ligne.Colonne;
                feuille.Cell(numLigne, 4).Value = ligne.Niveau;
                feuille.Cell(numLigne, 5).Value = ligne.Emplacement;
                feuille.Cell(numLigne, 6).Value = ligne.Support;
                feuille.Cell(numLigne, 7).Value = ligne.Article;
                feuille.Cell(numLigne, 8).Value = ligne.Libelle;
                feuille.Cell(numLigne, 9).Value = ligne.Quantite;

                numLigne++;
            }

            // Largeur des colonnes ajustée au contenu
            feuille.Columns(1, 9).AdjustToContents();

            classeur.SaveAs(cheminFichier);
        }
    }
}
