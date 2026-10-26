//using System;
//using System.Collections.Generic;
//using System.Text;


namespace Tri_Gei.Modeles
{
    public class LigneGei
    {
        // --- Identifiants techniques (remplis par la base / l'import) ---
        public int IdGei { get; set; }
        public int IdImport { get; set; }
        public int NumLigne { get; set; }

        // --- Données lues dans le fichier Excel ---
        public string Allee { get; set; } = "";
        public string Face { get; set; } = "";
        public string Colonne { get; set; } = "";
        public string Niveau { get; set; } = "";
        public string Emplacement { get; set; } = "";
        public string Support { get; set; } = "";
        public string Article { get; set; } = "";
        public string Libelle { get; set; } = "";
        public string Quantite { get; set; } = "";

        public string test { get; set; } = "bonjour";

        // --- Calculé par le tri serpentin ---
        public string Tri { get; set; } = "";
    }
}
