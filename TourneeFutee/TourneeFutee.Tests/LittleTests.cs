namespace TourneeFutee.Tests
{
    // Campagne de tests unitaires vérifiant le bon fonctionnement de l'algorithme de Little
    [TestClass]
    public class LittleTests
    {

        // ------------------------------ Partie 1 : test de quelques étapes de l'algorithme ------------------------------

        /* Teste l'opération de réduction de la matrice dans l'algorithme de Little
         * Vérifie tous les coefficients de la matrice réduite, ainsi que la valeur de la réduction totale
         * Correspond à la première réduction du cours
         */
        [TestMethod]
        public void ReductionTest()
        {
            Matrix m = SymetricCostsMatrix();

            Console.WriteLine("Matrice avant réduction :");
            m.Print();

            float reduction = Little.ReduceMatrix(m);

            Console.WriteLine("Matrice attendue après réduction :");
            Matrix expectedReducedMatrix = ExpectedReducedMatrix();
            expectedReducedMatrix.Print();

            Console.WriteLine("Matrice obtenue après réduction :");
            m.Print();

            // Vérifie les coefficients de la matrice réduite un par un
            CheckMatrixEquality(expectedReducedMatrix, m);

            // Vérifie la valeur de la réduction totale
            float expectedReduction = 2219;
            Assert.AreEqual(expectedReduction, reduction, $"La réduction totale de la matrice doit être de {expectedReduction}");
        }


        /* Teste l'étape de calcul du regret maximal dans l'algorithme de Little
         * Vérifie la position et la valeur du regret.
         * Correspond au premier calcul de regret du cours
         */
        [TestMethod]
        public void RegretTest()
        {
            Matrix m = ExpectedReducedMatrix(); // on repart de la première matrice réduite dans le cours

            Console.WriteLine("Matrice de coûts :");
            m.Print();

            (int i, int j, float value) maxRegret = Little.GetMaxRegret(m);

            Console.WriteLine($"Regret maximal calculé : {maxRegret}");

            (int i, int j, float value) expectedRegret = (2, 4, 218);

            Assert.AreEqual(expectedRegret.i, maxRegret.i, $"Le regret maximal se situe à la ligne {expectedRegret.i} de la matrice de coûts");
            Assert.AreEqual(expectedRegret.j, maxRegret.j, $"Le regret maximal se situe à la colonne {expectedRegret.j} de la matrice de coûts");
            Assert.AreEqual(expectedRegret.value, maxRegret.value, $"La valeur du regret maximal est {expectedRegret.value} de la matrice de coûts");
        }


        /* Teste la détection de trajets parasites dans l'algorithme de Little
        * dans le cas simple où le trajet parasite est simplement le trajet inverse d'un trajet déjà inclus
        * Ce test correspond à l'étape "Appel récursif n°1 - droite" du cours
        */
        [TestMethod]
        public void ForbiddenSegmentsTest_ReverseSegment()
        {
            // Trajets déjà inclus dans la tournée en construction
            List<(string source, string destination)> includedSegments = new List<(string, string)>
            {
                ("T", "M"), // Toulouse -> Marseille
            };

            // On ne teste que le trajet candidat Marseille -> Toulouse ici
            // C'est un exemple trivial de trajet parasite
            List<(string source, string destination, bool forbidden)> candidateSegments = new List<(string, string, bool)>
            {
                ("M", "T", true),  // Paris -> Nantes : parasite
            };

            // nombre de villes à visiter 
            // => une tournée est incomplète si elle visite un nombre de villes < `nbCities`
            int nbCities = 6;

            CheckForbiddenSegments(includedSegments, candidateSegments, nbCities);
        }


        /* Teste la détection de trajets parasites dans l'algorithme de Little
         * dans le cas où l'ajout d'un trajet termine prématurément une tournée comprenant déjà plusieurs trajets
         * Ce test correspond à l'étape "Appel récursif n°4 - droite" du cours
         */
        [TestMethod]
        public void ForbiddenSegmentsTest_Subtour()
        {
            // Trajets déjà inclus dans la tournée en construction
            List<(string source, string destination)> includedSegments = new List<(string, string)>
            {
                ("T", "M"), // Toulouse -> Marseille
                ("N", "T"), // Nantes -> Toulouse
                ("M", "S"), // Marseille -> Strasbourg
            };

            // Trajets pouvant être ajoutés, dont certains sont parasites
            List<(string source, string destination, bool forbidden)> candidateSegments = new List<(string, string, bool)>
            {
                ("P", "N", false),  // Paris -> Nantes : non parasite
                ("P", "L", false),  // Paris -> Lyon : non parasite
                ("L", "N", false),  // Lyon -> Nantes : non parasite
                ("L", "P", false),  // Lyon -> Paris : non parasite
                ("S", "N", true),   // Strasbourg -> Nantes : parasite
                ("S", "P", false),  // Strasbourg -> Paris : non parasite
                ("S", "L", false),  // Strasbourg -> Lyon : parasite
            };

            // nombre de villes à visiter 
            // => une tournée est incomplète si elle visite un nombre de villes < `nbCities`
            int nbCities = 6;

            CheckForbiddenSegments(includedSegments, candidateSegments, nbCities);
        }




        // ------------------------------ Partie 2 : test de l'algorithme de Little entier ------------------------------
        // Vérifie que les tournées calculées par l'algorithme de Little sont correctes

        // Test sur un problème symétrique (exemple du cours)
        [TestMethod]
        public void LittleSymetricCosts()
        {
            Graph graph = SymetricCostsGraph();
            Little b = new Little(graph);
            Tour tour = b.ComputeOptimalTour();
            SymetricCostsTourChecks(tour);
        }

        // Test sur un problème asymétrique (exemple du TD)
        [TestMethod]
        public void LittleAsymetricCosts()
        {
            Graph graph = ASymetricCostsGraph();
            Little b = new Little(graph);
            Tour tour = b.ComputeOptimalTour();
            ASymetricCostsTourChecks(tour);
        }


        // ---------------------- Méthodes utilitaires permettant de factoriser le code dans les tests ---------------------

        // Vérifie que les matrices `expected` et `actual` sont égales
        private void CheckMatrixEquality(Matrix expected, Matrix actual)
        {
            Assert.AreEqual(expected.NbRows, actual.NbRows, "Les matrices ont le même nombre de lignes");
            Assert.AreEqual(expected.NbColumns, actual.NbColumns, "Les matrices ont le même nombre de lignes");

            for (int i = 0; i < expected.NbRows; i++)
            {
                for (int j = 0; j < expected.NbColumns; j++)
                {

                    float expectedValue = expected.GetValue(i, j);
                    float actualValue = actual.GetValue(i, j);
                    Assert.AreEqual(expectedValue, actualValue, $"Le coefficient ({i},{j}) doit être égal à {expectedValue}");
                }
            }
        }

        // Affecte les coûts diagonaux de la matrice `m` à +infini
        private void SetDiagonalToInfinite(Matrix m)
        {
            for (int i = 0; i < m.NbRows; i++)
            {
                for (int j = 0; j < m.NbColumns; j++)
                {
                    m.SetValue(i, j, float.PositiveInfinity);
                }
            }
        }

        // Dans la matrice `m`, affecte les coûts en dessous de la diagonale par symétrie des coûts au dessus de la diagonale
        private void SetLowerSymetricCosts(Matrix m)
        {
            for (int i = 0; i < m.NbRows; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    float cost = m.GetValue(j, i);
                    m.SetValue(i, j, cost);
                }
            }
        }

        /* Construit une matrice symétrique, modélisant de tous les coûts des trajets possibles entre 6 villes françaises
        * Les coûts sont les distances à vol d'oiseau, en km, entre les villes
        * Il correspond à l'exemple développé dans le cours
        */
        private Matrix SymetricCostsMatrix()
        {
            Matrix m = new Matrix(nbRows: 6, nbColumns: 6);

            // diagonale = coûts infinis
            SetDiagonalToInfinite(m);

            // définition explicite des coûts au dessus de la diagonale
            m.SetValue(0, 1, 343);
            m.SetValue(0, 2, 465);
            m.SetValue(0, 3, 507);
            m.SetValue(0, 4, 697);
            m.SetValue(0, 5, 710);

            m.SetValue(1, 2, 588);
            m.SetValue(1, 3, 210);
            m.SetValue(1, 4, 661);
            m.SetValue(1, 5, 397);

            m.SetValue(2, 3, 791);
            m.SetValue(2, 4, 407);
            m.SetValue(2, 5, 737);

            m.SetValue(3, 4, 834);
            m.SetValue(3, 5, 397);

            m.SetValue(4, 5, 615);

            // Complément de la matrice par symétrie
            SetLowerSymetricCosts(m);

            return m;
        }

        // Renvoie la matrice attendue après réduction dans ReductionTest()
        private Matrix ExpectedReducedMatrix()
        {
            Matrix m = new Matrix(nbRows: 6, nbColumns: 6);

            m.SetValue(0, 0, float.PositiveInfinity);
            m.SetValue(0, 1, 0);
            m.SetValue(0, 2, 122);
            m.SetValue(0, 3, 164);
            m.SetValue(0, 4, 354);
            m.SetValue(0, 5, 180);

            m.SetValue(1, 0, 75);
            m.SetValue(1, 1, float.PositiveInfinity);
            m.SetValue(1, 2, 378);
            m.SetValue(1, 3, 0);
            m.SetValue(1, 4, 451);
            m.SetValue(1, 5, 0);

            m.SetValue(2, 0, 0);
            m.SetValue(2, 1, 181);
            m.SetValue(2, 2, float.PositiveInfinity);
            m.SetValue(2, 3, 384);
            m.SetValue(2, 4, 0);
            m.SetValue(2, 5, 143);

            m.SetValue(3, 0, 239);
            m.SetValue(3, 1, 0);
            m.SetValue(3, 2, 581);
            m.SetValue(3, 3, float.PositiveInfinity);
            m.SetValue(3, 4, 624);
            m.SetValue(3, 5, 0);

            m.SetValue(4, 0, 232);
            m.SetValue(4, 1, 254);
            m.SetValue(4, 2, 0);
            m.SetValue(4, 3, 427);
            m.SetValue(4, 4, float.PositiveInfinity);
            m.SetValue(4, 5, 21);

            m.SetValue(5, 0, 255);
            m.SetValue(5, 1, 0);
            m.SetValue(5, 2, 340);
            m.SetValue(5, 3, 0);
            m.SetValue(5, 4, 218);
            m.SetValue(5, 5, float.PositiveInfinity);

            return m;
        }

        /* Construit un graphe modélisant un problème avec des coûts symétriques 
         * => Graphe non-orienté et complet, matrice d'adjacence symétrique.
         * La matrice d'adjacence de ce graphe est donnée par la fonction SymetricCostsMatrix()
         * Correspond à l'exemple développé dans le cours.
         */
        private Graph SymetricCostsGraph()
        {
            bool directed = false;   // graphe non orienté : trajets = arêtes

            // Nom des villes
            List<string> cities = new List<string>
            {   "N", // Nantes
                "P", // Paris
                "T", // Toulouse
                "L", // Lille
                "M", // Marseille
                "S"  // Strasbourg
            };

            int nbCities = cities.Count;

            Matrix costMatrix = SymetricCostsMatrix();  // matrice de coûts symétrique

            Graph graph = CreateGraph(directed, cities, costMatrix);     // graphe ayant la matrice d'adjacence `costMatrix`

            return graph;
        }

        // Crée un graphe dont le nom des sommets est spécifié par `verticesNames` 
        // et le poids des arcs par la matrice d'adjacence `adjacencyMatrix`
        private Graph CreateGraph(bool directed, List<string> verticesNames, Matrix adjacencyMatrix)
        {
            Graph g = new Graph(directed: directed);
            int order = verticesNames.Count;

            // ajout des sommets
            foreach (string vertexName in verticesNames)
            {
                g.AddVertex(vertexName);
            }

            // ajout des arêtes / arcs
            for (int i = 0; i < order; i++)
            {


                int lowerBound = directed ? 0 : i + 1;  // graphe orienté => ajout d'arcs => utilisation de l'intégralité de la matrice => borne inférieure = 0     
                                                        // graphe non-orienté => ajout d'arêtes = moitié supérieure de la matrice de coûts => borne inférieure = i + 1

                for (int j = lowerBound; j < order; j++)
                {
                    string source = verticesNames[i];
                    string destination = verticesNames[j];
                    float cost = adjacencyMatrix.GetValue(i, j);

                    g.AddEdge(source, destination, cost);
                }
            }

            return g;
        }

        // Vérifie les propriétés de la tournée optimale dans le cas du problème symétrique
        private void SymetricCostsTourChecks(Tour tour)
        {
            // Trajets attendus dans la tournée
            List<(string source, string destination)> expectedTourSegments = new List<(string, string)>()
            {
                ("T", "M"),
                ("N", "T"),
                ("M", "S"),
                ("P", "N"),
                ("L", "P"),
                ("S", "L")
            };

            // Coût attendu de la tournée
            float expectedCost = 2437;

            // Vérifie que la tournée `tour` vérifie les attendus
            TourChecks(tour, expectedTourSegments, expectedCost, symetric: true);
        }


        /* Construit un graphe modélisant un problème avec des coûts asymétriques 
        * => Graphe orienté et complet, matrice d'adjacence asymétrique.
        * Il correspond à l'exercice d'appropriation donné en TD.
        */
        private Graph ASymetricCostsGraph()
        {
            Graph graph = new Graph(directed: true);

            // Villes
            graph.AddVertex("A");
            graph.AddVertex("B");
            graph.AddVertex("C");
            graph.AddVertex("D");
            graph.AddVertex("E");
            graph.AddVertex("F");

            // Distances au départ de A
            graph.AddEdge("A", "B", 1);
            graph.AddEdge("A", "C", 7);
            graph.AddEdge("A", "D", 3);
            graph.AddEdge("A", "E", 14);
            graph.AddEdge("A", "F", 2);

            // Distances au départ de B
            graph.AddEdge("B", "A", 3);
            graph.AddEdge("B", "C", 6);
            graph.AddEdge("B", "D", 9);
            graph.AddEdge("B", "E", 1);
            graph.AddEdge("B", "F", 24);

            // Distances au départ de C
            graph.AddEdge("C", "A", 6);
            graph.AddEdge("C", "B", 14);
            graph.AddEdge("C", "D", 3);
            graph.AddEdge("C", "E", 7);
            graph.AddEdge("C", "F", 3);

            // Distances au départ de D
            graph.AddEdge("D", "A", 2);
            graph.AddEdge("D", "B", 3);
            graph.AddEdge("D", "C", 5);
            graph.AddEdge("D", "E", 9);
            graph.AddEdge("D", "F", 11);

            // Distances au départ de E
            graph.AddEdge("E", "A", 15);
            graph.AddEdge("E", "B", 7);
            graph.AddEdge("E", "C", 11);
            graph.AddEdge("E", "D", 2);
            graph.AddEdge("E", "F", 4);

            // Distances au départ de F
            graph.AddEdge("F", "A", 20);
            graph.AddEdge("F", "B", 5);
            graph.AddEdge("F", "C", 13);
            graph.AddEdge("F", "D", 4);
            graph.AddEdge("F", "E", 18);

            return graph;
        }


        // Vérifie les propriétés de la tournée optimale dans le cas du problème asymétrique
        private void ASymetricCostsTourChecks(Tour tour)
        {
            List<(string source, string destination)> expectedTourSegments = new List<(string, string)>()
            {
                ("B", "E"),
                ("D", "A"),
                ("A", "C"),
                ("C", "F"),
                ("E", "D"),
                ("F", "B")
            };

            float expectedCost = 20;

            TourChecks(tour, expectedTourSegments, expectedCost, symetric: false);
        }

        // Vérifie que la tournée `tour` est de coût `expectedCost` et inclut les trajets contenus dans `expectedTourSegments` 
        // (ou leurs inverses, dans le cas d'un problème symétrique, spécifié par `symetric`) 
        private void TourChecks(Tour tour, List<(string, string)> expectedTourSegments, float expectedCost, bool symetric)
        {
            Console.WriteLine($"Tournée calculée :");
            tour.Print();

            // Vérification du coût de la tournée
            Assert.AreEqual(expectedCost, tour.Cost, $"Le coût de la tournée est {expectedCost}");

            // Vérification des trajets contenus dans la tournée
            Assert.AreEqual(expectedTourSegments.Count, tour.NbSegments, $"La tournée contient {expectedTourSegments.Count} trajets");

            bool containsAllSegments = true;
            bool containsAllReversedSegments = true;

            foreach ((string source, string destination) segment in expectedTourSegments)
            {
                bool containsSegment = tour.ContainsSegment(segment);
                containsAllSegments = containsAllSegments && containsSegment;

                if (symetric)
                {
                    bool containsReversedSegment = tour.ContainsSegment((segment.destination, segment.source));
                    containsAllReversedSegments = containsAllReversedSegments && containsReversedSegment;
                }
            }

            // Les tests de vérification des trajets inclus différent selon que le problème soit symétrique ou non

            // 1. Si le problème n'est pas symétrique, on s'attend a avoir exactement les trajets attendus (dans le même sens)
            if (!symetric)
            {
                Assert.IsTrue(containsAllSegments, $"La tournée inclut les trajets {string.Join(", ", expectedTourSegments)}");

            }
            // 2. Sinon, on s'attend a avoir les trajets attendus ou leurs inverses (mais dans ce cas tous les trajets doivent être inversés)
            else
            {
                bool containsAllExpectedSegments = containsAllSegments || containsAllReversedSegments;
                Assert.IsTrue(containsAllExpectedSegments, $"La tournée inclut les trajets {string.Join(", ", expectedTourSegments)} ou leurs inverses");
            }

        }

        /* Vérifie que les trajets potentiels stockés dans `candidateSegments` sont bien détectés comme des 
         * trajets parasites ou non par la fonction IsForbiddenSegment.
         * Pour chaque trajet potentiel `(source, destination, forbidden)`, IsForbiddenSegment doit renvoyer `forbidden` 
         */
        private void CheckForbiddenSegments(List<(string source, string destination)> includedSegments, List<(string source, string destination, bool forbidden)> candidateSegments, int nbCities)
        {
            foreach ((string source, string destination, bool forbidden) candidateSegment in candidateSegments)
            {
                (string, string) segment = (candidateSegment.source, candidateSegment.destination);
                bool isForbiddenSegment = Little.IsForbiddenSegment(segment, includedSegments, nbCities);

                Assert.AreEqual(candidateSegment.forbidden, isForbiddenSegment, $"Le trajet {segment} {(candidateSegment.forbidden ? "" : "n'")}est{(candidateSegment.forbidden ? " " : " pas ")}parasite");

            }
        }

    }
}
