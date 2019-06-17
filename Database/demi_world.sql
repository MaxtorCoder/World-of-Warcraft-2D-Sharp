/*
Navicat MySQL Data Transfer

Source Server         : Localhost
Source Server Version : 50719
Source Host           : localhost:3306
Source Database       : demi_world

Target Server Type    : MYSQL
Target Server Version : 50719
File Encoding         : 65001

Date: 2019-06-17 19:31:04
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for creature
-- ----------------------------
DROP TABLE IF EXISTS `creature`;
CREATE TABLE `creature` (
  `id` int(5) NOT NULL,
  `model_id` int(5) NOT NULL,
  `name` char(128) DEFAULT NULL,
  `behaviours` json DEFAULT NULL,
  PRIMARY KEY (`id`,`model_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Table structure for creature_behaviour
-- ----------------------------
DROP TABLE IF EXISTS `creature_behaviour`;
CREATE TABLE `creature_behaviour` (
  `id` int(11) NOT NULL,
  `type` int(11) NOT NULL,
  `sub_type` int(11) DEFAULT NULL,
  `name` char(255) DEFAULT NULL,
  `settings` json DEFAULT NULL,
  PRIMARY KEY (`id`,`type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
