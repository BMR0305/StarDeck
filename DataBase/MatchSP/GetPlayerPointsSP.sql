CREATE PROCEDURE GetPlayerPoints
@cardID_list NVARCHAR(300),
@pointsP INT OUTPUT
AS
BEGIN
	DECLARE @cardIDTable TABLE (cardID NVARCHAR(15))

	INSERT INTO @cardIDTable
	SELECT VALUE FROM string_split(@cardID_list,'#')

	SELECT @pointsP = SUM(c.battle_pts)
	FROM Cards as c
	JOIN @cardIDTable AS cid ON c.ID = cid.cardID  
END
GO