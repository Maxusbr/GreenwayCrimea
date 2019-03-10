
if EXISTS(Select Count([ResourceValue]) From [Settings].[Localization] where [ResourceKey] = 'JS.Readmore.MoreText' and [LanguageId] = 1)
begin 
	Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'JS.Readmore.MoreText', 'Показать все')
end

if EXISTS(Select Count([ResourceValue]) From [Settings].[Localization] where [ResourceKey] = 'JS.Readmore.MoreText' and [LanguageId] = 2)
begin 
	Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'JS.Readmore.MoreText', 'To show everything')
end

if EXISTS(Select Count([ResourceValue]) From [Settings].[Localization] where [ResourceKey] = 'JS.Readmore.LessText' and [LanguageId] = 1)
begin 
	Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'JS.Readmore.LessText', 'Скрыть текст')
end

if EXISTS(Select Count([ResourceValue]) From [Settings].[Localization] where [ResourceKey] = 'JS.Readmore.LessText' and [LanguageId] = 2)
begin 
	Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'JS.Readmore.LessText', 'To hide text')
end

GO--

UPDATE [Settings].[Localization] SET [ResourceValue] = 'Продажи' WHERE [ResourceKey] = 'Admin.Home.OrderGraphDasboard.Orders' AND [LanguageId] = 1
GO--



UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.22' WHERE [settingKey] = 'db_version'