CREATE PROCEDURE AddCoinsRanking
@playerID NVARCHAR(15)
AS
BEGIN
	UPDATE Users
	SET coins += 1
	WHERE ID = @playerID;

	UPDATE Users
	SET ranking += 1
	WHERE ID = @playerID;
END
GO