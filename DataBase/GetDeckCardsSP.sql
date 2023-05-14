CREATE PROCEDURE GetDeckCards
@deckID nvarchar(30),
@PlayerID nvarchar(30) OUTPUT,
@d_name nvarchar(30) OUTPUT
AS
BEGIN
	SELECT @PlayerID = Player_ID, @d_name = d_name
	FROM Deck
	WHERE @deckID = Deck_ID;

	SELECT c.ID, c.c_name, c.battle_pts, c.energy, c.c_image, c.c_type, c.race, c.c_status, c.c_description
	FROM Cards AS c
	JOIN Deck AS d ON @deckID = d.Deck_ID;
END
GO
