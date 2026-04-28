using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TourneeFutee;

namespace TourneeFutee.Tests
{
    /// <summary>
    /// Tests d'intégration pour la classe ServicePersistance.
    ///
    /// CONFIGURATION REQUISE avant de lancer ces tests :
    ///   1. Avoir un serveur MySQL accessible avec les paramètres ci-dessous.
    ///   2. Avoir exécuté le script init_db.sql pour créer les tables.
    ///   3. Adapter les constantes DB_* si vos paramètres diffèrent.
    ///
    /// Ces tests sont des tests d'INTÉGRATION : ils interagissent avec une
    /// vraie base de données. Utilisez une base de test dédiée (DB_NAME).
    /// </summary>
    [TestClass]
    public class PersistanceTests
    {
        // ─────────────────────────────────────────────────────────────────────
        // Paramètres de connexion à la base de données de TEST
        // Adaptez ces constantes à votre environnement.
        // ─────────────────────────────────────────────────────────────────────
        private const string DB_SERVER = "127.0.0.1";
        private const string DB_NAME   = "tourneefutee_test";   // base dédiée aux tests !
        private const string DB_USER   = "root";
        private const string DB_PWD    = "root";

        // ─────────────────────────────────────────────────────────────────────
        // Instance partagée du service (créée une seule fois par classe de test)
        // ─────────────────────────────────────────────────────────────────────
        private static ServicePersistance _service;

        // ─────────────────────────────────────────────────────────────────────
        // Initialisation de la classe de test (appelée une seule fois)
        // ─────────────────────────────────────────────────────────────────────
        [ClassInitialize]
        public static void ClassSetup(TestContext ctx)
        {
            // Crée le service de persistance (doit se connecter sans exception)
            _service = new ServicePersistance(DB_SERVER, DB_NAME, DB_USER, DB_PWD);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Graphes de test
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Construit le graphe asymétrique utilisé dans l'objectif 2 (6 villes).
        ///       A    B    C    D    E    F
        ///   A [  ∞    1    7    3   14    2 ]
        ///   B [  3    ∞    6    9    1   24 ]
        ///   C [  6   14    ∞    3    7    3 ]
        ///   D [  2    3    5    ∞    9   11 ]
        ///   E [ 15    7   11    2    ∞    4 ]  ← E->D=2, E->F=4
        ///   F [ 20    5   13    4   18    ∞ ]
        /// Tournée optimale : A→C→F→B→E→D→A (coût 20)
        /// </summary>
        private static Graph BuildAsymmetricGraph()
        {
            // FIX : paramètre renommé de `isOriented` vers `directed`
            // Graphe orienté à 6 sommets
            var g = new Graph(directed: true);

            // Ajout des sommets (valeur = 0.0 par défaut)
            g.AddVertex("A", 0f);
            g.AddVertex("B", 0f);
            g.AddVertex("C", 0f);
            g.AddVertex("D", 0f);
            g.AddVertex("E", 0f);
            g.AddVertex("F", 0f);

            // Ajout des arcs (source, destination, poids)
            g.AddEdge("A", "B",  1f); g.AddEdge("A", "C",  7f);
            g.AddEdge("A", "D",  3f); g.AddEdge("A", "E", 14f); g.AddEdge("A", "F",  2f);

            g.AddEdge("B", "A",  3f); g.AddEdge("B", "C",  6f);
            g.AddEdge("B", "D",  9f); g.AddEdge("B", "E",  1f); g.AddEdge("B", "F", 24f);

            g.AddEdge("C", "A",  6f); g.AddEdge("C", "B", 14f);
            g.AddEdge("C", "D",  3f); g.AddEdge("C", "E",  7f); g.AddEdge("C", "F",  3f);

            g.AddEdge("D", "A",  2f); g.AddEdge("D", "B",  3f);
            g.AddEdge("D", "C",  5f); g.AddEdge("D", "E",  9f); g.AddEdge("D", "F", 11f);

            g.AddEdge("E", "A", 15f); g.AddEdge("E", "B",  7f);
            g.AddEdge("E", "C", 11f); g.AddEdge("E", "D",  2f); g.AddEdge("E", "F",  4f);

            g.AddEdge("F", "A", 20f); g.AddEdge("F", "B",  5f);
            g.AddEdge("F", "C", 13f); g.AddEdge("F", "D",  4f); g.AddEdge("F", "E", 18f);

            return g;
        }

        /// <summary>
        /// Construit un graphe non orienté simple à 3 sommets pour les tests basiques.
        /// </summary>
        private static Graph BuildSimpleGraph()
        {
            // FIX : paramètre renommé de `isOriented` vers `directed`
            var g = new Graph(directed: false);
            g.AddVertex("X", 1.5f);
            g.AddVertex("Y", 2.0f);
            g.AddVertex("Z", 3.5f);
            g.AddEdge("X", "Y", 10f);
            g.AddEdge("Y", "Z", 20f);
            g.AddEdge("X", "Z", 30f);
            return g;
        }

        // ─────────────────────────────────────────────────────────────────────
        // TEST 1 : Connexion
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Vérifie que la connexion à la base de données s'établit sans exception.
        /// Si ce test échoue, vérifiez vos paramètres DB_* et que le serveur MySQL
        /// est bien démarré.
        /// </summary>
        [TestMethod]
        public void ConnectionTest()
        {
            // Si le constructeur n'a pas levé d'exception, la connexion est OK.
            Assert.IsNotNull(_service, "Le service de persistance ne doit pas être null.");
        }

        // ─────────────────────────────────────────────────────────────────────
        // TEST 2 : SaveGraph
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Vérifie que SaveGraph retourne un identifiant valide (> 0).
        /// </summary>
        [TestMethod]
        public void SaveGraphTest()
        {
            Graph g = BuildSimpleGraph();
            uint id = _service.SaveGraph(g);
            Assert.IsTrue(id > 0, $"SaveGraph doit retourner un id > 0. Obtenu : {id}");
        }

        // ─────────────────────────────────────────────────────────────────────
        // TEST 3 : SaveGraph + LoadGraph (graphe simple)
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Vérifie que le graphe rechargé est identique au graphe sauvegardé :
        /// même nombre de sommets, mêmes noms, mêmes valeurs, mêmes poids d'arcs.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadGraphTest_Simple()
        {
            Graph original = BuildSimpleGraph();
            uint id = _service.SaveGraph(original);
            Graph loaded = _service.LoadGraph(id);

            // Vérifier le nombre de sommets
            Assert.AreEqual(
                // FIX : VertexCount->Order
                original.Order, loaded.Order,
                "Le nombre de sommets doit être identique après rechargement.");

            // Vérifier que les sommets ont les mêmes noms et valeurs
            foreach (string name in new[] { "X", "Y", "Z" })
            {
               
                Assert.IsTrue(loaded.ContainsVertex(name),
                    $"Le sommet '{name}' doit exister dans le graphe rechargé.");
                Assert.AreEqual(
                    original.GetVertexValue(name), loaded.GetVertexValue(name), 0.001f,
                    $"La valeur du sommet '{name}' doit être identique.");
            }

            // Vérifier les poids des arcs
            Assert.AreEqual(10f, loaded.GetEdgeWeight("X", "Y"), 0.001f, "Poids X->Y incorrect.");
            Assert.AreEqual(20f, loaded.GetEdgeWeight("Y", "Z"), 0.001f, "Poids Y->Z incorrect.");
            Assert.AreEqual(30f, loaded.GetEdgeWeight("X", "Z"), 0.001f, "Poids X->Z incorrect.");

            // Vérifier que le graphe rechargé est non orienté
            // FIX : IsOriented -> Directed
            Assert.AreEqual(original.Directed, loaded.Directed,
                "La propriété Directed doit être identique.");
        }

        // ─────────────────────────────────────────────────────────────────────
        // TEST 4 : SaveGraph + LoadGraph (graphe asymétrique)
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Vérifie que le graphe asymétrique à 6 sommets (objectif 2) est
        /// correctement sauvegardé et rechargé.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadGraphTest_Asymmetric()
        {
            Graph original = BuildAsymmetricGraph();
            uint id = _service.SaveGraph(original);
            Graph loaded = _service.LoadGraph(id);
            // FIX : VertexCount->Order
            Assert.AreEqual(original.Order, loaded.Order,
                "Nombre de sommets différent après rechargement.");

            // Vérifier quelques poids caractéristiques
            Assert.AreEqual(1f,  loaded.GetEdgeWeight("A", "B"), 0.001f, "Poids A->B incorrect.");
            Assert.AreEqual(3f,  loaded.GetEdgeWeight("B", "A"), 0.001f, "Poids B->A incorrect.");
            Assert.AreEqual(20f, loaded.GetEdgeWeight("F", "A"), 0.001f, "Poids F->A incorrect.");
            Assert.AreEqual(2f,  loaded.GetEdgeWeight("E", "D"), 0.001f, "Poids E->D incorrect.");
            Assert.AreEqual(4f,  loaded.GetEdgeWeight("E", "F"), 0.001f, "Poids E->F incorrect.");

            Assert.IsTrue(loaded.Directed,
                "Le graphe rechargé doit être orienté.");
        }

        // ─────────────────────────────────────────────────────────────────────
        // TEST 5 : SaveTour
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Vérifie que SaveTour retourne un identifiant valide (> 0).
        /// </summary>
        [TestMethod]
        public void SaveTourTest()
        {
            Graph g = BuildAsymmetricGraph();
            uint graphId = _service.SaveGraph(g);

            // Créer la tournée optimale connue : A->C->F->B->E->D->A (coût 20)
            var tour = new Tour(new List<string> { "A", "C", "F", "B", "E", "D", "A" }, 20f);
            uint tourId = _service.SaveTour(graphId, tour);

            Assert.IsTrue(tourId > 0,
                $"SaveTour doit retourner un id > 0. Obtenu : {tourId}");
        }

        // ─────────────────────────────────────────────────────────────────────
        // TEST 6 : SaveTour + LoadTour
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Vérifie que la tournée rechargée est identique à la tournée sauvegardée :
        /// même séquence de sommets et même coût total.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadTourTest()
        {
            Graph g = BuildAsymmetricGraph();
            uint graphId = _service.SaveGraph(g);

            // Tournée optimale : A->C->F->B->E->D->A, coût 20
            var sequence = new List<string> { "A", "C", "F", "B", "E", "D", "A" };
            var originalTour = new Tour(sequence, 20f);
            uint tourId = _service.SaveTour(graphId, originalTour);

            Tour loadedTour = _service.LoadTour(tourId);

            // Vérifier le coût total
            // FIX : TotalCost -> Cost
            Assert.AreEqual(originalTour.Cost, loadedTour.Cost, 0.001f,
                "Le coût total de la tournée doit être identique après rechargement.");

            // Vérifier la séquence complète des sommets
            IList<string> loadedSeq = loadedTour.Vertices;
            Assert.AreEqual(sequence.Count, loadedSeq.Count,
                "La séquence de la tournée doit avoir le même nombre d'étapes.");

            for (int i = 0; i < sequence.Count; i++)
            {
                Assert.AreEqual(sequence[i], loadedSeq[i],
                    $"Étape {i} incorrecte. Attendu : '{sequence[i]}', obtenu : '{loadedSeq[i]}'.");
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // TEST 7 : Plusieurs graphes coexistant en base
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Vérifie que plusieurs graphes peuvent être sauvegardés simultanément
        /// en base sans interférence (chaque id renvoie bien le bon graphe).
        /// </summary>
        [TestMethod]
        public void MultipleGraphsTest()
        {
            Graph g1 = BuildSimpleGraph();
            Graph g2 = BuildAsymmetricGraph();

            uint id1 = _service.SaveGraph(g1);
            uint id2 = _service.SaveGraph(g2);

            Assert.AreNotEqual(id1, id2,
                "Deux graphes sauvegardés doivent avoir des identifiants différents.");

            Graph loaded1 = _service.LoadGraph(id1);
            Graph loaded2 = _service.LoadGraph(id2);
            // FIX : VertexCount -> Order
            Assert.AreEqual(g1.Order, loaded1.Order,
                "Le graphe 1 rechargé doit avoir le bon nombre de sommets.");
            Assert.AreEqual(g2.Order, loaded2.Order,
                "Le graphe 2 rechargé doit avoir le bon nombre de sommets.");
        }
    }
}
