CREATE PROCEDURE [dbo].[Contact_Delete] (
	@id BigInt
) AS
	DELETE FROM [dbo].[Contact]
	WHERE Id = @id