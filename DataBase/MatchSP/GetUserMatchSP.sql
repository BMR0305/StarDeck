CREATE PROCEDURE GetUserMatch
@email nvarchar(50)
AS
BEGIN
	
	DECLARE @User_ID nvarchar(15)
	SELECT @User_ID = u.ID 
	FROM Users AS u 
	WHERE u.email = @email

	SELECT *
	FROM Partida AS p
	WHERE (@User_ID = Player1 OR @User_ID = Player2) AND p_status = 'EC'
END
GO