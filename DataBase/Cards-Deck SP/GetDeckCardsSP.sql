CREATE PROCEDURE GetDeckCards
@deckID nvarchar(30)
AS
BEGIN
	SELECT c.ID, c.c_name, c.battle_pts, c.energy, c.c_image, c.c_type, c.race, c.c_status, c.c_description
	FROM Cards AS c
	JOIN Deck_Card AS d ON d.Card_ID = c.ID
	WHERE d.Deck_ID = @deckID;
END
GO
