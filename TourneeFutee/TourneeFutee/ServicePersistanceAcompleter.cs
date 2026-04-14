using System.Data;
using MySql.Data.MySqlClient;

namespace TourneeFutee
{
    /// <summary>
    /// Service de persistance permettant de sauvegarder et charger
    /// des graphes et des tournées dans une base de données MySQL.
    /// </summary>
    public class ServicePersistance
    {
        // ─────────────────────────────────────────────────────────────────────
        // Attributs privés
        // ─────────────────────────────────────────────────────────────────────

        private readonly string _connectionString;
        // TODO : si vous avez besoin de maintenir une connexion ouverte,
        //        ajoutez un attribut MySqlConnection ici.
        private readonly MySqlConnection _connection;

        // ─────────────────────────────────────────────────────────────────────
        // Constructeur
        // ─────────────────────────────────────────────────────────────────────
        #region Consignes
        /// <summary>
        /// Instancie un service de persistance et se connecte automatiquement
        /// à la base de données <paramref name="dbname"/> sur le serveur
        /// à l'adresse IP <paramref name="serverIp"/>.
        /// Les identifiants sont définis par <paramref name="user"/> (utilisateur)
        /// et <paramref name="pwd"/> (mot de passe).
        /// </summary>
        /// <param name="serverIp">Adresse IP du serveur MySQL.</param>
        /// <param name="dbname">Nom de la base de données.</param>
        /// <param name="user">Nom d'utilisateur.</param>
        /// <param name="pwd">Mot de passe.</param>
        /// <exception cref="Exception">Levée si la connexion échoue.</exception>
        #endregion
        public ServicePersistance(string serverIp, string dbname, string user, string pwd)
        {
            this._connectionString = $"SERVER={serverIp};" +
                                         $"DATABASE={dbname};" +
                                         $"UID={user};PASSWORD={pwd};";
            try //test si la connexion fonctionne
            {
                _connection = new MySqlConnection(this._connectionString);
                _connection.Open();
                if (_connection.State != ConnectionState.Open)
                {
                    throw new Exception("Connexion non ouverte");
                }
            }
            catch (Exception e) 
            {
                throw new Exception("Erreur de connexion à la base", e);
            }     
        }
        public void CloseConnection() //méthode pour fermer la connexion
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Méthodes publiques
        // ─────────────────────────────────────────────────────────────────────
        #region Consignes
        /// <summary>
        /// Sauvegarde le graphe <paramref name="g"/> en base de données
        /// (sommets et arcs inclus) et renvoie son identifiant.
        /// </summary>
        /// <param name="g">Le graphe à sauvegarder.</param>
        /// <returns>Identifiant du graphe en base de données (AUTO_INCREMENT).</returns>
        // TODO : implémenter la sauvegarde du graphe
        //
        // Ordre recommandé :
        //   1. INSERT dans la table Graphe -> récupérer l'id avec LAST_INSERT_ID()
        //   2. Pour chaque sommet de g : INSERT dans Sommet (valeur + graphe_id)
        //      -> conserver la correspondance sommet C# <-> id BdD
        //   3. Pour chaque arc de la matrice d'adjacence (poids != +inf) :
        //      INSERT dans Arc (sommet_source_id, sommet_dest_id, poids, graphe_id)
        //
        // Exemple pour récupérer l'id généré :
        //   uint id = Convert.ToUInt32(cmd.ExecuteScalar());
        #endregion
        public uint SaveGraph(Graph g)
        {
            //Insertion du graph
            uint graphId;
            using (MySqlConnection connexion = new MySqlConnection(_connectionString)) //gère la connexion
            {
                connexion.Open();
                string query = "INSERT INTO Graphe (est_oriente) VALUES (@oriente);";
                using (MySqlCommand cmdGraph = new MySqlCommand(query, connexion)) //gère la commande SQL pour l'insertion du graph
                {
                    cmdGraph.Parameters.AddWithValue("@oriente", g.Directed); //remplace le praramètre SQL par la valeur de Directed
                    cmdGraph.ExecuteNonQuery(); 
                }
                using (MySqlCommand cmdId = new MySqlCommand("SELECT LAST_INSERT_ID();", connexion)) //récupère l'id du graph qu'on vient d'enregistrer
                {
                    graphId = Convert.ToUInt32(cmdId.ExecuteScalar());
                }
                // Insertion des Sommets
                Dictionary<int, uint> SommetId=new Dictionary<int, uint>(); //On utilise un dictionnaire pour garder la correspondance sommet-id
                int i = 0;
                foreach (var sommet in g.Sommets)
                {
                    string querySommet = @"     
                                INSERT INTO Sommet (graphe_id, nom, valeur)
                                VALUES (@graphe_id, @nom, @valeur);";
                    using (MySqlCommand cmdSommet = new MySqlCommand(querySommet, connexion)) //gère la commande SQL pour l'insertion des sommets
                    {
                        cmdSommet.Parameters.AddWithValue("@graphe_id", graphId);
                        cmdSommet.Parameters.AddWithValue("@nom", sommet.Nom);
                        cmdSommet.Parameters.AddWithValue("@valeur", sommet.Valeur);
                        cmdSommet.ExecuteNonQuery();
                    }
                    using (MySqlCommand cmdID = new MySqlCommand("SELECT LAST_INSERT_ID();", connexion)) //récupère l'id du sommet
                    {
                        uint sommetId = Convert.ToUInt32(cmdID.ExecuteScalar());
                        SommetId[i] = sommetId;
                    }
                }
                //Insertion des arcs
                if (g.Directed == true) //Si le graph est orienté on parcours toute la matrice
                {
                    for (i = 0; i < g.Sommets.Count; i++)
                    {
                        for (int j = 0; j < g.Sommets.Count; j++)
                        {
                            float poids = g.Matrice.Matrice[i][j];
                            if (poids != g.Matrice.DefaultValue)
                            {
                                using (MySqlCommand cmdArc = new MySqlCommand(
                                    @"INSERT INTO Arc (graphe_id, sommet_source, sommet_dest, poids)
                                  VALUES (@gid, @src, @dst, @poids);", connexion))
                                {
                                    cmdArc.Parameters.AddWithValue("@gid", graphId);
                                    cmdArc.Parameters.AddWithValue("@src", SommetId[i]);
                                    cmdArc.Parameters.AddWithValue("@dst", SommetId[j]);
                                    cmdArc.Parameters.AddWithValue("@poids", poids);
                                    cmdArc.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                else // Si le graph est non orienté, on ne parcours que la moitié superieur pour ne pas faire de doublons 
                {
                    for (i = 0; i < g.Sommets.Count; i++)
                    {
                        for (int j = i + 1; j < g.Sommets.Count; j++)
                        {
                            float poids = g.Matrice.Matrice[i][j];
                            if (poids != g.Matrice.DefaultValue)
                            {
                                using (MySqlCommand cmd = new MySqlCommand(
                                    @"INSERT INTO Arc (graphe_id, sommet_source, sommet_dest, poids)
                                  VALUES (@gid, @src, @dst, @poids);", connexion))
                                {
                                    cmd.Parameters.AddWithValue("@gid", graphId);
                                    cmd.Parameters.AddWithValue("@src", SommetId[i]);
                                    cmd.Parameters.AddWithValue("@dst", SommetId[j]);
                                    cmd.Parameters.AddWithValue("@poids", poids);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            return graphId;
        }

        /// <summary>
        /// Charge depuis la base de données le graphe identifié par <paramref name="id"/>
        /// et renvoie une instance de la classe <see cref="Graph"/>.
        /// </summary>
        /// <param name="id">Identifiant du graphe à charger.</param>
        /// <returns>Instance de <see cref="Graph"/> reconstituée.</returns>
        public Graph LoadGraph(uint id)
        {
            // TODO : implémenter le chargement du graphe
            using (MySqlConnection connexion = new MySqlConnection(_connectionString)) //gère la connexion
            {
                connexion.Open();
                string requete = "SELECT * FROM Graphe WHERE id = @id;";
                MySqlCommand command = connexion.CreateCommand();
                command.CommandText = requete;
                MySqlDataReader reader = command.ExecuteReader();

                string[] valueString = new string[reader.FieldCount];
                while (reader.Read())
                {
                    MySqlDataReader reader = command1.ExecuteReader();

                    string[] valueString = new string[reader.FieldCount];
                    while (reader.Read())
                    {


                        string requete1 = "SELECT * FROM Sommet WHERE graphe_id = @id ORDER BY id;";
                        string requete2 = "SELECT * FROM Arc WHERE graphe_id = @id;";
                        // Ordre recommandé :
                        //   1. SELECT dans Graphe WHERE id = @id -> récupérer IsOriented, etc.
                        //   2. SELECT dans Sommet WHERE graphe_id = @id -> reconstruire les sommets
                        //      (respecter l'ordre d'insertion pour que les indices de la matrice
                        //       correspondent à ceux sauvegardés)
                        //   3. SELECT dans Arc WHERE graphe_id = @id -> reconstruire la matrice
                        //      d'adjacence en utilisant les correspondances sommet_id <-> indice

                        throw new NotImplementedException("LoadGraph non implémenté.");
                    }
                }
            }
        }

        /// <summary>
        /// Sauvegarde la tournée <paramref name="t"/> (effectuée dans le graphe
        /// identifié par <paramref name="graphId"/>) en base de données
        /// et renvoie son identifiant.
        /// </summary>
        /// <param name="graphId">Identifiant BdD du graphe dans lequel la tournée a été calculée.</param>
        /// <param name="t">La tournée à sauvegarder.</param>
        /// <returns>Identifiant de la tournée en base de données (AUTO_INCREMENT).</returns>
        public uint SaveTour(uint graphId, Tour t)
        {
            // TODO : implémenter la sauvegarde de la tournée
            //
            // Ordre recommandé :
            //   1. INSERT dans Tournee (cout_total, graphe_id) -> récupérer l'id
            //   2. Pour chaque sommet de la séquence (avec son numéro d'ordre) :
            //      INSERT dans EtapeTournee (tournee_id, numero_ordre, sommet_id)
            //
            // Attention : conserver l'ordre des étapes est essentiel pour
            //             pouvoir reconstruire la tournée fidèlement au chargement.

            throw new NotImplementedException("SaveTour non implémenté.");
        }

        /// <summary>
        /// Charge depuis la base de données la tournée identifiée par <paramref name="id"/>
        /// et renvoie une instance de la classe <see cref="Tour"/>.
        /// </summary>
        /// <param name="id">Identifiant de la tournée à charger.</param>
        /// <returns>Instance de <see cref="Tour"/> reconstituée.</returns>
        public Tour LoadTour(uint id)
        {
            // TODO : implémenter le chargement de la tournée
            //
            // Ordre recommandé :
            //   1. SELECT dans Tournee WHERE id = @id -> récupérer cout_total et graphe_id
            //   2. SELECT dans EtapeTournee JOIN Sommet WHERE tournee_id = @id
            //      ORDER BY numero_ordre -> reconstruire la séquence ordonnée de sommets
            //   3. Construire et retourner l'instance Tour

            throw new NotImplementedException("LoadTour non implémenté.");
        }

        // ─────────────────────────────────────────────────────────────────────
        // Méthodes utilitaires privées (à compléter selon vos besoins)
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Crée et retourne une nouvelle connexion MySQL ouverte.
        /// Encadrez toujours l'appel dans un bloc using pour garantir la fermeture.
        /// </summary>
        private MySqlConnection OpenConnection()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
