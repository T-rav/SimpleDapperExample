CREATE TABLE [dbo].[PhysicalAddresses] (
    [Id]           INT           IDENTITY NOT NULL,
    [CustomerId]   INT           NOT NULL,
    [AddressLine1] VARCHAR (100) NULL,
    [AddressLine2] VARCHAR (100) NULL,
    [City]         VARCHAR (100) NULL,
    [Province]     VARCHAR (50)  NOT NULL,
    [PostCode]     CHAR (4)      NOT NULL,
    [Created]      DATETIME      NOT NULL,
    [Modified]     DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PhysicalAddresses_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id])
);