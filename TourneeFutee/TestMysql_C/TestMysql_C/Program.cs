using MySql.Data.MySqlClient;
using System.Data;

namespace TestMysql_C
{
    class Program
    {
        static void Main(string[] args)
        {
        #region Connexion
            MySqlConnection maConnexion = null;
            try
            {
                string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=film;" +
                                         "UID=root;PASSWORD=mysqlroot123&";

                maConnexion = new MySqlConnection(connexionString);
                maConnexion.Open();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
                return;
            }
        #endregion

        #region Création de table
            string createTable = " CREATE TABLE Professeur (nom VARCHAR(25));";
            MySqlCommand command2 = maConnexion.CreateCommand();
            command2.CommandText = createTable;
            try
            {
                command2.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
                Console.ReadLine();
                return;
            }
            command2.Dispose();
        #endregion

        #region Insertion
            // CORRECTION : utilisation de command3 au lieu de command2 (déjà disposé)
            string insertTable = " INSERT INTO Professeur VALUES ('toto');";
            MySqlCommand command3 = maConnexion.CreateCommand();
            command3.CommandText = insertTable;
            try
            {
                command3.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
                Console.ReadLine();
                return;
            }
            command3.Dispose();
        #endregion

        #region Selection
            string requete = " SELECT * FROM personne;";

            // CORRECTIONS :
            // - "WHERE AND" remplacé par "WHERE"
            // - 'Acteur' entre guillemets simples SQL
            // - suppression de la jointure inutile sur cote c
            string requete1 = "SELECT p.nom, p.prenom FROM personne p, role r, participation pp, film f " +
                "WHERE r.libelle = 'Acteur' AND p.idPersonne = pp.idPersonne AND pp.idFilm = f.idFilm AND pp.idRole = r.idRole;";

            MySqlCommand command1 = maConnexion.CreateCommand();
            command1.CommandText = requete;

            MySqlDataReader reader = command1.ExecuteReader();

            string[] valueString = new string[reader.FieldCount];
            while (reader.Read())
            {
                string last_name = (string)reader["nom"];
                string first_name = reader["prenom"] == DBNull.Value ? "" : (string)reader["prenom"];
                Console.WriteLine(first_name + " " + last_name);

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                    Console.Write(valueString[i] + " , ");
                }
                Console.WriteLine();
            }
            reader.Close();
            command1.Dispose();
        #endregion

        #region Selection avec variable
            MySqlParameter nom = new MySqlParameter("@nom", MySqlDbType.VarChar);
            nom.Value = "Blier";

            string requete4 = "SELECT * FROM personne WHERE nom = @nom;";
            MySqlCommand command4 = maConnexion.CreateCommand();
            command4.CommandText = requete4;
            command4.Parameters.Add(nom);
            MySqlDataReader reader1 = command4.ExecuteReader();

            valueString = new string[reader1.FieldCount];
            while (reader1.Read())
            {
                for (int i = 0; i < reader1.FieldCount; i++)
                {
                    valueString[i] = reader1.GetValue(i).ToString();
                    Console.Write(valueString[i] + " , ");
                }
                Console.WriteLine();
            }
            // CORRECTION : reader1.Close() au lieu de reader.Close()
            reader1.Close();
            command4.Dispose();
        #endregion

            maConnexion.Close();
            Console.ReadLine();
        }
    }
}
