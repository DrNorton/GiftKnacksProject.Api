CREATE TABLE [dbo].[WishCategories]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [Name] NVARCHAR(150) NOT NULL, 
    [Description] NCHAR(250) NULL, 
    [ParentId] INT NULL
	 CONSTRAINT [FK_Parent_WishCategories] FOREIGN KEY ([ParentId]) REFERENCES [WishCategories]([Id]),
)
