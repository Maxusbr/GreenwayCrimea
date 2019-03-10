namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public interface IBizObjectFieldComparer<TBizObject> where TBizObject : IBizObject
    {
        FieldComparer FieldComparer { get; set; }
        bool CheckField(TBizObject bizObject);
        string FieldName { get; }
        string FieldValueObjectName { get; }

        /// <summary>
        /// Is valid comparer
        /// </summary>
        /// <returns></returns>
        bool IsValid();
    }
}
