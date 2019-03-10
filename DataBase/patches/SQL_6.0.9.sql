
Insert Into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values(1,'Search.Index.ResultEmpty','К сожалению, по вашему запросу ничего не найдено. Попробуйте изменить поисковый запрос или выбранный фильтр.')
Insert Into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values(2,'Search.Index.ResultEmpty','Unfortunately, your search returned no results. Try to change the search query or the selected filter.')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Boxberry.CalculateCourier','Включить расчет курьерской доставки')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Boxberry.CalculateCourier','Include courier delivery calculation')

GO--

CREATE TABLE [Order].[OrderAdditionalData](
	[OrderID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_AdditionalData] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO--

ALTER TABLE [Order].[OrderAdditionalData]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalData_Order] FOREIGN KEY([OrderID])
REFERENCES [Order].[Order] ([OrderID])
ON DELETE CASCADE
GO--

ALTER TABLE [Order].[OrderAdditionalData] CHECK CONSTRAINT [FK_AdditionalData_Order]
GO--


UPDATE [Settings].[Localization] SET ResourceValue = 'Подъезд' WHERE ResourceValue = 'Подьезд'

GO--
UPDATE [Settings].[InternalSettings] SET [settingValue] = '6.0.9' WHERE [settingKey] = 'db_version'