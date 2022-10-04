CREATE DATABASE Redarbor_DB

USE Redarbor_DB

CREATE TABLE [dbo].[Employee]
(
	[EmployeeId] [int] IDENTITY(1,1) PRIMARY KEY,
	[CompanyId] [int] NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[DeletedOn] DATETIME,
	[Email] VARCHAR(100),
	[Fax] VARCHAR(11),
	[Name] VARCHAR(100) NOT NULL,
	[Lastlogin] DATETIME,
	[Password] VARCHAR(100) NOT NULL,
	[PortalId] INT NOT NULL,
	[RoleId] INT NOT NULL,
	[StatusId] INT NOT NULL,
	[Telephone] VARCHAR(11),
	[UpdatedOn] DATETIME,
	[Username] VARCHAR(100)
)