namespace Tri_Gei;

using Tri_Gei.Donnees;
using Tri_Gei.Fichiers;
using Tri_Gei.Modeles;
using Tri_Gei.Traitements;

public partial class Fen1 : Form
{
    // Import en cours (équivalent du GLOBAL NomFichierSelectionXLS du WINDEV)
    private int? _idImportCourant;
    private string? _cheminFichierCourant;

    public Fen1()
    {
        InitializeComponent();

        // Etat Initial 
        btnImportFichier.Enabled = true;
        btnTrier.Enabled = false;
        btnExporter.Enabled = false;
    }

    private void btnTesterConnexion_Click(object sender, EventArgs e)
    {
        bool estConnecte = ConnexionBdd.Tester(out string message);

        MessageBox.Show(
            message,
            "Test de connexion",
            MessageBoxButtons.OK,
            estConnecte ? MessageBoxIcon.Information : MessageBoxIcon.Error);
    }

    private void btnImportFichier_Click(object sender, EventArgs e)
    {
        // 1. Sélection du fichier (= fSélecteur)
        using var dialogue = new OpenFileDialog
        {
            Title = "Sélectionnez un fichier",
            Filter = "Fichiers Excel (*.xlsx)|*.xlsx",
            InitialDirectory = @"C:\Reflex"
        };

        if (dialogue.ShowDialog() != DialogResult.OK)
        {
            return; // l'utilisateur a annulé
        }

        try
        {
            Cursor = Cursors.WaitCursor; // sablier pendant le traitement

            // 2. Lecture du fichier Excel (= boucle xlsDonnée)
            List<LigneGei> lignes = LecteurExcel.Lire(dialogue.FileName);

            // 3. Enregistrement en base avec traçabilité (= boucle HAjoute)
            var import = new ImportFichier
            {
                CheminSource = dialogue.FileName,
                Utilisateur = Environment.UserName
            };

            int idImport = DepotGei.EnregistrerImport(import, lignes);

            // 4. Mémorisation pour les étapes suivantes (tri, export)
            _idImportCourant = idImport;
            _cheminFichierCourant = dialogue.FileName;
          
            // 5. Affichage dans la grille (= TableAffiche)
            dgvGei.DataSource = lignes;

            MessageBox.Show(
                $"Import n°{idImport} terminé : {lignes.Count} lignes enregistrées.",
                "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Activation des boutons Trier et Exporter
            btnTrier.Enabled = true;
            btnExporter.Enabled = false;

        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"L'import a échoué :\n{ex.Message}",
                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default; // on remet le curseur normal, même en cas d'erreur
        }
    }

    private void btnTrier_Click(object sender, EventArgs e)
    {
        // On ne peut trier que si un fichier a été importé
        if (_idImportCourant == null)
        {
            MessageBox.Show("Importez d'abord un fichier.", "Tri",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int idImport = _idImportCourant.Value;

        try
        {
            Cursor = Cursors.WaitCursor;

            // 1. Lecture dans l'ordre de l'index (= HLitPremier sur AllNivColEmpID)
            List<LigneGei> lignes = DepotGei.LireLignesOrdreIndex(idImport);

            // 2. Calcul de la clé de tri serpentin
            ServiceTri.CalculerTri(lignes);

            // 3. Enregistrement de la clé en base (= HModifie)
            DepotGei.EnregistrerTri(lignes);

            // 4. Relecture triée et affichage (= TableTrie + TableAffiche)
            List<LigneGei> lignesTriees = DepotGei.LireLignesTriees(idImport);
            dgvGei.DataSource = lignesTriees;

            MessageBox.Show($"Tri terminé : {lignesTriees.Count} lignes.", "Tri",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Activation du bouton Exporter 
            btnExporter.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Le tri a échoué :\n{ex.Message}", "Erreur",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void btnExporter_Click(object sender, EventArgs e)
    {
        if (_idImportCourant == null || _cheminFichierCourant == null)
        {
            MessageBox.Show("Importez et triez d'abord un fichier.", "Export",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int idImport = _idImportCourant.Value;
        string cheminResultat = ExportFichier.ConstruireCheminResultat(_cheminFichierCourant);

        // Si le fichier _res existe déjà, on demande avant d'écraser
        if (File.Exists(cheminResultat))
        {
            DialogResult reponse = MessageBox.Show(
                $"Le fichier existe déjà :\n{cheminResultat}\n\nVoulez-vous le remplacer ?",
                "Export", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (reponse == DialogResult.No)
            {
                return;
            }
       
        }

        ExportFichier.Ecrire(cheminResultat, DepotGei.LireLignesTriees(idImport));

        MessageBox.Show("L'export a réussi !", "Export",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);

    }
}
