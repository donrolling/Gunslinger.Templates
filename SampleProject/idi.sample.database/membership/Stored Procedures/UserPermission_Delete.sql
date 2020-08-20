CREATE PROCEDURE [membership].[UserPermission_Delete] (
	@id BigInt
) AS
	DELETE FROM [membership].[UserPermission]
	WHERE Id = @id