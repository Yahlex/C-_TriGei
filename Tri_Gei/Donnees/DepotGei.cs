using Npgsql;
using Tri_Gei.Donnees;
using Tri_Gei.Modeles;
using Tri_Gei.Fichiers;


namespace Tri_Gei.Donnees;

/// <summary>
/// Implémentation PostgreSQL du dépôt GEI.
/// </summary>
public static class DepotGei
{
    public static int EnregistrerImport(ImportFichier import, List<LigneGei> lignes)
    {
        using NpgsqlConnection connexion = ConnexionBdd.Ouvrir();
        using NpgsqlTransaction transaction = connexion.BeginTransaction();

        // --- 1. En-tête de l'import (on récupère l'id généré) ---
        int idImport;


        using (var commande = new NpgsqlCommand(
            @"INSERT INTO import_fichier (chemin_source, utilisateur, nb_lignes)
              VALUES (@chemin_source, @utilisateur, @nb_lignes)
              RETURNING id_import",
            connexion, transaction))
        {
            commande.Parameters.AddWithValue("chemin_source", import.CheminSource);
            commande.Parameters.AddWithValue("utilisateur", import.Utilisateur);
            commande.Parameters.AddWithValue("nb_lignes", lignes.Count);

            idImport = Convert.ToInt32(commande.ExecuteScalar());
        }

        // --- 2. Les lignes du fichier, rattachées à cet import ---
        using (var commande = new NpgsqlCommand(
            @"INSERT INTO gei (id_import, num_ligne, allee, face, colonne, niveau,
                               emplacement, support, article, libelle, quantite)
              VALUES (@id_import, @num_ligne, @allee, @face, @colonne, @niveau,
                      @emplacement, @support, @article, @libelle, @quantite)",
            connexion, transaction))
        {
            foreach (LigneGei ligne in lignes)
            {
                ligne.IdImport = idImport;

                commande.Parameters.Clear();
                commande.Parameters.AddWithValue("id_import", ligne.IdImport);
                commande.Parameters.AddWithValue("num_ligne", ligne.NumLigne);
                commande.Parameters.AddWithValue("allee", ligne.Allee);
                commande.Parameters.AddWithValue("face", ligne.Face);
                commande.Parameters.AddWithValue("colonne", ligne.Colonne);
                commande.Parameters.AddWithValue("niveau", ligne.Niveau);
                commande.Parameters.AddWithValue("emplacement", ligne.Emplacement);
                commande.Parameters.AddWithValue("support", ligne.Support);
                commande.Parameters.AddWithValue("article", ligne.Article);
                commande.Parameters.AddWithValue("libelle", ligne.Libelle);
                commande.Parameters.AddWithValue("quantite", ligne.Quantite);

                commande.ExecuteNonQuery();
            }
        }

        // --- 3. Tout s'est bien passé : on valide ---
        transaction.Commit();

        import.IdImport = idImport;
        return idImport;
    }

    public static List<LigneGei> LireLignesOrdreIndex(int idImport)
    {
        string requete =
            @"SELECT id_gei, id_import, num_ligne, allee, face, colonne, niveau,
                 emplacement, support, article, libelle, quantite, tri
          FROM gei
          WHERE id_import = @id_import
          ORDER BY allee COLLATE ""C"", niveau COLLATE ""C"",
                   colonne COLLATE ""C"", emplacement COLLATE ""C"", id_gei";

        return LireLignes(requete, idImport);
    }

    /// <summary>
    /// Lit les lignes d'un import triées sur la clé de tri (ordre final du _res).
    /// </summary>
    public static List<LigneGei> LireLignesTriees(int idImport)
    {
        string requete =
            @"SELECT id_gei, id_import, num_ligne, allee, face, colonne, niveau,
                 emplacement, support, article, libelle, quantite, tri
          FROM gei
          WHERE id_import = @id_import
          ORDER BY tri, id_gei";

        return LireLignes(requete, idImport);
    }

    /// <summary>
    /// Exécute une requête de lecture et transforme chaque ligne SQL en objet LigneGei.
    /// </summary>
    private static List<LigneGei> LireLignes(string requete, int idImport)
    {
        List<LigneGei> lignes = new List<LigneGei>();

        using NpgsqlConnection connexion = ConnexionBdd.Ouvrir();
        using NpgsqlCommand commande = new NpgsqlCommand(requete, connexion);
        commande.Parameters.AddWithValue("id_import", idImport);

        using NpgsqlDataReader lecteur = commande.ExecuteReader();

        while (lecteur.Read())
        {
            LigneGei ligne = new LigneGei();

            ligne.IdGei = lecteur.GetInt32(lecteur.GetOrdinal("id_gei"));
            ligne.IdImport = lecteur.GetInt32(lecteur.GetOrdinal("id_import"));
            ligne.NumLigne = lecteur.GetInt32(lecteur.GetOrdinal("num_ligne"));
            ligne.Allee = lecteur.GetString(lecteur.GetOrdinal("allee"));
            ligne.Face = lecteur.GetString(lecteur.GetOrdinal("face"));
            ligne.Colonne = lecteur.GetString(lecteur.GetOrdinal("colonne"));
            ligne.Niveau = lecteur.GetString(lecteur.GetOrdinal("niveau"));
            ligne.Emplacement = lecteur.GetString(lecteur.GetOrdinal("emplacement"));
            ligne.Support = lecteur.GetString(lecteur.GetOrdinal("support"));
            ligne.Article = lecteur.GetString(lecteur.GetOrdinal("article"));
            ligne.Libelle = lecteur.GetString(lecteur.GetOrdinal("libelle"));
            ligne.Quantite = lecteur.GetString(lecteur.GetOrdinal("quantite"));
            ligne.Tri = lecteur.GetString(lecteur.GetOrdinal("tri"));

            lignes.Add(ligne);
        }

        return lignes;
    }

    public static void EnregistrerTri(List<LigneGei> lignes)
    {
        using NpgsqlConnection connexion = ConnexionBdd.Ouvrir();
        using NpgsqlTransaction transaction = connexion.BeginTransaction();

        using NpgsqlCommand commande = new NpgsqlCommand(
            @"UPDATE gei
          SET tri = @tri
          WHERE id_gei = @id_gei",
            connexion, transaction);

        foreach (LigneGei ligne in lignes)
        {
            commande.Parameters.Clear();
            commande.Parameters.AddWithValue("tri", ligne.Tri);
            commande.Parameters.AddWithValue("id_gei", ligne.IdGei);

            commande.ExecuteNonQuery();
        }

        transaction.Commit();
    }

}