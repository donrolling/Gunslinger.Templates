CREATE PROCEDURE [membership].[Permission_Insert] (
	 @name nvarchar(50)
	, @isActive bit
	, @createdBy bigint
	, @createdDate datetime
	, @modifiedBy bigint
	, @modifiedDate datetime
	, @id BigInt OUTPUT
) AS
	INSERT INTO [membership].[Permission] (
	   [Name]
	  , [IsActive]
	  , [CreatedBy]
	  , [CreatedDate]
	  , [ModifiedBy]
	  , [ModifiedDate]
	)
	VALUES (
	   @name
	  , @isActive
	  , @createdBy
	  , @createdDate
	  , @modifiedBy
	  , @modifiedDate
	)
	
	set @id = Scope_Identity()