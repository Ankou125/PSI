namespace TourneeFutee.Tests
{
    // Campagne de tests unitaires vérifiant le bon fonctionnement de la classe Matrix
    [TestClass]
    public class MatrixTests
    {

        // ------------------------------------------------ Tests unitaires ------------------------------------------------

        // --- Création de matrice ---

        // Vérifie les dimensions d'une matrice vide
        [TestMethod]
        public void EmpyMatrix()
        {
            CheckDimensions(nbRows: 0, nbColumns: 0);
        }

        // Vérifie les dimensions d'une matrice non vide
        [TestMethod]
        public void Dimensions23()
        {
            CheckDimensions(nbRows: 2, nbColumns: 3);   // matrice 2 x 3
            CheckDimensions(nbRows: 3, nbColumns: 2);   // matrice 3 x 2
        }

        // Vérifie qu'une dimension invalide lève une exception 
        [TestMethod]
        public void NegativeDimensions()
        {
            int positiveDimension = 1;
            int negativeDimension = -1;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new Matrix(nbRows: negativeDimension, nbColumns: positiveDimension);
            },
                $"Tenter de créer une matrice avec {negativeDimension} lignes doit lever une ArgumentOutOfRangeException"
            );

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new Matrix(nbRows: positiveDimension, nbColumns: negativeDimension);
            },
                $"Tenter de créer une matrice avec {negativeDimension} colonnes doit lever une ArgumentOutOfRangeException"
            );
        }

        /* Construit une matrice 2 x 3 et vérifie que toutes les cases d'une nouvelle matrice contiennent la valeur par défaut
         * Matrice attendue :
         * -1 -1 -1 
         * -1 -1 -1 
         */
        [TestMethod]
        public void DefaultValue()
        {
            int nbRows = 2;
            int nbColumns = 3;
            float defaultValue = -1;

            Matrix m = new Matrix(nbRows: nbRows, nbColumns: nbColumns, defaultValue: defaultValue);

            Assert.AreEqual(defaultValue, m.DefaultValue, $"La valeur par défaut de la matrice est {defaultValue}");
            CheckConstantValue(m: m, startRow: 0, endRow: nbRows, startColumn: 0, endColumn: nbColumns, constantValue: defaultValue);

            m.Print();
        }

        // --- GetValue() ---

        // Vérifie que GetValue() lève une exception si un des indices est invalide 
        [TestMethod]
        public void GetValueInvalidIndex()
        {
            Matrix m = new Matrix(nbRows: 2, nbColumns: 3);

            int negativeIndex = -1;
            int tooHighRow = 2;
            int tooHighColumn = 3;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.GetValue(negativeIndex, 1);
            },
                $"Récupérer une valeur sur la ligne {negativeIndex} doit lever une ArgumentOutOfRangeException"
            );

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.GetValue(tooHighRow, 1);
            },
                $"Récupérer une valeur sur la ligne {tooHighRow} doit lever une ArgumentOutOfRangeException"
            );

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.GetValue(1, negativeIndex);
            },
                $"Récupérer une valeur sur la colonne {negativeIndex} doit lever une ArgumentOutOfRangeException"
            );

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.GetValue(1, tooHighColumn);
            },
                $"Récupérer une valeur sur la colonne {tooHighColumn} doit lever une ArgumentOutOfRangeException"
            );
        }

        // --- SetValue() ---

        // Vérifie que SetValue() lève une exception si un des indices est invalide 
        [TestMethod]
        public void SetValueInvalidIndex()
        {
            Matrix m = new Matrix(nbRows: 2, nbColumns: 3);

            int negativeIndex = -1;
            int tooHighRow = 2;
            int tooHighColumn = 3;
            float value = 42;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.SetValue(negativeIndex, 1, value);
            },
                $"Affecter une valeur sur la ligne {negativeIndex} doit lever une ArgumentOutOfRangeException"
            );

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.SetValue(tooHighRow, 1, value);
            },
                $"Affecter une valeur sur la ligne {tooHighRow} doit lever une ArgumentOutOfRangeException"
            );

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.SetValue(1, negativeIndex, value);
            },
                $"Affecter une valeur sur la colonne {negativeIndex} doit lever une ArgumentOutOfRangeException"
            );

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.SetValue(1, tooHighColumn, value);
            },
                $"Affecter une valeur sur la colonne {tooHighColumn} doit lever une ArgumentOutOfRangeException"
            );
        }

        /* Vérifie le comportement de SetValue() sur toutes les cases d'une matrice 3 x 2
         * Affecte les coefficients de la matrice à :
         * 0 1 
         * 2 3 
         * 4 5 
         */
        [TestMethod]
        public void SetValues32()
        {
            CheckIncreasingValues(nbRows: 3, nbColumns: 2);
        }

        /* Vérifie le comportement de SetValue() sur toutes les cases d'une matrice 2 x 3
         * Affecte les coefficients de la matrice à :
         * 0 1 2 
         * 3 4 5 
         */
        [TestMethod]
        public void SetValues23()
        {
            CheckIncreasingValues(nbRows: 2, nbColumns: 3);
        }

        // --- AddRow() ---

        // Vérifie le comportement de AddRow() avec un indice invalide. Attendu :
        // - levée d'exception
        // - nombre de lignes inchangé
        [TestMethod]
        public void AddRowInvalidIndex()
        {
            Matrix m = new Matrix(nbRows: 2, nbColumns: 3);

            AddRowInvalidIndex(m, -1);  // inférieur au premier indice valide
            AddRowInvalidIndex(m, 3);   // supérieur au dernier indice valide
        }


        /* Vérifie le comportement de AddRow() pour l'ajout d'une ligne au début d'une matrice 2 x 3
         * Ancienne matrice :
         * 0 1 2 
         * 3 4 5 
         * Nouvelle matrice :
         * -1 -1 -1
         * 0 1 2 
         * 3 4 5 
         */
        [TestMethod]
        public void AddRowBeginning()
        {
            // Crée une matrice 2 x 3 et la remplit avec les valeurs 0, 1, 2, ..., 2 x 3 - 1 = 5
            // Puis insère une ligne à l'indice newRowIndex = 0 (début de matrice)
            int newRowIndex = 0;
            Matrix m = CreateMatrixAndAddRow(nbRows: 2, nbColumns: 3, newRowIndex: newRowIndex);

            // Vérifie les valeurs de la matrice après insertion de la ligne

            // Nouvelle ligne: remplie avec la valeur par défaut
            Assert.AreEqual(m.DefaultValue, m.GetValue(newRowIndex, 0), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[{newRowIndex}][0] = {m.DefaultValue}");
            Assert.AreEqual(m.DefaultValue, m.GetValue(newRowIndex, 1), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[{newRowIndex}][1] = {m.DefaultValue}");
            Assert.AreEqual(m.DefaultValue, m.GetValue(newRowIndex, 2), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[{newRowIndex}][2] = {m.DefaultValue}");

            // Anciennes valeurs
            Assert.AreEqual(0, m.GetValue(1, 0), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[1][0] = 0");
            Assert.AreEqual(1, m.GetValue(1, 1), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[1][1] = 1");
            Assert.AreEqual(2, m.GetValue(1, 2), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[1][2] = 2");
            Assert.AreEqual(3, m.GetValue(2, 0), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[2][0] = 3");
            Assert.AreEqual(4, m.GetValue(2, 1), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[2][1] = 4");
            Assert.AreEqual(5, m.GetValue(2, 2), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[2][2] = 5");
        }


        /* Vérifie le comportement de AddRow() pour l'ajout d'une ligne au milieu d'une matrice 2 x 3
         * Ancienne matrice :
         * 0 1 2 
         * 3 4 5 
         * Nouvelle matrice :
         * 0 1 2 
         * -1 -1 -1
         * 3 4 5 
         */
        [TestMethod]
        public void AddRowMiddle()
        {
            // Crée une matrice 2 x 3 et la remplit avec les valeurs 0, 1, 2, ..., 2 x 3 - 1 = 5
            // Puis insère une ligne à l'indice newRowIndex = 1 (milieu de matrice)
            int newRowIndex = 1;
            Matrix m = CreateMatrixAndAddRow(nbRows: 2, nbColumns: 3, newRowIndex: newRowIndex);

            // Vérifie les valeurs de la matrice après insertion de la ligne

            // Nouvelle ligne: remplie avec la valeur par défaut
            Assert.AreEqual(m.DefaultValue, m.GetValue(newRowIndex, 0), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[{newRowIndex}][0] = {m.DefaultValue}");
            Assert.AreEqual(m.DefaultValue, m.GetValue(newRowIndex, 1), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[{newRowIndex}][1] = {m.DefaultValue}");
            Assert.AreEqual(m.DefaultValue, m.GetValue(newRowIndex, 2), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[{newRowIndex}][2] = {m.DefaultValue}");

            // Anciennes valeurs
            Assert.AreEqual(0, m.GetValue(0, 0), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[0][0] = 0");
            Assert.AreEqual(1, m.GetValue(0, 1), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[0][1] = 1");
            Assert.AreEqual(2, m.GetValue(0, 2), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[0][2] = 2");
            Assert.AreEqual(3, m.GetValue(2, 0), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[2][0] = 3");
            Assert.AreEqual(4, m.GetValue(2, 1), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[2][1] = 4");
            Assert.AreEqual(5, m.GetValue(2, 2), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[2][2] = 5");
        }


        /* Vérifie le comportement de AddRow() pour l'ajout d'une ligne à la fin d'une matrice 2 x 3
         * Ancienne matrice :
         * 0 1 2 
         * 3 4 5 
         * Nouvelle matrice :
         * 0 1 2 
         * 3 4 5 
         * -1 -1 -1
         */
        [TestMethod]
        public void AddRowEnd()
        {
            // Crée une matrice 2 x 3 et la remplit avec les valeurs 0, 1, 2, ..., 2 x 3 - 1 = 5
            // Puis insère une ligne à l'indice newRowIndex = 2 (fin de matrice)
            int newRowIndex = 2;
            Matrix m = CreateMatrixAndAddRow(nbRows: 2, nbColumns: 3, newRowIndex: newRowIndex);

            // Vérifie les valeurs de la matrice après insertion de la ligne

            // Nouvelle ligne: remplie avec la valeur par défaut
            Assert.AreEqual(m.DefaultValue, m.GetValue(newRowIndex, 0), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[{newRowIndex}][0] = {m.DefaultValue}");
            Assert.AreEqual(m.DefaultValue, m.GetValue(newRowIndex, 1), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[{newRowIndex}][1] = {m.DefaultValue}");
            Assert.AreEqual(m.DefaultValue, m.GetValue(newRowIndex, 2), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[{newRowIndex}][2] = {m.DefaultValue}");

            // Anciennes valeurs
            Assert.AreEqual(0, m.GetValue(0, 0), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[0][0] = 0");
            Assert.AreEqual(1, m.GetValue(0, 1), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[0][1] = 1");
            Assert.AreEqual(2, m.GetValue(0, 2), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[0][2] = 2");
            Assert.AreEqual(3, m.GetValue(1, 0), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[1][0] = 3");
            Assert.AreEqual(4, m.GetValue(1, 1), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[1][1] = 4");
            Assert.AreEqual(5, m.GetValue(1, 2), $"Après ajout d'une ligne à l'indice {newRowIndex}, M[1][2] = 5");
        }


        // --- RemoveRow() ---

        // Vérifie le comportement de RemoveRow() avec un indice invalide. Attendu :
        // - levée d'exception
        // - nombre de lignes inchangé
        [TestMethod]
        public void RemoveRowInvalidIndex()
        {
            Matrix m = new Matrix(nbRows: 2, nbColumns: 3);

            RemoveRowInvalidIndex(m, -1);   // inférieur au premier indice valide
            RemoveRowInvalidIndex(m, 3);    // supérieur au dernier indice valide    
        }


        /* Vérifie le comportement de RemoveRow() pour l'ajout d'une colonne au début d'une matrice 3 x 4
          * Ancienne matrice :
          * 0 1 2 3
          * 4 5 6 7
          * 8 9 10 11
          * Nouvelle matrice :
          * 0 1 2 3
          * 8 9 10 11 
          */
        [TestMethod]
        public void RemoveRow()
        {
            // Crée une matrice 3 x 4 et la remplit avec les valeurs 0, 1, 2, ..., 3 x 4 - 1 = 11
            // Puis supprime la ligne à l'indice removeRowIndex
            int removeRowIndex = 1;
            Matrix m = CreateMatrixAndRemoveRow(nbRows: 3, nbColumns: 4, removeRowIndex: removeRowIndex);

            // Vérifie les valeurs de la matrice après suppression de la ligne
            Assert.AreEqual(0, m.GetValue(0, 0), $"Après suppression de la ligne à l'indice {removeRowIndex}, M[0][0] = 0");
            Assert.AreEqual(1, m.GetValue(0, 1), $"Après suppression de la ligne à l'indice {removeRowIndex}, M[0][1] = 1");
            Assert.AreEqual(2, m.GetValue(0, 2), $"Après suppression de la ligne à l'indice {removeRowIndex}, M[0][2] = 2");
            Assert.AreEqual(3, m.GetValue(0, 3), $"Après suppression de la ligne à l'indice {removeRowIndex}, M[0][3] = 3");
            Assert.AreEqual(8, m.GetValue(1, 0), $"Après suppression de la ligne à l'indice {removeRowIndex}, M[1][0] = 8");
            Assert.AreEqual(9, m.GetValue(1, 1), $"Après suppression de la ligne à l'indice {removeRowIndex}, M[1][1] = 9");
            Assert.AreEqual(10, m.GetValue(1, 2), $"Après suppression de la ligne à l'indice {removeRowIndex}, M[1][2] = 10");
            Assert.AreEqual(11, m.GetValue(1, 3), $"Après suppression de la ligne à l'indice {removeRowIndex}, M[1][3] = 11");
        }


        // --- AddColumn() ---

        // Vérifie le comportement de AddColumn() avec un indice invalide. Attendu :
        // - levée d'exception
        // - nombre de colonnes inchangé
        [TestMethod]
        public void AddColumnInvalidIndex()
        {
            Matrix m = new Matrix(nbRows: 2, nbColumns: 3);

            AddColumnInvalidIndex(m, -1);   // inférieur au premier indice valide
            AddColumnInvalidIndex(m, 4);    // supérieur au dernier indice valide    
        }

        /* Vérifie le comportement de AddColum() pour l'ajout d'une colonne au début d'une matrice 2 x 3
         * Ancienne matrice :
         * 0 1 2 
         * 3 4 5 
         * Nouvelle matrice :
         * -1 0 1 2 
         * -1 3 4 5  
         */
        [TestMethod]
        public void AddColumnBeginning()
        {

            // Crée une matrice 2 x 3 et la remplit avec les valeurs 0, 1, 2, ..., 2 x 3 - 1 = 5
            // Puis insère une colonne à l'indice newColumnIndex = 0 (début de matrice)
            int newColumnIndex = 0;
            Matrix m = CreateMatrixAndAddColumn(nbRows: 2, nbColumns: 3, newColumnIndex: newColumnIndex);

            // Vérifie les valeurs de la matrice après insertion de la colonne

            // Nouvelle colonne: remplie avec la valeur par défaut
            Assert.AreEqual(m.DefaultValue, m.GetValue(0, newColumnIndex), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][{newColumnIndex}] = {m.DefaultValue}");
            Assert.AreEqual(m.DefaultValue, m.GetValue(1, newColumnIndex), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][{newColumnIndex}] = {m.DefaultValue}");

            // Anciennes valeurs
            Assert.AreEqual(0, m.GetValue(0, 1), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][1] = 0");
            Assert.AreEqual(1, m.GetValue(0, 2), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][2] = 1");
            Assert.AreEqual(2, m.GetValue(0, 3), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][3] = 2");
            Assert.AreEqual(3, m.GetValue(1, 1), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][1] = 3");
            Assert.AreEqual(4, m.GetValue(1, 2), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][2] = 4");
            Assert.AreEqual(5, m.GetValue(1, 3), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][3] = 5");
        }

        /* Vérifie le comportement de AddColum() pour l'ajout d'une colonne au milieu d'une matrice 2 x 3
         * Ancienne matrice :
         * 0 1 2 
         * 3 4 5 
         * Nouvelle matrice :
         * 0 1 -1 2 
         * 3 4 -1 5 
         */
        [TestMethod]
        public void AddColumnMiddle()
        {
            // Crée une matrice 2 x 3 et la remplit avec les valeurs 0, 1, 2, ..., 2 x 3 - 1 = 5
            // Puis insère une colonne à l'indice newColumnIndex = 2 (milieu de matrice)
            int newColumnIndex = 2;
            Matrix m = CreateMatrixAndAddColumn(nbRows: 2, nbColumns: 3, newColumnIndex: newColumnIndex);

            // Vérifie les valeurs de la matrice après insertion de la colonne

            // Nouvelle colonne: remplie avec la valeur par défaut
            Assert.AreEqual(m.DefaultValue, m.GetValue(0, newColumnIndex), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][{newColumnIndex}] = {m.DefaultValue}");
            Assert.AreEqual(m.DefaultValue, m.GetValue(1, newColumnIndex), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][{newColumnIndex}] = {m.DefaultValue}");

            // Anciennes valeurs
            Assert.AreEqual(0, m.GetValue(0, 0), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][0] = 0");
            Assert.AreEqual(1, m.GetValue(0, 1), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][1] = 1");
            Assert.AreEqual(2, m.GetValue(0, 3), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][3] = 2");
            Assert.AreEqual(3, m.GetValue(1, 0), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][0] = 3");
            Assert.AreEqual(4, m.GetValue(1, 1), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][1] = 4");
            Assert.AreEqual(5, m.GetValue(1, 3), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][3] = 5");
        }

        /* Vérifie le comportement de AddColum() pour l'ajout d'une colonne à la fin d'une matrice 2 x 3
         * Ancienne matrice :
         * 0 1 2 
         * 3 4 5 
         * Nouvelle matrice :
         * 0 1 2 -1 
         * 3 4 5 -1
         */
        [TestMethod]
        public void AddColumnEnd()
        {
            // Crée une matrice 2 x 3 et la remplit avec les valeurs 0, 1, 2, ..., 2 x 3 - 1 = 5
            // Puis insère une colonne à l'indice newColumnIndex = 3 (fin de matrice)
            int newColumnIndex = 3;
            Matrix m = CreateMatrixAndAddColumn(nbRows: 2, nbColumns: 3, newColumnIndex: newColumnIndex);

            // Vérifie les valeurs de la matrice après insertion de la colonne

            // Nouvelle colonne: remplie avec la valeur par défaut
            Assert.AreEqual(m.DefaultValue, m.GetValue(0, newColumnIndex), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][{newColumnIndex}] = {m.DefaultValue}");
            Assert.AreEqual(m.DefaultValue, m.GetValue(1, newColumnIndex), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][{newColumnIndex}] = {m.DefaultValue}");

            // Anciennes valeurs
            Assert.AreEqual(0, m.GetValue(0, 0), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][0] = 0");
            Assert.AreEqual(1, m.GetValue(0, 1), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][1] = 1");
            Assert.AreEqual(2, m.GetValue(0, 2), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[0][2] = 2");
            Assert.AreEqual(3, m.GetValue(1, 0), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][0] = 3");
            Assert.AreEqual(4, m.GetValue(1, 1), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][1] = 4");
            Assert.AreEqual(5, m.GetValue(1, 2), $"Après ajout d'une colonne à l'indice {newColumnIndex}, M[1][2] = 5");
        }

        // Vérifie le comportement de RemoveColumn() avec un indice invalide. Attendu :
        // - levée d'exception
        // - nombre de colonnes inchangé
        [TestMethod]
        public void RemoveColumnInvalidIndex()
        {
            Matrix m = new Matrix(nbRows: 2, nbColumns: 3);

            RemoveColumnInvalidIndex(m, -1);   // inférieur au premier indice valide
            RemoveColumnInvalidIndex(m, 3);    // supérieur au dernier indice valide    
        }

        /* Vérifie le comportement de RemoveColumn() pour l'ajout d'une colonne au début d'une matrice 2 x 4
          * Ancienne matrice :
          * 0 1 2 3
          * 4 5 6 7
          * Nouvelle matrice :
          * 0 2 3
          * 4 6 7
          */
        [TestMethod]
        public void RemoveColumn()
        {
            // Crée une matrice 2 x 4 et la remplit avec les valeurs 0, 1, 2, ..., 2 x 4 - 1 = 7
            // Puis supprime la colonne à l'indice removeColumnIndex
            int removeColumnIndex = 1;
            Matrix m = CreateMatrixAndRemoveColumn(nbRows: 2, nbColumns: 4, removeColumnIndex: removeColumnIndex);

            // Vérifie les valeurs de la matrice après suppression de la ligne
            Assert.AreEqual(0, m.GetValue(0, 0), $"Après suppression de la colonne à l'indice {removeColumnIndex}, M[0][0] = 0");
            Assert.AreEqual(2, m.GetValue(0, 1), $"Après suppression de la colonne à l'indice {removeColumnIndex}, M[0][1] = 2");
            Assert.AreEqual(3, m.GetValue(0, 2), $"Après suppression de la colonne à l'indice {removeColumnIndex}, M[0][2] = 3");
            Assert.AreEqual(4, m.GetValue(1, 0), $"Après suppression de la colonne à l'indice {removeColumnIndex}, M[1][0] = 4");
            Assert.AreEqual(6, m.GetValue(1, 1), $"Après suppression de la colonne à l'indice {removeColumnIndex}, M[1][1] = 6");
            Assert.AreEqual(7, m.GetValue(1, 2), $"Après suppression de la colonne à l'indice {removeColumnIndex}, M[1][2] = 7");
        }



        // ---------------------- Méthodes utilitaires permettant de factoriser le code dans les tests ---------------------


        // --- Vérification des valeurs ---

        // Vérifie que la portion de la matric `m` entre les cases (`startRow`, `startColumn`) et (`endRow`, `endColumn`) contient `constantValue`
        private void CheckConstantValue(Matrix m, int startRow, int endRow, int startColumn, int endColumn, float constantValue)
        {
            for (int i = startRow; i < endRow; i++)
            {
                for (int j = startColumn; j < endColumn; j++)
                {
                    float value = m.GetValue(i, j);
                    Assert.AreEqual(value, constantValue, $"La valeur aux indices ({i},{j}) doit être {constantValue}");

                }
            }
        }

        // Verifie le fonctionnement de SetValue() sur toutes les cases de la matrice, avec des valeurs croissantes
        // On crée une matrice `nbRows` * `nbColumns` et affecte les valeurs 0, 1, 2, ..., `nbRows` * `nbColumns` - 1 à ses cases
        // puis on vérifie qu'on retrouve bien ces valeurs avec GetValue()
        private void CheckIncreasingValues(int nbRows, int nbColumns)
        {
            // Crée une matrice nbRows x nbColumns et la remplit avec les valeurs 0, 1, 2, ..., nbRows * nbColumns - 1 
            Matrix m = CreateAndFillMatrix(nbRows: nbRows, nbColumns: nbColumns);

            // On vérifie que les dimensions de la matrice n'ont pas été modifiées par les appels à SetValue()
            Assert.AreEqual(nbRows, m.NbRows, $"La matrice doit contenir {nbRows} lignes");
            Assert.AreEqual(nbColumns, m.NbColumns, $"La matrice doit contenir {nbColumns} colonnes");

            // On vérifie que la valeur de chaque case a bien été modifiée par SetValue()
            for (int i = 0; i < nbRows; i++)
            {
                for (int j = 0; j < nbColumns; j++)
                {
                    float expectedValue = i * nbColumns + j;
                    Assert.AreEqual(expectedValue, m.GetValue(i, j), $"La valeur aux indices ({i},{j}) doit être {expectedValue}");
                }
            }

            m.Print();
        }

        // --- Gestion des lignes ---

        // Crée une matrice de `nbRows` x `nbColumns`, avec les valeurs 0, 1, 2, ..., `nbRows` * `nbColumns` - 1 
        // Puis ajoute une ligne à l'indice `newRowIndex`, qui sera remplie avec `defaultValue`
        private Matrix CreateMatrixAndAddRow(int nbRows, int nbColumns, int newRowIndex, float defaultValue = -1)
        {
            Matrix m = CreateAndFillMatrix(nbRows: nbRows, nbColumns: nbColumns, defaultValue: defaultValue);

            Console.WriteLine($"Etat de la matrice avant insertion d'une ligne à l'indice {newRowIndex}:");
            m.Print();

            // Ajoute une ligne à l'indice newRowIndex et vérifie les dimensions
            AddRowAtIndex(m: m, newRowIndex: newRowIndex);

            Console.WriteLine($"Etat de la matrice après insertion d'une ligne à l'indice {newRowIndex}:");
            m.Print();

            return m;
        }

        // Ajoute une ligne à l'indice `newRowIndex` (valide) dans la matrice `m`, et vérifie l'évolution des dimensions
        private void AddRowAtIndex(Matrix m, int newRowIndex)
        {
            int nbRows = m.NbRows;
            int nbColumns = m.NbColumns;

            m.AddRow(newRowIndex);

            // Vérifie l'évolution des dimensions
            Assert.AreEqual(nbColumns, m.NbColumns, "Après ajout d'une ligne, le nombre de colonnes n'a pas changé");
            Assert.AreEqual(nbRows + 1, m.NbRows, $"Après ajout d'une ligne, le nombre de lignes est passé de {nbRows} à {nbRows + 1}");
        }

        // Tente d'ajouter une ligne à l'indice `invalidIndex` (invalide), et vérifie que :
        // - une exception est levée
        // - le nombre de lignes est inchangé
        private void AddRowInvalidIndex(Matrix m, int invalidIndex)
        {
            int nbRows = m.NbRows;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.AddRow(invalidIndex);
            },
                $"Tenter d'ajouter une ligne à l'indice {invalidIndex} doit lever une ArgumentOutOfRangeException"
            );

            Assert.AreEqual(nbRows, m.NbRows, "Le nombre de lignes n'a pas changé");
        }

        // Crée une matrice de `nbRows` x `nbColumns`, avec les valeurs 0, 1, 2, ..., `nbRows` * `nbColumns` - 1 
        // Puis supprime la ligne à l'indice `removeRowIndex`
        private Matrix CreateMatrixAndRemoveRow(int nbRows, int nbColumns, int removeRowIndex)
        {
            Matrix m = CreateAndFillMatrix(nbRows: nbRows, nbColumns: nbColumns);

            Console.WriteLine($"Etat de la matrice avant suppression de la ligne à l'indice {removeRowIndex}:");
            m.Print();

            // Supprime la ligne à l'indice removeRowIndex et vérifie les dimensions
            RemoveRowAtIndex(m: m, removeRowIndex: removeRowIndex);

            Console.WriteLine($"Etat de la matrice après suppression de la ligne à l'indice {removeRowIndex}:");
            m.Print();

            return m;
        }

        // Supprime la ligne à l'indice `removeRowIndex` (valide) dans la matrice `m`, et vérifie l'évolution des dimensions
        private void RemoveRowAtIndex(Matrix m, int removeRowIndex)
        {
            int nbRows = m.NbRows;
            int nbColumns = m.NbColumns;

            m.RemoveRow(removeRowIndex);

            // Vérifie l'évolution des dimensions
            Assert.AreEqual(nbColumns, m.NbColumns, "Après suppression d'une ligne, le nombre de colonnes n'a pas changé");
            Assert.AreEqual(nbRows - 1, m.NbRows, $"Après suppression d'une ligne, le nombre de lignes est passé de {nbRows} à {nbRows - 1}");
        }

        // Tente de supprimer une ligne à l'indice `invalidIndex` (invalide), et vérifie que :
        // - une exception est levée
        // - le nombre de lignes est inchangé
        private void RemoveRowInvalidIndex(Matrix m, int invalidIndex)
        {
            int nbRows = m.NbRows;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.RemoveRow(invalidIndex);
            },
                $"Tenter de supprimer une ligne à l'indice {invalidIndex} doit lever une ArgumentOutOfRangeException"
            );

            Assert.AreEqual(nbRows, m.NbRows, "Le nombre de lignes n'a pas changé");
        }


        // --- Gestion des colonnes ---

        // Crée une matrice de `nbRows` x `nbColumns`, avec les valeurs 0, 1, 2, ..., `nbRows` * `nbColumns` - 1 
        // Puis ajoute une colonne à l'indice `newColumnIndex`, qui sera remplie avec `defaultValue`
        private Matrix CreateMatrixAndAddColumn(int nbRows, int nbColumns, int newColumnIndex, float defaultValue = -1)
        {
            Matrix m = CreateAndFillMatrix(nbRows: nbRows, nbColumns: nbColumns, defaultValue: defaultValue);

            Console.WriteLine($"Etat de la matrice avant insertion d'une colonne à l'indice {newColumnIndex}:");
            m.Print();

            // Ajoute une colonne à l'indice newColumnIndex et vérifie les dimensions
            AddColumAtIndex(m: m, newColumnIndex: newColumnIndex);

            Console.WriteLine($"Etat de la matrice après insertion d'une colonne à l'indice {newColumnIndex}:");
            m.Print();

            return m;
        }

        // Ajoute une colonne à l'indice `newColumnIndex` (valide) dans la matrice `m`, et vérifie l'évolution des dimensions
        private void AddColumAtIndex(Matrix m, int newColumnIndex)
        {
            int nbRows = m.NbRows;
            int nbColumns = m.NbColumns;

            m.AddColumn(newColumnIndex);

            // Vérifie l'évolution des dimensions
            Assert.AreEqual(nbRows, m.NbRows, "Après ajout d'une colonne, le nombre de lignes n'a pas changé");
            Assert.AreEqual(nbColumns + 1, m.NbColumns, $"Après ajout d'une colonne, le nombre de colonnes est passé de {nbColumns} à {nbColumns + 1}");
        }

        // Tente d'ajouter une colonne à l'indice `invalidIndex` (invalide), et vérifie que :
        // - une exception est levée
        // - le nombre de colonnes est inchangé
        private void AddColumnInvalidIndex(Matrix m, int invalidIndex)
        {
            int nbColumns = m.NbColumns;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.AddColumn(invalidIndex);
            },
                $"Tenter d'ajouter une colonne à l'indice {invalidIndex} doit lever une ArgumentOutOfRangeException"
            );

            Assert.AreEqual(nbColumns, m.NbColumns, "Le nombre de colonnes n'a pas changé");
        }

        // Crée une matrice de `nbRows` x `nbColumns`, avec les valeurs 0, 1, 2, ..., `nbRows` * `nbColumns` - 1 
        // Puis supprime la colonne à l'indice `removeColumnIndex`
        private Matrix CreateMatrixAndRemoveColumn(int nbRows, int nbColumns, int removeColumnIndex)
        {
            Matrix m = CreateAndFillMatrix(nbRows: nbRows, nbColumns: nbColumns);

            Console.WriteLine($"Etat de la matrice avant suppression de la colonne à l'indice {removeColumnIndex}:");
            m.Print();

            // Supprime la colonne à l'indice removeColumnIndex et vérifie les dimensions
            RemoveColumnAtIndex(m: m, removeColumnIndex: removeColumnIndex);

            Console.WriteLine($"Etat de la matrice après suppression de la colonne à l'indice {removeColumnIndex}:");
            m.Print();

            return m;
        }

        // Supprime la colonne à l'indice `removeColumnIndex` (valide) dans la matrice `m`, et vérifie l'évolution des dimensions
        private void RemoveColumnAtIndex(Matrix m, int removeColumnIndex)
        {
            int nbRows = m.NbRows;
            int nbColumns = m.NbColumns;

            m.RemoveColumn(removeColumnIndex);

            // Vérifie l'évolution des dimensions
            Assert.AreEqual(nbRows, m.NbRows, "Après suppression d'une colonne, le nombre de lignes n'a pas changé");
            Assert.AreEqual(nbColumns - 1, m.NbColumns, $"Après suppression d'une colonne, le nombre de colonnes est passé de {nbColumns} à {nbColumns - 1}");
        }

        // Tente de supprimer une colonne à l'indice `invalidIndex` (invalide), et vérifie que :
        // - une exception est levée
        // - le nombre de colonnes est inchangé
        private void RemoveColumnInvalidIndex(Matrix m, int invalidIndex)
        {
            int nbColumns = m.NbColumns;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                m.RemoveColumn(invalidIndex);
            },
                $"Tenter de supprimer une colonne à l'indice {invalidIndex} doit lever une ArgumentOutOfRangeException"
            );

            Assert.AreEqual(nbColumns, m.NbColumns, "Le nombre de colonnes n'a pas changé");
        }


        // --- Divers ---

        // Crée une matrice `nbRows` x `nbColumns` et la remplit avec les valeurs 0, 1, 2, ..., `nbRows` * `nbColumns` - 1 
        private Matrix CreateAndFillMatrix(int nbRows, int nbColumns, float defaultValue = 0)
        {
            Matrix m = new Matrix(nbRows: nbRows, nbColumns: nbColumns, defaultValue: defaultValue);

            for (int i = 0; i < m.NbRows; i++)
            {
                for (int j = 0; j < m.NbColumns; j++)
                {
                    m.SetValue(i, j, i * m.NbColumns + j);
                }
            }

            return m;
        }


        // Vérifie les dimensions d'une matrice après sa création
        private void CheckDimensions(int nbRows, int nbColumns)
        {
            Matrix m = new Matrix(nbRows: nbRows, nbColumns: nbColumns);

            Assert.AreEqual(nbRows, m.NbRows, $"La matrice doit contenir {nbRows} lignes");
            Assert.AreEqual(nbColumns, m.NbColumns, $"La matrice doit contenir {nbColumns} colonnes");
        }

    }
}
