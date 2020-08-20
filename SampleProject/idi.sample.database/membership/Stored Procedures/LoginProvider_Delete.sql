CREATE PROCEDURE [membership].[LoginProvider_Delete] (
	@id BigInt
) AS
	DELETE FROM [membership].[LoginProvider]
	WHERE Id = @id