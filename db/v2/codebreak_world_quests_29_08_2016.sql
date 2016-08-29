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
CREATE DATABASE IF NOT EXISTS `codebreak_world` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `codebreak_world`;


-- Export de la structure de table codebreak_world. characterquest
CREATE TABLE IF NOT EXISTS `characterquest` (
  `Id` bigint(20) NOT NULL,
  `CurrentStepId` int(11) NOT NULL,
  `Done` bit(1) NOT NULL,
  `SerializedObjectives` text NOT NULL,
  PRIMARY KEY (`Id`,`CurrentStepId`),
  KEY `FK_quest_step` (`CurrentStepId`),
  CONSTRAINT `FK_quest_character` FOREIGN KEY (`Id`) REFERENCES `characterinstance` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_quest_step` FOREIGN KEY (`CurrentStepId`) REFERENCES `queststep` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Export de données de la table codebreak_world.characterquest : ~1 rows (environ)
DELETE FROM `characterquest`;
/*!40000 ALTER TABLE `characterquest` DISABLE KEYS */;
INSERT INTO `characterquest` (`Id`, `CurrentStepId`, `Done`, `SerializedObjectives`) VALUES
	(11074, 951, b'1', '3537:1');
/*!40000 ALTER TABLE `characterquest` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
