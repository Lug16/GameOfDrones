CREATE TABLE [dbo].[Player] (
    [IdPlayer]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    [CreationDate] AS            (getdate()),
    CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED ([IdPlayer] ASC)
);

