USE master
GO

IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'BackendSr')
BEGIN
	DROP DATABASE BackendSr
END

CREATE DATABASE BackendSr
GO

USE BackendSr
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'TblPermissionTypes') 
BEGIN
	CREATE TABLE dbo.TblPermissionTypes(
		Id INT IDENTITY,
		Description VARCHAR(200),
		Active BIT DEFAULT 1
) 

	EXEC sys.sp_addextendedproperty
	@name = N'MS_Description',
    @value = N'Table showing the types of permits.',
    @level0type = N'SCHEMA',@level0name = N'dbo',
    @level1type = N'TABLE',	@level1name = N'TblPermissionTypes';

    EXEC sys.sp_addextendedproperty 
	@name = N'MS_Description',
    @value = N'Unique ID.',
    @level0type = N'SCHEMA',@level0name = N'dbo',
    @level1type = N'TABLE',	@level1name = N'TblPermissionTypes',
    @level2type = N'COLUMN',@level2name = N'Id' 

    EXEC sys.sp_addextendedproperty 
	@name = N'MS_Description',
    @value = N'Permission description.',
    @level0type = N'SCHEMA',@level0name = N'dbo',
    @level1type = N'TABLE',	@level1name = N'TblPermissionTypes',
    @level2type = N'COLUMN',@level2name = N'Description' 

	EXEC sys.sp_addextendedproperty 
	@name = N'MS_Description',
    @value = N'Register status.',
    @level0type = N'SCHEMA',@level0name = N'dbo',
    @level1type = N'TABLE',	@level1name = N'TblPermissionTypes',
    @level2type = N'COLUMN',@level2name = N'Active' 

	ALTER TABLE dbo.TblPermissionTypes ADD CONSTRAINT PK_TblPermissionTypes PRIMARY KEY (Id)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'TblPermissions') 
BEGIN
	CREATE TABLE dbo.TblPermissions(
		Id INT IDENTITY,
		EmployeeForename VARCHAR(200),
		EmployeeSurename VARCHAR(200),
		PermissionType INT,
		PermissionDate DATETIME,
		Active BIT DEFAULT 1
) 

	EXEC sys.sp_addextendedproperty
	@name = N'MS_Description',
    @value = N'Table where permits are recorded.',
    @level0type = N'SCHEMA',@level0name = N'dbo',
    @level1type = N'TABLE',	@level1name = N'TblPermissions';

    EXEC sys.sp_addextendedproperty 
	@name = N'MS_Description',
    @value = N'Unique ID',
    @level0type = N'SCHEMA',@level0name = N'dbo',
    @level1type = N'TABLE',	@level1name = N'TblPermissions',
    @level2type = N'COLUMN',@level2name = N'Id' 
	
	EXEC sys.sp_addextendedproperty 
	@name = N'MS_Description',
    @value = N'Employee Forename.',
    @level0type = N'SCHEMA',@level0name = N'dbo',
    @level1type = N'TABLE',	@level1name = N'TblPermissions',
    @level2type = N'COLUMN',@level2name = N'EmployeeForename' 
	
	EXEC sys.sp_addextendedproperty 
	@name = N'MS_Description',
    @value = N'Employee Surename.',
    @level0type = N'SCHEMA',@level0name = N'dbo',
    @level1type = N'TABLE',	@level1name = N'TblPermissions',
    @level2type = N'COLUMN',@level2name = N'EmployeeSurename' 

	EXEC sys.sp_addextendedproperty 
	@name = N'MS_Description',
    @value = N'Permission Type.',
    @level0type = N'SCHEMA',@level0name = N'dbo',
    @level1type = N'TABLE',	@level1name = N'TblPermissions',
    @level2type = N'COLUMN',@level2name = N'PermissionType'
	
	EXEC sys.sp_addextendedproperty 
	@name = N'MS_Description',
    @value = N'Permission granted on Date.',
    @level0type = N'SCHEMA',@level0name = N'dbo',
    @level1type = N'TABLE',	@level1name = N'TblPermissions',
    @level2type = N'COLUMN',@level2name = N'PermissionDate'
	
	EXEC sys.sp_addextendedproperty 
	@name = N'MS_Description',
    @value = N'Register status.',
    @level0type = N'SCHEMA',@level0name = N'dbo',
    @level1type = N'TABLE',	@level1name = N'TblPermissions',
    @level2type = N'COLUMN',@level2name = N'Active' 

	ALTER TABLE dbo.TblPermissions ADD CONSTRAINT PK_TblPermissions PRIMARY KEY (Id)
	ALTER TABLE dbo.TblPermissions ADD CONSTRAINT FK_TblPermissions_TblPermissionTypes FOREIGN KEY (PermissionType) REFERENCES TblPermissionTypes (Id)
END


IF NOT EXISTS (SELECT 1 FROM dbo.TblPermissionTypes WHERE Description ='First Permit')
BEGIN
    INSERT INTO dbo.TblPermissionTypes (Description) VALUES ('First Permit')
END

IF NOT EXISTS (SELECT 1 FROM dbo.TblPermissionTypes WHERE Description ='Second Permit')
BEGIN
    INSERT INTO dbo.TblPermissionTypes (Description) VALUES ('Second Permit')
END

IF NOT EXISTS (SELECT 1 FROM dbo.TblPermissionTypes WHERE Description ='Third Permit')
BEGIN
    INSERT INTO dbo.TblPermissionTypes (Description) VALUES ('Third Permit')
END

IF NOT EXISTS (SELECT 1 FROM dbo.TblPermissionTypes WHERE Description ='Fourth Permit')
BEGIN
    INSERT INTO dbo.TblPermissionTypes (Description) VALUES ('Fourth Permit')
END

IF NOT EXISTS (SELECT 1 FROM dbo.TblPermissionTypes WHERE Description ='Fifth Permit')
BEGIN
    INSERT INTO dbo.TblPermissionTypes (Description) VALUES ('Fifth Permit')
END
