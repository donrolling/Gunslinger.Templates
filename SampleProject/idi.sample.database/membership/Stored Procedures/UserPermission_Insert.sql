CREATE PROCEDURE [membership].[UserPermission_Insert] (
	 @userId bigint
	, @permissionId bigint
	, @isActive bit
	, @createdBy bigint
	, @createdDate datetime
	, @modifiedBy bigint
	, @modifiedDate datetime
	, @id BigInt OUTPUT
) AS
	INSERT INTO [membership].[UserPermission] (
	   [UserId]
	  , [PermissionId]
	  , [IsActive]
	  , [CreatedBy]
	  , [CreatedDate]
	  , [ModifiedBy]
	  , [ModifiedDate]
	)
	VALUES (
	   @userId
	  , @permissionId
	  , @isActive
	  , @createdBy
	  , @createdDate
	  , @modifiedBy
	  , @modifiedDate
	)
	
	set @id = Scope_Identity()