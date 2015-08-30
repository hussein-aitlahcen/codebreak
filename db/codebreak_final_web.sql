-- --------------------------------------------------------
-- Hôte:                         127.0.0.1
-- Version du serveur:           5.6.20 - MySQL Community Server (GPL)
-- Serveur OS:                   Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Export de la structure de la base pour codebreak_web
CREATE DATABASE IF NOT EXISTS `codebreak_web` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `codebreak_web`;


-- Export de la structure de table codebreak_web. configvariable
CREATE TABLE IF NOT EXISTS `configvariable` (
  `Key` varchar(50) NOT NULL,
  `Value` varchar(50) NOT NULL,
  PRIMARY KEY (`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Export de données de la table codebreak_web.configvariable: ~0 rows (environ)
/*!40000 ALTER TABLE `configvariable` DISABLE KEYS */;
INSERT INTO `configvariable` (`Key`, `Value`) VALUES
	('community_forum_link', 'http://forum.earthscape.fr');
/*!40000 ALTER TABLE `configvariable` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
