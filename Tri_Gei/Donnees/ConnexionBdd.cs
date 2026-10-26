/*using System;
using System.Collections.Generic;
using System.Text;*/
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Tri_Gei.Donnees;

/// <summary>
/// Point d'accès unique à la base PostgreSQL tri_gei.
/// </summary>
public static class ConnexionBdd
{
    // Créée une seule fois, au premier usage (puis réutilisée)
    private static NpgsqlDataSource? _sourceDonnees;

    private static NpgsqlDataSource SourceDonnees =>
        _sourceDonnees ??= CreerSourceDonnees();

    private static NpgsqlDataSource CreerSourceDonnees()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        string? chaineConnexion = configuration.GetConnectionString("TriGei");

        if (string.IsNullOrWhiteSpace(chaineConnexion))
        {
            throw new InvalidOperationException(
                "Chaîne de connexion 'TriGei' absente de appsettings.json.");
        }

        return NpgsqlDataSource.Create(chaineConnexion);
    }

    /// <summary>
    /// Ouvre une connexion. À utiliser TOUJOURS avec "using".
    /// </summary>
    public static NpgsqlConnection Ouvrir() => SourceDonnees.OpenConnection();

    public static bool Tester(out string message)
    {
        try
        {
            using NpgsqlConnection connexion = Ouvrir();
            using NpgsqlCommand commande = new NpgsqlCommand("SELECT version()", connexion);

            object? resultat = commande.ExecuteScalar();

            message = $"Connexion réussie :\n{resultat}";
            return true;
        }
        catch (Exception ex)
        {
            message = $"Échec de la connexion :\n{ex.Message}";
            return false;
        }
    }
}