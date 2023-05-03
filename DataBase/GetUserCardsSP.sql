CREATE PROCEDURE GetCards
@email nvarchar(40)
AS
BEGIN
	SELECT Users.ID AS User_Key, User_card.card_key AS Card_key, Cards.c_image AS Card_Image, Cards.c_name AS Card_Name
	FROM User_card
	JOIN Cards ON User_card.card_key = Cards.ID
	JOIN Users ON User_card.user_key = Users.ID
	WHERE user_key = (SELECT Users.ID 
					  FROM Users 
					  WHERE email = @email)
END
GO