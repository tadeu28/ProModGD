-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: bpm2gp
-- ------------------------------------------------------
-- Server version	5.7.21-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `associationconf`
--

DROP TABLE IF EXISTS `associationconf`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `associationconf` (
  `Id` varchar(40) NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `DtCreation` datetime DEFAULT NULL,
  `idLanguage` varchar(40) DEFAULT NULL,
  `idGenre` varchar(40) DEFAULT NULL,
  `IsConstant` tinyint(1) DEFAULT NULL,
  `Inactive` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idLanguage` (`idLanguage`),
  KEY `idGenre` (`idGenre`),
  KEY `idLanguage_2` (`idLanguage`),
  KEY `idGenre_2` (`idGenre`),
  KEY `idLanguage_3` (`idLanguage`),
  KEY `idGenre_3` (`idGenre`),
  CONSTRAINT `FKB776CA795B2EAA8E` FOREIGN KEY (`idGenre`) REFERENCES `gamegenre` (`Id`),
  CONSTRAINT `FKB776CA79A7698927` FOREIGN KEY (`idLanguage`) REFERENCES `modelinglanguage` (`Id`),
  CONSTRAINT `FKE80744B51E209D62` FOREIGN KEY (`idLanguage`) REFERENCES `modelinglanguage` (`Id`),
  CONSTRAINT `FKE80744B58D3AFCEE` FOREIGN KEY (`idGenre`) REFERENCES `gamegenre` (`Id`),
  CONSTRAINT `FKE89E9FD51E209D62` FOREIGN KEY (`idLanguage`) REFERENCES `modelinglanguage` (`Id`),
  CONSTRAINT `FKE89E9FD58D3AFCEE` FOREIGN KEY (`idGenre`) REFERENCES `gamegenre` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `associationconf`
--

LOCK TABLES `associationconf` WRITE;
/*!40000 ALTER TABLE `associationconf` DISABLE KEYS */;
INSERT INTO `associationconf` VALUES ('4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f','Mapping Adventure Elements v.4','2017-12-26 10:11:49','b8b1b942-ac76-4752-abad-594aefdc463f','b8cd64e3-78da-41b1-b595-ab88050cc470',1,0);
/*!40000 ALTER TABLE `associationconf` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `associationconfelements`
--

DROP TABLE IF EXISTS `associationconfelements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `associationconfelements` (
  `Id` varchar(40) NOT NULL,
  `idLanguageElement` varchar(40) DEFAULT NULL,
  `idGenreElement` varchar(40) DEFAULT NULL,
  `idAssociation` varchar(40) DEFAULT NULL,
  `Inactive` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idLanguageElement` (`idLanguageElement`),
  KEY `idGenreElement` (`idGenreElement`),
  KEY `idAssociation` (`idAssociation`),
  KEY `idLanguageElement_2` (`idLanguageElement`),
  KEY `idGenreElement_2` (`idGenreElement`),
  KEY `idAssociation_2` (`idAssociation`),
  KEY `idLanguageElement_3` (`idLanguageElement`),
  KEY `idGenreElement_3` (`idGenreElement`),
  KEY `idAssociation_3` (`idAssociation`),
  CONSTRAINT `FK4BEC58E53B7BE5F1` FOREIGN KEY (`idAssociation`) REFERENCES `associationconf` (`Id`),
  CONSTRAINT `FK4BEC58E57F92B532` FOREIGN KEY (`idGenreElement`) REFERENCES `gamegenreelement` (`Id`),
  CONSTRAINT `FK4BEC58E58A35720A` FOREIGN KEY (`idLanguageElement`) REFERENCES `modelinglanguageelement` (`Id`),
  CONSTRAINT `FKA6629BFC3B7BE5F1` FOREIGN KEY (`idAssociation`) REFERENCES `associationconf` (`Id`),
  CONSTRAINT `FKA6629BFC7F92B532` FOREIGN KEY (`idGenreElement`) REFERENCES `gamegenreelement` (`Id`),
  CONSTRAINT `FKA6629BFC8A35720A` FOREIGN KEY (`idLanguageElement`) REFERENCES `modelinglanguageelement` (`Id`),
  CONSTRAINT `FKB8CDFC7A4505046B` FOREIGN KEY (`idLanguageElement`) REFERENCES `modelinglanguageelement` (`Id`),
  CONSTRAINT `FKB8CDFC7A678D322E` FOREIGN KEY (`idAssociation`) REFERENCES `associationconf` (`Id`),
  CONSTRAINT `FKB8CDFC7AF4619B9C` FOREIGN KEY (`idGenreElement`) REFERENCES `gamegenreelement` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `associationconfelements`
--

LOCK TABLES `associationconfelements` WRITE;
/*!40000 ALTER TABLE `associationconfelements` DISABLE KEYS */;
INSERT INTO `associationconfelements` VALUES ('00a4833b-15ee-4101-a7bc-c7b1a30cdce2','26345c60-b7be-4098-ab36-d5b075aad479','b3f9185d-81eb-4a82-ba6c-b385105d7e6d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('0281e8c1-2708-46ff-8fda-5b8a96aa3186','dcac9d99-0ad7-4a64-b1c2-97782c4ea189','b10def87-95b3-4846-9c05-8efafe36b225','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('0654c398-db9b-4c5e-b0c9-c192f4a77c72','b6f2efde-7e32-4614-b565-76d65cd9b781','07ecefe5-eacc-40e7-b973-d91639a6e939','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('1a568657-dacc-4399-8989-a01cb3087a69','af3af5ed-67f6-4ed9-b86b-e1c7861f091b','143d0fd1-15eb-44a9-a098-588c6ea90196','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('20209769-e52b-4ad1-8239-9dfac4f2f467','e733e91c-99a0-4eee-ab84-65f79b56b5e7','07ecefe5-eacc-40e7-b973-d91639a6e939','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('27abfc70-f0c6-49af-bedb-4da2b4c606bc','dc029987-fd24-489c-923a-31680c1569ec','07ecefe5-eacc-40e7-b973-d91639a6e939','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('2b6cf385-5644-425e-b09b-801ffa84fa20','aca11f49-5c32-47cc-93b2-8c815fbd5808','b3f9185d-81eb-4a82-ba6c-b385105d7e6d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('4765c509-9eee-403c-a897-754377f8df9c','731bfdaf-59d2-48bb-9a41-179a42f36176','56f309e0-8f8f-436a-a17f-bc0062758db6','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('4987ae0b-4f82-4395-aa78-57b2c9893b05','31eadc22-0fd8-45ba-b15a-fa822aaeda38','a2b2192a-69b5-44ee-9ca0-0606aed1acce','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('49cff4eb-0068-43be-a569-7af10491e475','66d7087e-764e-4e80-829b-c1522f96ae3c','a2b2192a-69b5-44ee-9ca0-0606aed1acce','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('54f1e7c4-5a27-4bd8-bb64-89cbf38ef33e','66d7087e-764e-4e80-829b-c1522f96ae3c','2047a756-2f40-4c4c-8a0c-722509cae42f','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('5f1937b4-5c2c-457c-b661-e5de6ecbddf7','45a8a364-d28d-4eab-8b65-4bf28f5bcf68','aafd3f32-8209-4ea6-90db-41cc2294e50f','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('67b610bd-4d00-442e-828d-304008be22fc','65e75f2d-86ec-47ad-862f-80e6f568d60f','b3f9185d-81eb-4a82-ba6c-b385105d7e6d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('6a1837b8-99d5-4795-90a1-2ac252955632','dcac9d99-0ad7-4a64-b1c2-97782c4ea189','ddeabd7a-2e66-4cf0-94b1-b10697094e9b','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('70766c1f-ffc4-40f5-966d-a19447ce0701','10a1ab6b-3184-4d24-b55a-bcde30861ead','aafd3f32-8209-4ea6-90db-41cc2294e50f','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('71ec28fa-5959-4d4b-bbf6-552597da3342','a6e2da4c-8b05-49c8-8406-467f4a47906b','f1c5fae6-f23c-4f4c-a77a-299fb4dcd21b','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('765c2918-df9f-4dc6-8174-be954c5dee52','af3af5ed-67f6-4ed9-b86b-e1c7861f091b','9d297fff-deaa-4497-a8cc-ad056a383e6d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('80c9886f-35d5-4304-9a4f-0b257faf2e1a','8fb31f6b-3955-447e-a6a6-657d26f0e78e','bd655881-0d4b-460b-9fbd-ea577d3aa417','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('8b63e0ca-e5b5-4bbe-8dce-b3bf4597279b','26345c60-b7be-4098-ab36-d5b075aad479','0398bdf4-bc79-42d6-8109-017b826b4b39','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('9d55bc14-097c-40b5-b9b5-b59f3888e573','992c92d5-1e08-4a00-ab4d-59856c83647a','b3f9185d-81eb-4a82-ba6c-b385105d7e6d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('ad054701-fb8a-4c9c-850c-f2583f9df4a7','a6e2da4c-8b05-49c8-8406-467f4a47906b','9d297fff-deaa-4497-a8cc-ad056a383e6d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('adbb6f7b-be4a-4e7e-921b-bbfd0a29b3fd','731bfdaf-59d2-48bb-9a41-179a42f36176','4a4d03ad-ae27-4f49-8289-260c62b1cd8d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('b238ccd6-18b1-4fec-b42f-4093171fe5fb','a6e2da4c-8b05-49c8-8406-467f4a47906b','fcb1c5b1-ab39-4178-b905-0efe19f3c6d5','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('b6b8e23b-719f-432e-8bfa-114684401398','731bfdaf-59d2-48bb-9a41-179a42f36176','b3f9185d-81eb-4a82-ba6c-b385105d7e6d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('b93ad02e-ebc4-4419-87d5-43a40e45c107','e9524f1e-ea42-4a61-9cdc-84605e673607','9d297fff-deaa-4497-a8cc-ad056a383e6d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('c166912a-b093-4604-a2cf-3c503438c95b','8a41aa9e-c368-4101-988c-e06b7dc669b3','07ecefe5-eacc-40e7-b973-d91639a6e939','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('c288de5b-6e3c-40c0-8da4-27eaa9fb0ebf','9ebe6f66-8f94-42b5-ab97-8b8b71201395','b3f9185d-81eb-4a82-ba6c-b385105d7e6d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('caeef544-8b9e-46b9-b9ad-6882afba4767','8fb31f6b-3955-447e-a6a6-657d26f0e78e','fd7e51ff-da9f-4eb5-bbba-24186283199b','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('cc680cc5-b0a3-436a-9868-f2488549a2ab','cce12713-1de1-4927-877c-030394691ea1','b3f9185d-81eb-4a82-ba6c-b385105d7e6d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('d4119eb6-7e4d-4cf8-8ad9-7d6c61d9598f','7780c25a-0962-4049-947d-042ab671fdcf','f20f75aa-0764-4b67-9171-e7d6ac6c9b57','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('dd905def-db9d-46f0-87b0-8561d06622ca','14ed9e30-b664-4583-93d9-3473106335fd','fdd6f089-28ff-4beb-8c97-bc7372e4566b','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('eba21687-60a7-4b3f-a5fc-68b51ea25729','e9524f1e-ea42-4a61-9cdc-84605e673607','143d0fd1-15eb-44a9-a098-588c6ea90196','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('ebecebda-039a-467f-be2c-afa9020006be','b1d8112b-48bb-4460-a043-59c182e65139','80dfa79c-b385-43f1-a2a4-6d8039b12b69','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('f4b0a66a-e026-4bc4-9a70-143ab210529c','046b189c-75b1-4f09-9d29-df91b8e2ca93','b3f9185d-81eb-4a82-ba6c-b385105d7e6d','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('f86a1712-0d78-4482-be1b-69820e57ab06','e9524f1e-ea42-4a61-9cdc-84605e673607','1e3a7065-d94a-4fc7-a608-1842f313c5ec','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('fa5c314e-c222-4609-b2b2-84124d546379','26345c60-b7be-4098-ab36-d5b075aad479','56f309e0-8f8f-436a-a17f-bc0062758db6','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL),('fd07729e-b962-4af6-a05e-55940700d1a7','a6e2da4c-8b05-49c8-8406-467f4a47906b','d7cb5f17-22cb-469b-b7ea-1ec019bd4a26','4c4117ca-b9c9-4667-8bcf-57f69bfd2f7f',NULL);
/*!40000 ALTER TABLE `associationconfelements` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `associationrules`
--

DROP TABLE IF EXISTS `associationrules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `associationrules` (
  `Id` varchar(40) NOT NULL,
  `idType` int(11) DEFAULT NULL,
  `Rule` varchar(255) DEFAULT NULL,
  `idAssocElement` varchar(40) DEFAULT NULL,
  `Operator` int(11) DEFAULT NULL,
  `Field` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idType` (`idType`),
  KEY `idAssocElement` (`idAssocElement`),
  KEY `idType_2` (`idType`),
  KEY `idAssocElement_2` (`idAssocElement`),
  KEY `idType_3` (`idType`),
  KEY `idAssocElement_3` (`idAssocElement`),
  CONSTRAINT `FK591C50C08B87DC31` FOREIGN KEY (`idType`) REFERENCES `associationtype` (`Id`),
  CONSTRAINT `FK591C50C0998A8DF3` FOREIGN KEY (`idAssocElement`) REFERENCES `associationconfelements` (`Id`),
  CONSTRAINT `FKA2B2E6CE34A4014D` FOREIGN KEY (`idType`) REFERENCES `associationtype` (`Id`),
  CONSTRAINT `FKA2B2E6CE9A54524D` FOREIGN KEY (`idAssocElement`) REFERENCES `associationconfelements` (`Id`),
  CONSTRAINT `FKCA0641EE34A4014D` FOREIGN KEY (`idType`) REFERENCES `associationtype` (`Id`),
  CONSTRAINT `FKCA0641EE9A54524D` FOREIGN KEY (`idAssocElement`) REFERENCES `associationconfelements` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `associationrules`
--

LOCK TABLES `associationrules` WRITE;
/*!40000 ALTER TABLE `associationrules` DISABLE KEYS */;
INSERT INTO `associationrules` VALUES ('074da41c-6a18-4ae5-9bc0-7f7a4556a51a',3,'*character*','eba21687-60a7-4b3f-a5fc-68b51ea25729',12,'documentation'),('0a7a4d44-480f-4471-9a0b-b0e05968d2cf',3,'*character*','765c2918-df9f-4dc6-8174-be954c5dee52',4,'documentation'),('0e717ae8-b59b-414c-9ab3-421b30aaa506',4,'','dd905def-db9d-46f0-87b0-8561d06622ca',0,NULL),('11e7384c-808e-4fb3-9db5-5781ad3a5449',4,'','ad054701-fb8a-4c9c-850c-f2583f9df4a7',0,NULL),('3334e86e-512c-46b6-a437-b5bf73c34dd5',4,'','b238ccd6-18b1-4fec-b42f-4093171fe5fb',0,NULL),('41c802ca-8dff-46dc-b6ab-fbec00a55443',4,'','54f1e7c4-5a27-4bd8-bb64-89cbf38ef33e',0,NULL),('4526d728-1d1f-42b7-8a96-1d9b5c7f6cbd',4,'','00a4833b-15ee-4101-a7bc-c7b1a30cdce2',0,''),('4cbcecb9-4345-4355-a09b-16813205147d',3,'*player*','eba21687-60a7-4b3f-a5fc-68b51ea25729',12,'documentation'),('5febda34-5071-40fd-a01e-66b61a36cce2',4,'','4765c509-9eee-403c-a897-754377f8df9c',0,''),('672973ee-0b24-4e11-b8f3-f3f451725bcf',1,'','caeef544-8b9e-46b9-b9ad-6882afba4767',7,'processRef'),('69d21eb0-10c7-4979-9a3a-1c60919299ae',1,'','765c2918-df9f-4dc6-8174-be954c5dee52',2,'processRef'),('6c67bf7c-cce2-4b85-bc25-438c284e70db',1,'','1a568657-dacc-4399-8989-a01cb3087a69',2,'processRef'),('70e65e88-7331-442a-b304-75e6da532b5c',5,'','2b6cf385-5644-425e-b09b-801ffa84fa20',11,''),('74781198-0aa5-44c9-8786-851bca131056',4,'','fd07729e-b962-4af6-a05e-55940700d1a7',0,NULL),('8adbf29c-331c-4ba5-8853-2ba82dcc621b',4,'','ebecebda-039a-467f-be2c-afa9020006be',0,NULL),('8eb705d7-2306-4fe2-8a7f-15fd5de29499',3,'','6a1837b8-99d5-4795-90a1-2ac252955632',1,'errorEventDefinition'),('948ed682-327b-49b4-a554-9a623f4fd5f1',4,'','fa5c314e-c222-4609-b2b2-84124d546379',0,''),('a1941de4-af8a-4f5a-9c16-6c58c0de5007',4,'','71ec28fa-5959-4d4b-bbf6-552597da3342',0,NULL),('a32cf64a-db0f-4772-8322-459739fb0176',3,'*player*','f86a1712-0d78-4482-be1b-69820e57ab06',4,'documentation'),('af211e64-c396-4ce9-9940-eb1ecd216ec2',6,'','80c9886f-35d5-4304-9a4f-0b257faf2e1a',0,'bpmn:process'),('c5fb1898-43f2-4cec-ad98-b2fda1b76d94',4,'','b6b8e23b-719f-432e-8bfa-114684401398',0,''),('cdd50d90-4f16-4305-9d2a-b478a1f6398c',3,'','6a1837b8-99d5-4795-90a1-2ac252955632',7,'errorEventDefinition'),('d51a49d2-e48f-439e-b5b4-639be9e7db8c',1,'','caeef544-8b9e-46b9-b9ad-6882afba4767',1,'processRef'),('d6928c48-c248-42d5-9a80-e24c5f1c7f79',3,'*character*','1a568657-dacc-4399-8989-a01cb3087a69',12,'documentation'),('df09f9bb-e54f-4c81-a98b-0a76fa605d56',3,'*character*','b93ad02e-ebc4-4419-87d5-43a40e45c107',4,'documentation'),('fbe92867-0830-4ac5-925a-c3b2e8c519c4',4,'','f4b0a66a-e026-4bc4-9a70-143ab210529c',0,NULL),('fe4ea616-1c87-435d-945e-30ecbb3fba06',1,'xxx','4c19ba21-8e1d-4c8f-b5ce-d0de7c2e4e96',3,'xxxxxxxx');
/*!40000 ALTER TABLE `associationrules` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `associationtype`
--

DROP TABLE IF EXISTS `associationtype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `associationtype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `associationtype`
--

LOCK TABLES `associationtype` WRITE;
/*!40000 ALTER TABLE `associationtype` DISABLE KEYS */;
INSERT INTO `associationtype` VALUES (1,'Element Attribute'),(3,'Model Element'),(4,'Not Explicit Only in the Model'),(5,'Model Activitie'),(6,'Process GameFlow From Some Process Element');
/*!40000 ALTER TABLE `associationtype` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `designer`
--

DROP TABLE IF EXISTS `designer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `designer` (
  `Id` varchar(40) NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `ShortBio` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `designer`
--

--
-- Table structure for table `designers`
--

DROP TABLE IF EXISTS `designers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `designers` (
  `idProject` varchar(40) NOT NULL,
  `idDesigner` varchar(40) NOT NULL,
  KEY `idDesigner` (`idDesigner`),
  KEY `idProject` (`idProject`),
  KEY `idDesigner_2` (`idDesigner`),
  KEY `idProject_2` (`idProject`),
  CONSTRAINT `FKBA3B980C27192DC4` FOREIGN KEY (`idProject`) REFERENCES `project` (`Id`),
  CONSTRAINT `FKBA3B980C66E920A6` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FKFA7685E3638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FKFA7685E36BCBAFA8` FOREIGN KEY (`idProject`) REFERENCES `project` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `designers`
--

--
-- Table structure for table `designmapping`
--

DROP TABLE IF EXISTS `designmapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `designmapping` (
  `Id` varchar(40) NOT NULL,
  `IdProject` varchar(40) DEFAULT NULL,
  `IdAssociationConf` varchar(40) DEFAULT NULL,
  `CreationDate` datetime DEFAULT NULL,
  `ModelScore` decimal(19,5) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IdProject` (`IdProject`),
  KEY `IdAssociationConf` (`IdAssociationConf`),
  KEY `IdProject_2` (`IdProject`),
  KEY `IdAssociationConf_2` (`IdAssociationConf`),
  KEY `IdProject_3` (`IdProject`),
  KEY `IdAssociationConf_3` (`IdAssociationConf`),
  CONSTRAINT `FK4273E3FE27192DC4` FOREIGN KEY (`IdProject`) REFERENCES `project` (`Id`),
  CONSTRAINT `FK4273E3FEF1C78DB4` FOREIGN KEY (`IdAssociationConf`) REFERENCES `associationconf` (`Id`),
  CONSTRAINT `FK8FC336966BCBAFA8` FOREIGN KEY (`IdProject`) REFERENCES `project` (`Id`),
  CONSTRAINT `FK8FC33696F825D39D` FOREIGN KEY (`IdAssociationConf`) REFERENCES `associationconf` (`Id`),
  CONSTRAINT `FK9F5A3F166BCBAFA8` FOREIGN KEY (`IdProject`) REFERENCES `project` (`Id`),
  CONSTRAINT `FK9F5A3F16F825D39D` FOREIGN KEY (`IdAssociationConf`) REFERENCES `associationconf` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `designmapping`
--
--
-- Table structure for table `designmappingerrors`
--

DROP TABLE IF EXISTS `designmappingerrors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `designmappingerrors` (
  `Id` varchar(40) NOT NULL,
  `Error` longblob,
  `idDesignMapping` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idDesignMapping` (`idDesignMapping`),
  KEY `idDesignMapping_2` (`idDesignMapping`),
  KEY `idDesignMapping_3` (`idDesignMapping`),
  CONSTRAINT `FK29A701F9506DFC06` FOREIGN KEY (`idDesignMapping`) REFERENCES `designmapping` (`Id`),
  CONSTRAINT `FK65B9C8BD4B4376E9` FOREIGN KEY (`idDesignMapping`) REFERENCES `designmapping` (`Id`),
  CONSTRAINT `FKFFE37C7D4B4376E9` FOREIGN KEY (`idDesignMapping`) REFERENCES `designmapping` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `designmappingerrors`
--

--
-- Table structure for table `designmappingscores`
--

DROP TABLE IF EXISTS `designmappingscores`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `designmappingscores` (
  `Id` varchar(40) NOT NULL,
  `GameGenreElement` varchar(255) DEFAULT NULL,
  `ModelElement` varchar(255) DEFAULT NULL,
  `ExpectedElements` decimal(19,5) DEFAULT NULL,
  `MappedElements` decimal(19,5) DEFAULT NULL,
  `idDesignMapping` varchar(40) DEFAULT NULL,
  `GameGenreElementId` varchar(40) DEFAULT NULL,
  `ModelElementId` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idDesignMapping` (`idDesignMapping`),
  KEY `idDesignMapping_2` (`idDesignMapping`),
  KEY `idDesignMapping_3` (`idDesignMapping`),
  CONSTRAINT `FK2899BD0B4B4376E9` FOREIGN KEY (`idDesignMapping`) REFERENCES `designmapping` (`Id`),
  CONSTRAINT `FKB5AB70CB4B4376E9` FOREIGN KEY (`idDesignMapping`) REFERENCES `designmapping` (`Id`),
  CONSTRAINT `FKDE5D3D29506DFC06` FOREIGN KEY (`idDesignMapping`) REFERENCES `designmapping` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `designmappingscores`
--

--
-- Table structure for table `gamedesignmappingelements`
--

DROP TABLE IF EXISTS `gamedesignmappingelements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gamedesignmappingelements` (
  `Id` varchar(40) NOT NULL,
  `Descricao` longtext,
  `idDesignMapping` varchar(40) DEFAULT NULL,
  `idAssocElement` varchar(40) DEFAULT NULL,
  `idGenreElement` varchar(40) DEFAULT NULL,
  `IsManual` tinyint(1) DEFAULT NULL,
  `ModelElementId` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idDesignMapping` (`idDesignMapping`),
  KEY `idAssocElement` (`idAssocElement`),
  KEY `idGenreElement` (`idGenreElement`),
  KEY `idDesignMapping_2` (`idDesignMapping`),
  KEY `idAssocElement_2` (`idAssocElement`),
  KEY `idGenreElement_2` (`idGenreElement`),
  KEY `idDesignMapping_3` (`idDesignMapping`),
  KEY `idAssocElement_3` (`idAssocElement`),
  KEY `idGenreElement_3` (`idGenreElement`),
  CONSTRAINT `FK6D24446D4B4376E9` FOREIGN KEY (`idDesignMapping`) REFERENCES `designmapping` (`Id`),
  CONSTRAINT `FK6D24446D7F92B532` FOREIGN KEY (`idGenreElement`) REFERENCES `gamegenreelement` (`Id`),
  CONSTRAINT `FK6D24446D9A54524D` FOREIGN KEY (`idAssocElement`) REFERENCES `associationconfelements` (`Id`),
  CONSTRAINT `FK77E5C1D64B4376E9` FOREIGN KEY (`idDesignMapping`) REFERENCES `designmapping` (`Id`),
  CONSTRAINT `FK77E5C1D67F92B532` FOREIGN KEY (`idGenreElement`) REFERENCES `gamegenreelement` (`Id`),
  CONSTRAINT `FK77E5C1D69A54524D` FOREIGN KEY (`idAssocElement`) REFERENCES `associationconfelements` (`Id`),
  CONSTRAINT `FK87F9F871506DFC06` FOREIGN KEY (`idDesignMapping`) REFERENCES `designmapping` (`Id`),
  CONSTRAINT `FK87F9F871998A8DF3` FOREIGN KEY (`idAssocElement`) REFERENCES `associationconfelements` (`Id`),
  CONSTRAINT `FK87F9F871F4619B9C` FOREIGN KEY (`idGenreElement`) REFERENCES `gamegenreelement` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gamedesignmappingelements`
--

--
-- Table structure for table `gamegenre`
--

DROP TABLE IF EXISTS `gamegenre`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gamegenre` (
  `Id` varchar(40) NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `RegisterDate` datetime DEFAULT NULL,
  `IsConstant` tinyint(1) DEFAULT NULL,
  `idDesigner` varchar(40) DEFAULT NULL,
  `Inactive` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idDesigner` (`idDesigner`),
  KEY `idDesigner_2` (`idDesigner`),
  KEY `idDesigner_3` (`idDesigner`),
  CONSTRAINT `FK2AAE7E1166E920A6` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FK77B9D801638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FK77BA5441638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gamegenre`
--

LOCK TABLES `gamegenre` WRITE;
/*!40000 ALTER TABLE `gamegenre` DISABLE KEYS */;
INSERT INTO `gamegenre` VALUES ('b8cd64e3-78da-41b1-b595-ab88050cc470','Adventure Game','Game adventure\'s game from Zahari et al. paper.','2017-12-26 08:50:34',1, null,0);
/*!40000 ALTER TABLE `gamegenre` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gamegenreelement`
--

DROP TABLE IF EXISTS `gamegenreelement`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gamegenreelement` (
  `Id` varchar(40) NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `idGenre` varchar(40) DEFAULT NULL,
  `RuleAbbled` tinyint(1) DEFAULT NULL,
  `Inactive` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idGenre` (`idGenre`),
  KEY `idGenre_2` (`idGenre`),
  KEY `idGenre_3` (`idGenre`),
  CONSTRAINT `FK518FD6B98D3AFCEE` FOREIGN KEY (`idGenre`) REFERENCES `gamegenre` (`Id`),
  CONSTRAINT `FK555FD2F98D3AFCEE` FOREIGN KEY (`idGenre`) REFERENCES `gamegenre` (`Id`),
  CONSTRAINT `FK5B59D7EF5B2EAA8E` FOREIGN KEY (`idGenre`) REFERENCES `gamegenre` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gamegenreelement`
--

LOCK TABLES `gamegenreelement` WRITE;
/*!40000 ALTER TABLE `gamegenreelement` DISABLE KEYS */;
INSERT INTO `gamegenreelement` VALUES ('0398bdf4-bc79-42d6-8109-017b826b4b39','Tasks','Tasks to be perfomed by the player.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('07ecefe5-eacc-40e7-b973-d91639a6e939','Interactions','Interactions among characters, players, objects and etc.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('143d0fd1-15eb-44a9-a098-588c6ea90196','Localization','Places in the game world.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('1e3a7065-d94a-4fc7-a608-1842f313c5ec','Player or Gamer','Person that play the game.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('2047a756-2f40-4c4c-8a0c-722509cae42f','Problems','Happening that motivate the gama. Generally is associated with game goal.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('4a4d03ad-ae27-4f49-8289-260c62b1cd8d','Quests','Sequence of task to reach a goal.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('56f309e0-8f8f-436a-a17f-bc0062758db6','Feedback','Responses after perform something.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('80dfa79c-b385-43f1-a2a4-6d8039b12b69','Goals','Game objectives.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('9d297fff-deaa-4497-a8cc-ad056a383e6d','Characters','People (or animated objects) represented in the games.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('a2b2192a-69b5-44ee-9ca0-0606aed1acce','Happenings','Plot events that happening and change the game flow.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('aafd3f32-8209-4ea6-90db-41cc2294e50f','Items and Objects','Resources acquired in the game','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('b10def87-95b3-4846-9c05-8efafe36b225','Solutions','Solutions of how to solve problems in the game','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('b3f9185d-81eb-4a82-ba6c-b385105d7e6d','Rules','Rules of interaction of the game. For instance: players with objects, bosses, and others.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('bd655881-0d4b-460b-9fbd-ea577d3aa417','Gameflow ','Sequence of happenings in the game.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('d7cb5f17-22cb-469b-b7ea-1ec019bd4a26','Theme','Main theme of the game.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('ddeabd7a-2e66-4cf0-94b1-b10697094e9b','Fails','Fails situations in the game.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('f1c5fae6-f23c-4f4c-a77a-299fb4dcd21b','Story','Game Story is a abstract of the happenings in the game','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('f20f75aa-0764-4b67-9171-e7d6ac6c9b57','Help and Orientation','Som game tips.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('fcb1c5b1-ab39-4178-b905-0efe19f3c6d5','Plot','It is a full storytelling of happenings in the game.','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('fd7e51ff-da9f-4eb5-bbba-24186283199b','Title','Name of the game','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0),('fdd6f089-28ff-4beb-8c97-bc7372e4566b','Game World','World and places where the game happens','b8cd64e3-78da-41b1-b595-ab88050cc470',NULL,0);
/*!40000 ALTER TABLE `gamegenreelement` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gamegenreelements`
--

DROP TABLE IF EXISTS `gamegenreelements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gamegenreelements` (
  `idGddElement` varchar(40) NOT NULL,
  `idGameGenre` varchar(40) NOT NULL,
  KEY `idGameGenre` (`idGameGenre`),
  KEY `idGddElement` (`idGddElement`),
  KEY `idGameGenre_2` (`idGameGenre`),
  KEY `idGddElement_2` (`idGddElement`),
  CONSTRAINT `FK2D33BBC87B1CE2DD` FOREIGN KEY (`idGameGenre`) REFERENCES `gamegenreelement` (`Id`),
  CONSTRAINT `FK2D33BBC8D5642D33` FOREIGN KEY (`idGddElement`) REFERENCES `gddconfigurationelements` (`Id`),
  CONSTRAINT `FK637D9E21D51AE6A` FOREIGN KEY (`idGameGenre`) REFERENCES `gamegenreelement` (`Id`),
  CONSTRAINT `FK637D9E2DE2D6E5D` FOREIGN KEY (`idGddElement`) REFERENCES `gddconfigurationelements` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gamegenreelements`
--

LOCK TABLES `gamegenreelements` WRITE;
/*!40000 ALTER TABLE `gamegenreelements` DISABLE KEYS */;
INSERT INTO `gamegenreelements` VALUES ('612e58e3-55e8-4793-be2e-b9d56c7041d3','ddeabd7a-2e66-4cf0-94b1-b10697094e9b'),('f2b2214b-b0ed-4484-a40d-d036b3ea1606','fd7e51ff-da9f-4eb5-bbba-24186283199b'),('aeebed44-a0a0-4927-b263-fd8328940b51','d7cb5f17-22cb-469b-b7ea-1ec019bd4a26'),('a19fa032-6ee9-47d7-ad77-b2e99bfd014b','f1c5fae6-f23c-4f4c-a77a-299fb4dcd21b'),('865ce221-733d-48a2-8760-b9c6bd1dc905','fcb1c5b1-ab39-4178-b905-0efe19f3c6d5'),('a6387f05-de07-4b04-b8cd-376f47e1b0b8','2047a756-2f40-4c4c-8a0c-722509cae42f'),('c2f1d687-497c-4ae0-9177-5f8f07766179','80dfa79c-b385-43f1-a2a4-6d8039b12b69'),('7ce9dada-9b8e-4fa8-a148-ace2d22c1a1c','0398bdf4-bc79-42d6-8109-017b826b4b39'),('7ce9dada-9b8e-4fa8-a148-ace2d22c1a1c','4a4d03ad-ae27-4f49-8289-260c62b1cd8d'),('80f66cd0-ed4d-4a3d-b398-d96b930eba85','2047a756-2f40-4c4c-8a0c-722509cae42f'),('80f66cd0-ed4d-4a3d-b398-d96b930eba85','a2b2192a-69b5-44ee-9ca0-0606aed1acce'),('80f66cd0-ed4d-4a3d-b398-d96b930eba85','b10def87-95b3-4846-9c05-8efafe36b225'),('80f66cd0-ed4d-4a3d-b398-d96b930eba85','ddeabd7a-2e66-4cf0-94b1-b10697094e9b'),('3996c3e0-94f0-47f3-ba96-ae8ab6eaf7b4','b10def87-95b3-4846-9c05-8efafe36b225'),('8431e299-d375-4c46-b577-31c680f82e96','ddeabd7a-2e66-4cf0-94b1-b10697094e9b'),('c2fa8814-e10d-4672-9cdc-e04ec7804433','9d297fff-deaa-4497-a8cc-ad056a383e6d'),('707d3266-ef95-4caa-841b-77b0d6175176','1e3a7065-d94a-4fc7-a608-1842f313c5ec'),('9d2de9aa-9add-4796-9f8e-cd8150df890d','9d297fff-deaa-4497-a8cc-ad056a383e6d'),('8b3faa72-e27b-4445-975e-a6224be06efc','aafd3f32-8209-4ea6-90db-41cc2294e50f'),('43ad4c55-aab4-43d3-8116-101c99fd6f0b','fdd6f089-28ff-4beb-8c97-bc7372e4566b'),('240c9250-2fe0-422c-b747-d44fd034bff0','143d0fd1-15eb-44a9-a098-588c6ea90196'),('54d4e5a4-c9f2-4e4e-b451-323edf2529d4','b3f9185d-81eb-4a82-ba6c-b385105d7e6d'),('88075902-715c-4af1-849f-47e8aedbb166','56f309e0-8f8f-436a-a17f-bc0062758db6'),('c74f6f42-ee3f-4fc9-a800-83398d8a7f20','bd655881-0d4b-460b-9fbd-ea577d3aa417'),('dcf380ed-95a0-48f8-b7da-255673647c7c','56f309e0-8f8f-436a-a17f-bc0062758db6'),('5b947e10-9ba6-4350-a776-4082c8d9ecd6','56f309e0-8f8f-436a-a17f-bc0062758db6'),('5b947e10-9ba6-4350-a776-4082c8d9ecd6','aafd3f32-8209-4ea6-90db-41cc2294e50f'),('599c36bd-792b-4d52-8f6b-21542d29b3c6','07ecefe5-eacc-40e7-b973-d91639a6e939');
/*!40000 ALTER TABLE `gamegenreelements` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gddconfiguration`
--

DROP TABLE IF EXISTS `gddconfiguration`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gddconfiguration` (
  `Id` varchar(40) NOT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `RegistrationDate` datetime DEFAULT NULL,
  `IsConstant` tinyint(1) DEFAULT NULL,
  `Inactive` tinyint(1) DEFAULT NULL,
  `idDesigner` varchar(40) DEFAULT NULL,
  `idGameGenre` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idDesigner` (`idDesigner`),
  KEY `idGameGenre` (`idGameGenre`),
  KEY `idDesigner_2` (`idDesigner`),
  KEY `idGameGenre_2` (`idGameGenre`),
  KEY `idDesigner_3` (`idDesigner`),
  KEY `idGameGenre_3` (`idGameGenre`),
  CONSTRAINT `FK9A34138F404F8A44` FOREIGN KEY (`idGameGenre`) REFERENCES `gamegenre` (`Id`),
  CONSTRAINT `FK9A34138F66E920A6` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FKDD576AD3638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FKDD576AD3DF369F58` FOREIGN KEY (`idGameGenre`) REFERENCES `gamegenre` (`Id`),
  CONSTRAINT `FKF8B7F89E638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FKF8B7F89EDF369F58` FOREIGN KEY (`idGameGenre`) REFERENCES `gamegenre` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gddconfiguration`
--

LOCK TABLES `gddconfiguration` WRITE;
/*!40000 ALTER TABLE `gddconfiguration` DISABLE KEYS */;
INSERT INTO `gddconfiguration` VALUES ('4abdc045-a2ef-424e-8ba1-373e5a32bde6','asasas','2018-04-13 16:37:01',0,1,null,'b8cd64e3-78da-41b1-b595-ab88050cc470'),('b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','Default Adventure GDD','2018-02-21 14:57:45',1,0,null,'b8cd64e3-78da-41b1-b595-ab88050cc470'),('ff68d227-ace0-45c1-a179-79dd7ef6948b','Teste','2018-02-21 15:14:13',0,1,null,'b8cd64e3-78da-41b1-b595-ab88050cc470');
/*!40000 ALTER TABLE `gddconfiguration` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gddconfigurationelements`
--

DROP TABLE IF EXISTS `gddconfigurationelements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gddconfigurationelements` (
  `Id` varchar(40) NOT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `Description` longtext,
  `idGddConfig` varchar(40) DEFAULT NULL,
  `idGddElement` varchar(40) DEFAULT NULL,
  `PresentationOrder` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idGddConfig` (`idGddConfig`),
  KEY `idGddElement` (`idGddElement`),
  KEY `idGddConfig_2` (`idGddConfig`),
  KEY `idGddElement_2` (`idGddElement`),
  KEY `idGddConfig_3` (`idGddConfig`),
  KEY `idGddElement_3` (`idGddElement`),
  CONSTRAINT `FKCF31B28414930EE8` FOREIGN KEY (`idGddConfig`) REFERENCES `gddconfiguration` (`Id`),
  CONSTRAINT `FKCF31B284D5642D33` FOREIGN KEY (`idGddElement`) REFERENCES `gddconfigurationelements` (`Id`),
  CONSTRAINT `FKE196894274F37DF8` FOREIGN KEY (`idGddConfig`) REFERENCES `gddconfiguration` (`Id`),
  CONSTRAINT `FKE1968942DE2D6E5D` FOREIGN KEY (`idGddElement`) REFERENCES `gddconfigurationelements` (`Id`),
  CONSTRAINT `FKEE07190714930EE8` FOREIGN KEY (`idGddConfig`) REFERENCES `gddconfiguration` (`Id`),
  CONSTRAINT `FKEE071907D5642D33` FOREIGN KEY (`idGddElement`) REFERENCES `gddconfigurationelements` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gddconfigurationelements`
--

LOCK TABLES `gddconfigurationelements` WRITE;
/*!40000 ALTER TABLE `gddconfigurationelements` DISABLE KEYS */;
INSERT INTO `gddconfigurationelements` VALUES ('240c9250-2fe0-422c-b747-d44fd034bff0','Scenarios (Places)',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','f3b687a9-2a11-430e-824f-f40622c9de47',31),('34c0e84a-d732-4475-9147-a48d8fbc84a3','Gameplay, Story and Concepts','That page should include some paragraphs about the story, telling the environment, characters, goals and problems. The gameplay must describe a brief idea of the gameflow.','b51429d5-0fa9-44ff-bc5e-e301bc35a4ca',NULL,7),('3996c3e0-94f0-47f3-ba96-ae8ab6eaf7b4','Success - Game Overs',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','b9cefcc6-5686-4fa9-8b00-1803bdecff90',17),('43ad4c55-aab4-43d3-8116-101c99fd6f0b','Environment','That item is the description of the game environment, the world, time, decades, etc.','b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','f3b687a9-2a11-430e-824f-f40622c9de47',30),('54d4e5a4-c9f2-4e4e-b451-323edf2529d4','Game Rules',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','68c54333-d620-4350-8744-75a99df26d65',34),('599c36bd-792b-4d52-8f6b-21542d29b3c6','Game Interactions',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','c978907e-1b1e-4d27-abbd-46c79d17b684',25),('5afa8dd8-03df-42f1-946a-39a6b69299a6','Game Project Definitions','Incluse some images, if possible, title (logo) and contact information, platforms, target audience, classification genre and date.','b51429d5-0fa9-44ff-bc5e-e301bc35a4ca',NULL,1),('5b947e10-9ba6-4350-a776-4082c8d9ecd6','Rewards',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','88075902-715c-4af1-849f-47e8aedbb166',37),('612e58e3-55e8-4793-be2e-b9d56c7041d3','te','te','ff68d227-ace0-45c1-a179-79dd7ef6948b',NULL,NULL),('68c54333-d620-4350-8744-75a99df26d65','Game Mechanics and Rules','Players and characters actions. Put here interactions among environment, characters, players, itens.','b51429d5-0fa9-44ff-bc5e-e301bc35a4ca',NULL,32),('707d3266-ef95-4caa-841b-77b0d6175176','Player(s)',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','c978907e-1b1e-4d27-abbd-46c79d17b684',22),('7ce9dada-9b8e-4fa8-a148-ace2d22c1a1c','Tasks and Quests',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','b9cefcc6-5686-4fa9-8b00-1803bdecff90',14),('80f66cd0-ed4d-4a3d-b398-d96b930eba85','Game Events',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','b9cefcc6-5686-4fa9-8b00-1803bdecff90',15),('8431e299-d375-4c46-b577-31c680f82e96','Fails - Game Overs',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','b9cefcc6-5686-4fa9-8b00-1803bdecff90',18),('865ce221-733d-48a2-8760-b9c6bd1dc905','Gameflow - Plot','How does the caracter evolve with the increase of difficulties? Here should be described the mechanics and the player feedback. And also, the gameflow and the sequence of facts.','b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','34c0e84a-d732-4475-9147-a48d8fbc84a3',10),('88075902-715c-4af1-849f-47e8aedbb166','Game Feedbacks',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','68c54333-d620-4350-8744-75a99df26d65',35),('8b3faa72-e27b-4445-975e-a6224be06efc','Itens and Objects',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','c978907e-1b1e-4d27-abbd-46c79d17b684',24),('966e57e3-6c84-4ad9-9e2c-1a99eaacd690','Project Story',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','5afa8dd8-03df-42f1-946a-39a6b69299a6',3),('9d2de9aa-9add-4796-9f8e-cd8150df890d','Enemies and Bosses',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','c978907e-1b1e-4d27-abbd-46c79d17b684',23),('a19fa032-6ee9-47d7-ad77-b2e99bfd014b','Game Story Abstract',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','34c0e84a-d732-4475-9147-a48d8fbc84a3',9),('a6387f05-de07-4b04-b8cd-376f47e1b0b8','Problems, Challenges and Motivations',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','34c0e84a-d732-4475-9147-a48d8fbc84a3',11),('aeebed44-a0a0-4927-b263-fd8328940b51','Theme',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','34c0e84a-d732-4475-9147-a48d8fbc84a3',8),('af4875c7-2302-4001-b697-36d2ee323b5b','Genre',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','5afa8dd8-03df-42f1-946a-39a6b69299a6',4),('b9cefcc6-5686-4fa9-8b00-1803bdecff90','Gameplay Concepts and Playability','Game features','b51429d5-0fa9-44ff-bc5e-e301bc35a4ca',NULL,13),('c2f1d687-497c-4ae0-9177-5f8f07766179','Goals and Objectives',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','34c0e84a-d732-4475-9147-a48d8fbc84a3',12),('c2fa8814-e10d-4672-9cdc-e04ec7804433','Characters',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','c978907e-1b1e-4d27-abbd-46c79d17b684',21),('c74f6f42-ee3f-4fc9-a800-83398d8a7f20','Game Flow',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','b9cefcc6-5686-4fa9-8b00-1803bdecff90',16),('c978907e-1b1e-4d27-abbd-46c79d17b684','Characters, Objects and Controls','Who control the player? What is the story? Are the some power ups? What are the palyer actions? Who are the characters and their stories?','b51429d5-0fa9-44ff-bc5e-e301bc35a4ca',NULL,20),('d8863478-eb24-4de1-a1b7-449d25c5b7ae','Scenes, Materials and Bonus','Is possible to make conversations and imaginate scenes among the characters here.','b51429d5-0fa9-44ff-bc5e-e301bc35a4ca',NULL,39),('dcf380ed-95a0-48f8-b7da-255673647c7c','Tasks and Quests Feedbacks',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','88075902-715c-4af1-849f-47e8aedbb166',36),('e6d9173f-53a0-4fdc-adab-8f2393760c99','Game Messages',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','88075902-715c-4af1-849f-47e8aedbb166',38),('ebfc8214-28ab-4aae-ae3b-bbe4e12193da','Game Abstract',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','5afa8dd8-03df-42f1-946a-39a6b69299a6',6),('f2b2214b-b0ed-4484-a40d-d036b3ea1606','Title',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','5afa8dd8-03df-42f1-946a-39a6b69299a6',2),('f3b687a9-2a11-430e-824f-f40622c9de47','Game World','Where does the game pass? What are the game environments and places? Put the plot here. ','b51429d5-0fa9-44ff-bc5e-e301bc35a4ca',NULL,29),('f6a94e40-0c55-4f4e-84a7-c384035c1d34','Target Audience',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','5afa8dd8-03df-42f1-946a-39a6b69299a6',5),('f8ac2483-6960-4c05-9dfc-970ea0863e04','Player / Characters Actions',NULL,'b51429d5-0fa9-44ff-bc5e-e301bc35a4ca','68c54333-d620-4350-8744-75a99df26d65',33);
/*!40000 ALTER TABLE `gddconfigurationelements` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `modelinglanguage`
--

DROP TABLE IF EXISTS `modelinglanguage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `modelinglanguage` (
  `Id` varchar(40) NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Version` varchar(255) DEFAULT NULL,
  `RegisterDate` datetime DEFAULT NULL,
  `IsConstant` tinyint(1) DEFAULT NULL,
  `Inactive` tinyint(1) DEFAULT NULL,
  `idDesigner` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idDesigner` (`idDesigner`),
  KEY `idDesigner_2` (`idDesigner`),
  KEY `idDesigner_3` (`idDesigner`),
  CONSTRAINT `FK29C26C96638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FK29F3EC96638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FKFC49570D66E920A6` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `modelinglanguage`
--

LOCK TABLES `modelinglanguage` WRITE;
/*!40000 ALTER TABLE `modelinglanguage` DISABLE KEYS */;
INSERT INTO `modelinglanguage` VALUES ('757a18f6-daaa-464e-a205-d10e8e9fe1c2','BPMN Copy','BPMN Copy\n [Copied from: BPMN and Othes Process Elements]','2.0','2018-01-26 14:29:47',0,1,null),('79aab718-d864-4cbc-8d61-84004f443793','2','2\n [Copied from: BPMN and Othes Process Elements]','2.0','2018-01-26 14:37:52',0,1,null),('b8b1b942-ac76-4752-abad-594aefdc463f','BPMN and Othes Process Elements','Business Process Modeling Notation Elements, and also some elements like goals, localizations, instances...','2.0','2017-12-26 08:51:37',1,0,null),('d106b0bf-f0fd-4dec-b17e-45cd43422b5c','files','x','1','2018-04-13 16:09:07',0,1,null);
/*!40000 ALTER TABLE `modelinglanguage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `modelinglanguageelement`
--

DROP TABLE IF EXISTS `modelinglanguageelement`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `modelinglanguageelement` (
  `Id` varchar(40) NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Description` longtext,
  `Metamodel` varchar(255) DEFAULT NULL,
  `idElement` varchar(40) DEFAULT NULL,
  `idLanguage` varchar(40) DEFAULT NULL,
  `RuleAbbled` tinyint(1) DEFAULT NULL,
  `Inactive` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idElement` (`idElement`),
  KEY `idLanguage` (`idLanguage`),
  KEY `idElement_2` (`idElement`),
  KEY `idLanguage_2` (`idLanguage`),
  KEY `idElement_3` (`idElement`),
  KEY `idLanguage_3` (`idLanguage`),
  CONSTRAINT `FK52D1644F1E209D62` FOREIGN KEY (`idLanguage`) REFERENCES `modelinglanguage` (`Id`),
  CONSTRAINT `FK52D1644FFBF1887E` FOREIGN KEY (`idElement`) REFERENCES `modelinglanguageelement` (`Id`),
  CONSTRAINT `FK7783E0701E209D62` FOREIGN KEY (`idLanguage`) REFERENCES `modelinglanguage` (`Id`),
  CONSTRAINT `FK7783E070FBF1887E` FOREIGN KEY (`idElement`) REFERENCES `modelinglanguageelement` (`Id`),
  CONSTRAINT `FKB2EB530BA7698927` FOREIGN KEY (`idLanguage`) REFERENCES `modelinglanguage` (`Id`),
  CONSTRAINT `FKB2EB530BD5489FE1` FOREIGN KEY (`idElement`) REFERENCES `modelinglanguageelement` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `modelinglanguageelement`
--

LOCK TABLES `modelinglanguageelement` WRITE;
/*!40000 ALTER TABLE `modelinglanguageelement` DISABLE KEYS */;
INSERT INTO `modelinglanguageelement` VALUES ('04045f2b-07ce-46ce-aad1-4460d3ccfa37','Data Association Flow (Input)','It represents the connection between resources and flow elements.','bpmn:dataInputAssociation',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('046b189c-75b1-4f09-9d29-df91b8e2ca93','Rules (not in BPMN)','The rules guide the process execution, interaction, courses and conditions.',NULL,NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('0c3656c1-86bf-45f7-b41f-e57c1f76fea9','Data Association Flow (Output)','It represents the connection between resources and flow elements.','bpmn:dataOutputAssociation',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('0f8d74ce-6ec3-472b-9b7f-f106b901b442','Annotations','Annotations and comments about the process.','bpmn:textAnnotation',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('10a1ab6b-3184-4d24-b55a-bcde30861ead','Data Object (Resource)','It respresents data objects as files, forms and etc.','bpmn:dataObjectReference',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('146d0ea3-e217-4042-b047-8aba5e34891b','Service Task','It is a task performed by a service','bpmn:serviceTask','d2e065f5-f397-4681-915e-733b1e42c9d4','79aab718-d864-4cbc-8d61-84004f443793',0,0),('14ed9e30-b664-4583-93d9-3473106335fd','Localization (not in BPMN)','Places and physics locals in the organization',NULL,NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('15f6ad79-5ae6-4519-b99b-42d602e0206d','Boundary Event','It is some event triggered inside of activities that can change the process flow or create parallel flows when its occour.','bpmn:boundaryEvent',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('19b1df44-be4c-4d1e-821d-07f848347f27','Sequence Flow','It represents a sequential temporal order of tasks.','bpmn:sequenceFlow',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('1e6c7e18-7373-4c2e-8da7-cd7d22d3e943','User Task','It is a task performed by an user','bpmn:userTask','26345c60-b7be-4098-ab36-d5b075aad479','b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('1e8bbb66-49ac-4cfa-b4ca-2d68bdd5c28a','User Task','It is a task performed by an user','bpmn:userTask','d2e065f5-f397-4681-915e-733b1e42c9d4','79aab718-d864-4cbc-8d61-84004f443793',0,0),('20f6985a-6dcd-4c6d-ad64-4c1013ed7326','Instances or Courses (not in BPMN)','This is some path (course) performed in the process.',NULL,NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('222cff41-7d04-4315-ade4-619faa28f5e6','Data Association Flow (Output)','It represents the connection between resources and flow elements.','bpmn:dataOutputAssociation',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('26345c60-b7be-4098-ab36-d5b075aad479','Task','They are simple activities performed by once.','bpmn:task',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('286a5077-47b0-46c9-9f68-19a392952219','Intermediate Event (Throw)','It this some happening that can change the process flow (throw)','bpmn:intermediateThrowEvent',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('287d871e-b0d2-4009-a513-443536e8859e','Sub Process','It is a composed activitie, that have into sequences of others tasks.','bpmn:subProcess',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('2c38050e-7a27-4da1-bae0-d77f1c5bb6b4','Task','They are simple activities performed by once.','bpmn:task',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('2c6020cf-d778-4c60-913e-605c2888b24d','Script Task','It is a task performed by a script','bpmn:scriptTask',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('2c8fbf84-6866-471a-8c50-abb8dfba511e','Data Store (Resource)','It represents system data, data bases, and stores.','bpmn:dataStoreReference',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('2f2f51f2-24dd-4678-b814-18bf52e963f9','Lane','Person or department responsable for execute activites.','bpmn:lane',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('31eadc22-0fd8-45ba-b15a-fa822aaeda38','Intermediate Event (Throw)','It this some happening that can change the process flow (throw)','bpmn:intermediateThrowEvent',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('369bd4a5-10e3-4f57-829e-99e4e797c18d','Exclusive Gateway','It is conditions creating unique path in the process follow some condition.','bpmn:exclusiveGateway',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('38c1f1c3-44cf-4ca6-979f-8bb0c445cb5a','Data Object (Resource)','It respresents data objects as files, forms and etc.','bpmn:dataObjectReference',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('3b7d9e17-b93f-46a6-8f1b-164bb01478b1','Goals (not in BPMN)','Goal to reach with the process.',NULL,NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('4078f22f-f1bf-4a05-883b-1cd256e40791','Localization (not in BPMN)','Places and physics locals in the organization',NULL,'4b917ff1-46f0-4c1c-b6a2-47a3083b34a5','757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('45a8a364-d28d-4eab-8b65-4bf28f5bcf68','Data Store (Resource)','It represents system data, data bases, and stores.','bpmn:dataStoreReference',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('4822d38b-bc95-4c51-8897-6adfefc1be8a','Pool or Participant (Black Box)','Hide process model or some process participant.','bpmn:participant',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('4a67bdd8-7195-4269-8ba7-f0dff4db26cc','Script Task','It is a task performed by a script','bpmn:scriptTask','26345c60-b7be-4098-ab36-d5b075aad479','b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('4b917ff1-46f0-4c1c-b6a2-47a3083b34a5','Rules (not in BPMN)','The rules guide the process execution, interaction, courses and conditions.',NULL,NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('4d9002e4-09c2-48ee-8d11-321c79f098ba','Localization (not in BPMN)','Places and physics locals in the organization',NULL,NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('4e0e1831-1664-4142-8320-dbcca67a2221','Pool or Participant (Black Box)','Hide process model or some process participant.','bpmn:participant',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('53a73fb7-2c6a-47d7-9349-fc220bdd3ea4','Instances or Courses (not in BPMN)','This is some path (course) performed in the process.',NULL,'4b917ff1-46f0-4c1c-b6a2-47a3083b34a5','757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('57139e89-7ab9-4093-be03-531899914b46','End Event','Final happening of the process','bpmn:endEvent',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('5b21d565-5a99-4951-b3a3-5c86399d8697','Business Rule Task','It task describe a process rule','bpmn:businessRuleTask','26345c60-b7be-4098-ab36-d5b075aad479','b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('619d6ac9-46ee-41cb-91d2-9c4ae12bff87','Message Flow','It indicates the communication among different pols.','bpmn:messageFlow',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('61e85b4a-0ac7-4295-a8ba-150bd591b097','Start Event','Initial happening of the process','bpmn:startEvent',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('65e75f2d-86ec-47ad-862f-80e6f568d60f','Exclusive Gateway','It is conditions creating unique path in the process follow some condition.','bpmn:exclusiveGateway',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('66d7087e-764e-4e80-829b-c1522f96ae3c','Start Event','Initial happening of the process','bpmn:startEvent',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('6cd67729-8cf0-49de-8479-3024b5573db1','Parallel Gateway','It is conditions creating parallel path in the process','bpmn:parallelGateway',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('6d1a84ee-e27e-41f6-a482-c7aa8eb74fff','Business Rule Task','It task describe a process rule','bpmn:businessRuleTask',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('6ece1a62-84b4-4081-a7b3-ee7be3d0ce51','Data Association Flow (Input)','It represents the connection between resources and flow elements.','bpmn:dataInputAssociation',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('6fe26812-b126-41e5-aa7a-b7012b55ce6e','Rules (not in BPMN)','The rules guide the process execution, interaction, courses and conditions.',NULL,NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('71ab38ff-9bad-4871-98ad-904f21499038','Exclusive Gateway','It is conditions creating unique path in the process follow some condition.','bpmn:exclusiveGateway',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('7219709c-baff-4e21-a284-54ad175eb50f','Pool or Participant','Sequence of activities to reach business goals, or some participant of the process.','bpmn:process',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('731bfdaf-59d2-48bb-9a41-179a42f36176','Sub Process','It is a composed activitie, that have into sequences of others tasks.','bpmn:subProcess',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('7372cd4f-3cd3-4085-8994-2d33751f696f','Start Event','Initial happening of the process','bpmn:startEvent',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('75ade56d-4bd4-4cf6-8f4b-0fa26cd08862','End Event','Final happening of the process','bpmn:endEvent',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('7780c25a-0962-4049-947d-042ab671fdcf','Annotations','Annotations and comments about the process.','bpmn:textAnnotation',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('84ed18ea-1824-4b3e-a54d-38080b1af471','Send Task','It is a task to send sometinh','bpmn:sendTask','d2e065f5-f397-4681-915e-733b1e42c9d4','79aab718-d864-4cbc-8d61-84004f443793',0,0),('8a41aa9e-c368-4101-988c-e06b7dc669b3','Sequence Flow','It represents a sequential temporal order of tasks.','bpmn:sequenceFlow',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('8e129a08-7f60-4aa8-a6ee-f538bef0cbaf','Parallel Gateway','It is conditions creating parallel path in the process','bpmn:parallelGateway',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('8fb31f6b-3955-447e-a6a6-657d26f0e78e','Pool or Participant','Sequence of activities to reach business goals, or some participant of the process.','bpmn:process',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('947cd243-2d5b-474d-9fac-33d9de36c2e5','Data Object (Resource)','It respresents data objects as files, forms and etc.','bpmn:dataObjectReference',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('974a8cfe-d0d6-4bce-ab7a-441a8b4126b3','Manual Task','It is a task perfoemed through manual way','bpmn:manualTask',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('98b8139a-f94a-4a2d-9037-340a8f1b1a1e','Receive Task','It is a task to receive something','bpmn:receiveTask',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('992c92d5-1e08-4a00-ab4d-59856c83647a','Boundary Event','It is some event triggered inside of activities that can change the process flow or create parallel flows when its occour.','bpmn:boundaryEvent',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',0,0),('9ebe6f66-8f94-42b5-ab97-8b8b71201395','Parallel Gateway','It is conditions creating parallel path in the process','bpmn:parallelGateway',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('9fe13031-48cb-404b-be9e-efbad426f26d','Receive Task','It is a task to receive something','bpmn:receiveTask','26345c60-b7be-4098-ab36-d5b075aad479','b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('a6e2da4c-8b05-49c8-8406-467f4a47906b','Instances or Courses (not in BPMN)','This is some path (course) performed in the process.',NULL,NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('aca11f49-5c32-47cc-93b2-8c815fbd5808','Intermediate Event (Catch)','It this some happening that can change the process flow (catch)','bpmn:intermediateCatchEvent',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('ae8fd939-7845-4c45-9091-f46511fa77f9','Data Store (Resource)','It represents system data, data bases, and stores.','bpmn:dataStoreReference',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('af3af5ed-67f6-4ed9-b86b-e1c7861f091b','Pool or Participant (Black Box)','Hide process model or some process participant.','bpmn:participant',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('b13c843c-fcff-4ce6-ba2f-7177ca4bba14','Inlcusive Gateway','It perform one or many paths in the process follwoing rules.','bpmn:inclusiveGateway',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('b1d8112b-48bb-4460-a043-59c182e65139','Goals (not in BPMN)','Goal to reach with the process.',NULL,NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('b6f2efde-7e32-4614-b565-76d65cd9b781','Data Association Flow (Input)','It represents the connection between resources and flow elements.','bpmn:dataInputAssociation',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('b8ae2e4b-b60f-4e13-ad1d-8faba0cb2f42','Receive Task','It is a task to receive something','bpmn:receiveTask','d2e065f5-f397-4681-915e-733b1e42c9d4','79aab718-d864-4cbc-8d61-84004f443793',0,0),('ba169fb0-5b52-485d-a489-37ae3730e77c','Intermediate Event (Catch)','It this some happening that can change the process flow (catch)','bpmn:intermediateCatchEvent',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('c0b6681c-d6e5-40c9-970b-22402562dd87','Inlcusive Gateway','It perform one or many paths in the process follwoing rules.','bpmn:inclusiveGateway',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('c4212b2f-10dd-41f8-b7c6-bf57b7c64d47','Message Flow','It indicates the communication among different pols.','bpmn:messageFlow',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('ca3857e5-d338-48de-9ed0-1f783c237a16','Intermediate Event (Throw)','It this some happening that can change the process flow (throw)','bpmn:intermediateThrowEvent',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('cce12713-1de1-4927-877c-030394691ea1','Inlcusive Gateway','It perform one or many paths in the process follwoing rules.','bpmn:inclusiveGateway',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('cf03ce15-6a5a-469c-a143-d400166fe0ac','Service Task','It is a task performed by a service','bpmn:serviceTask','26345c60-b7be-4098-ab36-d5b075aad479','b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('d022e66f-712a-498b-9ca1-41ac385a5782','Script Task','It is a task performed by a script','bpmn:scriptTask','d2e065f5-f397-4681-915e-733b1e42c9d4','79aab718-d864-4cbc-8d61-84004f443793',0,0),('d0df569b-965f-4750-a2cb-4c107febfb10','User Task','It is a task performed by an user','bpmn:userTask',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('d2e065f5-f397-4681-915e-733b1e42c9d4','Task','They are simple activities performed by once.','bpmn:task',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('d58edbab-02a2-4331-a08f-a33d3fc885df','Lane','Person or department responsable for execute activites.','bpmn:lane',NULL,'79aab718-d864-4cbc-8d61-84004f443793',0,0),('d7ab2b28-d4d8-4439-8d2e-ab3ee8bdff8f','Service Task','It is a task performed by a service','bpmn:serviceTask',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('d90c7f20-881a-446d-a4cc-a7743f701c66','X','X','X',NULL,'d106b0bf-f0fd-4dec-b17e-45cd43422b5c',0,0),('db31d8aa-53c6-4f8b-b7c1-add9b805ff5a','Boundary Event','It is some event triggered inside of activities that can change the process flow or create parallel flows when its occour.','bpmn:boundaryEvent',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('dc029987-fd24-489c-923a-31680c1569ec','Data Association Flow (Output)','It represents the connection between resources and flow elements.','bpmn:dataOutputAssociation',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('dcac9d99-0ad7-4a64-b1c2-97782c4ea189','End Event','Final happening of the process','bpmn:endEvent',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('dd298a35-2496-4036-9824-afed28d9d8de','Sequence Flow','It represents a sequential temporal order of tasks.','bpmn:sequenceFlow',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('ddb5783e-bc64-4aa6-96e8-381d16e8f347','Send Task','It is a task to send sometinh','bpmn:sendTask',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('e3a5f4dc-5d8a-4f1e-8ad3-f5323ecb19fc','Manual Task','It is a task perfoemed through manual way','bpmn:manualTask','26345c60-b7be-4098-ab36-d5b075aad479','b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('e6419fe2-9873-40b2-82f9-d4628c9865b0','Sub Process','It is a composed activitie, that have into sequences of others tasks.','bpmn:subProcess',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('e733e91c-99a0-4eee-ab84-65f79b56b5e7','Message Flow','It indicates the communication among different pols.','bpmn:messageFlow',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('e9196dc4-01e8-47de-8c72-203546dbefdb','Annotations','Annotations and comments about the process.','bpmn:textAnnotation',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('e9524f1e-ea42-4a61-9cdc-84605e673607','Lane','Person or department responsable for execute activites.','bpmn:lane',NULL,'b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0),('e9eb9120-dc07-4965-9bba-4b79b8b3f953','Business Rule Task','It task describe a process rule','bpmn:businessRuleTask','d2e065f5-f397-4681-915e-733b1e42c9d4','79aab718-d864-4cbc-8d61-84004f443793',0,0),('ea721987-9804-41f7-a4c4-45e5757aa979','Pool or Participant','Sequence of activities to reach business goals, or some participant of the process.','bpmn:process',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('eaf49be7-78bd-4621-8dbb-a4e9591550e9','Manual Task','It is a task perfoemed through manual way','bpmn:manualTask','d2e065f5-f397-4681-915e-733b1e42c9d4','79aab718-d864-4cbc-8d61-84004f443793',0,0),('ed91441b-2f00-47eb-8617-4aea53ba2554','Intermediate Event (Catch)','It this some happening that can change the process flow (catch)','bpmn:intermediateCatchEvent',NULL,'757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('f4ea44fc-e940-46db-b2f6-75909a0bf41f','i','i','i','d90c7f20-881a-446d-a4cc-a7743f701c66','d106b0bf-f0fd-4dec-b17e-45cd43422b5c',0,0),('f786738e-a5ff-46a0-81c9-94ce5ce9e114','Goals (not in BPMN)','Goal to reach with the process.',NULL,'4b917ff1-46f0-4c1c-b6a2-47a3083b34a5','757a18f6-daaa-464e-a205-d10e8e9fe1c2',0,0),('faadf43a-5bab-4ac8-898a-c5287d163bc2','Send Task','It is a task to send sometinh','bpmn:sendTask','26345c60-b7be-4098-ab36-d5b075aad479','b8b1b942-ac76-4752-abad-594aefdc463f',NULL,0);
/*!40000 ALTER TABLE `modelinglanguageelement` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `project`
--

DROP TABLE IF EXISTS `project`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `project` (
  `Id` varchar(40) NOT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `Description` longtext,
  `StartDate` datetime DEFAULT NULL,
  `LastUpdate` datetime DEFAULT NULL,
  `BpmnModelPath` varchar(255) DEFAULT NULL,
  `BpmnModel` longblob,
  `Inactive` tinyint(1) DEFAULT NULL,
  `idDesigner` varchar(40) DEFAULT NULL,
  `idGameGenre` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idDesigner` (`idDesigner`),
  KEY `idDesigner_2` (`idDesigner`),
  KEY `idGameGenre` (`idGameGenre`),
  KEY `idDesigner_3` (`idDesigner`),
  KEY `idGameGenre_2` (`idGameGenre`),
  CONSTRAINT `FKB269322166E920A6` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FKCFC6D47A638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FKCFC6D47ADF369F58` FOREIGN KEY (`idGameGenre`) REFERENCES `gamegenre` (`Id`),
  CONSTRAINT `FKCFC6D85A638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FKCFC6D85ADF369F58` FOREIGN KEY (`idGameGenre`) REFERENCES `gamegenre` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `project`
--

--
-- Table structure for table `projectgdd`
--

DROP TABLE IF EXISTS `projectgdd`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `projectgdd` (
  `Id` varchar(40) NOT NULL,
  `CreationDate` datetime DEFAULT NULL,
  `DesignerName` varchar(255) DEFAULT NULL,
  `idProject` varchar(40) DEFAULT NULL,
  `BasedOnMapping` tinyint(1) DEFAULT NULL,
  `GddContent` longblob,
  PRIMARY KEY (`Id`),
  KEY `idProject` (`idProject`),
  CONSTRAINT `FKA80BED646BCBAFA8` FOREIGN KEY (`idProject`) REFERENCES `project` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `projectgdd`
--

--
-- Table structure for table `projectgddsection`
--

DROP TABLE IF EXISTS `projectgddsection`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `projectgddsection` (
  `Id` varchar(40) NOT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `idProjectGdd` varchar(40) DEFAULT NULL,
  `idGddSection` varchar(40) DEFAULT NULL,
  `DtHoraCadastro` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idProjectGdd` (`idProjectGdd`),
  KEY `idGddSection` (`idGddSection`),
  CONSTRAINT `FK66A097DA2A8DB0BE` FOREIGN KEY (`idGddSection`) REFERENCES `projectgddsection` (`Id`),
  CONSTRAINT `FK66A097DA32715418` FOREIGN KEY (`idProjectGdd`) REFERENCES `projectgdd` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `projectgddsection`
--

--
-- Table structure for table `projectgddsectioncontent`
--

DROP TABLE IF EXISTS `projectgddsectioncontent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `projectgddsectioncontent` (
  `Id` varchar(40) NOT NULL,
  `Content` longtext,
  `GameGenreTitle` varchar(255) DEFAULT NULL,
  `Automatic` tinyint(1) DEFAULT NULL,
  `idGddSection` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idGddSection` (`idGddSection`),
  CONSTRAINT `FKB187314D2A8DB0BE` FOREIGN KEY (`idGddSection`) REFERENCES `projectgddsection` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `projectgddsectioncontent`
--

--
-- Table structure for table `projects`
--

DROP TABLE IF EXISTS `projects`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `projects` (
  `idDesigner` varchar(40) NOT NULL,
  `idProject` varchar(40) NOT NULL,
  KEY `idProject` (`idProject`),
  KEY `idDesigner` (`idDesigner`),
  KEY `idProject_2` (`idProject`),
  KEY `idDesigner_2` (`idDesigner`),
  CONSTRAINT `FK6E25D85A638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FK6E25D85A6BCBAFA8` FOREIGN KEY (`idProject`) REFERENCES `project` (`Id`),
  CONSTRAINT `FK9048BE8C27192DC4` FOREIGN KEY (`idProject`) REFERENCES `project` (`Id`),
  CONSTRAINT `FK9048BE8C66E920A6` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `projects`
--

LOCK TABLES `projects` WRITE;
/*!40000 ALTER TABLE `projects` DISABLE KEYS */;
/*!40000 ALTER TABLE `projects` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `projectsolicitation`
--

DROP TABLE IF EXISTS `projectsolicitation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `projectsolicitation` (
  `Id` varchar(40) NOT NULL,
  `idProject` varchar(40) DEFAULT NULL,
  `idDesigner` varchar(40) DEFAULT NULL,
  `Date` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `idProject` (`idProject`),
  KEY `idDesigner` (`idDesigner`),
  KEY `idProject_2` (`idProject`),
  KEY `idDesigner_2` (`idDesigner`),
  KEY `idProject_3` (`idProject`),
  KEY `idDesigner_3` (`idDesigner`),
  CONSTRAINT `FK78B2B071638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FK78B2B0716BCBAFA8` FOREIGN KEY (`idProject`) REFERENCES `project` (`Id`),
  CONSTRAINT `FK8410E4A327192DC4` FOREIGN KEY (`idProject`) REFERENCES `project` (`Id`),
  CONSTRAINT `FK8410E4A366E920A6` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FKD0DC67A1638896E9` FOREIGN KEY (`idDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FKD0DC67A16BCBAFA8` FOREIGN KEY (`idProject`) REFERENCES `project` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `projectsolicitation`
--

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user` (
  `Id` varchar(40) NOT NULL,
  `UserName` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Password` varchar(255) DEFAULT NULL,
  `IdDesigner` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IdDesigner` (`IdDesigner`),
  KEY `IdDesigner_2` (`IdDesigner`),
  KEY `IdDesigner_3` (`IdDesigner`),
  CONSTRAINT `FK564EC98966E920A6` FOREIGN KEY (`IdDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FK7185C15C638896E9` FOREIGN KEY (`IdDesigner`) REFERENCES `designer` (`Id`),
  CONSTRAINT `FK7185C17C638896E9` FOREIGN KEY (`IdDesigner`) REFERENCES `designer` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--


--
-- Table structure for table `version`
--

DROP TABLE IF EXISTS `version`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `version` (
  `Id` varchar(40) NOT NULL,
  `VersionNumber` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

INSERT INTO `bpm2gp`.`version` (`Id`, `VersionNumber`) VALUES ('026d741b-0e8f-4be2-b133-9438a7d02x21', '1');

--
-- Dumping data for table `version`
--

LOCK TABLES `version` WRITE;
/*!40000 ALTER TABLE `version` DISABLE KEYS */;
/*!40000 ALTER TABLE `version` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-03 13:07:27
