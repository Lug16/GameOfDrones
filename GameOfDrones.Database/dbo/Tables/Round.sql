CREATE TABLE [dbo].[Round] (
    [IdPlayer]     INT           NOT NULL,
    [IdGame]       INT           NOT NULL,
    [HandShape]    INT           CONSTRAINT [DF_Round_HandShape] DEFAULT ((0)) NOT NULL,
    [Won]          BIT           NOT NULL,
    [CreationDate] DATETIME2 (7) CONSTRAINT [DF_Round_CreationDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Round_1] PRIMARY KEY CLUSTERED ([IdPlayer] ASC, [IdGame] ASC),
    CONSTRAINT [FK_Round_Game1] FOREIGN KEY ([IdGame]) REFERENCES [dbo].[Game] ([IdGame]),
    CONSTRAINT [FK_Round_Player1] FOREIGN KEY ([IdPlayer]) REFERENCES [dbo].[Player] ([IdPlayer])
);







