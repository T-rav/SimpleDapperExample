CREATE TABLE [dbo].[Customers] (
    [Id]               INT          IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (80) NOT NULL,
    [AlternativeName]  VARCHAR (80) NOT NULL,
    [Email]            VARCHAR (80) NOT NULL,
    [Login]            VARCHAR (80) NULL,
    [TimeZoneId]       INT          NOT NULL,
    [ExternalSystemId] INT          NOT NULL,
    [Created]          DATETIME     NOT NULL,
    [Modifed]          DATETIME     NOT NULL
);

