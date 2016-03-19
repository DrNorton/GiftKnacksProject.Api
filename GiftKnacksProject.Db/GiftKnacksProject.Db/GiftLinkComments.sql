CREATE TABLE [dbo].[GiftLinkComments]
(
    [Id] BIGINT NOT NULL PRIMARY KEY Identity(1,1),
	[CommentId] BIGINT NOT NULL, 
    [GiftId] BIGINT NOT NULL,
    CONSTRAINT [FK_GiftLink] FOREIGN KEY ([GiftId]) REFERENCES [Gifts]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CommentLinkG] FOREIGN KEY ([CommentId]) REFERENCES [Comments]([Id]) ON DELETE CASCADE
)
