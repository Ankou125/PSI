-- =============================================================================
-- PSI 2025-2026 – Objectif 3 : Base de données
-- Script d'initialisation de la base de données TourneeFutee
--
-- Instructions :
--   1. Créez la base de données avec : CREATE DATABASE tourneefutee;
--   2. Sélectionnez-la avec      : USE tourneefutee;
--   3. Exécutez ce script complet pour créer toutes les tables.
--
-- TODO : compléter les parties marquées "TODO" ci-dessous.
-- =============================================================================

-- Supprimer les tables dans l'ordre inverse des dépendances (pour réinitialiser)
DROP TABLE IF EXISTS EtapeTournee;
DROP TABLE IF EXISTS Tournee;
DROP TABLE IF EXISTS Arc;
DROP TABLE IF EXISTS Sommet;
DROP TABLE IF EXISTS Graphe;

-- =============================================================================
-- Table : Graphe
-- Représente un graphe (orienté ou non).
-- =============================================================================
CREATE TABLE Graphe (
    id           INT UNSIGNED    NOT NULL AUTO_INCREMENT,
    est_oriente  TINYINT(1)      NOT NULL DEFAULT 0,   -- 0 = non orienté, 1 = orienté

    -- TODO : ajouter d'autres colonnes si nécessaire
    --        (ex : nom du graphe, nombre de sommets pour validation, ...)

    PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


-- =============================================================================
-- Table : Sommet
-- Représente un sommet appartenant à un graphe.
-- =============================================================================
CREATE TABLE Sommet (
    id          INT UNSIGNED    NOT NULL AUTO_INCREMENT,
    graphe_id   INT UNSIGNED    NOT NULL,
    nom         VARCHAR(50)     NOT NULL,               -- nom/label du sommet (ex : "A", "Paris")
    valeur      FLOAT           NULL,                   -- valeur associée au sommet (peut être NULL)

    -- TODO : ajouter d'autres colonnes si nécessaire
    --        (ex : indice dans la matrice d'adjacence pour faciliter le chargement)

    PRIMARY KEY (id),
    FOREIGN KEY (graphe_id) REFERENCES Graphe(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


-- =============================================================================
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

    -- TODO : ajouter d'autres colonnes si nécessaire

    PRIMARY KEY (id),
    FOREIGN KEY (graphe_id)     REFERENCES Graphe(id) ON DELETE CASCADE,
    FOREIGN KEY (sommet_source) REFERENCES Sommet(id) ON DELETE CASCADE,
    FOREIGN KEY (sommet_dest)   REFERENCES Sommet(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


-- =============================================================================
-- Table : Tournee
-- Représente une tournée optimale calculée par l'algorithme de Little
-- dans un graphe donné.
-- =============================================================================
CREATE TABLE Tournee (
    id          INT UNSIGNED    NOT NULL AUTO_INCREMENT,
    graphe_id   INT UNSIGNED    NOT NULL,
    cout_total  FLOAT           NOT NULL,

    -- TODO : ajouter d'autres colonnes si nécessaire

    PRIMARY KEY (id),
    FOREIGN KEY (graphe_id) REFERENCES Graphe(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


-- =============================================================================
-- Table : EtapeTournee
-- Représente une étape (un sommet visité à un certain rang) d'une tournée.
-- L'ordre des étapes est défini par la colonne numero_ordre.
-- =============================================================================
CREATE TABLE EtapeTournee (
    tournee_id      INT UNSIGNED    NOT NULL,
    numero_ordre    INT UNSIGNED    NOT NULL,            -- position dans la séquence (commence à 0 ou 1)
    sommet_id       INT UNSIGNED    NOT NULL,

    -- TODO : ajouter d'autres colonnes si nécessaire

    PRIMARY KEY (tournee_id, numero_ordre),
    FOREIGN KEY (tournee_id) REFERENCES Tournee(id) ON DELETE CASCADE,
    FOREIGN KEY (sommet_id)  REFERENCES Sommet(id)  ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


-- =============================================================================
-- Vérification : afficher les tables créées
-- =============================================================================
SHOW TABLES;
