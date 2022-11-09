CREATE TABLE [dbo].[Address] (
    [Id]     INT IDENTITY (1, 1) NOT NULL,
    [AccountId] NVARCHAR(50) NULL,
    [IsRedeemed] BIT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);