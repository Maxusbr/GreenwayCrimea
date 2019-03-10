
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Customers.RoleActionCategory.Tasks', 'Задачи');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Customers.RoleActionCategory.Tasks', 'Tasks');

insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values (1,'Admin.Cards.Index.ExportCards','Выгрузка карт')
insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values (2,'Admin.Cards.Index.ExportCards','Cards export')

GO--

if not exists (select * from Settings.Localization where ResourceKey = 'Js.Subscribe.ErrorAgreement')
begin
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'Js.Subscribe.ErrorAgreement','Необходимо согласиться с пользовательским соглашением')
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'Js.Subscribe.ErrorAgreement','You must agree with the user agreement')
end

GO--

	UPDATE Settings.Localization SET ResourceValue = 'В Вашем проекте не создан ни один бизнес-процесс.' WHERE LanguageId = 1  AND  ResourceKey = 'Admin.Tasks.BizProcessesNotSet' 
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Tasks.BizProcessesNotSet', 'In your project, no business process has been created.');

GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '6.0.2' WHERE [settingKey] = 'db_version'