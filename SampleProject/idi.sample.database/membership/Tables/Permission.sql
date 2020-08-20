CREATE TABLE [membership].[Permission] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Permission_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]    BIGINT        NULL,
    [CreatedDate]  DATETIME      NULL,
    [ModifiedBy]   BIGINT        NULL,
    [ModifiedDate] DATETIME      NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([Id] ASC)
);

