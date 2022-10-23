CREATE TABLE tblDonors (
  ID INT PRIMARY KEY IDENTITY,
  DonorID NVARCHAR(50) NOT NULL UNIQUE,
  LastName nvarchar(40),
  FirstName nvarchar(40),
  DateOfBirth date,
  Email nvarchar(40),
  Active AS CONVERT(BIT, (CASE WHEN InactiveDate IS NULL then 1 ELSE 0 END)),
  ReceiveConsentForm bit NOT NULL DEFAULT 0,
  ReceiveFinancialForm bit NOT NULL DEFAULT 0,
  InactiveDate datetime2(7) NULL,
  InactiveReason nvarchar(256) NULL,
  Notes nvarchar(1000)
);

CREATE TABLE tblAddresses(
  ID INT PRIMARY KEY IDENTITY,
  DonorID NVARCHAR(50) NOT NULL REFERENCES tblDonors(DonorId),
  AddressType int,
  Address1 nvarchar(100),
  Address2 nvarchar(100),
  City nvarchar(50),
  [State] VARCHAR(2), 
  Zipcode VARCHAR(10),
  CONSTRAINT UC_DonorAddressType UNIQUE(DonorId, AddressType)
);

CREATE TABLE [Identity].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](256) NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_IdentityUser] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
 ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

  CREATE TABLE [Identity].[UserClaim](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](256) NULL,
	[ClaimValue] [nvarchar](256) NULL,
 CONSTRAINT [PK_IdentityUserClaim] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
 ) ON [PRIMARY];

ALTER TABLE tblMilkKits ADD
  DatePaid DateTime,
  Finalized DateTime;

ALTER TABLE tblDNAKits ADD 
  MicrobialOrdered DateTime,
  GeneticOrdered DateTime,
  ToxicologyOrdered DateTime;