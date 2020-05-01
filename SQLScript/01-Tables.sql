CREATE TABLE dbo.Country(
	CountryId int IDENTITY(1,1) NOT NULL,
	CountryName nvarchar(100) NOT NULL,
	
 CONSTRAINT PK_Country PRIMARY KEY CLUSTERED 
(
	CountryId ASC
)
) 
GO

CREATE TABLE dbo.CovidDailyRecord(
	RecordId int IDENTITY(1,1) NOT NULL,
	CountryId int NOT NULL,
	Confirmed int NOT NULL,
	Deaths int NOT NULL,
	Recovered int NOT NULL,
	RecordDate datetime2(7) NOT NULL,
 CONSTRAINT PK_CovidDailyRecord PRIMARY KEY CLUSTERED 
(
	RecordId ASC
)
) 
GO

ALTER TABLE dbo.CovidDailyRecord  WITH CHECK ADD  CONSTRAINT FK_CovidDailyRecord_Country FOREIGN KEY(CountryId)
REFERENCES dbo.Country (CountryId)
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE dbo.CovidDailyRecord CHECK CONSTRAINT FK_CovidDailyRecord_Country
GO