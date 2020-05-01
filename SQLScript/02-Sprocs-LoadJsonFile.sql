CREATE PROC [dbo].[Country_Post]
	(
	@CountryName nvarchar(100)
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO dbo.Country (CountryName)
	VALUES(@CountryName)

	SELECT SCOPE_IDENTITY() AS CountryId
END
GO



CREATE PROC [dbo].[DailyRecord_Post]
	(
	@CountryName nvarchar(100),
	@Confirmed int,
	@Deaths int,
	@Recovered int,
	@RecordDate datetime2
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO dbo.CovidDailyRecord
		(CountryId, Confirmed, Deaths, Recovered, RecordDate)
	VALUES((SELECT TOP 1 CountryId from dbo.Country WHERE CountryName = @CountryName), @Confirmed, @Deaths, @Recovered, @RecordDate)

	SELECT RecordId, CountryId, Confirmed, Deaths, Recovered, RecordDate
	FROM dbo.CovidDailyRecord
	WHERE RecordId = SCOPE_IDENTITY()
END
GO