
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/25/2014 18:50:15
-- Generated from EDMX file: F:\Projects\gst-mart\GST_DB\GST.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [GSTMART];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CompanyGstSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GstSystems] DROP CONSTRAINT [FK_CompanyGstSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_PermissionUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Permissions] DROP CONSTRAINT [FK_PermissionUser];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyIndustry]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Industries] DROP CONSTRAINT [FK_CompanyIndustry];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyCurrencySchedular]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CurrencySchedulars] DROP CONSTRAINT [FK_CompanyCurrencySchedular];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AdminPaths]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdminPaths];
GO
IF OBJECT_ID(N'[dbo].[Companies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Companies];
GO
IF OBJECT_ID(N'[dbo].[Cycles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cycles];
GO
IF OBJECT_ID(N'[dbo].[GstSystems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GstSystems];
GO
IF OBJECT_ID(N'[dbo].[IpAddresses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IpAddresses];
GO
IF OBJECT_ID(N'[dbo].[LDAPs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LDAPs];
GO
IF OBJECT_ID(N'[dbo].[Permissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Permissions];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[CompanyUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyUsers];
GO
IF OBJECT_ID(N'[dbo].[Schedulars]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Schedulars];
GO
IF OBJECT_ID(N'[dbo].[Industries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Industries];
GO
IF OBJECT_ID(N'[dbo].[TaxCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaxCodes];
GO
IF OBJECT_ID(N'[dbo].[CustomCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomCodes];
GO
IF OBJECT_ID(N'[dbo].[TransactionTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionTypes];
GO
IF OBJECT_ID(N'[dbo].[CurrencyExchanges]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CurrencyExchanges];
GO
IF OBJECT_ID(N'[dbo].[OriginalCodeTaxes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OriginalCodeTaxes];
GO
IF OBJECT_ID(N'[dbo].[CurrencyTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CurrencyTypes];
GO
IF OBJECT_ID(N'[dbo].[CurrencyGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CurrencyGroups];
GO
IF OBJECT_ID(N'[dbo].[AuditLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AuditLogs];
GO
IF OBJECT_ID(N'[dbo].[Configurations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Configurations];
GO
IF OBJECT_ID(N'[dbo].[CurrencySchedulars]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CurrencySchedulars];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AdminPaths'
CREATE TABLE [dbo].[AdminPaths] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [adminpath1] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyName] nvarchar(max)  NOT NULL,
    [UserId] int  NULL,
    [Permission] nvarchar(max)  NOT NULL,
    [CompanyID] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL,
    [CurrencySchedularId] int  NULL
);
GO

-- Creating table 'Cycles'
CREATE TABLE [dbo].[Cycles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SystemId] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [Supply] nvarchar(max)  NOT NULL,
    [Purchase] nvarchar(max)  NOT NULL,
    [Ledger] nvarchar(max)  NOT NULL,
    [Footer] nvarchar(max)  NOT NULL,
    [Company] nvarchar(max)  NOT NULL,
    [CycleID] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NULL,
    [UserId] nvarchar(max)  NULL
);
GO

-- Creating table 'GstSystems'
CREATE TABLE [dbo].[GstSystems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SystemName] nvarchar(max)  NOT NULL,
    [SystemID] nvarchar(max)  NOT NULL,
    [CompanyId] int  NOT NULL
);
GO

-- Creating table 'IpAddresses'
CREATE TABLE [dbo].[IpAddresses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IP] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'LDAPs'
CREATE TABLE [dbo].[LDAPs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DomianName] nvarchar(max)  NOT NULL,
    [PortNumber] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [CompanyId] nvarchar(max)  NOT NULL,
    [CNBN] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Permissions'
CREATE TABLE [dbo].[Permissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreateCycle] nvarchar(max)  NOT NULL,
    [AccessSetting] nvarchar(max)  NOT NULL,
    [DownloadData] nvarchar(max)  NOT NULL,
    [PrintReport] nvarchar(max)  NOT NULL,
    [User_Id] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Usertype] nvarchar(max)  NOT NULL,
    [Createdate] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Mobilenumber] nvarchar(max)  NULL,
    [AdminID] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NULL,
    [CompanyID] nvarchar(max)  NULL,
    [LastLoginDate] nvarchar(max)  NULL,
    [Guid] nvarchar(max)  NULL
);
GO

-- Creating table 'CompanyUsers'
CREATE TABLE [dbo].[CompanyUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UID] nvarchar(max)  NOT NULL,
    [CID] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Schedulars'
CREATE TABLE [dbo].[Schedulars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Module] nvarchar(max)  NOT NULL,
    [Path] nvarchar(max)  NOT NULL,
    [Frequency] nvarchar(max)  NULL,
    [Unit] nvarchar(max)  NULL,
    [STime] nvarchar(max)  NULL,
    [Status] nvarchar(max)  NOT NULL,
    [LastRun] nvarchar(max)  NULL,
    [NextRun] nvarchar(max)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CompanyID] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Industries'
CREATE TABLE [dbo].[Industries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [CompanyId] int  NOT NULL
);
GO

-- Creating table 'TaxCodes'
CREATE TABLE [dbo].[TaxCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] nvarchar(max)  NOT NULL,
    [SystemId] nvarchar(max)  NOT NULL,
    [CustomCode] nvarchar(max)  NOT NULL,
    [OriginalCode] nvarchar(max)  NOT NULL,
    [TransactionType] nvarchar(max)  NOT NULL,
    [TaxRate] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [ReferenceNumber] nvarchar(max)  NOT NULL,
    [GST03Fields] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL,
    [DateType] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CustomCodes'
CREATE TABLE [dbo].[CustomCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Customcode] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TransactionTypes'
CREATE TABLE [dbo].[TransactionTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Transactiontype] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CurrencyExchanges'
CREATE TABLE [dbo].[CurrencyExchanges] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] nvarchar(max)  NOT NULL,
    [CurrencyCode] nvarchar(max)  NOT NULL,
    [Discription] nvarchar(max)  NOT NULL,
    [ConversionRate] nvarchar(max)  NOT NULL,
    [CompanyId] nvarchar(max)  NOT NULL,
    [CurrencyDate] datetime  NOT NULL
);
GO

-- Creating table 'OriginalCodeTaxes'
CREATE TABLE [dbo].[OriginalCodeTaxes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OriginalCode] nvarchar(max)  NOT NULL,
    [TaxRate] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CurrencyTypes'
CREATE TABLE [dbo].[CurrencyTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [currencytype] nvarchar(max)  NOT NULL,
    [currencycode] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CurrencyGroups'
CREATE TABLE [dbo].[CurrencyGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CurrencyGroupName] nvarchar(max)  NOT NULL,
    [CurrencyType] nvarchar(max)  NOT NULL,
    [TaxCode] nvarchar(max)  NOT NULL,
    [CreatedDate] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AuditLogs'
CREATE TABLE [dbo].[AuditLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [IpAddress] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Configurations'
CREATE TABLE [dbo].[Configurations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [logoname] nvarchar(max)  NOT NULL,
    [dateformat] nvarchar(max)  NOT NULL,
    [timeformat] nvarchar(max)  NOT NULL,
    [dbaddress] nvarchar(max)  NOT NULL,
    [dbname] nvarchar(max)  NOT NULL,
    [dbuserid] nvarchar(max)  NOT NULL,
    [dbpwd] nvarchar(max)  NOT NULL,
    [directorypath] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CurrencySchedulars'
CREATE TABLE [dbo].[CurrencySchedulars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FeedUrl] nvarchar(max)  NOT NULL,
    [FrequencyUnit] nvarchar(max)  NOT NULL,
    [Time] nvarchar(max)  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [Company_Id] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AdminPaths'
ALTER TABLE [dbo].[AdminPaths]
ADD CONSTRAINT [PK_AdminPaths]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cycles'
ALTER TABLE [dbo].[Cycles]
ADD CONSTRAINT [PK_Cycles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GstSystems'
ALTER TABLE [dbo].[GstSystems]
ADD CONSTRAINT [PK_GstSystems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'IpAddresses'
ALTER TABLE [dbo].[IpAddresses]
ADD CONSTRAINT [PK_IpAddresses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LDAPs'
ALTER TABLE [dbo].[LDAPs]
ADD CONSTRAINT [PK_LDAPs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Permissions'
ALTER TABLE [dbo].[Permissions]
ADD CONSTRAINT [PK_Permissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CompanyUsers'
ALTER TABLE [dbo].[CompanyUsers]
ADD CONSTRAINT [PK_CompanyUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Schedulars'
ALTER TABLE [dbo].[Schedulars]
ADD CONSTRAINT [PK_Schedulars]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Industries'
ALTER TABLE [dbo].[Industries]
ADD CONSTRAINT [PK_Industries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaxCodes'
ALTER TABLE [dbo].[TaxCodes]
ADD CONSTRAINT [PK_TaxCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomCodes'
ALTER TABLE [dbo].[CustomCodes]
ADD CONSTRAINT [PK_CustomCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionTypes'
ALTER TABLE [dbo].[TransactionTypes]
ADD CONSTRAINT [PK_TransactionTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CurrencyExchanges'
ALTER TABLE [dbo].[CurrencyExchanges]
ADD CONSTRAINT [PK_CurrencyExchanges]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OriginalCodeTaxes'
ALTER TABLE [dbo].[OriginalCodeTaxes]
ADD CONSTRAINT [PK_OriginalCodeTaxes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CurrencyTypes'
ALTER TABLE [dbo].[CurrencyTypes]
ADD CONSTRAINT [PK_CurrencyTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CurrencyGroups'
ALTER TABLE [dbo].[CurrencyGroups]
ADD CONSTRAINT [PK_CurrencyGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AuditLogs'
ALTER TABLE [dbo].[AuditLogs]
ADD CONSTRAINT [PK_AuditLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Configurations'
ALTER TABLE [dbo].[Configurations]
ADD CONSTRAINT [PK_Configurations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CurrencySchedulars'
ALTER TABLE [dbo].[CurrencySchedulars]
ADD CONSTRAINT [PK_CurrencySchedulars]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CompanyId] in table 'GstSystems'
ALTER TABLE [dbo].[GstSystems]
ADD CONSTRAINT [FK_CompanyGstSystem]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Companies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyGstSystem'
CREATE INDEX [IX_FK_CompanyGstSystem]
ON [dbo].[GstSystems]
    ([CompanyId]);
GO

-- Creating foreign key on [User_Id] in table 'Permissions'
ALTER TABLE [dbo].[Permissions]
ADD CONSTRAINT [FK_PermissionUser]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PermissionUser'
CREATE INDEX [IX_FK_PermissionUser]
ON [dbo].[Permissions]
    ([User_Id]);
GO

-- Creating foreign key on [CompanyId] in table 'Industries'
ALTER TABLE [dbo].[Industries]
ADD CONSTRAINT [FK_CompanyIndustry]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Companies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyIndustry'
CREATE INDEX [IX_FK_CompanyIndustry]
ON [dbo].[Industries]
    ([CompanyId]);
GO

-- Creating foreign key on [Company_Id] in table 'CurrencySchedulars'
ALTER TABLE [dbo].[CurrencySchedulars]
ADD CONSTRAINT [FK_CompanyCurrencySchedular]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyCurrencySchedular'
CREATE INDEX [IX_FK_CompanyCurrencySchedular]
ON [dbo].[CurrencySchedulars]
    ([Company_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------