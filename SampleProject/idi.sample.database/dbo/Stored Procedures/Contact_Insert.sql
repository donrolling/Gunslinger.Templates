CREATE PROCEDURE [dbo].[Contact_Insert] (
	 @firstName nvarchar(50)
	, @lastName nvarchar(50)
	, @isActive bit
	, @createdBy bigint
	, @createdDate datetime
	, @modifiedBy bigint
	, @modifiedDate datetime
	, @id BigInt OUTPUT
) AS
	INSERT INTO [dbo].[Contact] (
	   [FirstName]
	  , [LastName]
	  , [IsActive]
	  , [CreatedBy]
	  , [CreatedDate]
	  , [ModifiedBy]
	  , [ModifiedDate]
	)
	VALUES (
	   @firstName
	  , @lastName
	  , @isActive
	  , @createdBy
	  , @createdDate
	  , @modifiedBy
	  , @modifiedDate
	)
	
	set @id = Scope_Identity()