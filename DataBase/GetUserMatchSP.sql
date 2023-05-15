CREATE PROCEDURE GetUserMatch
@email nvarchar(50)
AS
BEGIN
	SELECT *
	FROM Partida AS p
	WHERE p.ID = (
		SELECT u.ID 
		FROM Users AS u 
		WHERE u.email = @email) AND p.p_status = 'EC'
END
GO