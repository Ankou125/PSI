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

        private readonly string _connectionString;
        Dictionary<int, uint> sommetId;
        Dictionary<int, uint> etapId;

        // TODO : si vous avez besoin de maintenir une connexion ouverte,
        //        ajoutez un attribut MySqlConnection ici.
        private readonly MySqlConnection _connection;

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
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            uint graphId;
            try
            {
                if (_connection.State != ConnectionState.Open) //Vérifie que la connexion est bien ouverte
                    _connection.Open();
                using (var transaction = _connection.BeginTransaction())
                {
                    try
                    {
                        //Insertion du graph
                        string query = "INSERT INTO Graphe (est_oriente) VALUES (@oriente);";
                        using (MySqlCommand cmdGraph = new MySqlCommand(query, _connection)) //gère la commande SQL pour l'insertion du graph
                        {
                            cmdGraph.Transaction = transaction;
                            cmdGraph.Parameters.AddWithValue("@oriente", g.Directed); //remplace le praramètre SQL par la valeur de Directed
                            cmdGraph.ExecuteNonQuery();
                            graphId = (uint)cmdGraph.LastInsertedId;
                        }
                        foreach (var sommet in g.Sommets)
                        {
                            string querySommet = @"     
                                        INSERT INTO Sommet (graphe_id, nom, valeur)
                                        VALUES (@graphe_id, @nom, @valeur);";
                            using (MySqlCommand cmdSommet = new MySqlCommand(querySommet, _connection)) //gère la commande SQL pour l'insertion des sommets
                            {
                                cmdSommet.Transaction = transaction;
                                cmdSommet.Parameters.AddWithValue("@graphe_id", graphId);
                                cmdSommet.Parameters.AddWithValue("@nom", sommet.Nom);
                                cmdSommet.Parameters.AddWithValue("@valeur", sommet.Valeur);
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
                                        VALUES (@gid, @src, @dst, @poids);", _connection))
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
                                        VALUES (@gid, @src, @dst, @poids);", _connection))
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
            if (_connection.State != ConnectionState.Open) //Vérifie que la connexion est bien ouverte
                _connection.Open();
            try
            {
                bool estOriente;
                int nbSommets;
                string requete = "SELECT * FROM Graphe WHERE id = @id;"; // Récupère les informations du graphe (est_oriente)
                using (MySqlCommand cmdGraphe = new MySqlCommand(requete, _connection))
                {
                    cmdGraphe.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader readerGraphe = cmdGraphe.ExecuteReader())
                    {
                        if (readerGraphe.Read())
                        {
                            estOriente = Convert.ToBoolean(readerGraphe["est_oriente"]);
                            nbSommets = Convert.ToInt32(readerGraphe["nb_sommets"]);
                        }
                        else
                        {
                            throw new Exception("Graphe non trouvé");
                        }
                    }
                }

                List<Sommet> sommets = new List<Sommet>(); // Charger les sommets dans l'ordre
                Dictionary<uint, int> idSommetVersIndex = new Dictionary<uint, int>(); // Pour faire le lien entre les id des sommets en BdD et leurs indices dans la matrice
                string requeteSommets = "SELECT * FROM Sommet WHERE graphe_id = @id ORDER BY id;"; // Récupère les sommets du graphe
                using (MySqlCommand cmdSommets = new MySqlCommand(requeteSommets, _connection))
                {
                    cmdSommets.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader readerSommets = cmdSommets.ExecuteReader())
                    {
                        int index = 0;
                        while (readerSommets.Read())
                        {
                            uint idSommet = (uint)readerSommets["id"];
                            string nom = readerSommets["nom"].ToString();
                            float valeur = readerSommets["valeur"] != DBNull.Value ? Convert.ToSingle(readerSommets["valeur"]) : 0;
                            Sommet s = new Sommet(nom, valeur);
                            s.Id = idSommet;
                            sommets.Add(s);
                            idSommetVersIndex[idSommet] = Sommet.Count - 1;
                        }
                    }
                }
                if (sommets.Count != nbSommets)
                {
                    throw new Exception("Nombre de sommets récupérés ne correspond pas au nombre indiqué dans la table Graphe");
                }
                Graph g = new Graph(estOriente, sommets);
                string requeteArcs = "SELECT * FROM Arc WHERE graphe_id = @id;"; // Récupère les arcs du graphe
                using (MySqlCommand cmdArcs = new MySqlCommand(requeteArcs, _connection))
                {
                    cmdArcs.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader readerArcs = cmdArcs.ExecuteReader())
                    {
                        while (readerArcs.Read())
                        {
                            uint idSource = (uint)readerArcs["sommet_source"];
                            uint idDest = (uint)readerArcs["sommet_dest"];
                            float poids = Convert.ToSingle(readerArcs["poids"]);
                            int indexSource = idSommetVersIndex[idSource];
                            int indexDest = idSommetVersIndex[idDest];
                            g.Matrice.Matrice[indexSource][indexDest] = poids;
                            if (!estOriente)
                            {
                                g.Matrice.Matrice[indexDest][indexSource] = poids; // Si le graphe est non orienté, on remplit aussi la case symétrique
                            }
                        }
                    }
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
                if (_connection.State != ConnectionState.Open) //Vérifie que la connexion est bien ouverte
                    _connection.Open();
                using (var transaction = _connection.BeginTransaction())
                {
                    try
                    {
                        List<Sommet> Ordre = Tour.Tri(t);
                        //Insertion de la Tournée
                        string query = "INSERT INTO Tournee (graphe_id, cout_total) VALUES (@graphId,@cout);";
                        using (MySqlCommand cmdTour = new MySqlCommand(query, _connection)) //gère la commande SQL 
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
                            using (MySqlCommand cmdEtape = new MySqlCommand(queryEtape, _connection)) //gère la commande SQL pour l'insertion des sommets
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

            if (_connection.State != ConnectionState.Open) //Vérifie que la connexion est bien ouverte
                _connection.Open();
            try
            {
                uint graphId;
                float coutTotal;
                string queryTournee = "SELECT * FROM Tournee WHERE id = @id;"; // Récupère les informations de la tournée (cout_total et graphe_id)
                using (MySqlCommand cmdTournee = new MySqlCommand(queryTournee, _connection))
                {
                    cmdTournee.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader readerTournee = cmdTournee.ExecuteReader())
                    {
                        if (readerTournee.Read())
                        {
                            graphId = (uint)readerTournee["graphe_id"];
                            coutTotal = (float)readerTournee["cout_total"];
                        }
                        else
                        {
                            throw new Exception("Tournée non trouvée");
                        }
                    }
                }
                List<Sommet> ordre = new List<Sommet>();
                string queryEtapes = @"SELECT s.* FROM EtapeTournee et
                                        JOIN Sommet s ON et.sommet_id = s.id
                                        WHERE et.tournee_id = @id
                                        ORDER BY et.numero_ordre;"; // Récupère les étapes de la tournée dans l'ordre
                using (MySqlCommand cmdEtapes = new MySqlCommand(queryEtapes, _connection))
                {
                    cmdEtapes.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader readerEtapes = cmdEtapes.ExecuteReader())
                    {
                        while (readerEtapes.Read())
                        {
                            Sommet sommet = new Sommet
                            {
                                string nom = readerEtapes["nom"].ToString();
                                float valeur = readerEtapes["valeur"] != DBNull.Value ? 0 : Convert.ToSingle(readerEtapes["valeur"]);
                                Sommet s = new Sommet(nom, valeur);
                                s.Id = (uint)readerEtapes["id"];
                                ordre.Add(s);
                            }
                        }
                    }
                }
                Tour tour = new Tour(ordre, coutTotal);
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
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
