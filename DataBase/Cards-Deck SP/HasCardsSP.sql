CREATE PROCEDURE HasCards
@email nvarchar(50),
@card_count INT OUTPUT
AS
BEGIN
	SELECT @card_count = COUNT(*)
	FROM User_card
	WHERE user_key = (SELECT ID FROM Users WHERE email = @email)
END
GO