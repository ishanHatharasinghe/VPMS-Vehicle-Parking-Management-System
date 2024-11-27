CREATE TABLE [dbo].[CarTbl] (
    [VNo]          INT          IDENTITY (1, 1) NOT NULL,
    [PlateNo]      VARCHAR (10) NULL,
    [Vtype]        VARCHAR (50) NULL,
    [Colour]       VARCHAR (50) NULL,
    [DriverName]   VARCHAR (50) NULL,
    [DriverNIC]    VARCHAR (13) NULL,
    [Phone]        VARCHAR (13) NULL,
    [FormattedVNo] AS           ('V'+right('000'+CONVERT([varchar](3),[VNo]),(3))) PERSISTED,
    PRIMARY KEY CLUSTERED ([PlateNo] ASC)
);



CREATE TABLE [dbo].[EntryTbl]
(
    [EntNo] INT IDENTITY (1, 1) NOT NULL,
    [SName] VARCHAR (10) NULL,
    [VNo] VARCHAR (10) NULL,
    [PlateNo] VARCHAR (10) NULL,
    [EntryTime] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([EntNo] ASC)
);


CREATE TABLE [dbo].[SectionTbl]
(
    [SNo] INT IDENTITY (1,1) NOT NULL,
    [SName] VARCHAR (10) NOT NULL,
    [Capacity] INT NULL,
    [SDescription] VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([SName] ASC)
);

CREATE TABLE [dbo].[ExitTbl]
(
    [ExitNo] INT IDENTITY (1, 1) NOT NULL,
    [SName] VARCHAR (10) NULL,
    [VNo] VARCHAR (10) NULL,
    [PlateNo] VARCHAR (10) NULL,
    [ExitTime] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([ExitNo] ASC)
);

CREATE TABLE [dbo].[ProfileTbl] (
    [ProfileNo]  INT           IDENTITY (1, 1) NOT NULL,
    [FullName]   VARCHAR (100) NULL,
    [Address]    VARCHAR (255) NULL,
    [NIC]        VARCHAR (13)  NOT NULL,
    [Phone]      VARCHAR (20)  NULL,
    [Email]      VARCHAR (100) NULL,
    [Gender]     VARCHAR (10)  NULL,
    [Post]       VARCHAR (50)  NULL,
    [Department] VARCHAR (100) NULL,
    [PlateNo]    VARCHAR (10)  NULL,
    [Vtype]      VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([NIC] ASC)
);

-- CarTbl: Stores car details with VNo as primary key
CREATE TABLE [dbo].[CarTbl]
(
    [VNo] INT IDENTITY (1, 1) NOT NULL,
    [PlateNo] VARCHAR(20) NOT NULL,  -- Adjusted length to 20
    [Vtype] VARCHAR(50) NULL,
    [Colour] VARCHAR(50) NULL,
    [DriverName] VARCHAR(100) NULL,  -- Increased length to 100
    [DriverNIC] VARCHAR(13) NULL,
    [Phone] VARCHAR(15) NULL,  -- Adjusted length to 15 for standard phone numbers
    PRIMARY KEY CLUSTERED ([VNo] ASC),
    UNIQUE ([PlateNo])  -- Ensures unique PlateNo
);

-- EntryTbl: Records vehicle entries with foreign key references to CarTbl and SectionTbl
CREATE TABLE [dbo].[EntryTbl]
(
    [EntNo] INT IDENTITY (1, 1) NOT NULL,
    [SName] VARCHAR(10) NOT NULL,
    [VNo] INT NOT NULL,  -- Changed to INT to match CarTbl's VNo
    [PlateNo] VARCHAR(20) NOT NULL,  -- Adjusted length to match CarTbl
    [EntryTime] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([EntNo] ASC),
    FOREIGN KEY ([VNo]) REFERENCES [dbo].[CarTbl]([VNo]),
    FOREIGN KEY ([PlateNo]) REFERENCES [dbo].[CarTbl]([PlateNo])
);

-- SectionTbl: Stores section information
CREATE TABLE [dbo].[SectionTbl]
(
    [SNo] INT IDENTITY (1, 1) NOT NULL,
    [SName] VARCHAR(10) NOT NULL UNIQUE,  -- Ensures unique section names
    [Capacity] INT NULL,
    [SDescription] VARCHAR(100) NULL,
    PRIMARY KEY CLUSTERED ([SNo] ASC)
);

-- ExitTbl: Records vehicle exits with foreign key references to CarTbl and SectionTbl
CREATE TABLE [dbo].[ExitTbl]
(
    [ExitNo] INT IDENTITY (1, 1) NOT NULL,
    [SName] VARCHAR(10) NOT NULL,
    [VNo] INT NOT NULL,  -- Changed to INT to match CarTbl's VNo
    [PlateNo] VARCHAR(20) NOT NULL,  -- Adjusted length to match CarTbl
    [ExitTime] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([ExitNo] ASC),
    FOREIGN KEY ([VNo]) REFERENCES [dbo].[CarTbl]([VNo]),
    FOREIGN KEY ([PlateNo]) REFERENCES [dbo].[CarTbl]([PlateNo])
);

-- ProfileTbl: Stores profile information with a foreign key reference to CarTbl's PlateNo
CREATE TABLE [dbo].[ProfileTbl] 
(
    [ProfileNo] INT IDENTITY (1, 1) NOT NULL,
    [FullName] VARCHAR(100) NULL,
    [Address] VARCHAR(255) NULL,
    [NIC] VARCHAR(20) NOT NULL UNIQUE,  -- Ensures unique NIC for each profile
    [Phone] VARCHAR(15) NULL,  -- Adjusted length to 15 for standard phone numbers
    [Email] VARCHAR(100) NULL,
    [Gender] VARCHAR(10) NULL,
    [Post] VARCHAR(50) NULL,
    [Department] VARCHAR(100) NULL,
    [PlateNo] VARCHAR(20) NULL,  -- Adjusted length to match CarTbl
    [Vtype] VARCHAR(50) NULL,
    PRIMARY KEY CLUSTERED ([ProfileNo] ASC),
    FOREIGN KEY ([PlateNo]) REFERENCES [dbo].[CarTbl]([PlateNo])
);
