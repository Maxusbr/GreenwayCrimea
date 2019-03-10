ALTER TABLE [Order].Lead ADD
	DeliveryDate datetime NULL,
	DeliveryTime nvarchar(255) NULL,
	ShippingMethodId int NULL,
	ShippingName nvarchar(255) NULL,
	ShippingCost float(53) NOT NULL CONSTRAINT DF_Lead_ShippingCost DEFAULT ((0)),	
	ShippingPickPoint nvarchar(MAX) NULL
	
GO--


ALTER TABLE [Order].[Order]
  ALTER COLUMN DeliveryTime nvarchar(255) NULL; 

GO--


UPDATE [Settings].[InternalSettings] SET [settingValue] = '6.0.1' WHERE [settingKey] = 'db_version'