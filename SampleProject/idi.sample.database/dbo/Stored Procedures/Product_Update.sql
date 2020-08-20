CREATE PROCEDURE [dbo].[Product_Update] (
	 @id bigint
	, @name nvarchar(50)
	, @description nvarchar(max)
	, @isActive bit
	, @createdBy bigint
	, @createdDate datetime
	, @modifiedBy bigint
	, @modifiedDate datetime
) AS
	UPDATE [dbo].[Product]
	SET
	   [Name] = @name
	  , [Description] = @description
	  , [IsActive] = @isActive
	  , [CreatedBy] = @createdBy
	  , [CreatedDate] = @createdDate
	  , [ModifiedBy] = @modifiedBy
	  , [ModifiedDate] = @modifiedDate
	WHERE @Id = @id