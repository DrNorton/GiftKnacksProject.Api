CREATE TABLE [dbo].[Profiles]
(
	[Id] BIGINT NOT NULL, 
    [FirstName] NVARCHAR(50) NULL, 
	[LastName] NVARCHAR(50) NULL, 
	[Country] NVARCHAR(2) NULL, 
	[City] NVARCHAR(150) NULL , 
    [AvatarUrl] NVARCHAR(250) NULL, 
    [AboutMe] NVARCHAR(250) NULL,
	[Birthday] datetime, 
	[HideBirthday] BIT NOT NULL DEFAULT 0,
    [IsFilled] BIT NOT NULL DEFAULT 0, 
    [LastLoginTime] DATETIME NULL,
    [Gender] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_Profiles] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Profiles_USERS] FOREIGN KEY ([Id]) REFERENCES [Users]([Id]),
	CONSTRAINT [FK_Profiles_Countries] FOREIGN KEY ([Country]) REFERENCES [Countries]([Id]),
	
)

