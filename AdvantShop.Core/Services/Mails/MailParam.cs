//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Text;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;

namespace AdvantShop.Mails
{
    public abstract class MailTemplate
    {
        public abstract MailType Type { get; }

        public string Subject { get; set; }
        public string Body { get; set; }

        public void BuildMail()
        {
            BuildMail(Type.ToString());
        }

        public void BuildMail(string mailType)
        {
            var mailFormat = MailFormatService.GetByType(mailType);
            if (mailFormat != null)
                BuildMail(mailFormat.FormatSubject, mailFormat.FormatText);
            else
            {
                BuildMail("", "");
                Debug.Log.Error("Mail type '" + mailType + "' not found");
            }
        }

        public void BuildMail(string subject, string mailBody)
        {
            var logo = SettingsMain.LogoImageName.IsNotEmpty()
                           ? String.Format("<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" />",
                                           SettingsMain.SiteUrl.Trim('/') + '/' +
                                           FoldersHelper.GetPathRelative(FolderType.Pictures, SettingsMain.LogoImageName, false),
                                           SettingsMain.ShopName)
                           : string.Empty;

            Body = mailBody != null
                        ? FormatString(mailBody).Replace("#LOGO#", logo).Replace("#MAIN_PHONE#", SettingsMain.Phone)
                        : string.Empty;

            Subject = subject != null
                        ? FormatString(subject)
                        : string.Empty;
        }

        protected virtual string FormatString(string formatedStr)
        {
            return string.Empty;
        }
    }
    
    public class RegistrationMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnRegistration; }
        }

        private readonly string _shopUrl;
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _regDate;
        private readonly string _password;
        private readonly string _subsrcibe;
        private readonly string _customerEmail;
        private readonly string _phone;
        private readonly string _patronymic;

        public RegistrationMailTemplate(string shopUrl, string firstName, string lastName, string regDate,
                                        string password, string subsrcibe, string customerEmail, string phone, string patronymic)
        {
            _shopUrl = shopUrl;
            _firstName = firstName;
            _lastName = lastName;
            _regDate = regDate;
            _password = password;
            _subsrcibe = subsrcibe;
            _customerEmail = customerEmail;
            _phone = phone;
            _patronymic = patronymic;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#EMAIL#", _customerEmail);
            formatedStr = formatedStr.Replace("#FIRSTNAME#", _firstName);
            formatedStr = formatedStr.Replace("#LASTNAME#", _lastName);
            formatedStr = formatedStr.Replace("#PHONE#", _phone);
            formatedStr = formatedStr.Replace("#REGDATE#", _regDate);
            formatedStr = formatedStr.Replace("#PASSWORD#", _password);
            formatedStr = formatedStr.Replace("#SUBSRCIBE#", _subsrcibe);
            formatedStr = formatedStr.Replace("#SHOPURL#", _shopUrl);
            formatedStr = formatedStr.Replace("#PATRONYMIC#", _patronymic);
            
            return formatedStr;
        }
    }

    public class PwdRepairMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnPwdRepair; }
        }

        private readonly string _recoveryCode;
        private readonly string _email;
        private readonly string _link;

        public PwdRepairMailTemplate(string recoveryCode, string email, string link)
        {
            _recoveryCode = recoveryCode;
            _email = email;
            _link = link;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#EMAIL#", _email);
            formatedStr = formatedStr.Replace("#RECOVERYCODE#", _recoveryCode);
            formatedStr = formatedStr.Replace("#LINK#", _link);
            return formatedStr;
        }
    }

    public class NewOrderMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnNewOrder; }
        }

        private readonly string _customerContacts;
        private readonly string _shippingMethod;
        private readonly string _paymentType;
        private readonly string _orderTable;
        private readonly string _currentCurrencyCode;
        private readonly string _totalPrice;
        private readonly string _comments;
        private readonly string _email;
        private readonly string _number;
        private readonly string _hash;
        private readonly string _firstName;
        private readonly string _lastName;

        public NewOrderMailTemplate(string number, string email, string customerContacts,
                                    string shippingMethod, string paymentType, string orderTable,
                                    string currentCurrencyCode, string totalPrice, string comments, 
                                    string hash, string firstName, string lastName)
        {
            _number = number;
            _email = email;
            _customerContacts = customerContacts;
            _shippingMethod = shippingMethod;
            _paymentType = paymentType;
            _orderTable = orderTable;
            _currentCurrencyCode = currentCurrencyCode;
            _totalPrice = totalPrice;
            _comments = comments;
            _hash = hash;
            _firstName = firstName;
            _lastName = lastName;
        }

        protected override string FormatString(string formatedStr)
        {
            var sb = new StringBuilder(formatedStr);

            sb.Replace("#ORDER_ID#", _number);
            sb.Replace("#NUMBER#", _number);
            sb.Replace("#EMAIL#", _email);
            sb.Replace("#CUSTOMERCONTACTS#", _customerContacts);
            sb.Replace("#SHIPPINGMETHOD#", _shippingMethod);
            sb.Replace("#PAYMENTTYPE#", _paymentType);
            sb.Replace("#ORDERTABLE#", _orderTable);
            sb.Replace("#CURRENTCURRENCYCODE#", _currentCurrencyCode);
            sb.Replace("#TOTALPRICE#", _totalPrice);
            sb.Replace("#COMMENTS#", _comments);
            sb.Replace("#BILLING_LINK#",
                                                SettingsMain.SiteUrl.Trim('/') + "/checkout/billing?number=" + _number + "&hash=" + _hash);
            sb.Replace("#FIRSTNAME#", _firstName);
            sb.Replace("#LASTNAME#", _lastName);

            return sb.ToString();
        }
    }

    public class OrderStatusMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnChangeOrderStatus; }
        }

        private readonly string _orderStatus;
        private readonly string _statusComment;
        private readonly string _number;
        private readonly string _orderTable;
        private readonly string _trackNumber;

        public OrderStatusMailTemplate(string orderStatus, string statusComment, string number, string orderTable, string trackNumber)
        {
            _orderStatus = orderStatus;
            _statusComment = statusComment;
            _number = number;
            _orderTable = orderTable;
            _trackNumber = trackNumber;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#ORDERID#", _number);
            formatedStr = formatedStr.Replace("#ORDERSTATUS#", _orderStatus);
            formatedStr = formatedStr.Replace("#STATUSCOMMENT#", _statusComment);
            formatedStr = formatedStr.Replace("#NUMBER#", _number);
            formatedStr = formatedStr.Replace("#ORDERTABLE#", _orderTable);
            formatedStr = formatedStr.Replace("#TRACKNUMBER#", _trackNumber);
            return formatedStr;
        }
    }
    
    //public class CustomMessageMailTemplate : MailTemplate
    //{
    //    public override MailType Type
    //    {
    //        get { return MailType.OnSendMessage; }
    //    }

    //    private readonly string _name;
    //    private readonly string _messageText;

    //    public CustomMessageMailTemplate(string name, string messageText)
    //    {
    //        _name = name;
    //        _messageText = messageText;
    //    }

    //    protected override string FormatString(string formatedStr)
    //    {
    //        formatedStr = formatedStr.Replace("#MESSAGETEXT#", _messageText);
    //        formatedStr = formatedStr.Replace("#NAME#", _name);
    //        return formatedStr;
    //    }
    //}
    
    public class FeedbackMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnFeedback; }
        }

        private readonly string _shopUrl;
        private readonly string _shopName;
        private readonly string _userName;
        private readonly string _userEmail;
        private readonly string _userPhone;
        private readonly string _subjectMessage;
        private readonly string _textMessage;
        private readonly string _orderNumber;

        public FeedbackMailTemplate(string shopUrl, string shopName, string userName, string userEmail,
                                    string userPhone, string subjectMessage, string textMessage, string orderNumber)
        {
            _shopUrl = shopUrl;
            _shopName = shopName;
            _userName = userName;
            _userEmail = userEmail;
            _userPhone = userPhone;
            _subjectMessage = subjectMessage;
            _textMessage = textMessage;
            _orderNumber = orderNumber;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#SHOPURL#", _shopUrl);
            formatedStr = formatedStr.Replace("#STORE_NAME#", _shopName);
            formatedStr = formatedStr.Replace("#USERNAME#", _userName);
            formatedStr = formatedStr.Replace("#USEREMAIL#", _userEmail);
            formatedStr = formatedStr.Replace("#USERPHONE#", _userPhone);
            formatedStr = formatedStr.Replace("#SUBJECTMESSAGE#", _subjectMessage);
            formatedStr = formatedStr.Replace("#TEXTMESSAGE#", _textMessage);
            formatedStr = formatedStr.Replace("#ORDERNUMBER#", _orderNumber);
            return formatedStr;
        }
    }

    public class ProductDiscussMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnProductDiscuss; }
        }

        private readonly string _sku;
        private readonly string _productName;
        private readonly string _productLink;
        private readonly string _author;
        private readonly string _date;
        private readonly string _text;
        private readonly string _deleteLink;
        private readonly string _email;

        public ProductDiscussMailTemplate(string sku, string productName, string productLink, string author, string date,
                                          string text, string deleteLink, string email)
        {
            _sku = sku;
            _productName = productName;
            _productLink = productLink;
            _author = author;
            _date = date;
            _text = text;
            _deleteLink = deleteLink;
            _email = email;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#PRODUCTNAME#", _productName);
            formatedStr = formatedStr.Replace("#PRODUCTLINK#", _productLink);
            formatedStr = formatedStr.Replace("#USERMAIL#", _email);
            formatedStr = formatedStr.Replace("#SKU#", _sku);
            formatedStr = formatedStr.Replace("#AUTHOR#", _author);
            formatedStr = formatedStr.Replace("#DATE#", _date);
            formatedStr = formatedStr.Replace("#DELETELINK#", _deleteLink);
            formatedStr = formatedStr.Replace("#TEXT#", _text);
            return formatedStr;
        }
    }

    public class ProductDiscussAnswerMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnProductDiscussAnswer; }
        }

        private readonly string _sku;
        private readonly string _productName;
        private readonly string _productLink;
        private readonly string _author;
        private readonly string _date;
        private readonly string _previousMsgText;
        private readonly string _answerText;

        public ProductDiscussAnswerMailTemplate(string sku, string productName, string productLink, string author, string date,
                                          string previousMsgText, string answerText)
        {
            _sku = sku;
            _productName = productName;
            _productLink = productLink;
            _author = author;
            _date = date;
            _previousMsgText = previousMsgText;
            _answerText = answerText;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#PRODUCTNAME#", _productName);
            formatedStr = formatedStr.Replace("#PRODUCTLINK#", _productLink);
            formatedStr = formatedStr.Replace("#SKU#", _sku);
            formatedStr = formatedStr.Replace("#AUTHOR#", _author);
            formatedStr = formatedStr.Replace("#DATE#", _date);
            formatedStr = formatedStr.Replace("#ANSWER_TEXT#", _answerText);
            formatedStr = formatedStr.Replace("#PREVIOUS_MSG_TEXT#", _previousMsgText);
            return formatedStr;
        }
    }

    //public class QuestionAboutProductMailTemplate : MailTemplate
    //{
    //    public override MailType Type
    //    {
    //        get { return MailType.OnQuestionAboutProduct; }
    //    }

    //    private readonly string _sku;
    //    private readonly string _productName;
    //    private readonly string _productLink;
    //    private readonly string _author;
    //    private readonly string _date;
    //    private readonly string _text;
    //    private readonly string _userMail;

    //    public QuestionAboutProductMailTemplate(string sku, string productName, string productLink, string author,
    //                                            string date, string text, string userMail)
    //    {
    //        _sku = sku;
    //        _productName = productName;
    //        _productLink = productLink;
    //        _author = author;
    //        _date = date;
    //        _text = text;
    //        _userMail = userMail;
    //    }

    //    protected override string FormatString(string formatedStr)
    //    {
    //        formatedStr = formatedStr.Replace("#AUTHOR#", _author);
    //        formatedStr = formatedStr.Replace("#DATE#", _date);
    //        formatedStr = formatedStr.Replace("#TEXT#", _text);
    //        formatedStr = formatedStr.Replace("#USERMAIL#", _userMail);
    //        formatedStr = formatedStr.Replace("#SKU#", _sku);
    //        formatedStr = formatedStr.Replace("#PRODUCTNAME#", _productName);
    //        formatedStr = formatedStr.Replace("#PRODUCTLINK#", _productLink);
    //        return formatedStr;
    //    }
    //}

    //public class SendToFriendMailTemplate : MailTemplate
    //{
    //    public override MailType Type
    //    {
    //        get { return MailType.OnSendFriend; }
    //    }

    //    private readonly string _to;
    //    private readonly string _from;
    //    private readonly string _url;

    //    public SendToFriendMailTemplate(string to,  string from, string url)
    //    {
    //        _to = to;
    //        _from = from;
    //        _url = url;
    //    }

    //    protected override string FormatString(string formatedStr)
    //    {
    //        formatedStr = formatedStr.Replace("#FROM#", _from);
    //        formatedStr = formatedStr.Replace("#URL#", _url);
    //        formatedStr = formatedStr.Replace("#TO#", _to);
    //        return formatedStr;
    //    }
    //}

    public class OrderByRequestMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnOrderByRequest; }
        }

        private readonly string _orderByRequestId;
        private readonly string _artNo;
        private readonly string _productName;
        private readonly string _quantity;
        private readonly string _userName;
        private readonly string _email;
        private readonly string _phone;
        private readonly string _comment;

        private readonly string _color;
        private readonly string _size;
        private readonly string _options;

        public OrderByRequestMailTemplate(string orderByRequestId, string artNo, string productName, string quantity,
                                          string userName, string email, string phone, string comment, string color,
                                          string size, string options)
        {
            _orderByRequestId = orderByRequestId;
            _artNo = artNo;
            _productName = productName;
            _quantity = quantity;
            _userName = userName;
            _email = email;
            _phone = phone;
            _comment = comment;
            _color = color;
            _size = size;
            _options = options;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#ORDERID#", _orderByRequestId);
            formatedStr = formatedStr.Replace("#ARTNO#", _artNo);
            formatedStr = formatedStr.Replace("#PRODUCTNAME#", _productName);
            formatedStr = formatedStr.Replace("#QUANTITY#", _quantity);
            formatedStr = formatedStr.Replace("#USERNAME#", _userName);
            formatedStr = formatedStr.Replace("#EMAIL#", _email);
            formatedStr = formatedStr.Replace("#PHONE#", _phone);
            formatedStr = formatedStr.Replace("#COMMENT#", _comment);

            formatedStr = formatedStr.Replace("#COLOR#", _color);
            formatedStr = formatedStr.Replace("#SIZE#", _size);
            formatedStr = formatedStr.Replace("#OPTIONS#", _options);
            return formatedStr;
        }
    }

    public class LinkByRequestMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnSendLinkByRequest; }
        }

        private readonly string _orderByRequestId;
        private readonly string _userName;
        private readonly string _artNo;
        private readonly string _productName;
        private readonly string _quantity;
        private readonly string _code;
        private readonly string _color;
        private readonly string _size;
        private readonly string _comment;

        public LinkByRequestMailTemplate(string orderByRequestId, string artNo, string productName, string quantity,
                                             string code, string userName, string comment, string color, string size)
        {
            _orderByRequestId = orderByRequestId;
            _artNo = artNo;
            _productName = productName;
            _quantity = quantity;
            _userName = userName;
            _comment = comment;
            _color = color;
            _size = size;
            _code = code;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#NUMBER#", _orderByRequestId);
            formatedStr = formatedStr.Replace("#USERNAME#", _userName);
            formatedStr = formatedStr.Replace("#ARTNO#", _artNo);
            formatedStr = formatedStr.Replace("#PRODUCTNAME#", _productName);
            formatedStr = formatedStr.Replace("#QUANTITY#", _quantity);
            formatedStr = formatedStr.Replace("#LINK#", SettingsMain.SiteUrl + "/preorder/linkbycode?code=" + _code);

            formatedStr = formatedStr.Replace("#COLOR#", _color);
            formatedStr = formatedStr.Replace("#SIZE#", _size);

            formatedStr = formatedStr.Replace("#COMMENT#", _comment);

            return formatedStr;
        }
    }

    public class FailureByRequestMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnSendFailureByRequest; }
        }

        private readonly string _orderByRequestId;
        private readonly string _userName;
        private readonly string _artNo;
        private readonly string _productName;
        private readonly string _quantity;
        private readonly string _color;
        private readonly string _size;
		private readonly string _comment;

        public FailureByRequestMailTemplate(string orderByRequestId, string artNo, string productName,
                                                string quantity, string userName, string comment, string color, string size)
        {
            _orderByRequestId = orderByRequestId;
            _artNo = artNo;
            _productName = productName;
            _quantity = quantity;
            _userName = userName;
			_comment = comment;
            _color = color;
            _size = size;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#NUMBER#", _orderByRequestId);
            formatedStr = formatedStr.Replace("#USERNAME#", _userName);
            formatedStr = formatedStr.Replace("#ARTNO#", _artNo);
            formatedStr = formatedStr.Replace("#PRODUCTNAME#", _productName);
            formatedStr = formatedStr.Replace("#QUANTITY#", _quantity);

            formatedStr = formatedStr.Replace("#COLOR#", _color);
            formatedStr = formatedStr.Replace("#SIZE#", _size);
			formatedStr = formatedStr.Replace("#COMMENT#", _comment);

            return formatedStr;
        }
    }

    public class CertificateMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnSendGiftCertificate; }
        }

        private readonly string _certificateCode;
        private readonly string _fromName;
        private readonly string _toName;
        private readonly string _sum;
        private readonly string _message;

        public CertificateMailTemplate(string certificateCode, string fromName, string toName, string sum,
                                       string message)
        {
            _certificateCode = certificateCode;
            _fromName = fromName;
            _toName = toName;
            _sum = sum;
            _message = message;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#CODE#", _certificateCode);
            formatedStr = formatedStr.Replace("#FROMNAME#", _fromName);
            formatedStr = formatedStr.Replace("#TONAME#", _toName);
            formatedStr = formatedStr.Replace("#LINK#", SettingsMain.SiteUrl);
            formatedStr = formatedStr.Replace("#SUM#", _sum);
            formatedStr = formatedStr.Replace("#MESSAGE#", _message);

            return formatedStr;
        }
    }

    public class BuyInOneClickMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnBuyInOneClick; }
        }

        private readonly string _number;
        private readonly string _name;
        private readonly string _phone;
        private readonly string _email;
        private readonly string _comment;
        private readonly string _orderTable;
        private readonly string _hash;

        public BuyInOneClickMailTemplate(string number, string name, string phone, string comment, string orderTable, string hash, string email="")
        {
            _number = number;
            _name = name;
            _phone = phone;
            _email = email;
            _comment = comment;
            _orderTable = orderTable;
            _hash = hash;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#ORDER_ID#", _number);
            formatedStr = formatedStr.Replace("#NUMBER#", _number);
            formatedStr = formatedStr.Replace("#NAME#", _name);
            formatedStr = formatedStr.Replace("#COMMENTS#", _comment);
            formatedStr = formatedStr.Replace("#PHONE#", _phone);
            formatedStr = formatedStr.Replace("#EMAIL#", _email);
            formatedStr = formatedStr.Replace("#ORDERTABLE#", _orderTable);
            formatedStr = formatedStr.Replace("#STORE_NAME#", SettingsMain.ShopName);
            formatedStr = formatedStr.Replace("#BILLING_LINK#",
                                                SettingsMain.SiteUrl.Trim('/') + "/checkout/billing?number=" + _number + "&hash=" + _hash);

            return formatedStr;
        }
    }

    public class BillingLinkMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnBillingLink; }
        }

        private readonly string _orderId;
        private readonly string _firstName;
        private readonly string _hash;
        private readonly string _comment;
        private readonly string _orderTable;
        private readonly string _customerContacts;
        private readonly string _orderNumber;

        public BillingLinkMailTemplate(string orderId, string orderNumber, string firstName, string customerContacts, string hash, string comment, string orderTable)
        {
            _orderId = orderId;
            _orderNumber = orderNumber;
            _firstName = firstName;
            _hash = hash;
            _comment = comment;
            _orderTable = orderTable;
            _customerContacts = customerContacts;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#ORDER_ID#", _orderNumber);
            formatedStr = formatedStr.Replace("#FIRSTNAME#", _firstName);
            formatedStr = formatedStr.Replace("#COMMENTS#", _comment);
            formatedStr = formatedStr.Replace("#BILLING_LINK#", 
                                                SettingsMain.SiteUrl.Trim('/') + "/checkout/billing?number=" + _orderNumber + "&hash=" + _hash);
            formatedStr = formatedStr.Replace("#ORDERTABLE#", _orderTable);
            formatedStr = formatedStr.Replace("#STORE_NAME#", SettingsMain.ShopName);
            formatedStr = formatedStr.Replace("#CUSTOMERCONTACTS#", _customerContacts);

            return formatedStr;
        }
    }

    public class SetOrderManagerMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnSetOrderManager; }
        }

        private readonly string _shopUrl;
        private readonly string _shopName;
        private readonly string _managerName;
        private readonly int _orderId;

        public SetOrderManagerMailTemplate(string shopUrl, string shopName, string managerName, int orderId)
        {
            _shopUrl = shopUrl;
            _shopName = shopName;
            _managerName = managerName;
            _orderId = orderId;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#SHOPURL#", _shopUrl);
            formatedStr = formatedStr.Replace("#STORE_NAME#", _shopName);
            formatedStr = formatedStr.Replace("#MANAGER_NAME#", _managerName);
            formatedStr = formatedStr.Replace("#ORDER_ID#", _orderId.ToString());
            return formatedStr;
        }
    }

    #region Task mail templates

    public class TaskMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnTaskCreated; }
        }

        private readonly string _taskId;
        private readonly string _taskGroup;
        private readonly string _name;
        private readonly string _description;
        private readonly string _status;
        private readonly string _priority;
        private readonly string _dueDate;
        private readonly string _dateCreated;
        private readonly string _assignedManager;
        private readonly string _assignedManagerLink;
        private readonly string _appointedManager;
        private readonly string _appointedManagerLink;
        private readonly string _taskUrl;
        private readonly string _attachments;

        public TaskMailTemplate(string taskId, string taskGroup, string name, string description, string status, string priority, string dueDate, string dateCreated, 
            string attachments, string assignedManager, string assignedCustomerId, string appointedManager, string appointedCustomerId)
        {
            _taskId = taskId;
            _taskGroup = HttpUtility.HtmlEncode(taskGroup);
            _name = HttpUtility.HtmlEncode(name);
            _description = description;
            _status = HttpUtility.HtmlEncode(status);
            _priority = HttpUtility.HtmlEncode(priority);
            _dueDate = dueDate;
            _dateCreated = dateCreated;
            _assignedManager = assignedManager;
            _assignedManagerLink = assignedCustomerId.IsNotEmpty() ? UrlService.GetAdminUrl("customers/edit/" + assignedCustomerId) : string.Empty;
            _appointedManager = appointedManager;
            _appointedManagerLink = appointedCustomerId.IsNotEmpty() ? UrlService.GetAdminUrl("customers/edit/" + appointedCustomerId) : string.Empty;
            _taskUrl = UrlService.GetAdminUrl("tasks/view/" + taskId);
            _attachments = attachments;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#TASK_ID#", _taskId);
            formatedStr = formatedStr.Replace("#TASK_GROUP#", _taskGroup);
            formatedStr = formatedStr.Replace("#TASK_NAME#", _name);
            formatedStr = formatedStr.Replace("#TASK_DESCRIPTION#", _description);
            formatedStr = formatedStr.Replace("#TASK_STATUS#", _status);
            formatedStr = formatedStr.Replace("#TASK_PRIORITY#", _priority);
            formatedStr = formatedStr.Replace("#DUEDATE#", _dueDate);
            formatedStr = formatedStr.Replace("#DATE_CREATED#", _dateCreated);
            formatedStr = formatedStr.Replace("#MANAGER_NAME#", _assignedManager);
            formatedStr = formatedStr.Replace("#MANAGER_LINK#", _assignedManagerLink);
            formatedStr = formatedStr.Replace("#APPOINTEDMANAGER#", _appointedManager);
            formatedStr = formatedStr.Replace("#APPOINTEDMANAGER_LINK#", _appointedManagerLink);
            formatedStr = formatedStr.Replace("#TASK_URL#", _taskUrl);
            formatedStr = formatedStr.Replace("#TASK_ATTACHMENTS#", _attachments);
            return formatedStr;
        }
    }

    public class TaskDeletedMailTemplate : TaskMailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnTaskDeleted; }
        }

        private readonly string _modifier;
        private readonly string _modifierLink;

        public TaskDeletedMailTemplate(string modifier, string modifierCustomerId,
            string taskId, string taskGroup, string name, string description, string status, string priority, string dueDate, string dateCreated, string attachments,
            string assignedManager, string assignedCustomerId, string appointedManager, string appointedCustomerId)
            : base(taskId, taskGroup, name, description, status, priority, dueDate, dateCreated, attachments, assignedManager, assignedCustomerId, appointedManager, appointedCustomerId)
        {
            _modifier = modifier;
            _modifierLink = modifierCustomerId.IsNotEmpty() ? UrlService.GetAdminUrl("customers/edit/" + modifierCustomerId) : string.Empty;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = base.FormatString(formatedStr);
            formatedStr = formatedStr.Replace("#MODIFIER#", _modifier);
            formatedStr = formatedStr.Replace("#MODIFIER_LINK#", _modifierLink);
            return formatedStr;
        }
    }

    public class TaskCommentAddedMailTemplate : TaskMailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnTaskCommentAdded; }
        }

        private readonly string _author;
        private readonly string _authorLink;
        private readonly string _comment;

        public TaskCommentAddedMailTemplate(string author, string authorCustomerId, string comment,
            string taskId, string taskGroup, string name, string description, string status, string priority, string dueDate, string dateCreated, string attachments,
            string assignedManager, string assignedCustomerId, string appointedManager, string appointedCustomerId)
            : base(taskId, taskGroup, name, description, status, priority, dueDate, dateCreated, attachments, assignedManager, assignedCustomerId, appointedManager, appointedCustomerId)
        {
            _author = author;
            _authorLink = authorCustomerId.IsNotEmpty() ? UrlService.GetAdminUrl("customers/edit/" + authorCustomerId) : string.Empty;
            _comment = comment;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = base.FormatString(formatedStr);
            formatedStr = formatedStr.Replace("#AUTHOR#", _author);
            formatedStr = formatedStr.Replace("#AUTHOR_LINK#", _authorLink);
            formatedStr = formatedStr.Replace("#COMMENT#", _comment);
            return formatedStr;
        }
    }

    public class TaskChangedMailTemplate : TaskMailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnTaskChanged; }
        }

        private readonly string _modifier;
        private readonly string _modifierLink;
        private readonly string _changesTable;

        public TaskChangedMailTemplate(string changesTable, string modifier, string modifierCustomerId,
            string taskId, string taskGroup, string name, string description, string status, string priority, string dueDate, string dateCreated, string attachments,
            string assignedManager, string assignedCustomerId, string appointedManager, string appointedCustomerId)
            : base(taskId, taskGroup, name, description, status, priority, dueDate, dateCreated, attachments, assignedManager, assignedCustomerId, appointedManager, appointedCustomerId)
        {
            _modifier = modifier;
            _modifierLink = modifierCustomerId.IsNotEmpty() ? UrlService.GetAdminUrl("customers/edit/" + modifierCustomerId) : string.Empty;
            _changesTable = changesTable;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = base.FormatString(formatedStr);
            formatedStr = formatedStr.Replace("#MODIFIER#", _modifier);
            formatedStr = formatedStr.Replace("#MODIFIER_LINK#", _modifierLink);
            formatedStr = formatedStr.Replace("#CHANGES_TABLE#", _changesTable);
            return formatedStr;
        }
    }

    [Obsolete("Use TaskMailTemplate")]
    public class SetManagerTaskMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnSetManagerTask; }
        }

        private readonly string _managerName;
        private readonly string _appointedManager;
        private readonly string _taskName;
        private readonly string _taskDescription;
        private readonly string _taskStatus;
        private readonly string _dueDate;

        public SetManagerTaskMailTemplate(string managerName, string appointedManager, string taskName,
                                          string taskDescription, string taskStatus, string dueDate)
        {
            _managerName = managerName;
            _appointedManager = appointedManager;
            _taskName = taskName;
            _taskDescription = taskDescription;
            _taskStatus = taskStatus;
            _dueDate = dueDate;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#MANAGER_NAME#", _managerName);
            formatedStr = formatedStr.Replace("#APPOINTEDMANAGER#", _appointedManager);
            formatedStr = formatedStr.Replace("#TASK_NAME#", _taskName);
            formatedStr = formatedStr.Replace("#TASK_DESCRIPTION#", _taskDescription);
            formatedStr = formatedStr.Replace("#TASK_STATUS#", _taskStatus);
            formatedStr = formatedStr.Replace("#DUEDATE#", _dueDate);
            return formatedStr;
        }
    }

    [Obsolete("Use ChangeTaskMailTemplate")]
    public class ChangeManagerTaskStatusMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnChangeManagerTaskStatus; }
        }

        private readonly string _managerName;
        private readonly string _appointedManager;
        private readonly string _taskName;
        private readonly string _taskDescription;
        private readonly string _taskStatus;
        private readonly string _dueDate;

        public ChangeManagerTaskStatusMailTemplate(string managerName, string appointedManager, string taskName,
                                                   string taskDescription, string taskStatus, string dueDate)
        {
            _managerName = managerName;
            _appointedManager = appointedManager;
            _taskName = taskName;
            _taskDescription = taskDescription;
            _taskStatus = taskStatus;
            _dueDate = dueDate;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#MANAGER_NAME#", _managerName);
            formatedStr = formatedStr.Replace("#APPOINTEDMANAGER#", _appointedManager);
            formatedStr = formatedStr.Replace("#TASK_NAME#", _taskName);
            formatedStr = formatedStr.Replace("#TASK_DESCRIPTION#", _taskDescription);
            formatedStr = formatedStr.Replace("#TASK_STATUS#", _taskStatus);
            formatedStr = formatedStr.Replace("#DUEDATE#", _dueDate);
            return formatedStr;
        }
    }

    #endregion

    public class LeadMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnLead; }
        }

        private readonly string _leadId;
        private readonly string _name;
        private readonly string _phone;
        private readonly string _email;
        private readonly string _comment;
        private readonly string _orderTable;

        public LeadMailTemplate(string leadId, string name, string phone, string comment, string orderTable, string email = "")
        {
            _leadId = leadId;
            _name = name;
            _phone = phone;
            _email = email;
            _comment = comment;
            _orderTable = orderTable;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#LEAD_ID#", _leadId);
            formatedStr = formatedStr.Replace("#NAME#", _name);
            formatedStr = formatedStr.Replace("#COMMENTS#", _comment);
            formatedStr = formatedStr.Replace("#PHONE#", _phone);
            formatedStr = formatedStr.Replace("#EMAIL#", _email);
            formatedStr = formatedStr.Replace("#ORDERTABLE#", _orderTable);
            formatedStr = formatedStr.Replace("#STORE_NAME#", SettingsMain.ShopName);

            return formatedStr;
        }
    }

    public class ChangeUserCommentTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnChangeUserComment; }
        }

        private readonly string _orderId;
        private readonly string _orderUserComment;
        private readonly string _number;

        public ChangeUserCommentTemplate(string orderId, string orderUserComment, string number)
        {
            _orderId = orderId;
            _orderUserComment = orderUserComment;
            _number = number;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#ORDER_ID#", _orderId);
            formatedStr = formatedStr.Replace("#ORDER_USER_COMMENT#", _orderUserComment);
            formatedStr = formatedStr.Replace("#NUMBER#", _number);
            formatedStr = formatedStr.Replace("#STORE_NAME#", SettingsMain.ShopName);

            return formatedStr;
        }
    }

    public class PayOrderTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnPayOrder; }
        }

        private readonly string _orderId;
        private readonly string _pay;
        private readonly string _number;
        private readonly string _sum;

        public PayOrderTemplate(string orderId, string number, string pay, string sum)
        {
            _orderId = orderId;
            _pay = pay;
            _number = number;
            _sum = sum;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#ORDER_ID#", _orderId);
            formatedStr = formatedStr.Replace("#PAY_STATUS#", _pay);
            formatedStr = formatedStr.Replace("#NUMBER#", _number);
            formatedStr = formatedStr.Replace("#STORE_NAME#", SettingsMain.ShopName);
            formatedStr = formatedStr.Replace("#SUM#", _sum);
            return formatedStr;
        }
    }

    public class SendToCustomerTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnSendToCustomer; }
        }

        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _patronymic;
        private readonly string _text;
        private readonly string _trackNumber;


        public SendToCustomerTemplate(string firstName, string lastName, string patronymic, string text, string trackNumber)
        {
            _firstName = firstName;
            _lastName = lastName;
            _patronymic = patronymic;
            _text = text;
            _trackNumber = trackNumber;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#TEXT#", _text);
            formatedStr = formatedStr.Replace("#FIRSTNAME#", _firstName);
            formatedStr = formatedStr.Replace("#LASTNAME#", _lastName);
            formatedStr = formatedStr.Replace("#PATRONYMIC#", _patronymic);
            formatedStr = formatedStr.Replace("#STORE_NAME#", SettingsMain.ShopName);
            formatedStr = formatedStr.Replace("#TRACKNUMBER#", _trackNumber);
            return formatedStr;
        }
    }

    public class UserRegisteredMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnUserRegistered; }
        }

        private readonly string _email;
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _regDate;
        private readonly string _hash;

        public UserRegisteredMailTemplate(string email, string firstName, string lastName, string regDate, string hash)
        {
            _email = email;
            _firstName = firstName;
            _lastName = lastName;
            _regDate = regDate;
            _hash = hash;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#EMAIL#", _email);
            formatedStr = formatedStr.Replace("#FIRSTNAME#", _firstName);
            formatedStr = formatedStr.Replace("#LASTNAME#", _lastName);
            formatedStr = formatedStr.Replace("#REGDATE#", _regDate);
            formatedStr = formatedStr.Replace("#STORE_NAME#", SettingsMain.ShopName);
            formatedStr = formatedStr.Replace("#STORE_URL#", SettingsMain.SiteUrl);
            formatedStr = formatedStr.Replace("#LINK#", UrlService.GetAdminUrl("account/setpassword?email=" + _email + "&hash=" + _hash));
            return formatedStr;
        }
    }

    public class UserPasswordRepairMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnUserPasswordRepair; }
        }

        private readonly string _email;
        private readonly string _hash;

        public UserPasswordRepairMailTemplate(string email, string hash)
        {
            _email = email;
            _hash = hash;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#EMAIL#", _email);
            formatedStr = formatedStr.Replace("#STORE_NAME#", SettingsMain.ShopName);
            formatedStr = formatedStr.Replace("#STORE_URL#", SettingsMain.SiteUrl);
            formatedStr = formatedStr.Replace("#LINK#", UrlService.GetAdminUrl("account/forgotpassword?email=" + _email + "&hash=" + _hash));
            return formatedStr;
        }
    }
    
    public class OrderCommentAddedMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnOrderCommentAdded; }
        }

        private readonly string _author;
        private readonly string _authorLink;
        private readonly string _comment;
        private readonly string _orderLink;
        private readonly string _orderNumber;

        public OrderCommentAddedMailTemplate(string author, string authorCustomerId, string comment, string orderId, string orderNumber)
        {
            _author = author;
            _authorLink = authorCustomerId.IsNotEmpty() ? UrlService.GetAdminUrl("customers/edit/" + authorCustomerId) : string.Empty;
            _comment = comment;
            _orderLink = orderId.IsNotEmpty() ? UrlService.GetAdminUrl("orders/edit/" + orderId) : string.Empty;
            _orderNumber = orderNumber;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#AUTHOR#", _author);
            formatedStr = formatedStr.Replace("#AUTHOR_LINK#", _authorLink);
            formatedStr = formatedStr.Replace("#COMMENT#", _comment);
            formatedStr = formatedStr.Replace("#ORDER_LINK#", _orderLink);
            formatedStr = formatedStr.Replace("#ORDER_NUMBER#", _orderNumber);
            return formatedStr;
        }
    }

    public class CustomerCommentAddedMailTemplate : MailTemplate
    {
        public override MailType Type
        {
            get { return MailType.OnCustomerCommentAdded; }
        }

        private readonly string _author;
        private readonly string _authorLink;
        private readonly string _comment;
        private readonly string _customerLink;
        private readonly string _customerName;

        public CustomerCommentAddedMailTemplate(string author, string authorCustomerId, string comment, string customerId, string customerName)
        {
            _author = author;
            _authorLink = authorCustomerId.IsNotEmpty() ? UrlService.GetAdminUrl("customers/edit/" + authorCustomerId) : string.Empty;
            _comment = comment;
            _customerLink = customerId.IsNotEmpty() ? UrlService.GetAdminUrl("customers/edit/" + customerId) : string.Empty;
            _customerName = customerName;
        }

        protected override string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#AUTHOR#", _author);
            formatedStr = formatedStr.Replace("#AUTHOR_LINK#", _authorLink);
            formatedStr = formatedStr.Replace("#COMMENT#", _comment);
            formatedStr = formatedStr.Replace("#CUSTOMER_LINK#", _customerLink);
            formatedStr = formatedStr.Replace("#CUSTOMER#", _customerName);
            return formatedStr;
        }
    }
}