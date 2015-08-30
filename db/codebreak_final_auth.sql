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

-- Export de la structure de la base pour codebreak_auth
DROP DATABASE IF EXISTS `codebreak_auth`;
CREATE DATABASE IF NOT EXISTS `codebreak_auth` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `codebreak_auth`;


-- Export de la structure de table codebreak_auth. account
DROP TABLE IF EXISTS `account`;
CREATE TABLE IF NOT EXISTS `account` (
  `Id` bigint(20) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Pseudo` varchar(20) NOT NULL,
  `Password` varchar(20) NOT NULL,
  `Power` int(11) NOT NULL,
  `Vip` int(11) NOT NULL,
  `CreationDate` datetime NOT NULL,
  `LastConnectionDate` datetime NOT NULL,
  `LastConnectionIP` varchar(16) NOT NULL,
  `RemainingSubscription` datetime NOT NULL,
  `Banned` tinyint(1) NOT NULL,
  `Question` varchar(20) NOT NULL,
  `Response` varchar(20) NOT NULL,
  `Email` varchar(20) NOT NULL,
  `Votes` int(11) NOT NULL,
  `heurevote` int(20) NOT NULL,
  `Points` int(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;

-- Export de données de la table codebreak_auth.account: 13 rows
/*!40000 ALTER TABLE `account` DISABLE KEYS */;
INSERT INTO `account` (`Id`, `Name`, `Pseudo`, `Password`, `Power`, `Vip`, `CreationDate`, `LastConnectionDate`, `LastConnectionIP`, `RemainingSubscription`, `Banned`, `Question`, `Response`, `Email`, `Votes`, `heurevote`, `Points`) VALUES
	(2, 'Smarken', 'Smarken123', 'test', 10, 0, '2014-08-09 23:51:23', '2015-03-11 15:18:38', '127.0.0.1', '2014-08-09 23:51:23', 0, '', '', '', 0, 0, 0),
	(5, 'Test', 'test123', 'test', 10, 0, '2014-08-09 23:51:23', '2015-03-11 15:19:28', '127.0.0.1', '2014-08-09 23:51:23', 0, '', '', '', 0, 0, 0),
	(8, 'Test1', 'test12', 'test', 10, 0, '2014-08-09 23:51:23', '2015-03-11 12:25:12', '127.0.0.1', '2014-08-09 23:51:23', 0, '', '', '', 0, 0, 0),
	(10, 'test2', 'test13', 'test', 10, 0, '2014-08-09 23:51:23', '2014-08-09 23:51:23', '0.0.0.0', '2014-08-09 23:51:23', 0, '', '', '', 0, 0, 0),
	(12, 'test3', 'test3', 'test', 10, 0, '2014-08-09 23:51:23', '2014-08-09 23:51:23', '0.0.0.0', '2014-08-09 23:51:23', 0, '', '', '', 0, 0, 0),
	(13, 'test4', 'test4', 'test', 10, 0, '2014-08-09 23:51:23', '2014-08-09 23:51:23', '0.0.0.0', '2014-08-09 23:51:23', 0, '', '', '', 0, 0, 0),
	(14, 'test5', 'test5', 'test', 10, 0, '2014-08-09 23:51:23', '2014-08-09 23:51:23', '0.0.0.0', '2014-08-09 23:51:23', 0, '', '', '', 0, 0, 0),
	(15, 'test6', 'test6', 'test', 10, 0, '2014-08-09 23:51:23', '2014-08-09 23:51:23', '0.0.0.0', '2014-08-09 23:51:23', 0, '', '', '', 0, 0, 0),
	(16, 'florian', 'Florian', 'florian', 10, 0, '2014-08-09 23:51:23', '2014-08-09 23:51:23', '0.0.0.0', '2014-08-09 23:51:23', 0, 'A quoi servent les P', 'Dana', 'marieoo34@free.fr', 5, 1422058843, 750),
	(0, 'Smarkeen', 'Smarkoun', 'test123', 10, 0, '2015-03-03 09:19:58', '2015-03-03 09:19:58', '0.0.0.0', '2015-03-03 09:19:58', 0, '', '', 'sdf@sdf.sdf', 0, 0, 0),
	(17, 'Smarkounni', 'dqsfdfsdf', 'sdfsdf', 10, 0, '2015-03-03 09:26:42', '2015-03-03 09:26:42', '0.0.0.0', '2015-03-03 09:26:42', 0, 'sdfzerzerzer', 'sdfgdfgzer', 'sdf@sdf.sdf', 0, 0, 0),
	(18, 'Smarkou', 'Testaaar', 'test123', 10, 0, '2015-03-03 10:02:59', '2015-03-03 10:02:59', '0.0.0.0', '2015-03-03 10:02:59', 1, 'sdfsdfzerzr ea', 'gertlfpgldfg', 'hdd@dd.comm', 0, 0, 0),
	(19, 'Smarkinou', 'sdfsdfsdf', 'sdfsdfsdf', 10, 0, '2015-03-03 10:07:46', '2015-03-03 10:07:46', '0.0.0.0', '2015-03-03 10:07:46', 0, 'sdfzerzerfsdf', 'zerzerzersdfsd', 'sdf@sdf.sdf', 0, 0, 0);
/*!40000 ALTER TABLE `account` ENABLE KEYS */;


-- Export de la structure de table codebreak_auth. comments
DROP TABLE IF EXISTS `comments`;
CREATE TABLE IF NOT EXISTS `comments` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` text,
  `date` date DEFAULT NULL,
  `content` text,
  `news` int(11) DEFAULT NULL,
  `afficher` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=latin1;

-- Export de données de la table codebreak_auth.comments: ~17 rows (environ)
/*!40000 ALTER TABLE `comments` DISABLE KEYS */;
INSERT INTO `comments` (`id`, `account`, `date`, `content`, `news`, `afficher`) VALUES
	(1, '1', '2012-11-12', 'SAlut maggle', 1, 1),
	(2, '1', '2012-11-12', 'Yo deuxieme ggle', 2, 1),
	(3, '1', '2012-11-12', 'ON REGARDE SI CA FAIT BIEN 1 SUR 2 POUR LECHAGMEENT DE COULEUR ?', 1, 1),
	(32, '1', '2012-11-12', 'Je test les commentaires\r\n', 2, 1),
	(33, '1', '2015-01-10', 'zervzer', 2, 1),
	(34, '', '2015-01-20', 'Bonjour, bon courage pour la suite :)', 2, 1),
	(35, '', '2015-01-20', 'Test', 2, 1),
	(36, '', '2015-01-20', 'test', 2, 1),
	(37, '', '2015-01-20', 'test', 2, 1),
	(38, '', '2015-01-20', 'test', 2, 1),
	(39, '5', '2015-01-20', 'test', 2, 1),
	(40, '5', '2015-01-20', 'test', 2, 1),
	(41, '5', '2015-01-20', 'test', 2, 1),
	(42, '5', '2015-01-20', 'test', 2, 1),
	(43, '5', '2015-01-20', 'Bonjour !', 2, 1),
	(44, '5', '2015-01-20', 'test', 3, 1),
	(45, '16', '2015-01-20', 'Fuzizz prend la bite ', 3, 1);
/*!40000 ALTER TABLE `comments` ENABLE KEYS */;


-- Export de la structure de table codebreak_auth. news
DROP TABLE IF EXISTS `news`;
CREATE TABLE IF NOT EXISTS `news` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` text,
  `author` text,
  `date` date DEFAULT NULL,
  `content` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- Export de données de la table codebreak_auth.news: ~2 rows (environ)
/*!40000 ALTER TABLE `news` DISABLE KEYS */;
INSERT INTO `news` (`id`, `title`, `author`, `date`, `content`) VALUES
	(2, 'Revus de l\'experience', 'Scrub', '2014-08-10', 'Bonsoir à tous, <br /><br /><center><img src=\'http://img4.hostingpics.net/pics/14339317XP.jpg\'></center><br /> L\'experience des monstres ont été revus : Les boss de la Zone baslevel xp dersomais beaucoup plus qu\'avent afin d\'ameliorer l\'xp des bas level qui était difficile. La zone pour bas level possède maintennant plus de monstres ! Bon jeu'),
	(3, 'Ouverture de Earthscape !', 'Equipe Earthscape', '2015-01-20', 'Bonjour à tous ! </br></br>Aujourd\'hui, l\'équipe Earthscape est fière d\'ouvrir au grand public notre Serveur Anka\'like. Néanmoins, l\'équipe continue à travailler sur le serveur Fun qui ne devrait pas tarder également.</br></br>Nous vous remercions pour votre fidélité et espérons vous voir très bientôt en jeu. Pour toutes questions relatifs au serveur, mais de vous rendre sur le forum.</br></br>Cordialement, l\'équipe Earthscape.');
/*!40000 ALTER TABLE `news` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
