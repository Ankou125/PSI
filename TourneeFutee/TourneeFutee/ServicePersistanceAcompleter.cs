using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Macs;
using System.Collections.Generic;
using System.Data;
using System.Transactions;

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
        private readonly string connectionString;
        private Dictionary<int, uint> sommetId;
        private Dictionary<int, uint> etapId;

        // TODO : si vous avez besoin de maintenir une connexion ouverte,  ajoutez un attribut MySqlConnection ici.
        private readonly MySqlConnection connection;

        // ─────────────────────────────────────────────────────────────────────
        // Constructeur
        // ─────────────────────────────────────────────────────────────────────
        public Dictionary<int, uint> SommetId
        {
            get { return this.sommetId; }
            set { this.sommetId = value; }
        }
        public Dictionary<int, uint> EtapeId
        {
            get { return this.etapId; }
            set { this.etapId = value; }
        }
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
            this.connectionString = $"SERVER={serverIp};" +
                                         $"DATABASE={dbname};" +
                                         $"UID={user};PASSWORD={pwd};";
            try //test si la connexion fonctionne
            {
                connection = new MySqlConnection(this.connectionString);
                connection.Open();
                if (connection.State != ConnectionState.Open)
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
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
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
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            uint graphId;
            try
            {
                if (connection.State != ConnectionState.Open) //Vérifie que la connexion est bien ouverte
                    connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //Insertion du graph
                        string query = "INSERT INTO Graphe (est_oriente,nb_sommets) VALUES (@oriente, @nb_sommets);";
                        using (MySqlCommand cmdGraph = new MySqlCommand(query, connection)) //gère la commande SQL pour l'insertion du graph
                        {
                            cmdGraph.Transaction = transaction;
                            cmdGraph.Parameters.AddWithValue("@oriente", g.Directed); //remplace le praramètre SQL par la valeur de Directed
                            cmdGraph.Parameters.AddWithValue("@nb_sommets", g.Sommets.Count);
                            cmdGraph.ExecuteNonQuery();
                            graphId = (uint)cmdGraph.LastInsertedId;
                        }
                        //Insertion des Sommets
                        for (int j = 0; j < g.Sommets.Count; j++)
                        {
                            var sommet = g.Sommets[j];
                            string querySommet = @"INSERT INTO Sommet (graphe_id, nom, valeur, indice_mat) VALUES (@graphe_id, @nom, @valeur, @indice);";
                            using (MySqlCommand cmdSommet = new MySqlCommand(querySommet, connection)) //gère la commande SQL pour l'insertion des sommets
                            {
                                cmdSommet.Transaction = transaction;
                                cmdSommet.Parameters.AddWithValue("@graphe_id", graphId);
                                cmdSommet.Parameters.AddWithValue("@nom", sommet.Nom);
                                cmdSommet.Parameters.AddWithValue("@valeur", sommet.Valeur);
                                cmdSommet.Parameters.AddWithValue("@indice", j);
                                cmdSommet.ExecuteNonQuery();
                                sommet.Id = (uint)cmdSommet.LastInsertedId;
                            }
                        }
                        //Insertion des arcs
                        int i = 0;
                        if (g.Directed) //Si le graph est orienté on parcours toute la matrice
                        {
                            for (i = 0; i < g.Sommets.Count; i++)
                            {
                                for (int j = 0; j < g.Sommets.Count; j++)
                                {
                                    float poids = g.Matrice.Matrice[i][j];
                                    if (Math.Abs(poids - g.Matrice.DefaultValue) > 0.0001f) //Moins risqué pour les float
                                    {
                                        using (MySqlCommand cmdArc = new MySqlCommand(
                                         @"INSERT INTO Arc (graphe_id, sommet_source, sommet_dest, poids)
                                        VALUES (@gid, @src, @dst, @poids);", connection))
                                        {
                                            cmdArc.Transaction = transaction;
                                            cmdArc.Parameters.AddWithValue("@gid", graphId);
                                            cmdArc.Parameters.AddWithValue("@src", g.Sommets[i].Id);
                                            cmdArc.Parameters.AddWithValue("@dst", g.Sommets[j].Id);
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
                                    if (Math.Abs(poids - g.Matrice.DefaultValue) > 0.0001f)
                                    {
                                        using (MySqlCommand cmd = new MySqlCommand(
                                            @"INSERT INTO Arc (graphe_id, sommet_source, sommet_dest, poids)
                                        VALUES (@gid, @src, @dst, @poids);", connection))
                                        {
                                            cmd.Transaction = transaction;
                                            cmd.Parameters.AddWithValue("@gid", graphId);
                                            cmd.Parameters.AddWithValue("@src", g.Sommets[i].Id);
                                            cmd.Parameters.AddWithValue("@dst", g.Sommets[j].Id);
                                            cmd.Parameters.AddWithValue("@poids", poids);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                return graphId;
            }
            finally
            {
                CloseConnection();
            }
        }
        #region Consignes
        /// <summary>
        /// Charge depuis la base de données le graphe identifié par <paramref name="id"/>
        /// et renvoie une instance de la classe <see cref="Graph"/>.
        /// </summary>
        /// <param name="id">Identifiant du graphe à charger.</param>
        /// <returns>Instance de <see cref="Graph"/> reconstituée.</returns>
        #endregion 
        public Graph LoadGraph(uint id)
        {
            if (connection.State != ConnectionState.Open) //Vérifie que la connexion est bien ouverte
                connection.Open();
            try
            {
                bool estOriente;
                int nbSommets;
                string requete1 = "SELECT est_oriente, nb_sommets FROM Graphe WHERE id = @id;"; // Récupère les informations du graphe (est_oriente)
                using (MySqlCommand command1 = new MySqlCommand(requete1, connection))
                {
                    command1.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader1 = command1.ExecuteReader())
                    {
                        if (reader1.Read())
                        {
                            estOriente = Convert.ToBoolean(reader1["est_oriente"]);
                            nbSommets = Convert.ToInt32(reader1["nb_sommets"]);
                        }
                        else
                        {
                            throw new Exception("Graphe non trouvé");
                        }
                    }
                }
                Graph g = new Graph(estOriente);
                Dictionary<uint, string> idSommetVersIndex = new Dictionary<uint, string>(); // Pour faire le lien entre les id des sommets en BdD et leurs indices dans la matrice
                string requete2 = @"SELECT id, nom, valeur, indice_mat FROM Sommet WHERE graphe_id = @id ORDER BY indice_mat;"; // Récupère les sommets du graphe
                using (MySqlCommand command2 = new MySqlCommand(requete2, connection))
                {
                    command2.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader2 = command2.ExecuteReader())
                    {
                        int index = 0;
                        while (reader2.Read())
                        {
                            uint idSommet = Convert.ToUInt32(reader2["id"]);
                            string nom = reader2["nom"].ToString();
                            float valeur = reader2["valeur"] == DBNull.Value ? 0 : Convert.ToSingle(reader2["valeur"]);
                            g.AddVertex(nom, valeur);
                            g.Sommets[g.Sommets.Count - 1].Id = idSommet; // Associe l'id du sommet en BdD à l'objet Sommet dans le graphe
                            idSommetVersIndex[idSommet] = nom;
                        }
                    }
                }
                if (g.Sommets.Count != nbSommets)
                {
                    throw new Exception("Nombre de sommets récupérés ne correspond pas au nombre indiqué dans la table Graphe");
                }
                string requete3 = @"SELECT sommet_source, sommet_dest, poids FROM Arc WHERE graphe_id = @id;";// Récupère les arcs du graphe
                using (MySqlCommand command3 = new MySqlCommand(requete3, connection))
                {
                    command3.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader3 = command3.ExecuteReader())
                    {
                        while (reader3.Read())
                        {
                            uint idSource = (uint)reader3["sommet_source"];
                            uint idDest = (uint)reader3["sommet_dest"];
                            float poids = Convert.ToSingle(reader3["poids"]);
                            string indexSource = idSommetVersIndex[idSource];
                            string indexDest = idSommetVersIndex[idDest];
                            g.AddEdge(indexSource, indexDest, poids);
                        }
                    }
                }
                return g;
            }
            finally
            {
                CloseConnection();
            }
        }

        #region Consignes
            /// <summary>
            /// Sauvegarde la tournée <paramref name="t"/> (effectuée dans le graphe
            /// identifié par <paramref name="graphId"/>) en base de données
            /// et renvoie son identifiant.
            /// </summary>
            /// <param name="graphId">Identifiant BdD du graphe dans lequel la tournée a été calculée.</param>
            /// <param name="t">La tournée à sauvegarder.</param>
            /// <returns>Identifiant de la tournée en base de données (AUTO_INCREMENT).</returns>
            /// // TODO : implémenter la sauvegarde de la tournée
            //
            // Ordre recommandé :
            //   1. INSERT dans Tournee (cout_total, graphe_id) -> récupérer l'id
            //   2. Pour chaque sommet de la séquence (avec son numéro d'ordre) :
            //      INSERT dans EtapeTournee (tournee_id, numero_ordre, sommet_id)
            //
            // Attention : conserver l'ordre des étapes est essentiel pour
            //             pouvoir reconstruire la tournée fidèlement au chargement.
            #endregion
        public uint SaveTour(uint graphId, Tour t) //Tri + enregistrement Etapes
        {
            uint idTour;
            if (t == null)
                throw new ArgumentNullException(nameof(t));

            try
            {
                if (connection.State != ConnectionState.Open) //Vérifie que la connexion est bien ouverte
                    connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var nextMap = new Dictionary<string, string>();
                        foreach (var (source, destination) in t.Parcours)
                        {
                            nextMap[source] = destination;
                        }
                        string start = t.Parcours[0].source;
                        string current = start;
                        List<Sommet> Ordre = new List<Sommet>();
                        do
                        {
                            Ordre.Add(t.Sommets.First(s => s.Nom == current));
                            current = nextMap[current];
                        } while (current != start);
                        //Insertion de la Tournée
                        string query = "INSERT INTO Tournee (graphe_id, cout_total) VALUES (@graphId,@cout);";
                        using (MySqlCommand cmdTour = new MySqlCommand(query, connection)) //gère la commande SQL 
                        {
                            cmdTour.Transaction = transaction;
                            cmdTour.Parameters.AddWithValue("@graphId", graphId); //remplace le praramètre SQL par l'id du graph
                            cmdTour.Parameters.AddWithValue("@cout", t.Cost); //remplace le praramètre SQL par la valeur de cost
                            cmdTour.ExecuteNonQuery();
                            idTour = (uint)cmdTour.LastInsertedId; //Récupère l'id de la tournée enregistrée
                        }
                        //Insertion Etapes de la tournée
                        int i = 0;
                        while (i < Ordre.Count)
                        {
                            string queryEtape = @"     
                            INSERT INTO EtapeTournee (tournee_id, numero_ordre, sommet_id)
                            VALUES (@tournee_id, @ordre, @sommet_id);";
                            using (MySqlCommand cmdEtape = new MySqlCommand(queryEtape, connection)) //gère la commande SQL pour l'insertion des sommets
                            {
                                cmdEtape.Transaction = transaction;
                                cmdEtape.Parameters.AddWithValue("@tournee_id", idTour);
                                cmdEtape.Parameters.AddWithValue("@ordre", i);
                                cmdEtape.Parameters.AddWithValue("@sommet_id", Ordre[i].Id);
                                cmdEtape.ExecuteNonQuery();
                            }
                            i++;
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    return idTour;
                }
            }
            finally
            {
                CloseConnection();
            }
        }
        #region Consignes
        /// <summary>
        /// Charge depuis la base de données la tournée identifiée par <paramref name="id"/>
        /// et renvoie une instance de la classe <see cref="Tour"/>.
        /// </summary>
        /// <param name="id">Identifiant de la tournée à charger.</param>
        /// <returns>Instance de <see cref="Tour"/> reconstituée.</returns>
        #endregion
        public Tour LoadTour(uint id)
        {
            // TODO : implémenter le chargement de la tournée
            //
            // Ordre recommandé :
            //   1. SELECT dans Tournee WHERE id = @id -> récupérer cout_total et graphe_id
            //   2. SELECT dans EtapeTournee JOIN Sommet WHERE tournee_id = @id
            //      ORDER BY numero_ordre -> reconstruire la séquence ordonnée de sommets
            //   3. Construire et retourner l'instance Tour
            // throw new NotImplementedException("LoadTour non implémenté.");

            if (connection.State != ConnectionState.Open) //Vérifie que la connexion est bien ouverte
                connection.Open();
            try
            {
                uint graphId;
                float coutTotal;
                string requete1 = "SELECT * FROM Tournee WHERE id = @id;"; // Récupère les informations de la tournée (cout_total et graphe_id)
                using (MySqlCommand command1= new MySqlCommand(requete1, connection))
                {
                    command1.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader1 = command1.ExecuteReader())
                    {
                        if (reader1.Read())
                        {
                            graphId = (uint)reader1["graphe_id"];
                            coutTotal = (float)reader1["cout_total"];
                        }
                        else
                        {
                            throw new Exception("Tournée non trouvée");
                        }
                    }
                }
                List<Sommet> ordre = new List<Sommet>();
                string requete2 = @"SELECT s.* FROM EtapeTournee et JOIN Sommet s ON et.sommet_id = s.id WHERE et.tournee_id = @id ORDER BY et.numero_ordre;"; // Récupère les étapes de la tournée dans l'ordre
                using (MySqlCommand command2 = new MySqlCommand(requete2, connection))
                {
                    command2.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            Sommet sommet = new Sommet(requete2, coutTotal);
                            {
                                string nom = reader2["nom"].ToString();
                                float valeur = reader2["valeur"] != DBNull.Value ? 0 : Convert.ToSingle(reader2["valeur"]);
                                Sommet s = new Sommet(nom, valeur);
                                s.Id = (uint)reader2["id"];
                                ordre.Add(s);
                            }
                        }
                    }
                }
                List<(string source, string destination)> parcours = new List<(string source, string destination)>();
                for (int i = 0; i < ordre.Count; i++)
                {
                    string source = ordre[i].Nom;
                    string destination = ordre[(i + 1) % ordre.Count].Nom;
                    parcours.Add((source, destination));
                }
                Tour tour = new Tour(parcours, coutTotal);
                return tour;
            }
            finally
            {
                CloseConnection();
            }
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
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
