
IF ((Select Count(*) From [Settings].[Localization] Where ResourceKey = 'Js.Bonus.PhoneMask') = 0)
BEGIN
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Js.Bonus.PhoneMask', '+7(999)999-99-99')
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Js.Bonus.PhoneMask', '+7(999)999-99-99')
END

IF ((Select Count(*) From [Settings].[Localization] Where ResourceKey = 'Js.Phone.PhoneMask') = 0)
BEGIN
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Js.Phone.PhoneMask', '')
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Js.Phone.PhoneMask', '')
END

GO--

INSERT INTO [CMS].[StaticBlock] ([Key],[InnerName],[Content],[Added],[Modified],[Enabled])
     VALUES ('CatalogLeft', 'Каталог слева под фильтром', '', getdate(), getdate(), 1)
GO--

INSERT INTO [CMS].[StaticBlock] ([Key],[InnerName],[Content],[Added],[Modified],[Enabled])
     VALUES ('CatalogRightBottom', 'Каталог под товарами', '', getdate(), getdate(), 1)
GO--



UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.17' WHERE [settingKey] = 'db_version'

