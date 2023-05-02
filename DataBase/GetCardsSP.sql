CREATE PROCEDURE GetRandomCards 
@num INT,
@ctypeList nvarchar(60)
AS
BEGIN
    -- Crea una tabla temporal para almacenar los tipos de cartas separados
    DECLARE @ctypeTable TABLE (ctype nvarchar(30))

    -- Divide la lista de tipos de cartas en valores individuales y los inserta en la tabla temporal
    INSERT INTO @ctypeTable
    SELECT value FROM STRING_SPLIT(@ctypeList, '#')

    -- Selecciona cartas aleatorias de los tipos de cartas especificados en la tabla temporal
    SELECT TOP (@num) *
    FROM Cards
    WHERE c_type IN (SELECT ctype FROM @ctypeTable)
    ORDER BY NEWID()
END
GO