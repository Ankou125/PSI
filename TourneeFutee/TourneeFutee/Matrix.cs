namespace TourneeFutee
{
    public class Matrix
    {
        private int nbRows;
        private int nbColumns;
        private float defaultValue;
        private List<List<float>> matrice;

        /* Crée une matrice de dimensions `nbRows` x `nbColums`.
         * Toutes les cases de cette matrice sont remplies avec `defaultValue`.
         * Lève une ArgumentOutOfRangeException si une des dimensions est négative
         */

        /* notes :
         * il n'y a pas de set pour les propriétés NbRows, NbColumns et DefaultValue
         * la matrice est implémentée par une liste de listes de float
         * 
         */

        public Matrix(int nbRows = 0, int nbColumns = 0, float defaultValue = 0)
        {
            if (nbRows < 0) throw new ArgumentOutOfRangeException(nameof(nbRows));
            if (nbColumns < 0) throw new ArgumentOutOfRangeException(nameof(nbColumns));
            this.nbColumns = nbColumns;
            this.nbRows = nbRows;
            this.defaultValue = defaultValue;
            this.matrice = new List<List<float>>();
            for (int i = 0; i < nbRows; i++)
            {
                List<float> ligne = new List<float>(); // on cree une nouvelle liste pour representer une ligne
                for (int j = 0; j < nbColumns; j++)
                {
                    ligne.Add(defaultValue);// on ajoute la valeur dans la case
                }
                matrice.Add(ligne);// on ajoute la ligne complete dans la matrice
            }
            // On crée une nouvelle liste pour chaque ligne pour éviter que toutes les lignes pointent vers la même référence.
        }

        public float DefaultValue
        {
            get { return defaultValue; }
        }

        public int NbRows
        {
            get { return nbRows; }
        }

        public int NbColumns
        {
            get { return nbColumns; }
        }

        public List<List<float>> Matrice
        {
            get { return this.matrice; }
            set { this.matrice = value; }
        }

        /* Insère une ligne à l'indice `i`. Décale les lignes suivantes vers le bas.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `i` = NbRows, insère une ligne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
         */
        public void AddRow(int i)
        {
            if (i < 0 || i > nbRows)   
                throw new ArgumentOutOfRangeException(nameof(i));
            List<float> newRow = new List<float>(nbColumns); 
            for (int  c = 0;  c < nbColumns ;  c++)
            {
                newRow.Add(defaultValue); // on ajoute la valeur dans chaque case de la nouvelle ligne

            }
            matrice.Insert(i, newRow);// on insere la nouvelle ligne dans la matrice a la position i et les lignes après i vont se decaler
            nbRows++;// ducoup on augmente de 1 le nombre total de lignes dans la matrice
        }

        /* Insère une colonne à l'indice `j`. Décale les colonnes suivantes vers la droite.
         * Toutes les cases de la nouvelle colonne  contiennent DefaultValue.
         * Si `j` = NbColums, insère une colonne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
         */
        public void AddColumn(int j)

        {
            if (j < 0 || j > nbColumns)   
                throw new ArgumentOutOfRangeException(nameof(j));
            for (int i = 0; i < nbRows; i++)
            {
                matrice[i].Insert(j, defaultValue);
            }
            nbColumns++;
        }

        // Supprime la ligne à l'indice `i`. Décale les lignes suivantes vers le haut.
        // Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
        public void RemoveRow(int i)
        {
            if (i < 0 || i >= nbRows)   
                throw new ArgumentOutOfRangeException(nameof(i));
            matrice.RemoveAt(i);
            nbRows--;
        }

        // Supprime la colonne à l'indice `j`. Décale les colonnes suivantes vers la gauche.
        // Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
        public void RemoveColumn(int j)
        {
            if (j < 0 || j >= nbColumns)   
                throw new ArgumentOutOfRangeException(nameof(j));
            for (int i = 0; i < nbRows; i++)
            {
                matrice[i].RemoveAt(j);
            }
            nbColumns--;
        }

        // Renvoie la valeur à la ligne `i` et colonne `j`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public float GetValue(int i, int j)
        {
            if (i < 0 || i >= nbRows)   
                throw new ArgumentOutOfRangeException(nameof(i));
            if (j < 0 || j >= nbColumns)
                throw new ArgumentOutOfRangeException(nameof(j));
            return matrice[i][j];
            // return 0.0f;
        }

        // Affecte la valeur à la ligne `i` et colonne `j` à `v`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public void SetValue(int i, int j, float v)
        {
            if (i < 0 || i >= nbRows)   
                throw new ArgumentOutOfRangeException(nameof(i));
            if (j < 0 || j >= nbColumns)
                throw new ArgumentOutOfRangeException(nameof(j));
            matrice[i][j] = v;// on change la valeur de la case situé a la ligne i et la colonne j par v
        }

        // Affiche la matrice
        // On parcourt chaque ligne puis chaque colonne et on affiche les valeurs.
        public void Print()
        {
            for (int i = 0; i < nbRows; i++)
            {
                for (int j = 0; j < nbColumns; j++)
                {
                    Console.Write(matrice[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}