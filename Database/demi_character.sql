/*
Navicat MySQL Data Transfer

Source Server         : Localhost
Source Server Version : 50719
Source Host           : localhost:3306
Source Database       : demi_character

Target Server Type    : MYSQL
Target Server Version : 50719
File Encoding         : 65001

Date: 2019-04-06 23:18:13
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for character_data
-- ----------------------------
DROP TABLE IF EXISTS `character_data`;
CREATE TABLE `character_data` (
  `user_id` int(11) NOT NULL,
  `character_id` varchar(36) NOT NULL,
  `name` varchar(12) DEFAULT NULL,
  `level` int(2) DEFAULT NULL,
  `class_id` int(2) DEFAULT NULL,
  `race_id` int(2) DEFAULT NULL,
  PRIMARY KEY (`user_id`,`character_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for character_location_data
-- ----------------------------
DROP TABLE IF EXISTS `character_location_data`;
CREATE TABLE `character_location_data` (
  `character_id` varchar(36) NOT NULL,
  `map_id` int(11) DEFAULT NULL,
  `cell_id` int(11) DEFAULT NULL,
  `x` float(255,1) DEFAULT NULL,
  `y` float(255,1) DEFAULT NULL,
  `direction` int(1) DEFAULT NULL,
  PRIMARY KEY (`character_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for character_spawns
-- ----------------------------
DROP TABLE IF EXISTS `character_spawns`;
CREATE TABLE `character_spawns` (
  `race_id` int(2) NOT NULL,
  `map_id` int(11) NOT NULL,
  PRIMARY KEY (`race_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
