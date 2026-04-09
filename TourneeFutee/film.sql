-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: film
-- ------------------------------------------------------
-- Server version	8.0.35
DROP DATABASE IF EXISTS film;
CREATE DATABASE film;
USE film;

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `cote`
--

DROP TABLE IF EXISTS `cote`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cote` (
  `idCote` int NOT NULL,
  `libelle` varchar(15) NOT NULL,
  PRIMARY KEY (`idCote`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cote`
--

LOCK TABLES `cote` WRITE;
/*!40000 ALTER TABLE `cote` DISABLE KEYS */;
INSERT INTO `cote` VALUES (1,'Chef d\'Oeuvre'),(2,'Remarquable'),(3,'Très bon'),(4,'Bon'),(5,'Moyen'),(6,'Pauvre'),(7,'Minable');
/*!40000 ALTER TABLE `cote` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `film`
-- CORRECTION : clé primaire sur idFilm seul (idCote n'est pas un identifiant)
--

DROP TABLE IF EXISTS `film`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `film` (
  `idFilm` int NOT NULL,
  `titre` varchar(45) DEFAULT NULL,
  `annee` int DEFAULT NULL,
  `pays` varchar(20) DEFAULT NULL,
  `idCote` int NOT NULL,
  PRIMARY KEY (`idFilm`),
  KEY `idCote` (`idCote`),
  CONSTRAINT `film_ibfk_1` FOREIGN KEY (`idCote`) REFERENCES `cote` (`idCote`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `film`
-- CORRECTIONS :
--   - Ajout du film 7 "Un Prophète" (manquant)
--   - Film 8 "Inexistant" remplacé par "P'tit con"
--

LOCK TABLES `film` WRITE;
/*!40000 ALTER TABLE `film` DISABLE KEYS */;
INSERT INTO `film` VALUES
  (1,'Air Force One',1997,'USA',5),
  (2,'Beau-Père',1981,'France',4),
  (3,'Maria Chapdelaine',1983,'Canada',4),
  (4,'Autour de Minuit',1986,'France',2),
  (5,'Patriot Games',1992,'USA',4),
  (6,'Operation Condor',1991,'Hong-Kong',5),
  (7,'Un Prophète',2009,'France',2),
  (8,'P\'tit con',1984,'France',4);
/*!40000 ALTER TABLE `film` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `personne`
--

DROP TABLE IF EXISTS `personne`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `personne` (
  `idPersonne` int NOT NULL,
  `prenom` varchar(15) DEFAULT NULL,
  `nom` varchar(15) NOT NULL,
  PRIMARY KEY (`idPersonne`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `personne`
-- CORRECTIONS :
--   - Ajout de Jacques Audiard (réalisateur d'Un Prophète)
--   - Ajout de Gérard Lauzier (réalisateur de P'tit con)
--

LOCK TABLES `personne` WRITE;
/*!40000 ALTER TABLE `personne` DISABLE KEYS */;
INSERT INTO `personne` VALUES
  (1,'W.','Petersen'),
  (2,'Harrison','Ford'),
  (3,'Gary','Oldman'),
  (4,'Glenn','Close'),
  (5,'Bertrand','Blier'),
  (6,'Patrick','Dewaere'),
  (7,'Ariel','Besse'),
  (8,'Gilles','Carle'),
  (9,'Carole','Laure'),
  (10,'Nick','Mancuso'),
  (11,'Yoland','Guérard'),
  (12,'Bertrand','Tavernier'),
  (13,'Dexter','Gordon'),
  (14,'François','Cluzet'),
  (15,'P.','Noyce'),
  (16,'Anne','Archer'),
  (17,'Patrick','Bergin'),
  (18,'Jackie','Chan'),
  (19,'Carol','Cheng'),
  (20,'Eva','Cobo de Garcia'),
  (21,'Geraldine','Nacache'),
  (22,'Leila','Bekhti'),
  (23,'Tahar','Rahim'),
  (24,'Jacques','Audiard'),
  (25,'Gérard','Lauzier');
/*!40000 ALTER TABLE `personne` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `idRole` int NOT NULL,
  `libelle` varchar(15) NOT NULL,
  PRIMARY KEY (`idRole`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (1,'Acteur'),(2,'Réalisateur');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `participation`
--

DROP TABLE IF EXISTS `participation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participation` (
  `idFilm` int NOT NULL,
  `idPersonne` int NOT NULL,
  `idRole` int NOT NULL,
  PRIMARY KEY (`idFilm`,`idPersonne`,`idRole`),
  KEY `idPersonne` (`idPersonne`),
  KEY `idRole` (`idRole`),
  CONSTRAINT `participation_ibfk_1` FOREIGN KEY (`idFilm`) REFERENCES `film` (`idFilm`),
  CONSTRAINT `participation_ibfk_2` FOREIGN KEY (`idPersonne`) REFERENCES `personne` (`idPersonne`),
  CONSTRAINT `participation_ibfk_3` FOREIGN KEY (`idRole`) REFERENCES `role` (`idRole`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `participation`
-- CORRECTIONS :
--   - Ajout des participations pour le film 7 (Un Prophète)
--   - Ajout du réalisateur pour le film 8 (P'tit con)
--

LOCK TABLES `participation` WRITE;
/*!40000 ALTER TABLE `participation` DISABLE KEYS */;
INSERT INTO `participation` VALUES
  -- Air Force One
  (1,1,2),(1,2,1),(1,3,1),(1,4,1),
  -- Beau-Père
  (2,5,2),(2,6,1),(2,7,1),
  -- Maria Chapdelaine
  (3,8,2),(3,9,1),(3,10,1),(3,11,1),
  -- Autour de Minuit
  (4,12,2),(4,13,1),(4,14,1),
  -- Patriot Games
  (5,2,1),(5,15,2),(5,16,1),(5,17,1),
  -- Operation Condor
  (6,18,1),(6,18,2),(6,19,1),(6,20,1),
  -- Un Prophète
  (7,24,2),(7,21,1),(7,22,1),(7,23,1),
  -- P'tit con
  (8,25,2),(8,7,1);
/*!40000 ALTER TABLE `participation` ENABLE KEYS */;
UNLOCK TABLES;

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed
