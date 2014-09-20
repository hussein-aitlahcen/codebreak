/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50527
Source Host           : localhost:3306
Source Database       : codebreak_auth

Target Server Type    : MYSQL
Target Server Version : 50527
File Encoding         : 65001

Date: 2014-09-20 13:59:49
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for account
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) NOT NULL,
  `Pseudo` varchar(20) NOT NULL,
  `Password` varchar(20) NOT NULL,
  `Power` int(11) NOT NULL,
  `CreationDate` datetime NOT NULL,
  `LastConnectionDate` datetime NOT NULL,
  `LastConnectionIP` varchar(16) NOT NULL,
  `RemainingSubscription` datetime NOT NULL,
  `Banned` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of account
-- ----------------------------
INSERT INTO `account` VALUES ('2', 'Smarken', 'Smarken123', '10151813', '1', '2014-08-09 23:51:23', '2014-08-09 23:51:23', '0.0.0.0', '2014-08-09 23:51:23', '0');
INSERT INTO `account` VALUES ('5', 'Test', 'test123', 'test', '0', '2014-08-09 00:00:00', '2014-08-09 00:00:00', '0.0.0.0', '2014-08-09 00:00:00', '0');
INSERT INTO `account` VALUES ('8', 'Test1', 'test123', 'test', '0', '2014-08-09 00:00:00', '2014-08-09 00:00:00', '0.0.0.0', '2014-08-09 00:00:00', '0');
INSERT INTO `account` VALUES ('10', 'test2', 'test123', 'test', '0', '2014-08-09 00:00:00', '2014-08-09 00:00:00', '0.0.0.0', '2014-08-09 00:00:00', '0');
INSERT INTO `account` VALUES ('12', 'test3', 'test123', 'test', '0', '2014-08-09 00:00:00', '2014-08-09 00:00:00', '0.0.0.0', '2014-08-09 00:00:00', '0');
INSERT INTO `account` VALUES ('13', 'test4', 'test123', 'test', '0', '2014-08-09 00:00:00', '2014-08-09 00:00:00', '0.0.0.0', '2014-08-09 00:00:00', '0');
INSERT INTO `account` VALUES ('14', 'test5', 'test123', 'test', '0', '2014-08-09 00:00:00', '2014-08-09 00:00:00', '0.0.0.0', '2014-08-09 00:00:00', '0');
INSERT INTO `account` VALUES ('15', 'test6', 'test123', 'test', '0', '2014-08-09 00:00:00', '2014-08-09 00:00:00', '0.0.0.0', '2014-08-09 00:00:00', '0');
