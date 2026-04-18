USE TourneeFutee2;

-- =====================================================
-- Voir les tables
-- =====================================================
SHOW TABLES;

-- =====================================================
-- Voir la structure des tables
-- =====================================================
DESCRIBE Graphe;
DESCRIBE Sommet;
DESCRIBE Arc;
DESCRIBE Tournee;
DESCRIBE EtapeTournee;

-- =====================================================
-- Voir tous les graphes
-- =====================================================
SELECT * FROM Graphe;

-- =====================================================
-- Voir tous les sommets
-- =====================================================
SELECT * FROM Sommet ORDER BY graphe_id, indice_mat;

-- =====================================================
-- Voir tous les arcs
-- =====================================================
SELECT * FROM Arc ORDER BY graphe_id, sommet_source, sommet_dest;

-- =====================================================
-- Voir toutes les tournées
-- =====================================================
SELECT * FROM Tournee;

-- =====================================================
-- Voir toutes les étapes de tournée
-- =====================================================
SELECT * FROM EtapeTournee ORDER BY tournee_id, numero_ordre;

-- =====================================================
-- Voir un graphe complet avec ses sommets
-- Remplacer 1 par l'id voulu
-- =====================================================
SELECT * FROM Graphe WHERE id = 1;
SELECT * FROM Sommet WHERE graphe_id = 1 ORDER BY indice_mat;
SELECT * FROM Arc WHERE graphe_id = 1;

-- =====================================================
-- Voir une tournée complète
-- Remplacer 1 par l'id voulu
-- =====================================================
SELECT * FROM Tournee WHERE id = 1;

SELECT et.numero_ordre, s.nom, s.valeur
FROM EtapeTournee et
JOIN Sommet s ON et.sommet_id = s.id
WHERE et.tournee_id = 1
ORDER BY et.numero_ordre;

-- =====================================================
-- Supprimer toutes les données
-- Attention !
-- =====================================================
DELETE FROM EtapeTournee;
DELETE FROM Tournee;
DELETE FROM Arc;
DELETE FROM Sommet;
DELETE FROM Graphe;

-- =====================================================
-- Réinitialiser les AUTO_INCREMENT
-- =====================================================
ALTER TABLE Graphe AUTO_INCREMENT = 1;
ALTER TABLE Sommet AUTO_INCREMENT = 1;
ALTER TABLE Arc AUTO_INCREMENT = 1;
ALTER TABLE Tournee AUTO_INCREMENT = 1;