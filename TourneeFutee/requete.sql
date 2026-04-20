-- conexion / base --
CREATE DATABASE IF NOT EXISTS TourneeFutee2;
USE TourneeFutee2; 

SHOW TABLES; -- Voir les tables

DESCRIBE Graphe;
DESCRIBE Sommet;
DESCRIBE Arc;
DESCRIBE Tournee;
DESCRIBE EtapeTournee; -- Voir la structure des tables

SELECT * FROM Graphe; -- Voir tous les graphes

SELECT * FROM Sommet ORDER BY graphe_id, indice_mat; -- Voir tous les sommets

SELECT * FROM Arc ORDER BY graphe_id, sommet_source, sommet_dest; -- Voir tous les arcs

SELECT * FROM Tournee; -- Voir toutes les tournées

SELECT * FROM EtapeTournee ORDER BY tournee_id, numero_ordre; -- Voir toutes les étapes de tournée

-- Voir un graphe complet avec ses sommets
-- Remplacer 1 par l'id voulu
SELECT * FROM Graphe WHERE id = 1;
SELECT * FROM Sommet WHERE graphe_id = 1 ORDER BY indice_mat;
SELECT * FROM Arc WHERE graphe_id = 1;

-- Voir une tournée complète
-- Remplacer 1 par l'id voulu
SELECT * FROM Tournee WHERE id = 1;
SELECT et.numero_ordre, s.nom, s.valeur
FROM EtapeTournee et
JOIN Sommet s ON et.sommet_id = s.id
WHERE et.tournee_id = 1
ORDER BY et.numero_ordre;

-- Supprimer toutes les données --> Attention !
DELETE FROM EtapeTournee;
DELETE FROM Tournee;
DELETE FROM Arc;
DELETE FROM Sommet;
DELETE FROM Graphe;

-- Réinitialiser les AUTO_INCREMENT
ALTER TABLE Graphe AUTO_INCREMENT = 1;
ALTER TABLE Sommet AUTO_INCREMENT = 1;
ALTER TABLE Arc AUTO_INCREMENT = 1;
ALTER TABLE Tournee AUTO_INCREMENT = 1;


-- Save graph --
INSERT INTO Graphe (est_oriente) VALUES (@oriente); -- insertion graphe

INSERT INTO Sommet (graphe_id, nom, valeur)
VALUES (@graphe_id, @nom, @valeur); -- insertion des sommets

INSERT INTO Arc (graphe_id, sommet_source, sommet_dest, poids)
VALUES (@gid, @src, @dst, @poids); -- insertion des arcs


-- load graph --
SELECT est_oriente, nb_sommets
FROM Graphe
WHERE id = @id; -- charger le graphe

SELECT id, nom, valeur, indice_mat
FROM Sommet
WHERE graphe_id = @id
ORDER BY indice_mat; -- charger les sommets

SELECT sommet_source, sommet_dest, poids
FROM Arc
WHERE graphe_id = @id; -- charger les arcs


-- save tour --
INSERT INTO Tournee (graphe_id, cout_total)
VALUES (@graphId, @cout); -- insertion de la tournee

INSERT INTO EtapeTournee (tournee_id, numero_ordre, sommet_id)
VALUES (@tournee_id, @ordre, @sommet_id); -- insertion des etapes 


-- load tour --
SELECT *
FROM Tournee
WHERE id = @id; -- charger la tournee --

SELECT s.*
FROM EtapeTournee et
JOIN Sommet s ON et.sommet_id = s.id
WHERE et.tournee_id = @id
ORDER BY et.numero_ordre; -- charger les étapes --


