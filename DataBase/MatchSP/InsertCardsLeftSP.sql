CREATE PROCEDURE InsertDeckCardsLeft
@deckID nvarchar(15),
@playerID nvarchar(15)
AS
BEGIN
	INSERT INTO CardsLeft (Player_ID,Card_ID)
	SELECT @playerID,dc.Card_ID
	FROM Deck_Card as dc
	WHERE dc.Deck_ID = @deckID;
END
GO