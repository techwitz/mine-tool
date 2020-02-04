CREATE TABLE [dbo].[Department] (
    [Uid]        BIGINT             IDENTITY (1, 1) NOT NULL,
    [EntityKey]  VARCHAR (16)       NOT NULL,
    [Name] VARCHAR (500)      NOT NULL,
    [Capacity]   INT                CONSTRAINT [DF_Department_Capacity] DEFAULT ((0)) NOT NULL,
    [Created]    DATETIMEOFFSET (7) CONSTRAINT [DF_Department_Created] DEFAULT (getutcdate()) NOT NULL,
    [RowStamp]   ROWVERSION         NOT NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY NONCLUSTERED ([Uid] ASC),
    CONSTRAINT [IX_Department] UNIQUE CLUSTERED ([EntityKey] ASC)
);

