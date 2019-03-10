using System.Collections.Generic;

namespace AdvantShop.Module.MoySklad.Domain
{
    #region Base

    public interface IEntityList<T> where T : class
    {
        MetaData Meta { get; set; }
        List<T> Rows { get; set; }
    }

    #endregion

    #region Product

    public class EntityProductResponse
    {
        public MetaData Meta { get; set; }
        public string Id { get; set; }
        public string ExternalCode { get; set; }
        public string Name { get; set; }
    }

    public class EntityProductsResponse : IEntityList<EntityProductResponse>
    {
        public MetaData Meta { get; set; }
        public List<EntityProductResponse> Rows { get; set; }
    }

    public class EntityProductMetadataResponse
    {
        public MetaData Meta { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<PriceType> PriceTypes { get; set; }
    }

    public class EntityVariantsResponse : IEntityList<EntityVariantResponse>
    {
        public MetaData Meta { get; set; }
        public List<EntityVariantResponse> Rows { get; set; }
    }

    public class EntityVariantResponse : EntityVariant
    {
        //public MetaData Meta { get; set; }
        public string Id { get; set; }
        //public string ExternalCode { get; set; }
    }

    #endregion

    #region Category

    public class EntityProductFoldersResponse : IEntityList<EntityProductFolderFromList>
    {
        public MetaData Meta { get; set; }
        public List<EntityProductFolderFromList> Rows { get; set; }
    }

    public class EntityProductFolderResponse
    {
        public MetaData Meta { get; set; }
        public string Id { get; set; }
        public string ExternalCode { get; set; }
        public string Name { get; set; }
    }

    #endregion

    #region State

    public class EntityStatesResponse : IEntityList<EntityStateResponse>
    {
        public MetaData Meta { get; set; }
        public List<EntityStateResponse> Rows { get; set; }
    }

    public class EntityStateResponse : EntityState
    {
        public MetaData Meta { get; set; }
    }

    #endregion

    #region CustomerOrder

    public class EntityCustomerOrdersResponse : IEntityList<EntityCustomerOrderResponse>
    {
        public MetaData Meta { get; set; }
        public List<EntityCustomerOrderResponse> Rows { get; set; }
    }

    public class EntityCustomerOrderResponse : EntityCustomerOrder
    {
        public MetaData Meta { get; set; }
    }

    #endregion

    #region Counterparty

    public class EntityCounterpartiesResponse : IEntityList<EntityCounterpartyResponse>
    {
        public MetaData Meta { get; set; }
        public List<EntityCounterpartyResponse> Rows { get; set; }
    }

    public class EntityCounterpartyResponse : EntityCounterparty
    {
        /// <summary>
        /// Ссылка на массив контактных лиц Контрагента
        /// </summary>
        public EntityMetaContainer Contactpersons { get; set; }
    }

    public class EntityCounterpartyContactpersonsResponse : IEntityList<EntityCounterpartyContactpersonResponse>
    {
        public MetaData Meta { get; set; }
        public List<EntityCounterpartyContactpersonResponse> Rows { get; set; }
    }

    public class EntityCounterpartyContactpersonResponse : EntityCounterpartyContactperson { }

    #endregion

}