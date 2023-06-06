CREATE PROCEDURE AddPlayerReady
@turnID NVARCHAR(15)
AS
BEGIN
	UPDATE Turn
	SET Players_Ready += 1
	WHERE Turn_ID = @turnID
END
GO