CREATE PROCEDURE [dbo].[Address_Delete] (
	@id BigInt
) AS
	DELETE FROM [dbo].[Address]
	WHERE Id = @id