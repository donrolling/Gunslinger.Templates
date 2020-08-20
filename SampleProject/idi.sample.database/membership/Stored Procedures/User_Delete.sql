CREATE PROCEDURE [membership].[User_Delete] (
	@id BigInt
) AS
	DELETE FROM [membership].[User]
	WHERE Id = @id