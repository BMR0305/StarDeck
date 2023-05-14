CREATE PROCEDURE GetPlayerDecks
@player_email nvarchar(50)
AS
BEGIN
	SELECT DISTINCT d.d_name, d.Deck_ID, d.Player_ID
	FROM Deck AS d
	WHERE d.Player_ID = (
		SELECT u.ID
		FROM Users AS u
		WHERE email = @player_email);
END
GO