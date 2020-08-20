CREATE TABLE [membership].[UserPermission] (
    [Id]           BIGINT   IDENTITY (1, 1) NOT NULL,
    [UserId]       BIGINT   NOT NULL,
    [PermissionId] BIGINT   NOT NULL,
    [IsActive]     BIT      CONSTRAINT [DF_UserPermission_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]    BIGINT   NULL,
    [CreatedDate]  DATETIME NULL,
    [ModifiedBy]   BIGINT   NULL,
    [ModifiedDate] DATETIME NULL,
    CONSTRAINT [PK_UserPermission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [membership].[Permission] ([Id]),
    CONSTRAINT [FK_UserPermission_User] FOREIGN KEY ([UserId]) REFERENCES [membership].[User] ([Id])
);

