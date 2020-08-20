CREATE TABLE [dbo].[Person] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [FirstName]    NVARCHAR (50) NOT NULL,
    [LastName]     NVARCHAR (50) NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Person_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]    BIGINT        NULL,
    [CreatedDate]  DATETIME      NULL,
    [ModifiedBy]   BIGINT        NULL,
    [ModifiedDate] DATETIME      NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id] ASC)
);

