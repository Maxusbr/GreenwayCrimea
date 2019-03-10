//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Mails
{
    public enum MailType
    {
        None,
        OnRegistration,
        OnPwdRepair,
        OnNewOrder,
        OnChangeOrderStatus,
        OnFeedback,
        OnProductDiscuss,
        OnOrderByRequest,
        OnSendLinkByRequest,
        OnSendFailureByRequest,
        OnSendGiftCertificate,
        OnBuyInOneClick,
        OnBillingLink,
        OnSetOrderManager,
        OnSetManagerTask,
        OnChangeManagerTaskStatus,
        OnLead,
        OnProductDiscussAnswer,
        OnChangeUserComment,
        OnPayOrder,
        OnTaskChanged,
        OnTaskCreated,
        OnTaskDeleted,
        OnTaskCommentAdded,
        OnSendToCustomer,
        OnUserRegistered,
        OnUserPasswordRepair,
        OnOrderCommentAdded,
        OnCustomerCommentAdded
    }
}