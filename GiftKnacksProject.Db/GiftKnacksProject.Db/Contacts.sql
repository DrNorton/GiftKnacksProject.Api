CREATE TABLE [dbo].[Contacts]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity(1,1),
	[ContactTypeId] INT NOT NULL, 
	[UserId] bigint Not null
    CONSTRAINT [FK_ContactsTypes_ToContacts] FOREIGN KEY ([ContactTypeId]) REFERENCES [ContactTypes]([Id]), 
    [Value] NVARCHAR(50) NULL, 
    [MainContact] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Users_ToContacts] FOREIGN KEY ([UserId]) REFERENCES [Profiles]([Id])
)
