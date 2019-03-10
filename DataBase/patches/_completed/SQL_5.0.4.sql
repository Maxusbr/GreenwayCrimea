
Insert Into [Settings].[TemplateSettings] ([Template],[Name],[Value]) 
	Select distinct ts.Template, 'SearchBlockLocation', 'TopMenu' 
	From [Settings].[TemplateSettings] as ts
	Where ((Select Count(*) From [Settings].[TemplateSettings] Where ts.Template = Template and Name = 'SearchBlockLocation') = 0)

GO--

Insert Into [Settings].[TemplateSettings] ([Template],[Name],[Value]) 
	Select distinct ts.Template, 'CountCategoriesInLine', '4' 
	From [Settings].[TemplateSettings] as ts
	Where ((Select Count(*) From [Settings].[TemplateSettings] Where ts.Template = Template and Name = 'CountCategoriesInLine') = 0)

GO--

Insert Into [Settings].[Settings] ([Name], [Value]) Values ('BuyInOneClick_LinkText','Купить в один клик')
GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.4' WHERE [settingKey] = 'db_version'
GO--

