/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
USE [GameOfDrones]
GO

IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE name = 'gameconnection')
BEGIN
USE [master]
CREATE LOGIN [gameconnection] WITH PASSWORD=N'654321', DEFAULT_DATABASE=[GameOfDrones], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
END

USE [GameOfDrones]
GO
IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE name = 'gameconnection')
BEGIN
CREATE USER [gameconnection] FOR LOGIN [gameconnection]
ALTER ROLE [db_owner] ADD MEMBER [gameconnection]
END
