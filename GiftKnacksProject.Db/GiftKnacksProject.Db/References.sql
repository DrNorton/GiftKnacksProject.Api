CREATE TABLE [dbo].[References]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Text] NVARCHAR(250) NULL, 
    [Rate] tinyint NULL, 
    [ReplyerId] BIGINT NULL, 
    [OwnerId] BIGINT NULL,
	[CreatedTime] DATETIME NULL, 
    CONSTRAINT [FK_References_ReplyUserId] FOREIGN KEY ([ReplyerId]) REFERENCES [Users]([Id]),
	CONSTRAINT [FK_References_OwnerUserId] FOREIGN KEY ([OwnerId]) REFERENCES [Users]([Id])
		
)

GO

CREATE TRIGGER [dbo].[AvgCalculateTrigger]
    ON [dbo].[References]
    FOR  INSERT
    AS
    BEGIN
        DECLARE @rate tinyint,@owner bigint,@avg float
        SET NoCount ON
		SELECT @rate = Rate FROM inserted
		SELECT @owner = OwnerId FROM inserted
		SELECT @avg = (SELECT AVG(Rate) FROM [dbo].[References] WHERE OwnerId = @owner)
		BEGIN
		   Update Users Set AvgRate=@avg
		END
    END