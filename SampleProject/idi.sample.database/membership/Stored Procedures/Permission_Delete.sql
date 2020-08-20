CREATE PROCEDURE [membership].[Permission_Delete] (
	@id BigInt
) AS
	DELETE FROM [membership].[Permission]
	WHERE Id = @id