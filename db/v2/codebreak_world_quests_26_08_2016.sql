-- --------------------------------------------------------
-- Hôte :                        127.0.0.1
-- Version du serveur:           5.6.20 - MySQL Community Server (GPL)
-- SE du serveur:                Win32
-- HeidiSQL Version:             9.3.0.4984
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Export de la structure de la base pour codebreak_world
DROP DATABASE IF EXISTS `codebreak_world`;
CREATE DATABASE IF NOT EXISTS `codebreak_world` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `codebreak_world`;


-- Export de la structure de table codebreak_world. characterquest
DROP TABLE IF EXISTS `characterquest`;
CREATE TABLE IF NOT EXISTS `characterquest` (
  `Id` bigint(20) NOT NULL,
  `CurrentStepId` int(11) NOT NULL,
  `SerializedObjectives` text NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_quest_step` (`CurrentStepId`),
  CONSTRAINT `FK_quest_character` FOREIGN KEY (`Id`) REFERENCES `characterinstance` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_quest_step` FOREIGN KEY (`CurrentStepId`) REFERENCES `queststep` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Export de données de la table codebreak_world.characterquest : ~0 rows (environ)
DELETE FROM `characterquest`;
/*!40000 ALTER TABLE `characterquest` DISABLE KEYS */;
INSERT INTO `characterquest` (`Id`, `CurrentStepId`, `SerializedObjectives`) VALUES
	(11074, 951, '3537:0');
/*!40000 ALTER TABLE `characterquest` ENABLE KEYS */;


-- Export de la structure de table codebreak_world. quest
DROP TABLE IF EXISTS `quest`;
CREATE TABLE IF NOT EXISTS `quest` (
  `Id` int(11) NOT NULL,
  `Description` varchar(200) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Export de données de la table codebreak_world.quest : ~0 rows (environ)
DELETE FROM `quest`;
/*!40000 ALTER TABLE `quest` DISABLE KEYS */;
INSERT INTO `quest` (`Id`, `Description`) VALUES
	(184, 'Piou ?');
/*!40000 ALTER TABLE `quest` ENABLE KEYS */;


-- Export de la structure de table codebreak_world. questobjective
DROP TABLE IF EXISTS `questobjective`;
CREATE TABLE IF NOT EXISTS `questobjective` (
  `Id` int(11) NOT NULL,
  `StepId` int(11) NOT NULL,
  `Type` int(11) NOT NULL,
  `X` int(11) NOT NULL,
  `Y` int(11) NOT NULL,
  `Parameters` text NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_objective_step` (`StepId`),
  KEY `FK_objective_type` (`Type`),
  CONSTRAINT `FK_objective_step` FOREIGN KEY (`StepId`) REFERENCES `queststep` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_objective_type` FOREIGN KEY (`Type`) REFERENCES `questobjectivetype` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Export de données de la table codebreak_world.questobjective : ~1 rows (environ)
DELETE FROM `questobjective`;
/*!40000 ALTER TABLE `questobjective` DISABLE KEYS */;
INSERT INTO `questobjective` (`Id`, `StepId`, `Type`, `X`, `Y`, `Parameters`) VALUES
	(3537, 951, 6, 0, 0, '928,1');
/*!40000 ALTER TABLE `questobjective` ENABLE KEYS */;


-- Export de la structure de table codebreak_world. questobjectivetype
DROP TABLE IF EXISTS `questobjectivetype`;
CREATE TABLE IF NOT EXISTS `questobjectivetype` (
  `Id` int(11) NOT NULL,
  `Description` text NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Export de données de la table codebreak_world.questobjectivetype : ~13 rows (environ)
DELETE FROM `questobjectivetype`;
/*!40000 ALTER TABLE `questobjectivetype` DISABLE KEYS */;
INSERT INTO `questobjectivetype` (`Id`, `Description`) VALUES
	(0, '#1'),
	(1, 'Aller voir #1'),
	(3, 'Ramener à #1 : x#3 #2'),
	(4, 'Découvrir la carte #1'),
	(5, 'Découvrir la zone #1'),
	(6, 'Vaincre x#2 #1 en un seul combat'),
	(7, 'Monstre à vaincre : #1'),
	(8, 'Utiliser : #1'),
	(9, 'Retourner voir #1'),
	(10, 'Escorter #1 #2'),
	(11, 'Vaincre un joueur en défi #1'),
	(12, 'Rapporter #3 âme de #2 à #1'),
	(13, 'Eliminer #1');
/*!40000 ALTER TABLE `questobjectivetype` ENABLE KEYS */;


-- Export de la structure de table codebreak_world. queststep
DROP TABLE IF EXISTS `queststep`;
CREATE TABLE IF NOT EXISTS `queststep` (
  `Id` int(11) NOT NULL,
  `QuestId` int(11) NOT NULL,
  `Order` int(11) NOT NULL,
  `Name` text NOT NULL,
  `Description` text NOT NULL,
  `Actions` text NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_step_quest` (`QuestId`),
  CONSTRAINT `FK_step_quest` FOREIGN KEY (`QuestId`) REFERENCES `quest` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Export de données de la table codebreak_world.queststep : ~1 rows (environ)
DELETE FROM `queststep`;
/*!40000 ALTER TABLE `queststep` DISABLE KEYS */;
INSERT INTO `queststep` (`Id`, `QuestId`, `Order`, `Name`, `Description`, `Actions`) VALUES
	(951, 184, 0, 'Ennemi intime.', 'Trouver et éliminer votre cible.', '2004:itemId=12009,194:kamas=10000');
/*!40000 ALTER TABLE `queststep` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
