CREATE PROCEDURE GetCards
@email nvarchar(40)
AS
BEGIN
	SELECT c.ID, c.c_name, c.battle_pts, c.energy, c.c_image, c.c_type, c.race, c.c_status, c.c_description
	FROM Cards AS c
	JOIN User_card ON User_card.card_key = c.ID
	JOIN Users ON User_card.user_key = Users.ID
	WHERE user_key = (SELECT Users.ID 
					  FROM Users 
					  WHERE email = @email)
END
GO