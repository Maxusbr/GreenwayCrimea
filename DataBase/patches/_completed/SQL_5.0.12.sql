
alter table Settings.Error404 alter column UrlReferer nvarchar(4000) null

GO--
Insert Into [CMS].[StaticBlock] ([Key],[InnerName],[Content],[Added],[Modified],[Enabled]) 
Values ('PrintOrderTop', 'Распечатка заказа блок сверху', '', getdate(), getdate(), 1)

Insert Into [CMS].[StaticBlock] ([Key],[InnerName],[Content],[Added],[Modified],[Enabled]) 
Values ('PrintOrderBottom', 'Распечатка заказа блок снизу', '', getdate(), getdate(), 1)

GO--
GO--
UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.12' WHERE [settingKey] = 'db_version'

