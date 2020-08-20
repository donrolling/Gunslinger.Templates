CREATE PROCEDURE [dbo].[Person_Delete] (
	@id BigInt
) AS
	DELETE FROM [dbo].[Person]
	WHERE Id = @id