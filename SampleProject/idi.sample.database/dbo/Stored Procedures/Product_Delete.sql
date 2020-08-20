CREATE PROCEDURE [dbo].[Product_Delete] (
	@id BigInt
) AS
	DELETE FROM [dbo].[Product]
	WHERE Id = @id