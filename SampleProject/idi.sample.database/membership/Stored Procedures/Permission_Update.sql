CREATE PROCEDURE [membership].[Permission_Update] (
	 @id bigint
	, @name nvarchar(50)
	, @isActive bit
	, @createdBy bigint
	, @createdDate datetime
	, @modifiedBy bigint
	, @modifiedDate datetime
) AS
	UPDATE [membership].[Permission]
	SET
	   [Name] = @name
	  , [IsActive] = @isActive
	  , [CreatedBy] = @createdBy
	  , [CreatedDate] = @createdDate
	  , [ModifiedBy] = @modifiedBy
	  , [ModifiedDate] = @modifiedDate
	WHERE @Id = @id