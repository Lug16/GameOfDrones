CREATE TABLE [dbo].[Game] (
    [IdGame]    INT           IDENTITY (1, 1) NOT NULL,
    [IdWinner]  INT           NOT NULL,
    [StartDate] DATETIME2 (7) NOT NULL,
    [EndDate]   DATETIME2 (7) NULL,
    CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED ([IdGame] ASC)
);

