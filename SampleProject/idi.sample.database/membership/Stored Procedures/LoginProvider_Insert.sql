CREATE PROCEDURE [membership].[LoginProvider_Insert] (
	 @userId bigint
	, @providerName nvarchar(50)
	, @login nvarchar(50)
	, @password nvarchar(500)
	, @isActive bit
	, @createdBy bigint
	, @createdDate datetime
	, @modifiedBy bigint
	, @modifiedDate datetime
	, @id BigInt OUTPUT
) AS
	INSERT INTO [membership].[LoginProvider] (
	   [UserId]
	  , [ProviderName]
	  , [Login]
	  , [Password]
	  , [IsActive]
	  , [CreatedBy]
	  , [CreatedDate]
	  , [ModifiedBy]
	  , [ModifiedDate]
	)
	VALUES (
	   @userId
	  , @providerName
	  , @login
	  , @password
	  , @isActive
	  , @createdBy
	  , @createdDate
	  , @modifiedBy
	  , @modifiedDate
	)
	
	set @id = Scope_Identity()