CREATE PROCEDURE GetPlayer
@email nvarchar(30)
AS
BEGIN
	SELECT *
	FROM Users
	WHERE email = @email
END
GO
