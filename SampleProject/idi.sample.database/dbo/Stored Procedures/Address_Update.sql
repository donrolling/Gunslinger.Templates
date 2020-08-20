CREATE PROCEDURE [dbo].[Address_Update] (
	 @id bigint
	, @address1 nvarchar(50)
	, @address2 nvarchar(50)
	, @city nvarchar(50)
	, @state nvarchar(2)
	, @zip nvarchar(10)
	, @isActive bit
	, @createdBy bigint
	, @createdDate datetime
	, @modifiedBy bigint
	, @modifiedDate datetime
) AS
	UPDATE [dbo].[Address]
	SET
	   [Address1] = @address1
	  , [Address2] = @address2
	  , [City] = @city
	  , [State] = @state
	  , [Zip] = @zip
	  , [IsActive] = @isActive
	  , [CreatedBy] = @createdBy
	  , [CreatedDate] = @createdDate
	  , [ModifiedBy] = @modifiedBy
	  , [ModifiedDate] = @modifiedDate
	WHERE @Id = @id