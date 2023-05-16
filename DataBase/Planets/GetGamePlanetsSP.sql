CREATE PROCEDURE GetGamePlanets
AS
BEGIN
	DECLARE @temp_planets TABLE
	(
		ID VARCHAR(15) NOT NULL PRIMARY KEY,
		p_name VARCHAR(35),
		p_image VARCHAR(MAX),
		p_description VARCHAR(1000),
		p_effect VARCHAR(500),
		p_type VARCHAR(30), --Raro, Basico, Popular
		p_status VARCHAR(10), --Activo (default), Desactivado
		UNIQUE(p_name)
	);

	DECLARE @counter INT = 0;

	WHILE @counter < 3
	BEGIN
		DECLARE @rand FLOAT = RAND();
		INSERT INTO @temp_planets (ID, p_name, p_image, p_description, p_effect, p_type, p_status)
		SELECT TOP 1 *
		FROM Planet
		WHERE p_type =
			CASE
				WHEN @rand <= 0.50 THEN 'Popular'
				WHEN @rand > 0.5 AND @rand <= 0.75 THEN 'Basico'
				ELSE 'Raro'
			END
			AND ID NOT IN (SELECT ID FROM @temp_planets);

		IF @@ROWCOUNT > 0
			SET @counter += 1;
	END;

	SELECT * FROM @temp_planets;
END;
GO
