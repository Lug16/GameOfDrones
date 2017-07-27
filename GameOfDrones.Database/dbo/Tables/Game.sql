CREATE TABLE [dbo].[Game] (
    [IdGame]    INT           IDENTITY (1, 1) NOT NULL,
    [IdWinner]  INT           NULL,
    [StartDate] DATETIME2 (7) CONSTRAINT [DF_Game_StartDate] DEFAULT (getdate()) NOT NULL,
    [EndDate]   DATETIME2 (7) NULL,
    CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED ([IdGame] ASC),
    CONSTRAINT [FK_Game_Player] FOREIGN KEY ([IdWinner]) REFERENCES [dbo].[Player] ([IdPlayer])
);





