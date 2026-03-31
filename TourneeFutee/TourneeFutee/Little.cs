namespace TourneeFutee
{
    // Résout le problème de voyageur de commerce défini par le graphe `graph`
    // en utilisant l'algorithme de Little
    public class Little
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        private Matrix matrice;
        private Graph graph;
        int nbSommets;

        // Instancie le planificateur en spécifiant le graphe modélisant un problème de voyageur de commerce
        public Little(Graph graph)
        {
            if (graph == null) 
                throw new ArgumentNullException(nameof(graph));
            this.graph = graph;
            this.matrice = new Matrix(graph.NbVertices, graph.NbVertices, float.PositiveInfinity);
            this.matrice = graph.matrice.Clone() as Matrix;

            // TODO : implémenter
        }
        public Matrix Matrice
        {
            get { return matrice; }
            set { matrice = value; }
        }
        public Graph Graph
        {
            get { return graph; }
            set { graph = value; }
        }
        public int NbSommets
        {
            get { return nbSommets;}
        }
        // Instancie le planificateur en spécifiant le graphe modélisant un problème de voyageur de commerce
        public Little(Graph graph)
        {
            this.matrice = graph.Matrice;
            this.graph = graph;
            this.nbSommets = graph.Sommets.Count;
        }

        // Trouve la tournée optimale dans le graphe `this.graph`
        // (c'est à dire le cycle hamiltonien de plus faible coût)
        public Tour ComputeOptimalTour()
        {
            // TODO : implémenter
            return new Tour();
        }

        // --- Méthodes utilitaires réalisant des étapes de l'algorithme de Little


        // Réduit la matrice `m` et revoie la valeur totale de la réduction
        // Après appel à cette méthode, la matrice `m` est *modifiée*.
        public static float ReduceMatrix(Matrix m)
        {
            // TODO : implémenter
            return 0.0f;
        }

        // Renvoie le regret de valeur maximale dans la matrice de coûts `m` sous la forme d'un tuple `(int i, int j, float value)`
        // où `i`, `j`, et `value` contiennent respectivement la ligne, la colonne et la valeur du regret maximale
        public static (int i, int j, float value) GetMaxRegret(Matrix m)
        {
            // TODO : implémenter
            return (0, 0, 0.0f);

        }

        /* Renvoie vrai si le segment `segment` est un trajet parasite, c'est-à-dire s'il ferme prématurément la tournée incluant les trajets contenus dans `includedSegments`
         * Une tournée est incomplète si elle visite un nombre de villes inférieur à `nbCities`
         */
        public static bool IsForbiddenSegment((string source, string destination) segment, List<(string source, string destination)> includedSegments, int nbCities)
        {

            int i = 0;
            string current = segment.destination;
            int length = 1;
            while (true)
            {
                bool found = false;
                while(i<includedSegments.Count) //Recherche si il n'est pas déjà possible de faire ce chemin (cherche un cycle)
                {
                    if (includedSegments[i].source == current)
                    {
                        current = includedSegments[i].destination;
                        length++;
                        found = true;
                        break;
                    }
                    i++;
                }             
                if (found==false)
                    return false;
                if (current == segment.source) // cycle autorisé seulement si complet
                {
                    if (length < nbCities)
                        return true;
                    else
                        return false;
                }
            }
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }
}
