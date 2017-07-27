CREATE TABLE [dbo].[Player] (
    [IdPlayer]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    [GamesPlayed]  INT           CONSTRAINT [DF_Player_GamesPlayed] DEFAULT ((0)) NULL,
    [Victories]    INT           NULL,
    [CreationDate] DATETIME2 (7) CONSTRAINT [DF_Player_CreationDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED ([IdPlayer] ASC)
);







