CREATE PROCEDURE GetPlanetByID
@ID nvarchar(15)
AS
BEGIN
	SELECT *
	FROM Planet
	WHERE ID = @ID
END
GO