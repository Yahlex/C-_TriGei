//using System;
//using System.Collections.Generic;
//using System.Text;

namespace Tri_Gei.Modeles
{
    public class ImportFichier
    {
        // --- Identifiant (généré par la base) ---
        public int IdImport { get; set; }

        // --- Renseigné à l'import (colonnes NOT NULL) ---
        public string CheminSource { get; set; } = "";
        public string Utilisateur { get; set; } = "";
        public  DateTime DateImport { get; set; }

        // --- Renseigné plus tard (colonnes nullables) ---
        public  string? CheminResultat { get; set; }
        public  DateTime? DateExport { get; set; }
        public int? NbLignes { get; set; }
    }
}
