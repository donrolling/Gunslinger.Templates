CREATE PROCEDURE [dbo].[Address_Insert] (
	 @address1 nvarchar(50)
	, @address2 nvarchar(50)
	, @city nvarchar(50)
	, @state nvarchar(2)
	, @zip nvarchar(10)
	, @isActive bit
	, @createdBy bigint
	, @createdDate datetime
	, @modifiedBy bigint
	, @modifiedDate datetime
	, @id BigInt OUTPUT
) AS
	INSERT INTO [dbo].[Address] (
	   [Address1]
	  , [Address2]
	  , [City]
	  , [State]
	  , [Zip]
	  , [IsActive]
	  , [CreatedBy]
	  , [CreatedDate]
	  , [ModifiedBy]
	  , [ModifiedDate]
	)
	VALUES (
	   @address1
	  , @address2
	  , @city
	  , @state
	  , @zip
	  , @isActive
	  , @createdBy
	  , @createdDate
	  , @modifiedBy
	  , @modifiedDate
	)
	
	set @id = Scope_Identity()