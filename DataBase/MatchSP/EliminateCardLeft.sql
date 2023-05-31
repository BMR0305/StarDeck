CREATE PROCEDURE EliminateCardLeft
@playerID NVARCHAR(15),
@cardID NVARCHAR(15)
AS
BEGIN
	DELETE FROM CardsLeft
	WHERE Player_ID = @playerID AND Card_ID = @cardID
END
GO