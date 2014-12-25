
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/18/2014 13:27:46
-- Generated from EDMX file: D:\Code\GST_Mart\dev\GST_DB\Stage.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [STAGEDBGSTMASRT];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ErrorLoggers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ErrorLoggers];
GO
IF OBJECT_ID(N'[dbo].[tbl_DupLedger]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_DupLedger];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTCompany]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTCompany];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTDupCompany]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTDupCompany];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTDupPurchase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTDupPurchase];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTErrorCompany]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTErrorCompany];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTErrorFooter]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTErrorFooter];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTFooter]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTFooter];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTMissingCompany]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTMissingCompany];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTMissingFooter]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTMissingFooter];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTMissingPurchase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTMissingPurchase];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTPurchase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTPurchase];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTPurchaseError]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTPurchaseError];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTSupply]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTSupply];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTSupplyError]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTSupplyError];
GO
IF OBJECT_ID(N'[dbo].[tbl_Ledger]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Ledger];
GO
IF OBJECT_ID(N'[dbo].[tbl_MissingLedger]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_MissingLedger];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTDupSupply]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTDupSupply];
GO
IF OBJECT_ID(N'[dbo].[tbl_GSTMissingSuplly]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GSTMissingSuplly];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ErrorLoggers'
CREATE TABLE [dbo].[ErrorLoggers] (
    [Flat_File_Source_Error_Output_Column] varchar(max)  NULL,
    [ErrorCode] int  NULL,
    [ErrorColumn] int  NULL,
    [id] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CycleID] nvarchar(50)  NULL,
    [CompanyID] nvarchar(50)  NULL,
    [Error_Description] varchar(max)  NULL,
    [Error_ColumnName] varchar(max)  NULL
);
GO

-- Creating table 'tbl_DupLedger'
CREATE TABLE [dbo].[tbl_DupLedger] (
    [Ledger_ID] bigint IDENTITY(1,1) NOT NULL,
    [Company_Code] varchar(50)  NULL,
    [Record_Identifier] nvarchar(1)  NULL,
    [Transaction_Date] datetime  NULL,
    [Account_ID] varchar(50)  NULL,
    [Account_Name] varchar(100)  NULL,
    [Transaction_Description] varchar(100)  NULL,
    [Transaction_ID] varchar(50)  NULL,
    [Source_Document_ID] varchar(50)  NULL,
    [Source_Type] varchar(50)  NULL,
    [Transaction_Type] varchar(50)  NULL,
    [Tax_Code] varchar(50)  NULL,
    [Name] varchar(100)  NULL,
    [Debit] decimal(19,4)  NULL,
    [Credit] decimal(19,4)  NULL,
    [Balance] decimal(19,4)  NULL,
    [CycleID] nvarchar(50)  NULL,
    [CreatedDate] datetime  NULL,
    [CompanyID] nvarchar(50)  NULL
);
GO

-- Creating table 'tbl_GSTCompany'
CREATE TABLE [dbo].[tbl_GSTCompany] (
    [Company_Code] varchar(50)  NOT NULL,
    [Record_Identifier] varchar(50)  NULL,
    [Company_Name] varchar(50)  NULL,
    [Company_BRN] varchar(50)  NOT NULL,
    [Company_GST_No] varchar(50)  NULL,
    [Period_Start] datetime  NULL,
    [Period_End] datetime  NULL,
    [File_Creation_Date] datetime  NULL,
    [Product_Version] varchar(50)  NULL,
    [GAF_Version] varchar(50)  NULL,
    [CycleID] nvarchar(50)  NULL,
    [CompanyID] nvarchar(50)  NULL,
    [Id] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tbl_GSTDupCompany'
CREATE TABLE [dbo].[tbl_GSTDupCompany] (
    [Company_Code] varchar(50)  NOT NULL,
    [Record_Identifier] varchar(50)  NOT NULL,
    [Company_Name] varchar(50)  NOT NULL,
    [Company_BRN] varchar(50)  NOT NULL,
    [Company_GST_No] varchar(50)  NOT NULL,
    [Period_Start] datetime  NOT NULL,
    [Period_End] datetime  NOT NULL,
    [File_Creation_Date] datetime  NOT NULL,
    [Product_Version] varchar(50)  NOT NULL,
    [GAF_Version] varchar(50)  NOT NULL,
    [CycleID] nvarchar(50)  NOT NULL,
    [CompanyID] nvarchar(50)  NOT NULL,
    [Id] bigint  NOT NULL
);
GO

-- Creating table 'tbl_GSTDupPurchase'
CREATE TABLE [dbo].[tbl_GSTDupPurchase] (
    [Purchase_ID] bigint IDENTITY(1,1) NOT NULL,
    [Company_Code] varchar(50)  NULL,
    [Record_Identifier] varchar(50)  NULL,
    [Supplier_BRN] varchar(50)  NULL,
    [Supplier_Name] varchar(100)  NULL,
    [Invoice_Date] datetime  NULL,
    [Invoice_No] varchar(50)  NULL,
    [Import_Declaration_No] varchar(50)  NULL,
    [Line_No] varchar(50)  NULL,
    [Product_Description] varchar(200)  NULL,
    [Purchase_Value_MYR] decimal(18,2)  NULL,
    [Purchase_Value_GST_Amount] decimal(18,2)  NULL,
    [Tax_Code] varchar(50)  NULL,
    [Foreign_Currency_Code] varchar(50)  NULL,
    [Purchase_Foreign_Currency_Amount] decimal(18,2)  NULL,
    [Purchase_Currency_Purchase_Amount_GST] decimal(18,2)  NULL,
    [YR] int  NULL,
    [MTH] int  NULL,
    [Cycle_ID] nvarchar(max)  NULL,
    [Value_Exempt_Supplies] decimal(11,2)  NULL,
    [Transaction_Type] nvarchar(50)  NULL,
    [Supplier_Number] nvarchar(20)  NULL,
    [Product_Code] nvarchar(20)  NULL,
    [CompanyID] nvarchar(max)  NULL,
    [CreateDate] datetime  NULL
);
GO

-- Creating table 'tbl_GSTErrorCompany'
CREATE TABLE [dbo].[tbl_GSTErrorCompany] (
    [Flat_File_Source_Error_Output_Column] varchar(max)  NULL,
    [ErrorCode] int  NULL,
    [ErrorColumn] int  NULL,
    [id] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CycleID] nvarchar(50)  NULL,
    [CompanyID] nvarchar(50)  NULL,
    [Error_Description] varchar(max)  NULL,
    [Error_ColumnName] varchar(max)  NULL
);
GO

-- Creating table 'tbl_GSTErrorFooter'
CREATE TABLE [dbo].[tbl_GSTErrorFooter] (
    [Flat_File_Source_Error_Output_Column] varchar(max)  NULL,
    [ErrorCode] int  NULL,
    [ErrorColumn] int  NULL,
    [id] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CycleID] nvarchar(50)  NULL,
    [CompanyID] nvarchar(50)  NULL,
    [Error_Description] varchar(max)  NULL,
    [Error_ColumnName] varchar(max)  NULL
);
GO

-- Creating table 'tbl_GSTFooter'
CREATE TABLE [dbo].[tbl_GSTFooter] (
    [Company_Code] varchar(50)  NOT NULL,
    [Record_Identifier] varchar(50)  NULL,
    [Purchase_Count] decimal(11,0)  NULL,
    [Purchase_Amount_Sum] decimal(11,2)  NULL,
    [Purchase_GST_Amount_GST] decimal(11,2)  NULL,
    [Supply_Count] decimal(11,0)  NULL,
    [Supply_Amount_Sum] decimal(11,2)  NULL,
    [Supply_GST_Amount_Sum] decimal(11,2)  NULL,
    [Ledger_Count] decimal(11,0)  NULL,
    [Debit_Sum] decimal(11,2)  NULL,
    [Credit_Sum] decimal(11,2)  NULL,
    [Balance_Sum] decimal(11,2)  NULL,
    [CycleID] nvarchar(50)  NULL,
    [CompanyID] nvarchar(50)  NULL,
    [id] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tbl_GSTMissingCompany'
CREATE TABLE [dbo].[tbl_GSTMissingCompany] (
    [Company_Code] varchar(50)  NOT NULL,
    [Record_Identifier] varchar(50)  NULL,
    [Company_Name] varchar(50)  NULL,
    [Company_BRN] varchar(50)  NOT NULL,
    [Company_GST_No] varchar(50)  NULL,
    [Period_Start] datetime  NULL,
    [Period_End] datetime  NULL,
    [File_Creation_Date] datetime  NULL,
    [Product_Version] varchar(50)  NULL,
    [GAF_Version] varchar(50)  NULL,
    [CycleID] nvarchar(50)  NULL,
    [CompanyID] nvarchar(50)  NULL,
    [Id] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tbl_GSTMissingFooter'
CREATE TABLE [dbo].[tbl_GSTMissingFooter] (
    [Company_Code] varchar(50)  NOT NULL,
    [Record_Identifier] varchar(50)  NOT NULL,
    [Purchase_Count] decimal(11,0)  NOT NULL,
    [Purchase_Amount_Sum] decimal(11,2)  NOT NULL,
    [Purchase_GST_Amount_GST] decimal(11,2)  NOT NULL,
    [Supply_Count] decimal(11,0)  NOT NULL,
    [Supply_Amount_Sum] decimal(11,2)  NOT NULL,
    [Supply_GST_Amount_Sum] decimal(11,2)  NOT NULL,
    [Ledger_Count] decimal(11,0)  NOT NULL,
    [Debit_Sum] decimal(11,2)  NOT NULL,
    [Credit_Sum] decimal(11,2)  NOT NULL,
    [Balance_Sum] decimal(11,2)  NOT NULL,
    [CycleID] nvarchar(50)  NOT NULL,
    [CompanyID] nvarchar(50)  NOT NULL,
    [id] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tbl_GSTMissingPurchase'
CREATE TABLE [dbo].[tbl_GSTMissingPurchase] (
    [Purchase_ID] bigint IDENTITY(1,1) NOT NULL,
    [Company_Code] varchar(20)  NULL,
    [Record_Identifier] varchar(50)  NULL,
    [Supplier_BRN] varchar(50)  NULL,
    [Supplier_Name] varchar(100)  NULL,
    [Invoice_Date] datetime  NULL,
    [Invoice_No] varchar(50)  NULL,
    [Import_Declaration_No] varchar(50)  NULL,
    [Line_No] varchar(50)  NULL,
    [Product_Description] varchar(200)  NULL,
    [Purchase_Value_MYR] decimal(18,2)  NULL,
    [Purchase_Value_GST_Amount] decimal(18,2)  NULL,
    [Tax_Code] varchar(50)  NULL,
    [Foreign_Currency_Code] varchar(50)  NULL,
    [Purchase_Foreign_Currency_Amount] decimal(18,2)  NULL,
    [Purchase_Currency_Purchase_Amount_GST] decimal(18,2)  NULL,
    [YR] int  NULL,
    [MTH] int  NULL,
    [Cycle_ID] nvarchar(max)  NULL,
    [Value_Exempt_Supplies] decimal(11,2)  NULL,
    [Transaction_Type] varchar(50)  NULL,
    [Supplier_Number] varchar(50)  NULL,
    [Product_Code] varchar(50)  NULL,
    [CompanyID] nvarchar(max)  NULL,
    [CreateDate] datetime  NULL
);
GO

-- Creating table 'tbl_GSTPurchase'
CREATE TABLE [dbo].[tbl_GSTPurchase] (
    [Purchase_ID] bigint IDENTITY(1,1) NOT NULL,
    [Company_Code] varchar(50)  NULL,
    [Record_Identifier] varchar(50)  NULL,
    [Supplier_BRN] varchar(50)  NULL,
    [Supplier_Name] varchar(100)  NULL,
    [Invoice_Date] datetime  NULL,
    [Invoice_No] varchar(50)  NULL,
    [Import_Declaration_No] varchar(50)  NULL,
    [Line_No] varchar(50)  NULL,
    [Product_Description] varchar(200)  NULL,
    [Purchase_Value_MYR] decimal(18,2)  NULL,
    [Purchase_Value_GST_Amount] decimal(18,2)  NULL,
    [Tax_Code] varchar(50)  NULL,
    [Foreign_Currency_Code] varchar(50)  NULL,
    [Purchase_Foreign_Currency_Amount] decimal(18,2)  NULL,
    [Purchase_Currency_Purchase_Amount_GST] decimal(18,2)  NULL,
    [YR] int  NULL,
    [MTH] int  NULL,
    [Cycle_ID] nvarchar(max)  NULL,
    [Value_Exempt_Supplies] decimal(11,2)  NULL,
    [Transaction_Type] varchar(50)  NULL,
    [Supplier_Number] varchar(50)  NULL,
    [Product_Code] varchar(50)  NULL,
    [CompanyID] nvarchar(50)  NULL,
    [CreateDate] datetime  NULL
);
GO

-- Creating table 'tbl_GSTPurchaseError'
CREATE TABLE [dbo].[tbl_GSTPurchaseError] (
    [Flat_File_Source_Error_Output_Column] varchar(max)  NULL,
    [ErrorCode] int  NULL,
    [ErrorColumn] int  NULL,
    [id] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CycleID] nvarchar(50)  NULL,
    [CompanyID] nvarchar(50)  NULL,
    [Error_Description] varchar(max)  NULL,
    [Error_ColumnName] varchar(max)  NULL
);
GO

-- Creating table 'tbl_GSTSupply'
CREATE TABLE [dbo].[tbl_GSTSupply] (
    [Supply_ID] bigint IDENTITY(1,1) NOT NULL,
    [Company_Code] varchar(50)  NULL,
    [Record_Identifier] varchar(50)  NULL,
    [Customer_Name] varchar(100)  NULL,
    [Customer_BRN] varchar(50)  NULL,
    [Invoice_Date] datetime  NULL,
    [Invoice_No] varchar(50)  NULL,
    [Line_No] varchar(50)  NULL,
    [Product_Description] varchar(200)  NULL,
    [Sales_Value_MYR] decimal(18,2)  NULL,
    [Sales_Value_GST_Amount] decimal(18,2)  NULL,
    [Tax_Code] varchar(50)  NOT NULL,
    [Country] varchar(50)  NULL,
    [Foreign_Currency_Code] varchar(50)  NULL,
    [Sales_Foreign_Currency_Amount] decimal(18,2)  NULL,
    [Sales_Currency_Sales_Amount_GST] decimal(18,2)  NULL,
    [YR] int  NULL,
    [MTH] int  NULL,
    [Cycle_ID] nvarchar(max)  NULL,
    [Transaction_Type] varchar(50)  NULL,
    [Customer_Code] varchar(50)  NULL,
    [Product_Code] varchar(50)  NULL,
    [Destination_Export] varchar(50)  NULL,
    [CompanyID] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_GSTSupplyError'
CREATE TABLE [dbo].[tbl_GSTSupplyError] (
    [Flat_File_Source_Error_Output_Column] varchar(max)  NULL,
    [ErrorCode] int  NULL,
    [ErrorColumn] int  NULL,
    [id] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CycleID] nvarchar(50)  NULL,
    [CompanyID] nvarchar(50)  NULL,
    [Error_Description] varchar(max)  NULL,
    [Error_ColumnName] varchar(max)  NULL
);
GO

-- Creating table 'tbl_Ledger'
CREATE TABLE [dbo].[tbl_Ledger] (
    [Ledger_ID] bigint IDENTITY(1,1) NOT NULL,
    [Company_Code] varchar(50)  NULL,
    [Record_Identifier] nvarchar(1)  NULL,
    [Transaction_Date] datetime  NULL,
    [Account_ID] varchar(50)  NULL,
    [Account_Name] varchar(100)  NULL,
    [Transaction_Description] varchar(100)  NULL,
    [Transaction_ID] varchar(50)  NULL,
    [Source_Document_ID] varchar(50)  NULL,
    [Source_Type] varchar(50)  NULL,
    [Transaction_Type] varchar(50)  NULL,
    [Tax_Code] varchar(50)  NULL,
    [Name] varchar(100)  NULL,
    [Debit] decimal(19,4)  NULL,
    [Credit] decimal(19,4)  NULL,
    [Balance] decimal(19,4)  NULL,
    [CycleID] nvarchar(50)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CompanyID] nvarchar(50)  NULL
);
GO

-- Creating table 'tbl_MissingLedger'
CREATE TABLE [dbo].[tbl_MissingLedger] (
    [Ledger_ID] bigint IDENTITY(1,1) NOT NULL,
    [Company_Code] varchar(50)  NULL,
    [Record_Identifier] nvarchar(1)  NULL,
    [Transaction_Date] datetime  NULL,
    [Account_ID] varchar(50)  NULL,
    [Account_Name] varchar(100)  NULL,
    [Transaction_Description] varchar(100)  NULL,
    [Name] varchar(100)  NULL,
    [Transaction_ID] varchar(50)  NULL,
    [Source_Document_ID] varchar(50)  NULL,
    [Source_Type] varchar(50)  NULL,
    [Debit] decimal(18,2)  NULL,
    [Credit] decimal(18,2)  NULL,
    [Balance] decimal(18,2)  NULL,
    [Transaction_Type] varchar(50)  NULL,
    [Tax_Code] varchar(50)  NULL,
    [CompanyID] nvarchar(max)  NOT NULL,
    [CycleID] nvarchar(max)  NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'tbl_GSTDupSupply'
CREATE TABLE [dbo].[tbl_GSTDupSupply] (
    [Supply_ID] bigint IDENTITY(1,1) NOT NULL,
    [Company_Code] varchar(50)  NULL,
    [Record_Identifier] varchar(50)  NULL,
    [Customer_Name] varchar(100)  NULL,
    [Customer_BRN] varchar(50)  NULL,
    [Invoice_Date] datetime  NULL,
    [Invoice_No] varchar(50)  NULL,
    [Line_No] varchar(50)  NULL,
    [Product_Description] varchar(200)  NULL,
    [Sales_Value_MYR] decimal(18,2)  NULL,
    [Sales_Value_GST_Amount] decimal(18,2)  NULL,
    [Tax_Code] varchar(50)  NULL,
    [Country] varchar(50)  NULL,
    [Foreign_Currency_Code] varchar(50)  NULL,
    [Sales_Foreign_Currency_Amount] decimal(18,2)  NULL,
    [Sales_Currency_Sales_Amount_GST] decimal(18,2)  NULL,
    [YR] int  NULL,
    [MTH] int  NULL,
    [Cycle_ID] nvarchar(max)  NULL,
    [Transaction_Type] varchar(50)  NULL,
    [Customer_Code] varchar(50)  NULL,
    [Product_Code] varchar(50)  NULL,
    [Destination_Export] varchar(50)  NULL,
    [CompanyID] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_GSTMissingSuplly'
CREATE TABLE [dbo].[tbl_GSTMissingSuplly] (
    [Supply_ID] bigint IDENTITY(1,1) NOT NULL,
    [Company_Code] varchar(50)  NULL,
    [Record_Identifier] varchar(50)  NULL,
    [Customer_Name] varchar(100)  NULL,
    [Customer_BRN] varchar(50)  NULL,
    [Invoice_Date] datetime  NULL,
    [Invoice_No] varchar(50)  NULL,
    [Line_No] varchar(50)  NULL,
    [Product_Description] varchar(200)  NULL,
    [Sales_Value_MYR] decimal(18,2)  NULL,
    [Sales_Value_GST_Amount] decimal(18,2)  NULL,
    [Tax_Code] varchar(50)  NULL,
    [Country] varchar(50)  NULL,
    [Foreign_Currency_Code] varchar(50)  NULL,
    [Sales_Foreign_Currency_Amount] decimal(18,2)  NULL,
    [Sales_Currency_Sales_Amount_GST] decimal(18,2)  NULL,
    [YR] int  NULL,
    [MTH] int  NULL,
    [Cycle_ID] nvarchar(max)  NULL,
    [Transaction_Type] varchar(50)  NULL,
    [Customer_Code] varchar(50)  NULL,
    [Product_Code] varchar(50)  NULL,
    [Destination_Export] varchar(50)  NULL,
    [CompanyID] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'ErrorLoggers'
ALTER TABLE [dbo].[ErrorLoggers]
ADD CONSTRAINT [PK_ErrorLoggers]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Ledger_ID] in table 'tbl_DupLedger'
ALTER TABLE [dbo].[tbl_DupLedger]
ADD CONSTRAINT [PK_tbl_DupLedger]
    PRIMARY KEY CLUSTERED ([Ledger_ID] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_GSTCompany'
ALTER TABLE [dbo].[tbl_GSTCompany]
ADD CONSTRAINT [PK_tbl_GSTCompany]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_GSTDupCompany'
ALTER TABLE [dbo].[tbl_GSTDupCompany]
ADD CONSTRAINT [PK_tbl_GSTDupCompany]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Purchase_ID] in table 'tbl_GSTDupPurchase'
ALTER TABLE [dbo].[tbl_GSTDupPurchase]
ADD CONSTRAINT [PK_tbl_GSTDupPurchase]
    PRIMARY KEY CLUSTERED ([Purchase_ID] ASC);
GO

-- Creating primary key on [id] in table 'tbl_GSTErrorCompany'
ALTER TABLE [dbo].[tbl_GSTErrorCompany]
ADD CONSTRAINT [PK_tbl_GSTErrorCompany]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tbl_GSTErrorFooter'
ALTER TABLE [dbo].[tbl_GSTErrorFooter]
ADD CONSTRAINT [PK_tbl_GSTErrorFooter]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tbl_GSTFooter'
ALTER TABLE [dbo].[tbl_GSTFooter]
ADD CONSTRAINT [PK_tbl_GSTFooter]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_GSTMissingCompany'
ALTER TABLE [dbo].[tbl_GSTMissingCompany]
ADD CONSTRAINT [PK_tbl_GSTMissingCompany]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'tbl_GSTMissingFooter'
ALTER TABLE [dbo].[tbl_GSTMissingFooter]
ADD CONSTRAINT [PK_tbl_GSTMissingFooter]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Purchase_ID] in table 'tbl_GSTMissingPurchase'
ALTER TABLE [dbo].[tbl_GSTMissingPurchase]
ADD CONSTRAINT [PK_tbl_GSTMissingPurchase]
    PRIMARY KEY CLUSTERED ([Purchase_ID] ASC);
GO

-- Creating primary key on [Purchase_ID] in table 'tbl_GSTPurchase'
ALTER TABLE [dbo].[tbl_GSTPurchase]
ADD CONSTRAINT [PK_tbl_GSTPurchase]
    PRIMARY KEY CLUSTERED ([Purchase_ID] ASC);
GO

-- Creating primary key on [id] in table 'tbl_GSTPurchaseError'
ALTER TABLE [dbo].[tbl_GSTPurchaseError]
ADD CONSTRAINT [PK_tbl_GSTPurchaseError]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Supply_ID] in table 'tbl_GSTSupply'
ALTER TABLE [dbo].[tbl_GSTSupply]
ADD CONSTRAINT [PK_tbl_GSTSupply]
    PRIMARY KEY CLUSTERED ([Supply_ID] ASC);
GO

-- Creating primary key on [id] in table 'tbl_GSTSupplyError'
ALTER TABLE [dbo].[tbl_GSTSupplyError]
ADD CONSTRAINT [PK_tbl_GSTSupplyError]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Ledger_ID] in table 'tbl_Ledger'
ALTER TABLE [dbo].[tbl_Ledger]
ADD CONSTRAINT [PK_tbl_Ledger]
    PRIMARY KEY CLUSTERED ([Ledger_ID] ASC);
GO

-- Creating primary key on [Ledger_ID] in table 'tbl_MissingLedger'
ALTER TABLE [dbo].[tbl_MissingLedger]
ADD CONSTRAINT [PK_tbl_MissingLedger]
    PRIMARY KEY CLUSTERED ([Ledger_ID] ASC);
GO

-- Creating primary key on [Supply_ID] in table 'tbl_GSTDupSupply'
ALTER TABLE [dbo].[tbl_GSTDupSupply]
ADD CONSTRAINT [PK_tbl_GSTDupSupply]
    PRIMARY KEY CLUSTERED ([Supply_ID] ASC);
GO

-- Creating primary key on [Supply_ID] in table 'tbl_GSTMissingSuplly'
ALTER TABLE [dbo].[tbl_GSTMissingSuplly]
ADD CONSTRAINT [PK_tbl_GSTMissingSuplly]
    PRIMARY KEY CLUSTERED ([Supply_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------