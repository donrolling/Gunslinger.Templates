CREATE TABLE [membership].[LoginProvider] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserId]       BIGINT         NOT NULL,
    [ProviderName] NVARCHAR (50)  NOT NULL,
    [Login]        NVARCHAR (50)  NOT NULL,
    [Password]     NVARCHAR (500) NOT NULL,
    [IsActive]     BIT            CONSTRAINT [DF_LoginProvider_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]    BIGINT         NULL,
    [CreatedDate]  DATETIME       NULL,
    [ModifiedBy]   BIGINT         NULL,
    [ModifiedDate] DATETIME       NULL,
    CONSTRAINT [PK_UserLogin] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LoginProvider_User] FOREIGN KEY ([UserId]) REFERENCES [membership].[User] ([Id])
);

