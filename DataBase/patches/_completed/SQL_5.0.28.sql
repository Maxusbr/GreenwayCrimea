alter table [Order].OrderCurrency add RoundNumbers float null

GO--

alter table [Order].OrderCurrency add EnablePriceRounding bit null

GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.28' WHERE [settingKey] = 'db_version'