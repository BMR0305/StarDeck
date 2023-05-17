CREATE PROCEDURE UpdateUserStatus
@email nvarchar(50),
@status nvarchar(10)
AS
BEGIN
	UPDATE Users
	SET u_status = @status
	WHERE email = @email;
END
GO