CREATE PROCEDURE [dbo].[Contact_Update] (
	 @id bigint
	, @firstName nvarchar(50)
	, @lastName nvarchar(50)
	, @isActive bit
	, @createdBy bigint
	, @createdDate datetime
	, @modifiedBy bigint
	, @modifiedDate datetime
) AS
	UPDATE [dbo].[Contact]
	SET
	   [FirstName] = @firstName
	  , [LastName] = @lastName
	  , [IsActive] = @isActive
	  , [CreatedBy] = @createdBy
	  , [CreatedDate] = @createdDate
	  , [ModifiedBy] = @modifiedBy
	  , [ModifiedDate] = @modifiedDate
	WHERE @Id = @id