using System;
using System.Collections.Generic;
using System.Text;
using Tri_Gei.Modeles;

namespace Tri_Gei.Traitements
{
    public static class ServiceTri
    {
        // <summary>
        /// Calcule la clé Tri de chaque ligne.
        /// PRÉREQUIS : les lignes doivent être dans l'ordre allée, niveau, colonne, emplacement, id
        /// (= l'index AllNivColEmpID du WINDEV).
        /// </summary>
        public static void CalculerTri(List<LigneGei> lignes)
        {
            string niveauPrecedent = "++";   // valeur impossible : force une rupture dès la 1re ligne
            bool descendant = true;          // basculera à false dès la 1re ligne → 1er niveau ascendant

            foreach (LigneGei ligne in lignes)
            {
                // Rupture sur le niveau → on change de sens
                if (ligne.Niveau != niveauPrecedent)
                {
                    descendant = !descendant;
                    niveauPrecedent = ligne.Niveau;
                }

                string allee = ligne.Allee.Trim();
                string niveau = ligne.Niveau.Trim();
                string colonne = ligne.Colonne.Trim();
                string emplacement = ligne.Emplacement.Trim();

                //  Syntaxe ternaire
                //  ligne.Tri = descendant
                //    ? allee + niveau + Inverser(colonne) + Inverser(emplacement)
                //    : allee + niveau + colonne + emplacement;
                if (descendant)
                {
                    ligne.Tri = allee + niveau + Inverser(colonne) + Inverser(emplacement);
                }
                else
                {
                    ligne.Tri = allee + niveau + colonne + emplacement;
                }
            }
        }

        /// <summary>
        /// Inverse chaque caractère pour obtenir un tri descendant en triant en ascendant
        /// (transposition de f_inverse) : 0↔9, A↔Z, autre → '*'.
        /// </summary>
        public static string Inverser(string texte)
        {
            var resultat = new StringBuilder();

            foreach (char caractere in texte)
            {
                if (caractere >= '0' && caractere <= '9')
                {
                    resultat.Append((char)('9' - caractere + '0'));
                }
                else if (caractere >= 'A' && caractere <= 'Z')
                {
                    resultat.Append((char)('Z' - caractere + 'A'));
                }
                else
                {
                    resultat.Append('*');
                }
            }

            return resultat.ToString();
        }
    }
}
