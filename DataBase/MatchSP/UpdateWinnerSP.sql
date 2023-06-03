CREATE PROCEDURE UpdateWinner
@playerID NVARCHAR(15),
@gameID NVARCHAR(15)
AS
BEGIN
	UPDATE Partida
	SET Winner = @playerID
	WHERE ID = @gameID;

	UPDATE Partida
	SET p_status = 'T'
	WHERE ID = @gameID
END
GO



