CREATE PROCEDURE [membership].[UserPermission_Update] (
	 @id bigint
	, @userId bigint
	, @permissionId bigint
	, @isActive bit
	, @createdBy bigint
	, @createdDate datetime
	, @modifiedBy bigint
	, @modifiedDate datetime
) AS
	UPDATE [membership].[UserPermission]
	SET
	   [UserId] = @userId
	  , [PermissionId] = @permissionId
	  , [IsActive] = @isActive
	  , [CreatedBy] = @createdBy
	  , [CreatedDate] = @createdDate
	  , [ModifiedBy] = @modifiedBy
	  , [ModifiedDate] = @modifiedDate
	WHERE @Id = @id