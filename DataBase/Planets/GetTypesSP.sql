CREATE PROCEDURE GetTypes @types nvarchar(max) OUTPUT
AS
BEGIN
	SELECT @types = STRING_AGG(p_type,'#')
	FROM
	(
		SELECT DISTINCT p_type
		FROM Planet
	) AS tipos;
END
GO