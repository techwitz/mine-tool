CREATE TABLE [dbo].[VentilationCapacity] (
    [Uid]           BIGINT             IDENTITY (1, 1) NOT NULL,
    [EntityKey]     VARCHAR (16)       NOT NULL,
    [UnitName]      VARCHAR (500)      NOT NULL,
    [DepartmentUid] BIGINT             NOT NULL,
    [Capacity]      INT                NOT NULL,
    [Created]       DATETIMEOFFSET (7) CONSTRAINT [DF_VentilationCapacity_Created] DEFAULT (getutcdate()) NOT NULL,
    [RowStamp]      ROWVERSION         NOT NULL,
    CONSTRAINT [PK_VentilationCapacity] PRIMARY KEY NONCLUSTERED ([EntityKey] ASC),
    CONSTRAINT [FK_VentilationCapacity_VentilationCapacity] FOREIGN KEY ([DepartmentUid]) REFERENCES [dbo].[Department] ([Uid])
);


GO
CREATE UNIQUE CLUSTERED INDEX [IX_VentilationCapacity]
    ON [dbo].[VentilationCapacity]([Uid] ASC);

