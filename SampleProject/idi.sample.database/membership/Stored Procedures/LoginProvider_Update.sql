CREATE PROCEDURE [membership].[LoginProvider_Update] (
	 @id bigint
	, @userId bigint
	, @providerName nvarchar(50)
	, @login nvarchar(50)
	, @password nvarchar(500)
	, @isActive bit
	, @createdBy bigint
	, @createdDate datetime
	, @modifiedBy bigint
	, @modifiedDate datetime
) AS
	UPDATE [membership].[LoginProvider]
	SET
	   [UserId] = @userId
	  , [ProviderName] = @providerName
	  , [Login] = @login
	  , [Password] = @password
	  , [IsActive] = @isActive
	  , [CreatedBy] = @createdBy
	  , [CreatedDate] = @createdDate
	  , [ModifiedBy] = @modifiedBy
	  , [ModifiedDate] = @modifiedDate
	WHERE @Id = @id