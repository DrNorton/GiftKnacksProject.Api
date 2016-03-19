CREATE TABLE [dbo].[Gifts]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity(1,1), 
	[Name] NVARCHAR(50) NULL,
	[Country] NVARCHAR(2) NULL, 
	[UserId] BIGINT NOT NULL, 
	[City] NVARCHAR(150) NULL,
	[Location] NVARCHAR(150) NULL, 
	[FromDate] DATETIME NULL, 
    [ToDate] DATETIME NULL, 
	[Benefit] NVARCHAR(150) NULL, 
    [Description] NVARCHAR(150) NULL, 
	[StatusId] INT NOT NULL, 
    CONSTRAINT [FK_Gifts_Countries] FOREIGN KEY ([Country]) REFERENCES [Countries]([Id]),
	CONSTRAINT [FK_Gifts_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
	CONSTRAINT [FK_Gifts_GiftWishStatuses] FOREIGN KEY ([StatusId]) REFERENCES [GiftWishStatuses]([Id])


)

