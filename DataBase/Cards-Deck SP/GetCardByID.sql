CREATE PROCEDURE GetCard
@cardID NVARCHAR(15)
AS
BEGIN
	SELECT *
	FROM Cards
	WHERE ID = @cardID
END
GO