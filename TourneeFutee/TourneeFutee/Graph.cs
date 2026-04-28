namespace TourneeFutee
{
    public class Graph
    {

        // TODO : ajouter tous les attributs que vous jugerez pertinents 

        private int order;
        private bool directed;
        private Matrix matrice;
        private Dictionary<string, int> nomSommets; //associe le nom des sommets à leur indice
        private List<Sommet> sommets; //Indice d'un sommet dans la liste correspond à l'indice dans la matrice
        // --- Construction du graphe ---

        // Contruit un graphe (`directed`=true => orienté)
        // La valeur `noEdgeValue` est le poids modélisant l'absence d'un arc (0 par défaut)
        public Graph(bool directed, float noEdgeValue = 0)
        {
            this.order = 0;
            this.directed = directed;
            this.matrice= new Matrix(0,0,noEdgeValue);
            nomSommets = new Dictionary<string, int>();
            sommets = new List<Sommet>();
        }

        // --- Propriétés ---

        // Propriété : ordre du graphe
        public int Order
        {
            get { return this.order; }
        }
        // Propriété : graphe orienté ou non
        public bool Directed
        {
            get { return this.directed; }
        }
        //Dictionnaire des noms
        public Dictionary<string, int> NomSommets
        {
            get { return this.nomSommets; }
            set { this.nomSommets = value; }
        }
        public Matrix Matrice
        {
            get { return this.matrice; }
            set { this.matrice = value; }
        }
        public List<Sommet> Sommets
        {
            get { return this.sommets; }
            set { this.sommets = value; }
        }

        // --- Gestion des sommets ---

        // Ajoute le sommet de nom `name` et de valeur `value` (0 par défaut) dans le graphe
        // Lève une ArgumentException s'il existe déjà un sommet avec le même nom dans le graphe
        public void AddVertex(string name, float value = 0)
        {
            if (nomSommets.ContainsKey(name)) throw new ArgumentException("Nom déjà utilisé");
            Sommet s = new Sommet(name, value);
            int c = sommets.Count;
            sommets.Add(s);
            nomSommets.Add(name, c);
            matrice.AddRow(c);
            matrice.AddColumn(c);
            order++; 
        }

        // Supprime le sommet de nom `name` du graphe (et tous les arcs associés)
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public void RemoveVertex(string name)
        {
            if (!nomSommets.ContainsKey(name)) throw new ArgumentException("Sommet innexistant"); // on verifie si le sommet existe dans le dictionnaire
            int i = nomSommets[name];  // on récupère l'indice du sommet dans le graphe
            matrice.RemoveColumn(i);// on enleve la colonne i dans la matrice 
            matrice.RemoveRow(i);
            sommets.RemoveAt(i);
            nomSommets.Remove(name);
            foreach (var key in nomSommets.Keys)    // On modifie les indices associés au nom des sommets
            {
                if (nomSommets[key] > i) // si un sommet avait un indice plus grand que celui qu'on a supprimé alors il va se décaler d'une place vers la gauche dans la liste
                {
                    nomSommets[key]--;  // on enlève 1 à son indice pour remettre les bons numero
                }
            }
            order--;  // on diminue de 1 le nombre tota de sommets dans le graphe
        }

        // Renvoie la valeur du sommet de nom `name`
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public float GetVertexValue(string name)
        {
            if (!nomSommets.ContainsKey(name)) throw new ArgumentException("Sommet innexistant");
            int i = nomSommets[name];
            return sommets[i].Valeur;
        }

        // Affecte la valeur du sommet de nom `name` à `value`
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public void SetVertexValue(string name, float value)
        {
            if (!nomSommets.ContainsKey(name)) throw new ArgumentException("Sommet innexistant");
            int i = nomSommets[name];
            sommets[i].Valeur=value; //change la valeur d'un sommmet 
        }


        // Renvoie la liste des noms des voisins du sommet de nom `vertexName`
        // (si ce sommet n'a pas de voisins, la liste sera vide)
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public List<string> GetNeighbors(string vertexName)
        {
            List<string> neigborNames = new List<string>();// on cree une liste vide qui va contenir les nom des sommets voisins
            if (!nomSommets.ContainsKey(vertexName))
                throw new ArgumentException("Ce sommet n'existe pas");
            int i = nomSommets[vertexName]; // on recupere l indice du sommet dans la matrice (correspond a la ligne de la matrice)
            for (int j = 0; j < order; j++)
            {
                if (matrice.GetValue(i, j) != matrice.DefaultValue)// si la valeur dans la matrice n est pas la valeur par defaut il existe un arc entre le sommet i et le sommet j
                {
                    neigborNames.Add(sommets[j].Nom); // on ajoute le nom du sommet j dans la liste des voisins
                }
            }
            return neigborNames;// on revoit la liste des voisnis trouvés
        }

        // --- Gestion des arcs ---

        /* Ajoute un arc allant du sommet nommé `sourceName` au sommet nommé `destinationName`, avec le poids `weight` (1 par défaut)
         * Si le graphe n'est pas orienté, ajoute aussi l'arc inverse, avec le même poids
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - il existe déjà un arc avec ces extrémités
         */
        public void AddEdge(string sourceName, string destinationName, float weight = 1)
        {
            if (!nomSommets.ContainsKey(sourceName) || !nomSommets.ContainsKey(destinationName))
                throw new ArgumentException("Le sommet n'exitse pas");
            int i = nomSommets[sourceName]; //départ
            int j = nomSommets[destinationName]; //arrivée
            if (matrice.GetValue(i, j) != matrice.DefaultValue)
                throw new ArgumentException("Arc déjà existant");
            matrice.SetValue(i, j, weight);//on met la valeur du poids dans la matrice
            if (this.directed==false)//arc pas oriente
            {
                matrice.SetValue(j, i, weight);//on fait donc dans les deux sens
            }
        }

        /* Supprime l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` du graphe
         * Si le graphe n'est pas orienté, supprime aussi l'arc inverse
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public void RemoveEdge(string sourceName, string destinationName)
        {
            if (!nomSommets.ContainsKey(sourceName) || !nomSommets.ContainsKey(destinationName))
                throw new ArgumentException("Le sommet est inexistant");
            int i = nomSommets[sourceName];
            int j = nomSommets[destinationName];
            if (matrice.GetValue(i, j) == matrice.DefaultValue) // on verifie si l arc existe. si la valeur est celle par défaut il n y a pas d arc
                throw new ArgumentException("L'arc est inexistant");
            matrice.SetValue(i, j, matrice.DefaultValue); // on remet la valeur par défaut dans la matrice donc suppr l arc entre i et j
            if (this.directed == false)
            {
                matrice.SetValue(j, i, matrice.DefaultValue); //suppr l arc da,s les deux sens si pas orienté 
            }
        }

        /* Renvoie le poids de l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName`
         * Si le graphe n'est pas orienté, GetEdgeWeight(A, B) = GetEdgeWeight(B, A) 
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public float GetEdgeWeight(string sourceName, string destinationName)
        {
            if (!nomSommets.ContainsKey(sourceName) || !nomSommets.ContainsKey(destinationName))
                throw new ArgumentException("Le sommet est inexistant");
            int i = nomSommets[sourceName];
            int j = nomSommets[destinationName];
            if (matrice.GetValue(i, j) == matrice.DefaultValue)
                throw new ArgumentException("L'arc est inexistant");
            return matrice.GetValue(i, j); // on renvoie la valeur stockée dans la matrice = poids
        }

        /* Affecte le poids l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` à `weight` 
         * Si le graphe n'est pas orienté, affecte le même poids à l'arc inverse
         * Lève une ArgumentException si un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         */
        public void SetEdgeWeight(string sourceName, string destinationName, float weight)
        {
            if (!nomSommets.ContainsKey(sourceName) || !nomSommets.ContainsKey(destinationName))
                throw new ArgumentException("Le sommet est inexistant");
            int i = nomSommets[sourceName];
            int j = nomSommets[destinationName];
            matrice.SetValue(i, j, weight);
            if (this.directed == false)
            {
                matrice.SetValue(j, i, weight); //on change le poids dans les deux sens si pas orienté
            }
        }
    }
}
