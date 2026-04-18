namespace TourneeFutee
{
    // Résout le problème de voyageur de commerce défini par le graphe `graph`
    // en utilisant l'algorithme de Little
    public class Little
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        Matrix matrice;
        Graph graphe;
        int nbSommets;

        // Instancie le planificateur en spécifiant le graphe modélisant un problème de voyageur de commerce
        public Matrix Matrice
        {
            get { return matrice; }
            set { matrice = value; }
        }
        public Graph Graph
        {
            get { return graphe; }
            set { graphe = value; }
        }
        public int NbSommets
        {
            get { return nbSommets; }
        }
        // Instancie le planificateur en spécifiant le graphe modélisant un problème de voyageur de commerce
        public Little(Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));
            this.matrice = graph.Matrice.Clone();
            this.graphe = graph;
            this.nbSommets = graph.Sommets.Count;
        }

        // Trouve la tournée optimale dans le graphe `this.graph`
        // (c'est à dire le cycle hamiltonien de plus faible coût)
        public Tour ComputeOptimalTour()
        {
            Matrix m = this.matrice.Clone();
            for (int i = 0; i < m.NbRows; i++)
            {
                m.SetValue(i, i, float.PositiveInfinity);
            }
            float borneInitiale = ReduceMatrix(m);
            List<(string source, string destination)> segmentsChoisis = new List<(string source, string destination)>();
            List<string> villesSources = new List<string>();
            List<string> villesDestinations = new List<string>();
            foreach (var sommet in this.graphe.Sommets)
            {
                villesSources.Add(sommet.Nom);
                villesDestinations.Add(sommet.Nom);
            }
            float meilleurCout = float.PositiveInfinity;
            List<(string source, string destination)> meilleursSegments = new List<(string source, string destination)>();

            Search(m, segmentsChoisis, villesSources, villesDestinations, borneInitiale, ref meilleurCout, ref meilleursSegments);
            if (meilleursSegments.Count == 0)
                return new Tour();

            return new Tour(meilleursSegments, meilleurCout);
        }
        private void Search(Matrix m, List<(string source, string destination)> segmentsChoisis, List<string> villesSources, List<string> villesDestinations, float borneCourante, ref float meilleurCout, ref List<(string source, string destination)> meilleursSegments)
        {
            if (borneCourante >= meilleurCout)
                return;

            if (m.NbRows == 2 && m.NbColumns == 2) // Cas terminal : matrice 2x2 => on termine directement
            {
                List<(string source, string destination)> tourneeFinale = new List<(string source, string destination)>(segmentsChoisis);
                for (int i = 0; i < 2; i++)
                {
                    bool found = false;
                    for (int j = 0; j < 2; j++)
                    {
                        float valeur = m.GetValue(i, j);
                        var candidat = (villesSources[i], villesDestinations[j]);
                        if (valeur != float.PositiveInfinity && !IsForbiddenSegment(candidat, tourneeFinale, this.nbSommets))
                        {
                            tourneeFinale.Add(candidat);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        return;
                }
                if (tourneeFinale.Count == this.nbSommets)
                {
                    float coutReel = ComputeTourCost(tourneeFinale);
                    if (coutReel < meilleurCout)
                    {
                        meilleurCout = coutReel;
                        meilleursSegments = new List<(string source, string destination)>(tourneeFinale);
                    }
                }
                return;
            }
            if (m.NbRows == 0 || m.NbColumns == 0)
                return;
            var regret = GetMaxRegret(m);
            int iRegret = regret.i;
            int jRegret = regret.j;
            if (iRegret < 0 || jRegret < 0)
                return;
            string source = villesSources[iRegret];
            string destination = villesDestinations[jRegret];
            var segment = (source, destination);

            if (!IsForbiddenSegment(segment, segmentsChoisis, this.nbSommets)) // 1. Branche incluse d'abord
            {
                Matrix mIncluse = m.Clone();
                List<(string source, string destination)> nouveauxSegmentsChoisis = new List<(string source, string destination)>(segmentsChoisis);
                List<string> villesSourcesRestantes = new List<string>(villesSources);
                List<string> villesDestinationsRestantes = new List<string>(villesDestinations);
                nouveauxSegmentsChoisis.Add(segment);
                mIncluse.RemoveRow(iRegret);
                mIncluse.RemoveColumn(jRegret);
                villesSourcesRestantes.RemoveAt(iRegret);
                villesDestinationsRestantes.RemoveAt(jRegret);
                for (int r = 0; r < mIncluse.NbRows; r++)
                {
                    for (int c = 0; c < mIncluse.NbColumns; c++)
                    {
                        var candidat = (villesSourcesRestantes[r], villesDestinationsRestantes[c]);
                        if (IsForbiddenSegment(candidat, nouveauxSegmentsChoisis, this.nbSommets))
                        {
                            mIncluse.SetValue(r, c, float.PositiveInfinity);
                        }
                    }
                }
                float reductionInclus = 0.0f;
                if (mIncluse.NbRows > 0 && mIncluse.NbColumns > 0)
                {
                    reductionInclus = ReduceMatrix(mIncluse);
                }
                float borneInclus = borneCourante + reductionInclus;
                if (borneInclus < meilleurCout)
                {
                    Search(
                        mIncluse,
                        nouveauxSegmentsChoisis,
                        villesSourcesRestantes,
                        villesDestinationsRestantes,
                        borneInclus,
                        ref meilleurCout,
                        ref meilleursSegments
                    );
                }
            }

            Matrix mExclue = m.Clone();  // 2. Branche exclue ensuite
            mExclue.SetValue(iRegret, jRegret, float.PositiveInfinity);
            float reductionExclus = ReduceMatrix(mExclue);
            float borneExclus = borneCourante + reductionExclus;
            if (borneExclus < meilleurCout)
            {
                Search(
                    mExclue,
                    new List<(string source, string destination)>(segmentsChoisis),
                    new List<string>(villesSources),
                    new List<string>(villesDestinations),
                    borneExclus,
                    ref meilleurCout,
                    ref meilleursSegments
                );
            }
        }
        private float ComputeTourCost(List<(string source, string destination)> segments)
        {
            float total = 0.0f;
            foreach (var segment in segments)
            {
                total += this.graphe.GetEdgeWeight(segment.source, segment.destination);
            }
            return total;
        }

        // --- Méthodes utilitaires réalisant des étapes de l'algorithme de Little

        // Réduit la matrice `m` et revoie la valeur totale de la réduction
        // Après appel à cette méthode, la matrice `m` est *modifiée*.
        public static float ReduceMatrix(Matrix m)
        {
            float reduction = 0.0f; // création d'une variable pour stocker la valeur totale de la réduction
            for (int i = 0; i < m.NbRows; i++)
            {
                float minRow = float.PositiveInfinity; // initialisation du minimum de la ligne à l'infini positif
                for (int j = 0; j < m.NbColumns; j++)
                {
                    float value = m.GetValue(i, j);
                    if (value < minRow)
                    {
                        minRow = value;
                    }
                } // recherche du minimum de la ligne en parcourant tous les éléments de la ligne
                if (minRow > 0 && minRow != float.PositiveInfinity) // condition pour effectuer réduction
                {
                    for (int j = 0; j < m.NbColumns; j++)
                    {
                        float value = m.GetValue(i, j);
                        if (value != float.PositiveInfinity)
                        {
                            m.SetValue(i, j, value - minRow);
                        }
                    } // réduction de la ligne en soustrayant le minimum de chaque élément de la ligne
                    reduction += minRow;
                }
            }
            for (int j = 0; j < m.NbColumns; j++) // même processus pour les colonnes
            {
                float minCol = float.PositiveInfinity;
                for (int i = 0; i < m.NbRows; i++)
                {
                    float value = m.GetValue(i, j);
                    if (value < minCol)
                    {
                        minCol = value;
                    }
                }
                if (minCol > 0 && minCol != float.PositiveInfinity)
                {
                    for (int i = 0; i < m.NbRows; i++)
                    {
                        float value = m.GetValue(i, j);
                        if (value != float.PositiveInfinity)
                        {
                            m.SetValue(i, j, value - minCol);
                        }
                    }
                    reduction += minCol;
                }
            }
            return reduction;
        }

        // Renvoie le regret de valeur maximale dans la matrice de coûts `m` sous la forme d'un tuple `(int i, int j, float value)`
        // où `i`, `j`, et `value` contiennent respectivement la ligne, la colonne et la valeur du regret maximale
        public static (int i, int j, float value) GetMaxRegret(Matrix m)
        {
            float maxRegret = -1.0f; //stock le plus grand regret
            int bestI = 0;
            int bestJ = 0;

            for (int i = 0; i < m.NbRows; i++) //parcours la matrice
            {
                for (int j = 0; j < m.NbColumns; j++)
                {
                    if (m.GetValue(i, j) == 0) //si on trouve un 0 dans la matrice
                    {
                        float minRow = float.PositiveInfinity; //on met à infini comme ca n'importe quelle valeurs sera plus petite
                        float minCol = float.PositiveInfinity;

                        
                        for (int k = 0; k < m.NbColumns; k++) //on cherche le minimum sur la colonne
                        {
                            if (k != j) //pour ignorer la case nulle
                            {
                                float val = m.GetValue(i, k); //une autre case de la matrice
                                if (val < minRow) //si la valeur est plus petite que notre minimum actuel on la garde
                                {
                                    minRow = val; //elle devient le nouveau minimum 
                                }
                            }
                        }
                        for (int k = 0; k < m.NbRows; k++) //on fait la meme chose pour la ligne
                        {
                            if (k != i)
                            {
                                float val = m.GetValue(k, j);
                                if (val < minCol)
                                {
                                    minCol = val;
                                }
                            }
                        }//contient la plus petite valeur de la colonne j sauf celle de la case i,j
                        if (minRow == float.PositiveInfinity)
                        {
                            minRow = 0; //si on a pas trouvé un plus petit minimum que l'infini alors on met à 0
                        }
                        if (minCol == float.PositiveInfinity)
                        {
                            minCol = 0;
                        }
                        float regret = minRow + minCol; //calcul du regret
                        if (regret > maxRegret) //si le regret qu on vient de calculer est plus grand que le précédent alors on remplace
                        {
                            maxRegret = regret;
                            bestI = i;
                            bestJ = j;
                        }
                    }
                }
            }
            if (maxRegret == -1.0f)//si aucun regret a été trouvé on met à -1
            {
                return (-1, -1, 0.0f);
            }
            return (bestI, bestJ, maxRegret);//on renvoit la ligne, la colonne du regret maximal et sa valeur 
        }


        /* Renvoie vrai si le segment `segment` est un trajet parasite, c'est-à-dire s'il ferme prématurément la tournée incluant les trajets contenus dans `includedSegments`
         * Une tournée est incomplète si elle visite un nombre de villes inférieur à `nbCities`
         */
        public static bool IsForbiddenSegment((string source, string destination) segment, List<(string source, string destination)> includedSegments, int nbCities)
        {

            int i = 0;
            string current = segment.destination;
            int length = 1;
            if (includedSegments.Count(s => s.source == segment.source) > 0)
                return true;
            while (true)
            {
                i = 0;
                bool found = false;
                while (i < includedSegments.Count) //Recherche si il n'est pas déjà possible de faire ce chemin (cherche un cycle)
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
                if (found == false)
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
    }
}
