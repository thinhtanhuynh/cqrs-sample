SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for OrderItems
-- ----------------------------
DROP TABLE IF EXISTS `OrderItems`;
CREATE TABLE `OrderItems`  (
  `id` varchar(255),
  `productId` varchar(255),
  `productName` varchar(255),
  `quantity` int NOT NULL,
  `price` decimal(10, 2) NOT NULL,
  `orderId` varchar(255),
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `IDX_orderId_productId`(`orderId` ASC, `productId` ASC) USING BTREE,
  CONSTRAINT `ref_orderId` FOREIGN KEY (`orderId`) REFERENCES `Orders` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
);

-- ----------------------------
-- Table structure for Orders
-- ----------------------------
DROP TABLE IF EXISTS `Orders`;
CREATE TABLE `Orders`  (
  `id` varchar(255),
  `customerName` varchar(255),
  `total` int NOT NULL,
  `orderDate` datetime NOT NULL,
  `status` varchar(255),
  PRIMARY KEY (`id`) USING BTREE
);


SET FOREIGN_KEY_CHECKS = 1;