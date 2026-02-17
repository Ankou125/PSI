using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace TourneeFutee
{
    public class Matrix
    {
        int nbRows;
        int nbColumns;
        float defaultValue;
        List<List<float>> matrice;

        /* Crée une matrice de dimensions `nbRows` x `nbColums`.
         * Toutes les cases de cette matrice sont remplies avec `defaultValue`.
         * Lève une ArgumentOutOfRangeException si une des dimensions est négative
         */

        /* notes : 
         * il n'y a pas de set pour les propriétés NbRows, NbColumns et DefaultValue
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
            int i= 0;
            int j = 0;
            List<float> ligne= new List<float>();
            while (i < nbColumns)
            {
                ligne.Add(defaultValue);
                i++;
            }
            while(j< nbRows)
            {
                matrice.Add(ligne);
                j++;
            }
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
            var newRow = new List<float>(nbColumns);
            for (int  c = 0;  c < nbColumns ;  c++)
            {
                newRow.Add(defaultValue);
            }
            matrice.Insert(i, newRow);
            nbRows++;
        }

        /* Insère une colonne à l'indice `j`. Décale les colonnes suivantes vers la droite.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `j` = NbColums, insère une colonne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
         */
        public void AddColumn(int j)
        {
            if (j < 0 || j > nbColumns)   
                throw new ArgumentOutOfRangeException(nameof(j));
            for (int r = 0; r < nbRows; r++)
            {
                matrice[r].Insert(j, defaultValue);
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
            for (int r = 0; r < nbRows; r++)
            {
                matrice[r].RemoveAt(j);
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
            matrice[i][j] = v;
        }

        // Affiche la matrice
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
        } // à verif 

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }


}
