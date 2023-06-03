CREATE PROCEDURE GetCardsPlayed
@playerID NVARCHAR(15),
@gameID NVARCHAR(15)
AS
BEGIN
	SELECT *
	FROM CardPlayed
	WHERE PlayerID = @playerID AND GameID = @gameID
END
GO