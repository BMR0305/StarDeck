CREATE PROCEDURE GetCardPlayedInTurn
@gameID NVARCHAR(15),
@turnID NVARCHAR(15),
@playerID NVARCHAR(50)
AS
BEGIN
	SELECT *
	FROM CardPlayed
	WHERE GameID = @gameID AND PlayerID = @playerID AND Turn = @turnID
END
GO