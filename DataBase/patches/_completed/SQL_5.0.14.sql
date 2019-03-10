
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_Call_SrcNum' AND object_id = OBJECT_ID('[Customers].[Call]'))
BEGIN
	CREATE NONCLUSTERED INDEX [IX_Call_SrcNum] ON [Customers].[Call]([SrcNum])
END
GO--

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_Call_DstNum' AND object_id = OBJECT_ID('[Customers].[Call]'))
BEGIN
	CREATE NONCLUSTERED INDEX [IX_Call_DstNum] ON [Customers].[Call]([DstNum])
END
GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.14' WHERE [settingKey] = 'db_version'

