CREATE PROCEDURE CountCardsLeft
@playerID NVARCHAR(15),
@cards_left INT OUTPUT
AS
BEGIN
	SELECT COUNT(*)
	FROM CardsLeft
	WHERE Player_ID = @playerID
END
GO