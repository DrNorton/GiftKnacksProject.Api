CREATE TABLE [dbo].WishGiftLinks
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
	[UserId] BIGINT NOT NULL, 
	[WishId] BIGINT NOT NULL, 
	[GiftId] BIGINT NOT NULL, 
	[CreatedTime] DATETIME NULL, 
	CONSTRAINT [FK_WishGiftLinks_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
	CONSTRAINT [FK_WishGiftLinks_Wishes] FOREIGN KEY ([WishId]) REFERENCES [Wishes]([Id]),
	CONSTRAINT [FK_WishGiftLinks_Gifts] FOREIGN KEY ([GiftId]) REFERENCES [Gifts]([Id])
)
