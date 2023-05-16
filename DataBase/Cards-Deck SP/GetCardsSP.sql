CREATE PROCEDURE GetRandomCards 
@num INT,
@ctypeList nvarchar(60)
AS
BEGIN
    -- Declares temp table to save card types requested.
    DECLARE @ctypeTable TABLE (ctype nvarchar(30))

    -- Divides the string provided with the card types to insert them to the temp table.
    INSERT INTO @ctypeTable
    SELECT value FROM STRING_SPLIT(@ctypeList, '#')

    -- Selects random cards from the requested types in the temp table.
    SELECT TOP (@num) *
    FROM Cards
    WHERE c_type IN (SELECT ctype FROM @ctypeTable)
    ORDER BY NEWID()
END
GO