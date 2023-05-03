CREATE PROCEDURE ValidateUser
@mail nvarchar(40),
@password nvarchar(40),
@error_message nvarchar(50) OUTPUT
AS
BEGIN
	--Sets error to user found, so if the user exists the API receives this message.
	SET @error_message = 'User found'
	--Declares count variable to see if the user exists.
	DECLARE @count INT
	SET @count = 0
	--Looks for the user with the email and password provided
	SELECT @count = COUNT(*)
	FROM Users
	WHERE email = @mail AND u_password = @password

	IF @count = 0
	BEGIN
		IF NOT EXISTS (SELECT * FROM Users WHERE email = @mail)
			SET @error_message = 'Incorrect email'
		ELSE IF NOT EXISTS (SELECT * FROM Users WHERE u_password = @password)
			SET @error_message = 'Incorrect Password'
	END
END
GO