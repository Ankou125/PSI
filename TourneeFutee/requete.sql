-- =============================================================================
-- CONNEXION / BASE
-- =============================================================================
CREATE DATABASE IF NOT EXISTS TourneeFutee2;
USE TourneeFutee2;

SHOW TABLES;

DESCRIBE Graphe;
DESCRIBE Sommet;
DESCRIBE Arc;
DESCRIBE Tournee;
DESCRIBE EtapeTournee;

SELECT * FROM Graphe;
SELECT * FROM Sommet ORDER BY graphe_id, indice_mat;
SELECT * FROM Arc ORDER BY graphe_id, sommet_source, sommet_dest;
SELECT * FROM Tournee;
SELECT * FROM EtapeTournee ORDER BY tournee_id, numero_ordre;


-- =============================================================================
-- VOIR UN GRAPHE COMPLET
-- Remplacer 1 par l'id voulu
-- =============================================================================
SELECT * FROM Graphe WHERE id = 1;

SELECT * 
FROM Sommet
WHERE graphe_id = 1
ORDER BY indice_mat;

SELECT *
FROM Arc
WHERE graphe_id = 1
ORDER BY sommet_source, sommet_dest;


-- =============================================================================
-- VOIR UNE TOURNEE COMPLETE
-- Remplacer 1 par l'id voulu
-- =============================================================================
SELECT *
FROM Tournee
WHERE id = 1;

SELECT et.numero_ordre, s.id, s.nom, s.valeur
FROM EtapeTournee et
JOIN Sommet s ON et.sommet_id = s.id
WHERE et.tournee_id = 1
ORDER BY et.numero_ordre;


-- =============================================================================
-- SUPPRIMER TOUTES LES DONNEES
-- Attention !
-- =============================================================================
DELETE FROM EtapeTournee;
DELETE FROM Tournee;
DELETE FROM Arc;
DELETE FROM Sommet;
DELETE FROM Graphe;

ALTER TABLE Graphe AUTO_INCREMENT = 1;
ALTER TABLE Sommet AUTO_INCREMENT = 1;
ALTER TABLE Arc AUTO_INCREMENT = 1;
ALTER TABLE Tournee AUTO_INCREMENT = 1;


-- =============================================================================
-- SAVE GRAPH
-- =============================================================================

-- Insertion graphe
INSERT INTO Graphe (est_oriente, nb_sommets)
VALUES (@oriente, @nb_sommets);

-- Insertion sommet
INSERT INTO Sommet (graphe_id, nom, valeur, indice_mat)
VALUES (@graphe_id, @nom, @valeur, @indice);

-- Insertion arc
INSERT INTO Arc (graphe_id, sommet_source, sommet_dest, poids)
VALUES (@gid, @src, @dst, @poids);


-- =============================================================================
-- LOAD GRAPH
-- =============================================================================

-- Charger le graphe
SELECT est_oriente, nb_sommets
FROM Graphe
WHERE id = @id;

-- Charger les sommets
SELECT id, nom, valeur, indice_mat
FROM Sommet
WHERE graphe_id = @id
ORDER BY indice_mat;

-- Charger les arcs
SELECT sommet_source, sommet_dest, poids
FROM Arc
WHERE graphe_id = @id;


-- =============================================================================
-- SAVE TOUR
-- =============================================================================

-- Insertion tournée
INSERT INTO Tournee (graphe_id, cout_total)
VALUES (@graphId, @cout);

-- Retrouver l'id SQL d'un sommet à partir du graphe + du nom
SELECT id
FROM Sommet
WHERE graphe_id = @graphId AND nom = @nom
LIMIT 1;

-- Insertion étape
INSERT INTO EtapeTournee (tournee_id, numero_ordre, sommet_id)
VALUES (@tournee_id, @ordre, @sommet_id);


-- =============================================================================
-- LOAD TOUR
-- =============================================================================

-- Charger le coût de la tournée
SELECT cout_total
FROM Tournee
WHERE id = @id;

-- Charger les étapes avec les noms des sommets
SELECT s.nom
FROM EtapeTournee et
JOIN Sommet s ON et.sommet_id = s.id
WHERE et.tournee_id = @id
ORDER BY et.numero_ordre;