--FinancialModel DB creation script 
--KYGM Services LLC - 2021
--All Rights Reserved

DROP DATABASE IF EXISTS FincialModel
GO

CREATE DATABASE FinancialModel;
GO

USE FinancialModel;
GO

DROP TABLE IF EXISTS Sectors;
GO

DROP TABLE IF EXISTS Securities 
GO

DROP TABLE IF EXISTS HistoricalSecurity;
GO

DROP TABLE IF EXISTS HistoricalSector;
GO

--Sectors table: covers major economic sectors
CREATE TABLE Sectors (
	ID INT NOT NULL IDENTITY(1,1),
	SectorName VARCHAR(255) NOT NULL,
	PRIMARY KEY (ID)
);
GO

--Securities table: covers stocks, etf's, real estate, commodoties
CREATE TABLE Securities (
	ID INT NOT NULL IDENTITY(1,1),
	SecurityName VARCHAR(255) NOT NULL,
	SecurityTicker VARCHAR(255),
	SecuritySector INT NOT NULL,
	PRIMARY KEY (ID),
	SectorID INT FOREIGN KEY REFERENCES Sectors(ID)
);
GO

--HistoricalData table: stores historical data about a security
CREATE TABLE HistoricalSecurity(
	ID INT NOT NULL IDENTITY(1,1),
	SecurityMovement DECIMAL NOT NULL,
	DateCalculated DATETIME NOT NULL,
	SecurityID INT FOREIGN KEY REFERENCES Securities(ID),
);
GO

--HistoricalSector: stores historical data about a sector
CREATE TABLE HistoricalSector(
	ID INT NOT NULL IDENTITY (1,1),
	SectorMovement DECIMAL NOT NULL,
	DateCalculated DATETIME NOT NULL,
	SectorID INT FOREIGN KEY REFERENCES Sectors(ID)
);
GO
