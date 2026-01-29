namespace TourneeFutee.Tests
{
    // Campagne de tests unitaires vérifiant le bon fonctionnement de la classe Graph
    [TestClass]
    public class GraphTests
    {

        // --- Création d'un graphe ---

        // Vérifie que l'ordre d'un graphe vide est 0
        [TestMethod]
        public void EmptyGraph()
        {
            EmptyGraph(directed: true);     // graphe orienté
            EmptyGraph(directed: false);    // graphe non orienté
        }

        // --- Ajout d'un sommet ---

        // Vérifie que AddVertex() ajoute un sommet avec la bonne valeur et incrémente l'ordre du graphe
        [TestMethod]
        public void AddVertex_VertexAdded()
        {
            AddVertex_VertexAdded(directed: true);     // graphe orienté
            AddVertex_VertexAdded(directed: false);    // graphe non orienté
        }

        // Vérifie qu'un sommet ajouté avec AddVertex() n'est relié à aucun arc, et n'a donc aucun voisin 
        [TestMethod]
        public void AddVertex_NoEdge()
        {
            AddVertex_NoEdge(directed: true);     // graphe orienté
            AddVertex_NoEdge(directed: false);    // graphe non orienté
        }

        // Vérifie que AddVertex() lève une exception si on tente d'ajouter deux fois le même sommet
        [TestMethod]
        public void AddDuplicateVertex()
        {
            AddDuplicateVertex(directed: true);     // graphe orienté
            AddDuplicateVertex(directed: false);    // graphe non orienté
        }

        // --- Suppression d'un sommet ---

        // Vérifie que RemoveVertex() lève une exception si le sommet n'existe pas
        [TestMethod]
        public void RemoveUnexistingVertex()
        {
            RemoveUnexistingVertex(directed: true);     // graphe orienté
            RemoveUnexistingVertex(directed: false);    // graphe non orienté
        }

        // Vérifie que RemoveVertex() supprime un sommet et décrémente l'ordre du graphe
        [TestMethod]
        public void RemoveVertex_VertexRemoved()
        {
            RemoveVertex_VertexRemoved(directed: true);     // graphe orienté
            RemoveVertex_VertexRemoved(directed: false);    // graphe non orienté
        }

        // Vérifie que RemoveVertex() supprime les arcs reliés au sommet supprimé
        [TestMethod]
        public void RemoveVertex_EdgesRemoved()
        {
            RemoveVertex_EdgesRemoved(directed: true);     // graphe orienté
            RemoveVertex_EdgesRemoved(directed: false);    // graphe non orienté
        }

        // --- Valeur d'un sommet ---

        // Vérifie que GetVertexValue() lève une exception si le sommet n'existe pas
        [TestMethod]
        public void GetUnexistingVertexValue()
        {
            GetUnexistingVertexValue(directed: true);     // graphe orienté
            GetUnexistingVertexValue(directed: false);    // graphe non orienté
        }

        // Vérifie que SetVertexValue() lève une exception si le sommet n'existe pas
        [TestMethod]
        public void SetUnexistingVertexValue()
        {
            SetUnexistingVertexValue(directed: true);     // graphe orienté
            SetUnexistingVertexValue(directed: false);    // graphe non orienté
        }

        // Vérifie que SetVertexValue() modifie la valeur d'un sommet existant
        [TestMethod]
        public void SetVertexValue()
        {
            SetVertexValue(directed: true);     // graphe orienté
            SetVertexValue(directed: false);    // graphe non orienté
        }


        // --- Ajout d'un arc ---

        // Vérifie que AddEdge() ajoute un arc avec la bonne valeur et modifie le voisinage des extrémités
        [TestMethod]
        public void AddEdge()
        {
            AddEdge(directed: true);     // graphe orienté
            AddEdge(directed: false);    // graphe non orienté
        }

        // Vérifie que AddEdge() lève une exception si on tente d'ajouter deux fois le même arc
        [TestMethod]
        public void AddDuplicateEdge()
        {
            AddDuplicateEdge(directed: true);     // graphe orienté
            AddDuplicateEdge(directed: false);    // graphe non orienté
        }

        // Vérifie que AddEdge() lève une exception si une de ses extrémités n'existe pas
        [TestMethod]
        public void AddEdgeUnexistingVertex()
        {
            AddEdgeUnexistingVertex(directed: true);     // graphe orienté
            AddEdgeUnexistingVertex(directed: false);    // graphe non orienté
        }


        // --- Suppression d'un arc ---

        // Vérifie que RemoveEdge() lève une exception si une de ses extrémités n'existe pas
        [TestMethod]
        public void RemoveEdgeUnexistingVertex()
        {
            RemoveEdgeUnexistingVertex(directed: true);     // graphe orienté
            RemoveEdgeUnexistingVertex(directed: false);    // graphe non orienté
        }

        // Vérifie que RemoveEdge() lève une exception si l'arc n'existe pas
        [TestMethod]
        public void RemoveUnexistingEdge()
        {
            RemoveUnexistingEdge(directed: true);     // graphe orienté
            RemoveUnexistingEdge(directed: false);    // graphe non orienté
        }

        // Vérifie que RemoveEdge() supprime un arc et modifie le voisinage des extrémités
        [TestMethod]
        public void RemoveEdge()
        {
            RemoveEdge(directed: true);     // graphe orienté
            RemoveEdge(directed: false);    // graphe non orienté
        }

        // --- Poids d'un arc ---

        // Vérifie que GetEdgeWeight() lève une exception si une de ses extrémités n'existe pas
        [TestMethod]
        public void GetEdgeWeightUnexistingVertex()
        {
            GetEdgeWeightUnexistingVertex(directed: true);     // graphe orienté
            GetEdgeWeightUnexistingVertex(directed: false);    // graphe non orienté
        }

        // Vérifie que GetEdgeWeight() lève une exception si l'arc n'existe pas
        [TestMethod]
        public void GetUnexistingEdgeWeight()
        {
            GetUnexistingEdgeWeight(directed: true);     // graphe orienté
            GetUnexistingEdgeWeight(directed: false);    // graphe non orienté
        }

        // Vérifie que SetEdgeWeight() lève une exception si une de ses extrémités n'existe pas
        [TestMethod]
        public void SetEdgeWeightUnexistingVertex()
        {
            SetEdgeWeightUnexistingVertex(directed: true);     // graphe orienté
            SetEdgeWeightUnexistingVertex(directed: false);    // graphe non orienté
        }


        // Vérifie que SetEdgeWeight() modifie la valeur d'un arc existant
        [TestMethod]
        public void SetEdgeWeight()
        {
            SetEdgeWeight(directed: true);     // graphe orienté
            SetEdgeWeight(directed: false);    // graphe non orienté
        }



        // ---------------------- Méthodes utilitaires permettant de factoriser le code dans les tests ---------------------

        // --- Création d'un graphe ---

        // Vérifie que l'ordre d'un graphe vide est 0
        // le un graphe orienté ou non, selon la valeur de `directed` 
        private void EmptyGraph(bool directed)
        {
            Graph g = new Graph(directed: directed);
            Assert.AreEqual(0, g.Order, $"L'ordre d'un graphe vide est 0");
        }


        // --- Ajout d'un sommet ---

        // Vérifie que AddVertex() ajoute un sommet avec la bonne valeur et incrémente l'ordre du graphe
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void AddVertex_VertexAdded(bool directed)
        {
            Graph g = new Graph(directed: directed);

            float aValue = 1;
            g.AddVertex("A", aValue);
            Assert.AreEqual(1, g.Order, "L'ordre du graphe est 1 après l'ajout de A");
            Assert.AreEqual(aValue, g.GetVertexValue("A"), $"La valeur de A est {aValue}");

            float bValue = 2;
            g.AddVertex("B", bValue);
            Assert.AreEqual(2, g.Order, "L'ordre du graphe est 2 après l'ajout de B");
            Assert.AreEqual(bValue, g.GetVertexValue("B"), $"La valeur de A est {bValue}");
        }

        // Vérifie qu'un sommet ajouté avec AddVertex() n'est lié à aucun arc, et n'a donc aucun voisin 
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void AddVertex_NoEdge(bool directed)
        {
            Graph g = new Graph(directed: directed);

            g.AddVertex("A");
            g.AddVertex("B", 42);

            // Vérifie que A n'a aucun arc ni voisin
            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.GetEdgeWeight("A", "B");
            },
                "Il n'y a pas d'arc entre A et B"
            );

            List<string> aNeighbors = g.GetNeighbors("A");
            Assert.IsFalse(aNeighbors.Contains("B"), "B n'est pas dans le voisinage de A");

            // Si le graphe n'est pas orienté, vérifie que B n'a aucun arc ni voisin
            if (!directed)
            {
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    g.GetEdgeWeight("B", "A");
                },
                    "Il n'y a pas d'arc entre B et A"
                );

                List<string> bNeighbors = g.GetNeighbors("B");
                Assert.IsFalse(bNeighbors.Contains("A"), "A n'est pas dans le voisinage de B");
            }

        }


        // Vérifie que AddVertex() lève une exception si on tente d'ajouter deux fois le même sommet
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void AddDuplicateVertex(bool directed)
        {
            Graph g = new Graph(directed: directed);

            g.AddVertex("A");

            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.AddVertex("A");
            },
                "Une ArgumentException est levée si on tente d'ajouter deux sommets avec le même nom"
            );

        }


        // --- Suppression d'un sommet ---

        // Vérifie que RemoveVertex() lève une exception si le sommet n'existe pas
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void RemoveUnexistingVertex(bool directed)
        {
            Graph g = new Graph(directed: directed);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.RemoveVertex("A");
            },
                "Une ArgumentException est levée si on tente de supprimer un sommet inexistant"
            );
        }

        // Crée la situation initiale commune aux tests de RemoveVertex()
        private Graph RemoveVertexSetup(bool directed)
        {

            Graph g = new Graph(directed: directed);

            // Graphe avec deux sommets A et B, reliés par un arc
            g.AddVertex("A");
            g.AddVertex("B");
            g.AddEdge("A", "B");

            // suppression de B
            g.RemoveVertex("B");

            return g;
        }

        // Vérifie que RemoveVertex() supprime un sommet et décrémente l'ordre du graphe
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void RemoveVertex_VertexRemoved(bool directed)
        {
            Graph g = RemoveVertexSetup(directed);

            // --- vérifications ---

            // décrément de l'ordre du graphe
            Assert.AreEqual(1, g.Order, "L'ordre du graphe doit être 1 après la suppression de B");

            // suppresion de B
            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.GetVertexValue("B");
            },
                "Tenter de récupérer la valeur de B doit lever une ArgumentException car B n'existe plus"
            );
        }

        // Vérifie que RemoveVertex() supprime les arcs reliés au sommet supprimé
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void RemoveVertex_EdgesRemoved(bool directed)
        {
            // situation initiale : crée un graphe avec un arc entre A et B, puis supprime B
            Graph g = RemoveVertexSetup(directed);

            // vérifie que l'arc (B,A) a été supprimé
            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.GetEdgeWeight("B", "A");
            },
                "Il n'y a plus d'arc entre B et A après suppression de B"
            );

            // dans un graphe non-orienté, vérifie que l'arc (A,B) a également été supprimé
            if (!directed)
            {
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    g.GetEdgeWeight("A", "B");
                },
                    "Il n'y a plus d'arc entre A et B après suppression de B"
                );
            }

            // vérifie que B n'est plus voisin de A
            List<string> aNeighbors = g.GetNeighbors("A");
            Assert.IsFalse(aNeighbors.Contains("B"), "B n'est plus dans le voisinage de A");
        }

        // --- Valeur d'un sommet ---

        // Vérifie que GetVertexValue() lève une exception si le sommet n'existe pas
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void GetUnexistingVertexValue(bool directed)
        {
            Graph g = new Graph(directed: directed);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.GetVertexValue("A");
            },
                "Une ArgumentException est levée si on tente de récupérer la valeur d'un sommet inexistant"
            );

        }

        // Vérifie que SetVertexValue() lève une exception si le sommet n'existe pas
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void SetUnexistingVertexValue(bool directed)
        {
            Graph g = new Graph(directed: directed);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.SetVertexValue("A", 42);
            },
                "Une ArgumentException est levée si on tente de modifier la valeur d'un sommet inexistant"
            );

        }

        // Vérifie que SetVertexValue() modifie la valeur d'un sommet
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void SetVertexValue(bool directed)
        {
            Graph g = new Graph(directed: directed);

            float firstValue = 1;
            g.AddVertex("A", firstValue);

            Assert.AreEqual(firstValue, g.GetVertexValue("A"), $"Avant modification, la valeur de A est {firstValue}");

            float secondValue = 2;
            g.SetVertexValue("A", secondValue);
            Assert.AreEqual(secondValue, g.GetVertexValue("A"), $"Après modification, la valeur de A est {secondValue}");
        }


        // --- Ajout d'un arc ---

        // Vérifie que AddEdge() ajoute un arc et modifie le voisinage des extrémités
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void AddEdge(bool directed)
        {
            Graph g = new Graph(directed: directed);

            g.AddVertex("A");
            g.AddVertex("B");

            float weight = 42;
            g.AddEdge("A", "B", weight);

            // vérification de la présence de l'arc (A,B)
            Assert.AreEqual(weight, g.GetEdgeWeight("A", "B"), $"Le poids de l'arc (A,B) est {weight}");

            List<string> aNeighbors = g.GetNeighbors("A");
            Assert.IsTrue(aNeighbors.Contains("B"), "B est dans le voisinage de A");

            // Si le graphe n'est pas orienté, vérification de la présence de l'arc (B,A)
            if (!directed)
            {
                Assert.AreEqual(weight, g.GetEdgeWeight("B", "A"), $"Le poids de l'arc (B,A) est {weight}");

                List<string> bNeighbors = g.GetNeighbors("B");
                Assert.IsTrue(bNeighbors.Contains("A"), "A est dans le voisinage de B");
            }
        }

        // Vérifie que AddEdge() lève une exception si on tente d'ajouter deux fois le même arc
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void AddDuplicateEdge(bool directed)
        {
            Graph g = new Graph(directed: directed);

            g.AddVertex("A");
            g.AddVertex("B");

            g.AddEdge("A", "B");

            // Dans tous les cas (graphe orienté ou non), on ne peut pas ajouter l'arc (A,B) une deuxième fois
            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.AddEdge("A", "B");
            },
                 "Une ArgumentException est levée si on tente d'ajouter une deuxième fois l'arc (A,B)"
            );

            // La possibilité d'ajouter l'arc (B,A) dépend de l'orientation du graphe

            // Si le graphe est orienté, l'ajout est possible. On vérifie qu'il a bien été effectué
            if (directed)
            {
                float weight = 42;
                g.AddEdge("B", "A", weight);
                Assert.AreEqual(weight, g.GetEdgeWeight("B", "A"), $"Le poids de l'arc (B,A) est {weight}");
            }
            else
            // Sinon, l'ajout n'est pas possible, car l'arc (B,A) existe déjà implicitement : les arcs (A,B) et (B,A) modélisent la même arête : {A,B}
            {
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    g.AddEdge("B", "A");
                },
                     "Une ArgumentException est levée si on tente d'ajouter l'arc (B,A), car il existe déjà implicitement après l'ajout de (A,B)"
                );
            }

        }

        // Vérifie que AddEdge() lève une exception si une de ses extrémités n'existe pas
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void AddEdgeUnexistingVertex(bool directed)
        {
            Graph g = new Graph(directed: directed);

            g.AddVertex("A");

            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.AddEdge("A", "B");
            },
                 "Une ArgumentException est levée si on tente d'ajouer un arc entre A et B, alors que B n'existe pas"
            );

        }


        // --- Suppression d'un arc ---

        // Vérifie que RemoveEdge() lève une exception si une de ses extrémités n'existe pas
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void RemoveEdgeUnexistingVertex(bool directed)
        {
            Graph g = new Graph(directed: directed);

            g.AddVertex("A");

            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.RemoveEdge("A", "B");
            },
                 "Une ArgumentException est levée si on tente de supprimer l'arc (A,B) alors que B n'existe pas"
            );

        }

        // Vérifie que RemoveEdge() lève une exception l'arc à supprimer l'existe pas
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void RemoveUnexistingEdge(bool directed)
        {
            Graph g = new Graph(directed: directed);

            g.AddVertex("A");
            g.AddVertex("B");
            g.AddVertex("C");

            g.AddEdge("A", "B");

            // Dans tous les cas (graphe orienté ou non), l'arc (A,C) n'existe pas (mais ses extrémités si)
            // tentative de suppression => ArgumentException
            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.RemoveEdge("A", "C");
            },
                 "Une ArgumentException est levée si on tente de supprimer l'arc (A,C) alors qu'il n'existe pas (mais ses extrémités si)"
            );

            // La suppression de l'arc (B,A) dépend de l'orientation du graphe

            // Si le graphe est orienté, alors la suppression de (B,A) doit échouer, car cet arc n'existe pas (seul (A,B) existe)
            if (directed)
            {
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    g.RemoveEdge("B", "A");
                },
                     "Une ArgumentException est levée si on tente de supprimer l'arc (B,A), car seul (A,B) existe"
                );
            }
            else
            // Sinon, elle doit réussir, car supprimer l'arc (A,B) ou (B,A) revient à supprimer l'arête {A,B}
            {
                g.RemoveEdge("B", "A"); // aucune d'exception levée

                // les arcs (A,B) et (B,A) ont été supprimés
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    g.GetEdgeWeight("A", "B");
                },
                     "L'arc (A,B) n'est plus présent : tenter d'en consulter la valeur lève une ArgumentException"
                );

                Assert.ThrowsException<ArgumentException>(() =>
                {
                    g.RemoveEdge("B", "A");
                },
                     "L'arc (B,A) n'est plus présent : tenter d'en consulter la valeur lève une ArgumentException"
                );
            }
        }


        // Vérifie que RemoveEdge() supprime un arc et modifie le voisinage des extrémités
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void RemoveEdge(bool directed)
        {
            Graph g = new Graph(directed: directed);

            g.AddVertex("A");
            g.AddVertex("B");

            // Crée un lien bidirectionnel entre A et B
            float weight = 42;
            g.AddEdge("A", "B", weight);

            if (directed)   // si le graphe n'est pas orienté, on n'a rien à faire : la création de (A,B) crée automatiquement (B,A)
                            // si le graphe est orienté, on ajoute l'arc inverse (B,A) et on lui affecte un poids opposé pour le différencier de (A,B)
            {
                g.AddEdge("B", "A", -weight);
            }

            g.RemoveEdge("A", "B");

            // Dans tous les cas (graphe orienté ou non), l'arc (A,B) n'existe plus
            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.GetEdgeWeight("A", "B");
            },
                 "Tenter de récupérer le poids de l'arc (A,B) doit lever une ArgumentException car il n'existe plus"
            );

            List<string> aNeighbors = g.GetNeighbors("A");
            Assert.IsFalse(aNeighbors.Contains("B"), "B n'est plus dans le voisinage de A");

            // La suppression de l'arc (B,A) dépend de l'orientation du graphe
            List<string> bNeighbors = g.GetNeighbors("B");

            // Si le graphe est orienté, alors la suppression de (A,B) l'entraîne pas la suppression de (B,A)
            if (directed)
            {
                Assert.AreEqual(-weight, g.GetEdgeWeight("B", "A"), $"L'arc (B,A) n'a pas été supprimé et son poids est {-weight}");
                Assert.IsTrue(bNeighbors.Contains("A"), "A est toujours dans le voisinage de B");
            }
            else
            // Sinon, la suppression de l'arc (A,B) entraîne la suppression de (B,A)
            {
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    g.GetEdgeWeight("B", "A");
                },
                     "La suppression de l'arc (A,B) entraîne la suppression de (B,A)"
                );

                Assert.IsFalse(bNeighbors.Contains("A"), "A n'est plus dans le voisinage de B");
            }
        }


        // --- Poids d'un arc ---

        // Vérifie que GetEdgeWeight() lève une exception si une de ses extrémités n'existe pas
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void GetEdgeWeightUnexistingVertex(bool directed)
        {
            Graph g = new Graph(directed: directed);

            g.AddVertex("A");

            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.GetEdgeWeight("A", "B");
            },
                 "Une ArgumentException est levée si on tente de consulter le poids de l'arc (A,B) alors que B n'existe pas"
            );
        }

        // Vérifie que GetEdgeWeight() lève une exception si l'arc n'existe pas
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void GetUnexistingEdgeWeight(bool directed)
        {
            Graph g = new Graph(directed: directed);

            g.AddVertex("A");
            g.AddVertex("B");
            g.AddVertex("C");

            float weight = 42;
            g.AddEdge("A", "B", weight);

            // Dans tous les cas (graphe orienté ou non), l'arc (A,C) n'existe pas (mais ses extrémités si)
            // tentative de consulation du poids => ArgumentException
            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.GetEdgeWeight("A", "C");
            },
                 "Une ArgumentException est levée si on tente de consulter le poids l'arc (A,C) alors qu'il n'existe pas (mais ses extrémités si)"
            );

            // La consultation du poids de l'arc (B,A) est possible ou non, selon l'orientation du graphe

            // Si le graphe est orienté, alors la consultation du poids de (B,A) doit échouer, car cet arc n'existe pas (seul (A,B) existe)
            if (directed)
            {
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    g.GetEdgeWeight("B", "A");
                },
                     "Une ArgumentException est levée si on tente de consulter le poids de l'arc (B,A), car seul (A,B) existe"
                );
            }
            else
            // Sinon, elle doit réussir, car consulter le poids de l'arc (A,B) ou (B,A) revient à consulter le poids de l'arête {A,B}
            {
                Assert.AreEqual(weight, g.GetEdgeWeight("B", "A"), $"Le poids de l'arc (B,A) et le même que celui de l'arc (A,B) : {weight}");
            }
        }

        // Vérifie que SetEdgeWeight() lève une exception si une de ses extrémités n'existe pas
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void SetEdgeWeightUnexistingVertex(bool directed)
        {
            Graph g = new Graph(directed: directed);

            g.AddVertex("A");

            Assert.ThrowsException<ArgumentException>(() =>
            {
                g.SetEdgeWeight("A", "B", 42);
            },
                 "Une ArgumentException est levée si on tente de modifier le poids de l'arc (A,B) alors que B n'existe pas"
            );
        }

        // Vérifie que SetEdgeWeight() modifie la valeur d'un arc existant
        // dans un graphe orienté ou non, selon la valeur de `directed` 
        private void SetEdgeWeight(bool directed)
        {
            Graph g = new Graph(directed: directed);
            g.AddVertex("A");
            g.AddVertex("B");

            float firstWeight = 1;
            g.AddEdge("A", "B", firstWeight);

            Assert.AreEqual(firstWeight, g.GetEdgeWeight("A", "B"), $"Avant modification du poids de l'arc (A,B), la valeur du poids de l'arc (A,B) est {firstWeight}");

            float secondWeight = 2;
            g.SetEdgeWeight("A", "B", secondWeight);

            // dans tous les cas (graphe orienté ou non), le poids de l'arc (A,B) doit être modifié
            Assert.AreEqual(secondWeight, g.GetEdgeWeight("A", "B"), $"Après modification du poids de l'arc (A,B), la valeur du poids de l'arc (A,B) est {secondWeight}");

            // si le graphe n'est pas orienté, le poids de l'arc (B,A) doit également être modifié
            if (!directed)
            {
                Assert.AreEqual(secondWeight, g.GetEdgeWeight("B", "A"), $"Après modification du poids de l'arc (A,B), la valeur du poids de l'arc (B,A) est {secondWeight}");
            }
        }
    }
}
