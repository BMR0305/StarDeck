CREATE PROCEDURE GetRandomFromDeck
@playerID nvarchar(15),
@num INT
AS
BEGIN
	SELECT TOP (@num) *
	FROM CardsLeft
	WHERE Player_ID = @playerID
	ORDER BY NEWID()
END
GO
