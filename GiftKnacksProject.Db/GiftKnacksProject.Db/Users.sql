CREATE TABLE [dbo].[Users]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Email] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(300) NOT NULL, 
    [DateRegister] DATETIME NOT NULL, 
    [ConfirmMail] BIT NOT NULL DEFAULT 0, 
    [EmailStamp] NVARCHAR(200) NULL, 
    [LastLoginTime] DATETIME NULL, 
    [AvgRate] FLOAT NOT NULL DEFAULT 0, 
    [TotalClosed] BIGINT NOT NULL DEFAULT 0 
)
