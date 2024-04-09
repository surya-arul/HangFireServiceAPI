-- Creating database 
CREATE DATABASE ClaysysDB;
GO

-- Selecting database 
USE ClaysysDB;
GO

-- Creating table (tblClaysysEmployees) 
CREATE TABLE tblClaysysEmployees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100),
    Department VARCHAR(100)
);

-- Creating database for hangfire
CREATE DATABASE HangFireDB;
GO