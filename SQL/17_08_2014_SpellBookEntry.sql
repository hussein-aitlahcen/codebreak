CREATE TABLE [dbo].[SpellBookEntry] (
    [Id]          BIGINT IDENTITY (1, 1) NOT NULL,
    [CharacterId] BIGINT NOT NULL,
    [SpellId]     INT    NOT NULL,
    [Level]       INT    NOT NULL,
    [Position]    INT    NOT NULL,
    CONSTRAINT [PK_SpellBookEntry] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SpellBookEntry_ToCharacter] FOREIGN KEY ([CharacterId]) REFERENCES [dbo].[Character] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

