CREATE TABLE [dbo].[Round] (
    [IdRound]  INT IDENTITY (1, 1) NOT NULL,
    [IdPlayer] INT NOT NULL,
    [IdGame]   INT NOT NULL,
    [Score]    INT NOT NULL,
    CONSTRAINT [PK_Round] PRIMARY KEY CLUSTERED ([IdRound] ASC),
    CONSTRAINT [FK_Round_Game] FOREIGN KEY ([IdPlayer]) REFERENCES [dbo].[Game] ([IdGame]),
    CONSTRAINT [FK_Round_Player] FOREIGN KEY ([IdPlayer]) REFERENCES [dbo].[Player] ([IdPlayer])
);

