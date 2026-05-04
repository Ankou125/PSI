-- PSI 2025-2026 – Objectif 3 : Base de données
-- Instructions :
--   1. Créez la base de données avec : CREATE DATABASE tourneefutee;
--   2. Sélectionnez-la avec      : USE tourneefutee;
--   3. Exécutez ce script complet pour créer toutes les tables.
CREATE DATABASE IF NOT EXISTS TourneeFutee2; 
USE TourneeFutee2;
-- Supprimer les tables dans l'ordre inverse des dépendances (pour réinitialiser)
DROP TABLE IF EXISTS EtapeTournee;
DROP TABLE IF EXISTS Tournee;
DROP TABLE IF EXISTS Arc;
DROP TABLE IF EXISTS Sommet;
DROP TABLE IF EXISTS Graphe;

CREATE TABLE Graphe (
    id           INT UNSIGNED    NOT NULL AUTO_INCREMENT,
    est_oriente  TINYINT(1)		 NOT NULL DEFAULT 0,   -- 0 = non orienté, 1 = orienté
	nb_sommets   INT UNSIGNED 	 NOT NULL,
    PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE Sommet (
    id          INT UNSIGNED    NOT NULL AUTO_INCREMENT,
    graphe_id   INT UNSIGNED    NOT NULL,
    nom         VARCHAR(50)     NOT NULL,               -- nom/label du sommet (ex : "A", "Paris")
    valeur      FLOAT           NULL,                   -- valeur associée au sommet (peut être NULL)
    indice_mat	INT UNSIGNED	NOT NULL,
    PRIMARY KEY (id),
    UNIQUE KEY uk_sommet_indice (graphe_id, indice_mat),
    UNIQUE KEY uk_sommet_nom (graphe_id, nom), -- contrainte d'unicite du nom par graphe
    FOREIGN KEY (graphe_id) REFERENCES Graphe(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Table : Arc
-- Représente un arc (ou une arête) entre deux sommets d'un graphe.
-- Pour un graphe non orienté, un seul arc est stocké par paire (source < dest),
-- ou deux arcs symétriques — à vous de choisir et de documenter votre choix.
-- =============================================================================
CREATE TABLE Arc (
    id              INT UNSIGNED    NOT NULL AUTO_INCREMENT,
    graphe_id       INT UNSIGNED    NOT NULL,
    sommet_source   INT UNSIGNED    NOT NULL,            -- FK vers Sommet (départ)
    sommet_dest     INT UNSIGNED    NOT NULL,            -- FK vers Sommet (arrivée)
    poids           FLOAT           NOT NULL,
    PRIMARY KEY (id),
    UNIQUE KEY uk_arc (graphe_id, sommet_source, sommet_dest),
    FOREIGN KEY (graphe_id)     REFERENCES Graphe(id) ON DELETE CASCADE,
    FOREIGN KEY (sommet_source) REFERENCES Sommet(id) ON DELETE CASCADE,
    FOREIGN KEY (sommet_dest)   REFERENCES Sommet(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE Tournee (
    id          INT UNSIGNED    NOT NULL AUTO_INCREMENT,
    graphe_id   INT UNSIGNED    NOT NULL,
    cout_total  FLOAT           NOT NULL,
	PRIMARY KEY (id),
    FOREIGN KEY (graphe_id) REFERENCES Graphe(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE EtapeTournee (
    tournee_id      INT UNSIGNED    NOT NULL,
    numero_ordre    INT UNSIGNED    NOT NULL,            -- position dans la séquence (commence à 0 ou 1)
    sommet_id       INT UNSIGNED    NOT NULL,
    PRIMARY KEY (tournee_id, numero_ordre),
    FOREIGN KEY (tournee_id) REFERENCES Tournee(id) ON DELETE CASCADE,
    FOREIGN KEY (sommet_id)  REFERENCES Sommet(id)  ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

SHOW TABLES;