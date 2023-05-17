CREATE PROCEDURE SetDeck
@deck_id nvarchar(15),
@email nvarchar(50)
AS
BEGIN
	UPDATE Users
	SET current_deck = @deck_id
	WHERE email = @email;
END
GO