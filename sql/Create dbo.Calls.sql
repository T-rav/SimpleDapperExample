CREATE TABLE [dbo].[Calls]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
	[ExternalSystemId] INT NOT NULL, 
	[ExternalCallId] INT NOT NULL,
	[ConferenceId] VARCHAR(36) NOT NULL, 
	[Cld] VARCHAR(50) NULL, 
	[Cli] VARCHAR(50) NULL, 
	[ServiceType] VARCHAR(50) NOT NULL,
	[Network] VARCHAR(50) NOT NULL, 
	[RecordType] VARCHAR(50) NOT NULL, 
	[BilledSeconds] INT NOT NULL, 
	[UtcBillTime] DATETIME NOT NULL,
	[Created] DATETIME NOT NULL, 
	[Modified] DATETIME NOT NULL
);