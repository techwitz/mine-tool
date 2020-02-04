
MERGE INTO [dbo].[Department] AS Target
USING (VALUES
  ('093B7339-340C-42', 'Longwall', 115, SYSUTCDATETIME())
 ,('1C61D8E9-AF2C-44', 'Tailgate', 70, SYSUTCDATETIME())
 ,('87AD048A-647C-41', 'Maingate 13', 49, SYSUTCDATETIME())
 ,('33201D14-D23B-43', 'Maingate 14', 52, SYSUTCDATETIME())
) AS Source ([EntityKey],[Name],[Capacity],[Created])
ON (Target.[EntityKey] = Source.[EntityKey])
WHEN MATCHED AND (Target.[Name] <> Source.[Name]) THEN
    UPDATE SET
    [Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET THEN
    INSERT([EntityKey],[Name],[Capacity],[Created])
    VALUES(Source.[EntityKey],Source.[Name], Source.[Capacity], Source.[Created])
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;