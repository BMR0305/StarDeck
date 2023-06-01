CREATE PROCEDURE SetTurnActivePlayer
@turnID NVARCHAR(15),
@playerID NVARCHAR(15)
AS
BEGIN
	UPDATE Turn
	SET Active_Player = @playerID
	WHERE Turn_ID = @turnID
END
GO