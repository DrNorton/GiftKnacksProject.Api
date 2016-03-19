CREATE TABLE [dbo].[WishLinkComments]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity(1,1),
	[CommentId] BIGINT NOT NULL, 
    [WishId] BIGINT NOT NULL,
    CONSTRAINT [FK_WishLink] FOREIGN KEY ([WishId]) REFERENCES [Wishes]([Id]),
    CONSTRAINT [FK_CommentLink] FOREIGN KEY ([CommentId]) REFERENCES [Comments]([Id])
)
