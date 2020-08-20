CREATE TABLE [dbo].[Address] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [Address1]     NVARCHAR (50) NOT NULL,
    [Address2]     NVARCHAR (50) NOT NULL,
    [City]         NVARCHAR (50) NOT NULL,
    [State]        NVARCHAR (2)  NOT NULL,
    [Zip]          NVARCHAR (10) NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Address_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]    BIGINT        NULL,
    [CreatedDate]  DATETIME      NULL,
    [ModifiedBy]   BIGINT        NULL,
    [ModifiedDate] DATETIME      NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([Id] ASC)
);

