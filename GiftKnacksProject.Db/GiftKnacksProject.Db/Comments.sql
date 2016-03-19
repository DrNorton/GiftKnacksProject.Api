CREATE TABLE [dbo].[Comments]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [ParentCommentId] BIGINT NULL, 
    [UserId] BIGINT NOT NULL, 
    [Text] NVARCHAR(300) NOT NULL, 
    [UpdateTime] DATETIME NOT NULL, 
    CONSTRAINT [FK_Comments_Comments_Parent] FOREIGN KEY ([ParentCommentId]) REFERENCES [Comments]([Id]),
	CONSTRAINT [FK_Comments_ToUsers] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]) ON DELETE CASCADE
)
