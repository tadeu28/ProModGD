CREATE TABLE `projectfile` (
  `Id` varchar(40) NOT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `Description` LONGTEXT DEFAULT NULL,
  `FileName` varchar(255) DEFAULT NULL,
  `idProject` varchar(40) DEFAULT NULL,
  `DtPersistence` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idProject` (`idProject`),
  CONSTRAINT `FKC5A484D96BCBAFA8` FOREIGN KEY (`idProject`) REFERENCES `project` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

DELETE FROM `bpm2gp`.`version`;

INSERT INTO `bpm2gp`.`version` (`Id`, `VersionNumber`) VALUES ('026d741b-0e8f-4be2-b133-9438a7d02x21', '2');
