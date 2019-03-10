
Alter Table [dbo].[Modules]
Add NeedUpdate bit Null
GO--

Update [dbo].[Modules] Set [NeedUpdate] = 0
GO--

Alter Table [dbo].[Modules]
Alter Column NeedUpdate bit Not Null
GO--

update catalog.category set sorting=sorting+1 where sorting > 0

GO--
update [Settings].[Settings]  set [Value]='True' where Name = 'BuyInOneClick_CreateOrder'

GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.3' WHERE [settingKey] = 'db_version'
GO--

