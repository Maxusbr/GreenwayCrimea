//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Scheduler;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Module.MoySklad.Domain;
using AdvantShop.Orders;
using System.Threading.Tasks;
using AdvantShop.Diagnostics;
using System.Linq;

namespace AdvantShop.Module.MoySklad
{
    public class MoySklad : IModule, IOrderChanged, IModuleTask, ICustomerChange, IContactChange
    {
        private static readonly string moduleStringId = "MoySklad";
        public static readonly int MaxLenArt = 50;

        #region Implementation of IModule

        public string ModuleStringId
        {
            get { return moduleStringId; }
        }

        public string ModuleName
        {
            get { return GetModuleName(); }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                {
                    new MoySkladSetting(),
                    //new MoySkladImportExcel(),
                    new MoySkladExportExcel()
                };
            }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return true;
        }

        public bool InstallModule()
        {
            return UpdateModule();
        }

        public bool UninstallModule()
        {

            if (ImportStatisticMoySkladRequest.IsRun)
                return false;

            ModuleSettingsProvider.RemoveSqlSetting("MoySkladCSSO", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladSyncProp", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladSyncDescr", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladSyncOrders", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladNamePropWeight", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladNamePropSize", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladNamePropBrand", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladNamePropDiscount", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladPropNoLoad", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladNameCharactColor", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladNameCharactSize", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("UpdateEnableProduct", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("ProductNoEnableNotSuncMoysklad", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("DeleteOfferNotSuncMoysklad", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("CreateNewCategoryEnabled", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("UseZip", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladApiLogin", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MoySkladApiPassword", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("UpdateCustomersAndContacts", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("UpdateOrdersStatuses", ModuleStringId);

            if (Directory.Exists(ModuleSettingsProvider.GetAbsolutePath() + @"\App_Data\filesmysklad\"))
            {
                Directory.Delete(ModuleSettingsProvider.GetAbsolutePath() + @"\App_Data\filesmysklad\", true);
            }
            

            // TABLE [Catalog].[ProductSuncMoysklad]
            Remove_Table_Catalog_ProductSuncMoysklad();

            // TABLE [Catalog].[OfferSuncMoysklad]
            Remove_Table_Catalog_OfferSuncMoysklad();

            // PROCEDURE [Catalog].[sp_ProductNoEnabledNotSuncMoysklad]
            Remove_Procedure_Catalog_sp_ProductNoEnabledNotSuncMoysklad();

            // PROCEDURE [Catalog].[sp_DeleteOfferNotSuncMoysklad]
            Remove_Procedure_Catalog_sp_DeleteOfferNotSuncMoysklad();

            return true;
        }

        public bool UpdateModule()
        {

            if (ImportStatisticMoySkladRequest.IsRun)
                return false;

            if (!Directory.Exists(ModuleSettingsProvider.GetAbsolutePath() + @"\App_Data\filesmysklad\"))
            {
                Directory.CreateDirectory(ModuleSettingsProvider.GetAbsolutePath() + @"\App_Data\filesmysklad\");
            }

            // Update settings
            UpdateSettings();

            // TABLE [Order].[OrderSendMoysklad]
            if (!ModulesRepository.IsExistsModuleTable("Order", "OrderSendMoysklad"))
                Add_Table_Order_OrderSendMoysklad();

            ModulesRepository.ModuleExecuteNonQuery(@"INSERT INTO [Order].[OrderSendMoysklad]
                    SELECT [OrderID], 1 FROM [Order].[Order] WHERE NOT [OrderID] IN (SELECT [OrderSendMoysklad].[OrderID] FROM [Order].[OrderSendMoysklad])",
                CommandType.Text);


            // TABLE [Catalog].[ProductFromMoysklad]
            if (ModulesRepository.IsExistsModuleTable("Catalog", "ProductFromMoysklad"))
            {
                ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Catalog].[ProductFromMoysklad]') AND name = N'PK_ProductFromMoysklad')
                    ALTER TABLE [Catalog].[ProductFromMoysklad] DROP CONSTRAINT [PK_ProductFromMoysklad]",
                    CommandType.Text);

                ModulesRepository.ModuleExecuteNonQuery(@"IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Catalog].[ProductFromMoysklad]') AND name = N'PK_ProductFromMoysklad4_1')
                    BEGIN
                        ALTER TABLE [Catalog].[ProductFromMoysklad] ADD  CONSTRAINT [PK_ProductFromMoysklad4_1] PRIMARY KEY CLUSTERED 
                        (
	                        [ProductExternalId] ASC,
	                        [ProductId] ASC
                        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
                    END", CommandType.Text);
            }
            else
                Add_Table_Catalog_ProductFromMoysklad();


            // TABLE [Catalog].[OfferFromMoysklad]
            if (!ModulesRepository.IsExistsModuleTable("Catalog", "OfferFromMoysklad"))
                Add_Table_Catalog_OfferFromMoysklad();


            // TABLE [Order].[OrderItemsFromMoysklad]
            if (!ModulesRepository.IsExistsModuleTable("Order", "OrderItemsFromMoysklad"))
                Add_Table_Order_OrderItemsFromMoysklad();


            // TABLE [Catalog].[ProductSuncMoysklad]
            if (!ModulesRepository.IsExistsModuleTable("Catalog", "ProductSuncMoysklad"))
                Add_Table_Catalog_ProductSuncMoysklad();


            // PROCEDURE [Catalog].[sp_ProductNoEnabledNotSuncMoysklad]
            if (!ModulesRepository.IsExistsModuleProcedure("sp_ProductNoEnabledNotSuncMoysklad"))
                Add_Procedure_Catalog_sp_ProductNoEnabledNotSuncMoysklad();


            // TABLE [Catalog].[OfferSuncMoysklad]
            if (!ModulesRepository.IsExistsModuleTable("Catalog", "OfferSuncMoysklad"))
                Add_Table_Catalog_OfferSuncMoysklad();


            // PROCEDURE [Catalog].[sp_DeleteOfferNotSuncMoysklad]
            if (!ModulesRepository.IsExistsModuleProcedure("sp_DeleteOfferNotSuncMoysklad"))
                Add_Procedure_Catalog_sp_DeleteOfferNotSuncMoysklad();

            return true;
        }

        private void UpdateSettings()
        {
            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladCSSO", ModuleStringId))
                ModuleSettingsProvider.SetSettingValue("MoySkladCSSO", 100, ModuleStringId);

            if (!ModuleSettingsProvider.IsSqlSettingExist("UpdateEnableProduct", ModuleStringId))
                UpdateEnableProduct = EnUpdateEnableProduct.None;

            if (!ModuleSettingsProvider.IsSqlSettingExist("ProductNoEnableNotSuncMoysklad", ModuleStringId))
                IsSetNoEnableProductNotSuncMoysklad = true;

            if (!ModuleSettingsProvider.IsSqlSettingExist("DeleteOfferNotSuncMoysklad", ModuleStringId))
                IsDeleteOfferNotSuncMoysklad = true;

            if (!ModuleSettingsProvider.IsSqlSettingExist("CreateNewCategoryEnabled", ModuleStringId))
                IsNewCategoryEnabled = true;

            if (!ModuleSettingsProvider.IsSqlSettingExist("UseZip", ModuleStringId))
                UseZip = false;

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladSyncProp", ModuleStringId))
                SyncProperty = EnSyncProperty.Adding;

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladSyncDescr", ModuleStringId))
                SyncDescription = EnSyncDescription.AddNew;

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladSyncOrders", ModuleStringId))
                SyncOrders = EnSyncOrders.New;

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladNamePropWeight", ModuleStringId))
                ModuleSettingsProvider.SetSettingValue("MoySkladNamePropWeight", string.Empty, ModuleStringId);

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladNamePropSize", ModuleStringId))
                ModuleSettingsProvider.SetSettingValue("MoySkladNamePropSize", string.Empty, ModuleStringId);

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladNamePropBrand", ModuleStringId))
                ModuleSettingsProvider.SetSettingValue("MoySkladNamePropBrand", string.Empty, ModuleStringId);

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladNamePropDiscount", ModuleStringId))
                ModuleSettingsProvider.SetSettingValue("MoySkladNamePropDiscount", string.Empty, ModuleStringId);

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladPropNoLoad", ModuleStringId))
                ModuleSettingsProvider.SetSettingValue("MoySkladPropNoLoad", string.Empty, ModuleStringId);

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladNameCharactColor", ModuleStringId))
                ModuleSettingsProvider.SetSettingValue("MoySkladNameCharactColor", string.Empty, ModuleStringId);

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladNamePropDiscount", ModuleStringId))
                ModuleSettingsProvider.SetSettingValue("MoySkladNamePropDiscount", string.Empty, ModuleStringId);

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladApiLogin", ModuleStringId))
                ModuleSettingsProvider.SetSettingValue("MoySkladApiLogin", string.Empty, ModuleStringId);

            if (!ModuleSettingsProvider.IsSqlSettingExist("MoySkladApiPassword", ModuleStringId))
                ModuleSettingsProvider.SetSettingValue("MoySkladApiPassword", string.Empty, ModuleStringId);

            if (!ModuleSettingsProvider.IsSqlSettingExist("UpdateCustomersAndContacts", ModuleStringId))
                UpdateCustomersAndContacts = false;

            if (!ModuleSettingsProvider.IsSqlSettingExist("UpdateOrdersStatuses", ModuleStringId))
                UpdateOrdersStatuses = false;
        }

        #endregion

        #region Add/Remove objects database

        private void Add_Table_Order_OrderSendMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"CREATE TABLE [Order].[OrderSendMoysklad](
	                    [OrderID] [int] NOT NULL,
	                    [IsSendService] [bit] NOT NULL,
                     CONSTRAINT [PK_OrderSendMoysklad] PRIMARY KEY CLUSTERED 
                    (
	                    [OrderID] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]", CommandType.Text);


            ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Order].[OrderSendMoysklad]  WITH CHECK ADD  CONSTRAINT [FK_OrderSendMoysklad_Order] FOREIGN KEY([OrderID])
                    REFERENCES [Order].[Order] ([OrderID])
                    ON UPDATE CASCADE
                    ON DELETE CASCADE", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(
                @"ALTER TABLE [Order].[OrderSendMoysklad] CHECK CONSTRAINT [FK_OrderSendMoysklad_Order]",
                CommandType.Text, null);

            ModulesRepository.ModuleExecuteNonQuery(
                @"ALTER TABLE [Order].[OrderSendMoysklad] ADD  CONSTRAINT [DF_OrderSendMoysklad_IsSendService]  DEFAULT ((1)) FOR [IsSendService]",
                CommandType.Text, null);
        }

        private void Remove_Table_Order_OrderSendMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Order].[FK_OrderSendMoysklad_Order]') AND parent_object_id = OBJECT_ID(N'[Order].[OrderSendMoysklad]'))
                    ALTER TABLE [Order].[OrderSendMoysklad] DROP CONSTRAINT [FK_OrderSendMoysklad_Order]",
                CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_OrderSendMoysklad_IsSendService]') AND type = 'D')
                    BEGIN
                    ALTER TABLE [Order].[OrderSendMoysklad] DROP CONSTRAINT [DF_OrderSendMoysklad_IsSendService]
                    END", CommandType.Text, null);

            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Order].[OrderSendMoysklad]') AND type in (N'U'))
                    DROP TABLE [Order].[OrderSendMoysklad]", CommandType.Text);
        }

        private void Add_Table_Catalog_ProductFromMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"CREATE TABLE [Catalog].[ProductFromMoysklad](
	                    [ProductExternalId] [nvarchar](255) NOT NULL,
	                    [ProductId] [int] NOT NULL,
                     CONSTRAINT [PK_ProductFromMoysklad] PRIMARY KEY CLUSTERED  
                    (
	                    [ProductExternalId] ASC,
	                    [ProductId] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Catalog].[ProductFromMoysklad]  WITH CHECK ADD  CONSTRAINT [FK_ProductFromMoysklad_Product] FOREIGN KEY([ProductId])
                    REFERENCES [Catalog].[Product] ([ProductId])
                    ON UPDATE CASCADE
                    ON DELETE CASCADE", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(
                @"ALTER TABLE [Catalog].[ProductFromMoysklad] CHECK CONSTRAINT [FK_ProductFromMoysklad_Product]",
                CommandType.Text, null);
        }

        private void Remove_Table_Catalog_ProductFromMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Catalog].[FK_ProductFromMoysklad_Product]') AND parent_object_id = OBJECT_ID(N'[Catalog].[ProductFromMoysklad]'))
                    ALTER TABLE [Catalog].[ProductFromMoysklad] DROP CONSTRAINT [FK_ProductFromMoysklad_Product]",
                CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Catalog].[ProductFromMoysklad]') AND type in (N'U'))
                    DROP TABLE [Catalog].[ProductFromMoysklad]", CommandType.Text);
        }

        private void Add_Table_Catalog_OfferFromMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"CREATE TABLE [Catalog].[OfferFromMoysklad](
	                    [OfferExternalId] [nvarchar](255) NOT NULL,
	                    [OfferID] [int] NOT NULL,
                     CONSTRAINT [PK_OfferFromMoysklad] PRIMARY KEY CLUSTERED 
                    (
	                    [OfferExternalId] ASC,
	                    [OfferID] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Catalog].[OfferFromMoysklad]  WITH CHECK ADD  CONSTRAINT [FK_OfferFromMoysklad_Offer] FOREIGN KEY([OfferID])
                    REFERENCES [Catalog].[Offer] ([OfferID])
                    ON UPDATE CASCADE
                    ON DELETE CASCADE", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(
                @"ALTER TABLE [Catalog].[OfferFromMoysklad] CHECK CONSTRAINT [FK_OfferFromMoysklad_Offer]",
                CommandType.Text,
                null);
        }

        private void Remove_Table_Catalog_OfferFromMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Catalog].[FK_OfferFromMoysklad_Offer]') AND parent_object_id = OBJECT_ID(N'[Catalog].[OfferFromMoysklad]'))
                    ALTER TABLE [Catalog].[OfferFromMoysklad] DROP CONSTRAINT [FK_OfferFromMoysklad_Offer]",
                CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Catalog].[OfferFromMoysklad]') AND type in (N'U'))
                    DROP TABLE [Catalog].[OfferFromMoysklad]", CommandType.Text);
        }

        private void Add_Table_Order_OrderItemsFromMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"CREATE TABLE [Order].[OrderItemsFromMoysklad](
	                    [OfferExternalId] [nvarchar](255) NOT NULL,
	                    [OrderItemID] [int] NOT NULL,
                     CONSTRAINT [PK_OrderItemsFromMoysklad] PRIMARY KEY CLUSTERED 
                    (
	                    [OfferExternalId] ASC,
	                    [OrderItemID] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Order].[OrderItemsFromMoysklad]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemsFromMoysklad_OrderItems] FOREIGN KEY([OrderItemID])
                    REFERENCES [Order].[OrderItems] ([OrderItemID])
                    ON UPDATE CASCADE
                    ON DELETE CASCADE", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(
                @"ALTER TABLE [Order].[OrderItemsFromMoysklad] CHECK CONSTRAINT [FK_OrderItemsFromMoysklad_OrderItems]",
                CommandType.Text);
        }

        private void Remove_Table_Order_OrderItemsFromMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(
                @"IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Order].[FK_OrderItemsFromMoysklad_OrderItems]') AND parent_object_id = OBJECT_ID(N'[Order].[OrderItemsFromMoysklad]'))
                    ALTER TABLE [Order].[OrderItemsFromMoysklad] DROP CONSTRAINT [FK_OrderItemsFromMoysklad_OrderItems]",
                CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Order].[OrderItemsFromMoysklad]') AND type in (N'U'))
                    DROP TABLE [Order].[OrderItemsFromMoysklad]", CommandType.Text);
        }

        private void Add_Table_Catalog_ProductSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"CREATE TABLE [Catalog].[ProductSuncMoysklad](
	                    [ProductId] [int] NOT NULL
                    ) ON [PRIMARY]", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Catalog].[ProductSuncMoysklad]  WITH CHECK ADD  CONSTRAINT [FK_ProductSuncMoysklad_Product] FOREIGN KEY([ProductId])
                    REFERENCES [Catalog].[Product] ([ProductId])
                    ON DELETE CASCADE", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(
                @"ALTER TABLE [Catalog].[ProductSuncMoysklad] CHECK CONSTRAINT [FK_ProductSuncMoysklad_Product]",
                CommandType.Text);
        }

        private void Remove_Table_Catalog_ProductSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Catalog].[FK_ProductSuncMoysklad_Product]') AND parent_object_id = OBJECT_ID(N'[Catalog].[ProductSuncMoysklad]'))
                    ALTER TABLE [Catalog].[ProductSuncMoysklad] DROP CONSTRAINT [FK_ProductSuncMoysklad_Product]",
                CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Catalog].[ProductSuncMoysklad]') AND type in (N'U'))
                    DROP TABLE [Catalog].[ProductSuncMoysklad]", CommandType.Text);
        }

        private void Add_Procedure_Catalog_sp_ProductNoEnabledNotSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"CREATE PROCEDURE [Catalog].[sp_ProductNoEnabledNotSuncMoysklad]
                    AS
                    BEGIN
                    	UPDATE [Catalog].[Product]
                    	   SET [Enabled] = 0
                    	WHERE [ProductId] IN (SELECT [ProductFromMoysklad].[ProductId] FROM [Catalog].[ProductFromMoysklad]) AND NOT [ProductId] IN (SELECT [ProductSuncMoysklad].[ProductId] FROM [Catalog].[ProductSuncMoysklad])
                    END", CommandType.Text);
        }

        private void Remove_Procedure_Catalog_sp_ProductNoEnabledNotSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Catalog].[sp_ProductNoEnabledNotSuncMoysklad]') AND type in (N'P', N'PC'))
                    DROP PROCEDURE [Catalog].[sp_ProductNoEnabledNotSuncMoysklad]", CommandType.Text);
        }

        private void Add_Table_Catalog_OfferSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"CREATE TABLE [Catalog].[OfferSuncMoysklad](
	                    [OfferID] [int] NOT NULL
                    ) ON [PRIMARY]", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Catalog].[OfferSuncMoysklad]  WITH CHECK ADD  CONSTRAINT [FK_OfferSuncMoysklad_Offer] FOREIGN KEY([OfferID])
                    REFERENCES [Catalog].[Offer] ([OfferID])
                    ON DELETE CASCADE", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(
                @"ALTER TABLE [Catalog].[OfferSuncMoysklad] CHECK CONSTRAINT [FK_OfferSuncMoysklad_Offer]",
                CommandType.Text,
                null);
        }

        private void Remove_Table_Catalog_OfferSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Catalog].[FK_OfferSuncMoysklad_Offer]') AND parent_object_id = OBJECT_ID(N'[Catalog].[OfferSuncMoysklad]'))
                    ALTER TABLE [Catalog].[OfferSuncMoysklad] DROP CONSTRAINT [FK_OfferSuncMoysklad_Offer]",
                CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Catalog].[OfferSuncMoysklad]') AND type in (N'U'))
                    DROP TABLE [Catalog].[OfferSuncMoysklad]", CommandType.Text);
        }

        private void Add_Procedure_Catalog_sp_DeleteOfferNotSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"CREATE PROCEDURE [Catalog].[sp_DeleteOfferNotSuncMoysklad]
                    AS
                    BEGIN
                    	DELETE FROM [Catalog].[Offer]
                    	WHERE [OfferID] IN (SELECT [OfferFromMoysklad].[OfferID] FROM [Catalog].[OfferFromMoysklad]) AND NOT [OfferID] IN (SELECT [OfferSuncMoysklad].[OfferID] FROM [Catalog].[OfferSuncMoysklad])
                    END", CommandType.Text);
        }

        private void Remove_Procedure_Catalog_sp_DeleteOfferNotSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Catalog].[sp_DeleteOfferNotSuncMoysklad]') AND type in (N'P', N'PC'))
                    DROP PROCEDURE [Catalog].[sp_DeleteOfferNotSuncMoysklad]", CommandType.Text);
        }

        #endregion

        #region Implementation of IOrderChanged

        public void DoOrderAdded(IOrder order)
        {
            if (order.IsDraft)
                return;

            UpdateOrderItems(order.OrderID);
            SetSendService(order.OrderID, true);
        }

        public void DoOrderChangeStatus(IOrder order)
        {
            if (order.IsDraft)
                return;
            
            if (SyncOrders == EnSyncOrders.All)
                SetSendService(order.OrderID, true);
        }

        public void DoOrderUpdated(IOrder order)
        {
            if (order.IsDraft)
                return;

            UpdateOrderItems(order.OrderID);
            if (SyncOrders == EnSyncOrders.All)
                SetSendService(order.OrderID, true);
        }

        public void PayOrder(int orderId, bool payed)
        {
            var order = OrderService.GetOrder(orderId);
            if (order == null || order.IsDraft)
                return;

            if (SyncOrders == EnSyncOrders.All)
                SetSendService(orderId, true);
        }

        public void DoOrderDeleted(int orderId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Order].[OrderSendMoysklad] WHERE [OrderID] = @OrderID", CommandType.Text,
                new SqlParameter {ParameterName = "@OrderID", Value = orderId});
        }

        #endregion

        #region Implementation of IModuleTask

        public List<TaskSetting> GetTasks()
        {
            return new List<TaskSetting>()
            {
                new TaskSetting()
                {
                    Enabled = true,
                    JobType = typeof (UpdateOrderStatusJob).FullName + "," + typeof (UpdateOrderStatusJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Minutes,
                    TimeInterval = 15
                }
            };
        }

        #endregion

        #region Implementation of ICustomerChange

        void ICustomerChange.Add(Customer customer)
        {
            if (UpdateCustomersAndContacts)
                Task.Factory.StartNew(() => ExecutAction(() => MoySkladCustomerServices.AddOrUpdateCustomer(customer)));
        }

        void ICustomerChange.Update(Customer customer)
        {
            if (UpdateCustomersAndContacts)
                Task.Factory.StartNew(() => ExecutAction(() => MoySkladCustomerServices.AddOrUpdateCustomer(customer)));
        }

        void ICustomerChange.Delete(Guid customerId)
        {
            //
        }

        #endregion

        #region Implementation of IContactChange

        void IContactChange.Add(CustomerContact contact)
        {
            if (UpdateCustomersAndContacts)
                Task.Factory.StartNew(() => ExecutAction(() => MoySkladCustomerServices.AddOrUpdateCustomerContact(contact)));
        }

        void IContactChange.Update(CustomerContact contact)
        {
            if (UpdateCustomersAndContacts)
                Task.Factory.StartNew(() => ExecutAction(() => MoySkladCustomerServices.AddOrUpdateCustomerContact(contact)));
        }

        void IContactChange.Delete(Guid contactId)
        {
            //
        }

        #endregion

        private string GetModuleName()
        {
            string result = string.Empty;

            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
            {
                case "ru":
                    result = "МойСклад";
                    break;

                case "en":
                    result = "МойСклад";
                    break;

                default:
                    result = "МойСклад";
                    break;
            }

            return result;
        }

        public static string GetModuleStringId()
        {
            return moduleStringId;
        }

        #region Module Settings

        public enum EnSyncDescription : byte
        {
            None = 0,
            AddNew = 1,
            Always = 2
        }

        public enum EnSyncOrders : byte
        {
            New = 0,
            All = 1
        }

        public enum EnSyncProperty : byte
        {
            None = 0,
            Adding = 1,
            OneToOne = 2
        }

        public enum EnUpdateEnableProduct : byte
        {
            None = 0,
            OnlyNew = 1,
            Always = 2
        }

        public EnSyncProperty SyncProperty
        {
            get
            {
                return (EnSyncProperty) ModuleSettingsProvider.GetSettingValue<byte>("MoySkladSyncProp", ModuleStringId);
            }
            set { ModuleSettingsProvider.SetSettingValue("MoySkladSyncProp", (byte) value, ModuleStringId); }
        }

        public EnSyncDescription SyncDescription
        {
            get
            {
                return
                    (EnSyncDescription)
                        ModuleSettingsProvider.GetSettingValue<byte>("MoySkladSyncDescr", ModuleStringId);
            }
            set { ModuleSettingsProvider.SetSettingValue("MoySkladSyncDescr", (byte) value, ModuleStringId); }
        }

        public EnSyncOrders SyncOrders
        {
            get
            {
                return (EnSyncOrders) ModuleSettingsProvider.GetSettingValue<byte>("MoySkladSyncOrders", ModuleStringId);
            }
            set { ModuleSettingsProvider.SetSettingValue("MoySkladSyncOrders", (byte) value, ModuleStringId); }
        }

        public EnUpdateEnableProduct UpdateEnableProduct
        {
            get
            {
                return
                    (EnUpdateEnableProduct)
                        ModuleSettingsProvider.GetSettingValue<byte>("UpdateEnableProduct", ModuleStringId);
            }
            set { ModuleSettingsProvider.SetSettingValue("UpdateEnableProduct", (byte) value, ModuleStringId); }
        }

        public bool IsSetNoEnableProductNotSuncMoysklad
        {
            get
            {
                return ModuleSettingsProvider.GetSettingValue<bool>("ProductNoEnableNotSuncMoysklad", ModuleStringId);
            }
            set { ModuleSettingsProvider.SetSettingValue("ProductNoEnableNotSuncMoysklad", value, ModuleStringId); }
        }

        public bool IsDeleteOfferNotSuncMoysklad
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("DeleteOfferNotSuncMoysklad", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("DeleteOfferNotSuncMoysklad", value, ModuleStringId); }
        }

        public bool AvailablePreOrder
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AvailablePreOrder", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AvailablePreOrder", value, ModuleStringId); }
        }

        /// <summary>
        /// Артикул модификации не менять на артикул товара если 1 модификация
        /// </summary>
        public bool DontChangeOfferArtnoToProductArtno
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("DontChangeOfferArtnoToProductArtno", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("DontChangeOfferArtnoToProductArtno", value, ModuleStringId); }
        }

        public bool DeleteOffersWithZeroAmount
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("DeleteOffersWithZeroAmount", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("DeleteOffersWithZeroAmount", value, ModuleStringId); }
        }

        public bool IsNewCategoryEnabled
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("CreateNewCategoryEnabled", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CreateNewCategoryEnabled", value, ModuleStringId); }
        }

        public bool UpdateOnlyProducts
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UpdateOnlyProducts", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("UpdateOnlyProducts", value, ModuleStringId); }
        }

        public bool UseZip
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UseZip", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("UseZip", value, ModuleStringId); }
        }

        public bool UpdateCustomersAndContacts
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UpdateCustomersAndContacts", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("UpdateCustomersAndContacts", value, ModuleStringId); }
        }

        public bool UpdateOrdersStatuses
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UpdateOrdersStatuses", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("UpdateOrdersStatuses", value, ModuleStringId); }
        }

        public string OrderPrefix
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("OrderPrefix", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("OrderPrefix", value, ModuleStringId); }
        }

        /// <summary>
        /// Не обновлять кол-во у товаров
        /// </summary>
        public bool NotUpdateAmount
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("NotUpdateAmount", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("NotUpdateAmount", value, ModuleStringId); }
        }

        /// <summary>
        /// Не обновлять цену у товаров
        /// </summary>
        public bool NotUpdatePrice
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("NotUpdatePrice", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("NotUpdatePrice", value, ModuleStringId); }
        }

        #endregion

        #region Module Methods

        public static List<Order> GetOrdersSendService(int top)
        {
            return ModulesRepository.ModuleExecuteReadList(
                "SELECT TOP (@count) [Order].* FROM [Order].[Order] RIGHT JOIN [Order].[OrderSendMoysklad] ON [Order].[OrderID] = [OrderSendMoysklad].[OrderID] WHERE [OrderSendMoysklad].[IsSendService] = 1 and [Order].[IsDraft] = 0",
                CommandType.Text, OrderService.GetOrderFromReader,
                new SqlParameter {ParameterName = "@count", Value = top});
        }

        public static bool SetSendService(int orderId, bool value)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                @"IF ((SELECT COUNT([OrderID]) FROM [Order].[OrderSendMoysklad] WHERE [OrderID] = @OrderID) = 0)
                BEGIN
	                INSERT INTO [Order].[OrderSendMoysklad] ([OrderID],[IsSendService]) VALUES (@OrderID, @IsSendService)
                END
                ELSE
                BEGIN
	                UPDATE [Order].[OrderSendMoysklad] SET [IsSendService] = @IsSendService WHERE [OrderID] = @OrderID
                END",
                CommandType.Text,
                new SqlParameter("@IsSendService", value),
                new SqlParameter("@OrderID", orderId)
                );
            return true;
        }

        public static int GetProductIdByName(string name)
        {
            return
                ModulesRepository.ModuleExecuteScalar<int>(
                    "SELECT ProductID FROM [Catalog].[Product] WHERE Name=@Name", CommandType.Text,
                    new SqlParameter("@Name", name));
        }

        public static Product GetProductByName(string name)
        {
            int productId = GetProductIdByName(name);
            return productId > 0 ? ProductService.GetProduct(productId) : null;
        }

        public static int GetProductIdByMoyskladId(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return
                    ModulesRepository.ModuleExecuteScalar<int>(
                        "SELECT ProductId FROM [Catalog].[ProductFromMoysklad] WHERE ProductExternalId=@ProductExternalId",
                        CommandType.Text, new SqlParameter("@ProductExternalId", id));
            else
                return 0;
        }

        public static string GetMoyskladIdByProductId(int productId)
        {
            if (productId > 0)
                return
                    ModulesRepository.ModuleExecuteScalar<string>(
                        "SELECT ProductExternalId FROM [Catalog].[ProductFromMoysklad] WHERE ProductId=@ProductId",
                        CommandType.Text, new SqlParameter("@ProductId", productId));
            else
                return string.Empty;
        }

        public static Product GetProductByMoyskladId(string id)
        {
            int productId = GetProductIdByMoyskladId(id);
            return productId > 0 ? ProductService.GetProduct(productId) : null;
        }

        public static void AddProductExternalId(string externalId, int productId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                @"Delete From [Catalog].[ProductFromMoysklad] Where ProductId = @ProductId; 
                INSERT INTO [Catalog].[ProductFromMoysklad] ([ProductExternalId],[ProductId]) VALUES (@ProductExternalId, @ProductId)", 
                CommandType.Text,
                new SqlParameter("@ProductExternalId", externalId ?? (object) DBNull.Value),
                new SqlParameter("@ProductId", productId));
        }

        public static int GetOfferIdByMoyskladId(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return
                    ModulesRepository.ModuleExecuteScalar<int>(
                        "SELECT OfferID FROM [Catalog].[OfferFromMoysklad] WHERE OfferExternalId=@OfferExternalId",
                        CommandType.Text, new SqlParameter("@OfferExternalId", id));
            else
                return 0;
        }

        public static string GetMoyskladIdByOfferId(int offerId)
        {
            if (offerId > 0)
                return
                    ModulesRepository.ModuleExecuteScalar<string>(
                        "SELECT OfferExternalId FROM [Catalog].[OfferFromMoysklad] WHERE OfferID=@OfferID",
                        CommandType.Text, new SqlParameter("@OfferID", offerId));
            else
                return string.Empty;
        }

        public static Offer GetOfferByMoyskladId(string id)
        {
            int offerId = GetOfferIdByMoyskladId(id);
            return offerId > 0 ? OfferService.GetOffer(offerId) : null;
        }

        public static void AddOfferExternalId(string externalId, int offerId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                @"Delete From [Catalog].[OfferFromMoysklad] Where OfferID = @OfferID; 
                  INSERT INTO [Catalog].[OfferFromMoysklad] ([OfferExternalId],[OfferID]) VALUES (@OfferExternalId, @OfferID)",
                CommandType.Text,
                new SqlParameter("@OfferExternalId", externalId ?? (object) DBNull.Value),
                new SqlParameter("@OfferID", offerId));
        }

        public static void UpdateOrderItems(int orderId)
        {
            foreach (OrderItem orderItem in OrderService.GetOrderItems(orderId))
            {
                if (string.IsNullOrEmpty(GetMoyskladIdByOrderItemId(orderItem.OrderItemID)))
                {
                    Offer offer = OfferService.GetOffer(orderItem.ArtNo);
                    if (offer != null)
                    {
                        string offerMoyskladId = GetMoyskladIdByOfferId(offer.OfferId);
                        if (!string.IsNullOrEmpty(offerMoyskladId))
                            AddOrderItemExternalId(offerMoyskladId, orderItem.OrderItemID);
                    }
                }
            }
        }

        public static string GetMoyskladIdByOrderItemId(int orderItemId)
        {
            if (orderItemId > 0)
                return
                    ModulesRepository.ModuleExecuteScalar<string>(
                        "SELECT OfferExternalId FROM [Order].[OrderItemsFromMoysklad] WHERE OrderItemID=@OrderItemID",
                        CommandType.Text, new SqlParameter("@OrderItemID", orderItemId));
            else
                return string.Empty;
        }

        public static void AddOrderItemExternalId(string externalId, int orderItemId)
        {
            ModulesRepository.ModuleExecuteNonQuery(@"INSERT INTO [Order].[OrderItemsFromMoysklad]
                   ([OfferExternalId]
                   ,[OrderItemID])
             VALUES
                   (@OfferExternalId,
                    @OrderItemID)", CommandType.Text,
                new SqlParameter("@OfferExternalId",
                    externalId ?? (object) DBNull.Value),
                new SqlParameter("@OrderItemID", orderItemId));
        }

        public static void AddProductSuncMoysklad(int productId)
        {
            ModulesRepository.ModuleExecuteNonQuery(@"INSERT INTO [Catalog].[ProductSuncMoysklad]
                   ([ProductId])
             VALUES
                   (@ProductId)", CommandType.Text,
                new SqlParameter("@ProductId", productId));
        }

        public static void ClearProductSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery("DELETE FROM [Catalog].[ProductSuncMoysklad]", CommandType.Text);
        }

        public static void ProductNoEnabledNotSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery("[Catalog].[sp_ProductNoEnabledNotSuncMoysklad]",
                CommandType.StoredProcedure);
        }

        public static void AddOfferSuncMoysklad(int offerId)
        {
            ModulesRepository.ModuleExecuteNonQuery(@"INSERT INTO [Catalog].[OfferSuncMoysklad]
                   ([OfferID])
             VALUES
                   (@OfferID)", CommandType.Text,
                new SqlParameter("@OfferID", offerId));
        }

        public static void ClearOfferSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery("DELETE FROM [Catalog].[OfferSuncMoysklad]", CommandType.Text);
        }

        public static void DeleteOfferNotSuncMoysklad()
        {
            ModulesRepository.ModuleExecuteNonQuery("[Catalog].[sp_DeleteOfferNotSuncMoysklad]",
                CommandType.StoredProcedure);
        }

        public static void DeleteOffersWithZeroAmoount()
        {
            ModulesRepository.ModuleExecuteNonQuery(//"delete from catalog.offer where amount <= 0",
                "DELETE FROM [Catalog].[Offer] " +
                "WHERE[OfferID] IN(SELECT[OfferFromMoysklad].[OfferID] FROM[Catalog].[OfferFromMoysklad]) AND amount <= 0",
                CommandType.Text);
        }


        public static string TrimArtNo(string artNo)
        {
            return TrimArtNo(artNo, MaxLenArt);
        }

        public static string TrimArtNo(string artNo, int maxLen)
        {
            if (!string.IsNullOrEmpty(artNo) && artNo.Length > maxLen)
                return artNo.Substring(0, maxLen);

            return artNo;
        }

        public static string GuidToString(Guid guid)
        {
            return guid.ToString().Replace("-", string.Empty);
        }

        public static string JoinNoEmpty(string separator, params string[] values)
        {
            return JoinNoEmpty(separator, (IEnumerable<string>)values);
        }

        public static string JoinNoEmpty(string separator, IEnumerable<string> values)
        {
            return String.Join(separator, values.Where(x => !string.IsNullOrEmpty(x)));
        }

        public static string ContactToLined(CustomerContact contact)
        {
            return JoinNoEmpty(", ", contact.Name, contact.Country, contact.Region, contact.City, contact.Zip, contact.Street);
        }

        public static string ContactToLined(OrderCustomer contact)
        {
            return JoinNoEmpty(", ", contact.FirstName + " " + contact.LastName, contact.Country, contact.Region, contact.City, contact.Zip, contact.Street, contact.House, contact.Structure, contact.CustomField1, contact.CustomField2, contact.CustomField3);
        }

        public static string AddressToLined(OrderCustomer contact)
        {
            return JoinNoEmpty(", ", contact.Country, contact.Region, contact.City, contact.Zip, contact.Street, contact.House, contact.Structure, contact.CustomField1, contact.CustomField2, contact.CustomField3);
        }

        private static void ExecutAction(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        #endregion

        private class MoySkladImportExcel : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Загрузка дополнительных свойств";

                        case "en":
                            return "Loading more properties";

                        default:
                            return "Loading more properties";
                    }
                }
            }

            public string File
            {
                get { return "MoySkladImportExcel.ascx"; }
            }

            #endregion
        }

        private class MoySkladSetting : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Настройки";

                        case "en":
                            return "Settings";

                        default:
                            return "Settings";
                    }
                }
            }

            public string File
            {
                get { return "MoySkladModule.ascx"; }
            }

            #endregion
        }

        private class MoySkladExportExcel : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Выгрузка товаров";
                            
                        default:
                            return "Export products";
                    }
                }
            }

            public string File
            {
                get { return "MoySkladExportExcel.ascx"; }
            }

            #endregion
        }

        #region Statistic

        public class ImportStatisticData
        {
            public long Add;
            public long Error;
            public bool IsCompleted;
            public bool IsFaild;
            public bool IsRun;
            public long Processed;
            public long Total;
            public long Update;
        }

        public sealed class ImportStatisticMoySkladExcel
        {
            private static readonly object SyncObject = new object();
            private static readonly ImportStatisticData _data = new ImportStatisticData {IsCompleted = true};

            public static readonly string VirtualFileLogPath = FoldersHelper.GetPath(FolderType.PriceTemp,
                "ImportLogMoySkladExcel.txt", false);

            public static readonly string FileLog = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp,
                "ImportLogMoySkladExcel.txt");

            public static ImportStatisticData Data
            {
                get { return _data; }
            }

            public static long TotalRow
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.Total;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.Total = value;
                    }
                }
            }

            public static long RowPosition
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.Processed;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.Processed = value;
                    }
                }
            }

            public static bool IsRun
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.IsRun;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.IsRun = value;
                    }
                }
            }

            public static long TotalUpdateRow
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.Update;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.Update = value;
                    }
                }
            }

            //public static long TotalAddRow
            //{
            //    get
            //    {
            //        lock (SyncObject)
            //        {
            //            return _data.Add;
            //        }
            //    }
            //    set
            //    {
            //        lock (SyncObject)
            //        {
            //            _data.Add = value;
            //        }
            //    }
            //}

            public static long TotalErrorRow
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.Error;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.Error = value;
                    }
                }
            }

            public static Thread ThreadImport { get; set; }

            public static void Init()
            {
                RowPosition = 0; // так как строка в экселя начинаеться с 1
                TotalRow = 0;
                IsRun = false;
                TotalUpdateRow = 0;
                TotalErrorRow = 0;

                if (File.Exists(FileLog))
                {
                    File.Delete(FileLog);
                }
            }

            public static void WriteLog(string message)
            {
                lock (SyncObject)
                {
                    using (var fs = new FileStream(FileLog, FileMode.Append, FileAccess.Write))
                    using (var sw = new StreamWriter(fs, Encoding.UTF8))
                        sw.WriteLine(message);
                }
            }
        }

        public sealed class ImportStatisticMoySkladRequest
        {
            private static readonly object SyncObject = new object();
            private static readonly ImportStatisticData _data = new ImportStatisticData {IsCompleted = true};
            public static string FileLog = string.Empty;

            public static ImportStatisticData Data
            {
                get { return _data; }
            }

            public static long TotalRow
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.Total;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.Total = value;
                    }
                }
            }

            public static long RowPosition
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.Processed;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.Processed = value;
                    }
                }
            }

            public static bool IsRun
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.IsRun;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.IsRun = value;
                    }
                }
            }

            public static bool IsCompleted
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.IsCompleted;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.IsCompleted = value;
                    }
                }
            }

            public static bool IsFaild
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.IsFaild;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.IsFaild = value;
                    }
                }
            }

            public static long TotalUpdateRow
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.Update;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.Update = value;
                    }
                }
            }

            public static long TotalAddRow
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.Add;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.Add = value;
                    }
                }
            }

            public static long TotalErrorRow
            {
                get
                {
                    lock (SyncObject)
                    {
                        return _data.Error;
                    }
                }
                set
                {
                    lock (SyncObject)
                    {
                        _data.Error = value;
                    }
                }
            }

            public static Thread ThreadImport { get; set; }

            public static void Init(string fileLog)
            {
                RowPosition = 0;
                TotalRow = 0;
                IsRun = false;
                IsCompleted = false;
                IsFaild = false;
                TotalUpdateRow = 0;
                TotalAddRow = 0;
                TotalErrorRow = 0;
                FileLog = fileLog;

                if (File.Exists(FileLog))
                {
                    File.Delete(FileLog);
                }
            }


            public static void WriteLog(string message)
            {
                lock (SyncObject)
                {
                    if (!string.IsNullOrEmpty(FileLog))
                        using (var fs = new FileStream(FileLog, FileMode.Append, FileAccess.Write))
                        using (var sw = new StreamWriter(fs, Encoding.UTF8))
                            sw.WriteLine(message);
                }
            }
        }

        #endregion

    }
}