CREATE TABLE [dbo].[Product] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [Description]  NVARCHAR (MAX) NOT NULL,
    [IsActive]     BIT            CONSTRAINT [DF_Product_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]    BIGINT         NULL,
    [CreatedDate]  DATETIME       NULL,
    [ModifiedBy]   BIGINT         NULL,
    [ModifiedDate] DATETIME       NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Id] ASC)
);

