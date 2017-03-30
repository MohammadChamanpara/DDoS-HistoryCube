
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/18/2014 17:07:46
-- Generated from EDMX file: D:\Google Drive\Work\Stone\SourceCode\Stone\Model\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Stone_DB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[RatioHistories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RatioHistories];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'RatioHistories'
CREATE TABLE [dbo].[RatioHistories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [d] int  NOT NULL,
    [R0] int  NOT NULL,
    [R1] int  NOT NULL,
    [R2] int  NOT NULL,
    [R3] int  NOT NULL,
    [R4] int  NOT NULL,
    [R5] int  NOT NULL,
    [R6] int  NOT NULL,
    [R7] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'RatioHistories'
ALTER TABLE [dbo].[RatioHistories]
ADD CONSTRAINT [PK_RatioHistories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------