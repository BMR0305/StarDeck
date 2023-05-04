CREATE PROCEDURE GetPlanet
@name nvarchar(35)
AS
BEGIN
	SELECT *
	FROM Planet
	WHERE @name = p_name
END
GO