﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdvantShop.Module.SmsNotifications.sms4b {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="SMS4B", ConfigurationName="sms4b.WSSMSoap")]
    public interface WSSMSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/StartSession", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        long StartSession(string Login, string Password, short Gmt);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/StartSession", ReplyAction="*")]
        System.Threading.Tasks.Task<long> StartSessionAsync(string Login, string Password, short Gmt);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/CancelGroup", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int CancelGroup(long SessionID, long Group);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/CancelGroup", ReplyAction="*")]
        System.Threading.Tasks.Task<int> CancelGroupAsync(long SessionID, long Group);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/CloseSession", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int CloseSession(long SessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/CloseSession", ReplyAction="*")]
        System.Threading.Tasks.Task<int> CloseSessionAsync(long SessionID);
        
        // CODEGEN: Parameter 'List' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlArrayItemAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/GroupSMS", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        AdvantShop.Module.SmsNotifications.sms4b.GroupSMSResponse GroupSMS(AdvantShop.Module.SmsNotifications.sms4b.GroupSMSRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/GroupSMS", ReplyAction="*")]
        System.Threading.Tasks.Task<AdvantShop.Module.SmsNotifications.sms4b.GroupSMSResponse> GroupSMSAsync(AdvantShop.Module.SmsNotifications.sms4b.GroupSMSRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/CheckSMS", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        AdvantShop.Module.SmsNotifications.sms4b.CheckSMSResult CheckSMS(long SessionId, string[] Guids);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/CheckSMS", ReplyAction="*")]
        System.Threading.Tasks.Task<AdvantShop.Module.SmsNotifications.sms4b.CheckSMSResult> CheckSMSAsync(long SessionId, string[] Guids);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/LoadSMS", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        AdvantShop.Module.SmsNotifications.sms4b.LoadSMSResult LoadSMS(long SessionId, string ChangesFrom, int Flags);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/LoadSMS", ReplyAction="*")]
        System.Threading.Tasks.Task<AdvantShop.Module.SmsNotifications.sms4b.LoadSMSResult> LoadSMSAsync(long SessionId, string ChangesFrom, int Flags);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/ParamSMS", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        AdvantShop.Module.SmsNotifications.sms4b.ParamSMSResult ParamSMS(long SessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/ParamSMS", ReplyAction="*")]
        System.Threading.Tasks.Task<AdvantShop.Module.SmsNotifications.sms4b.ParamSMSResult> ParamSMSAsync(long SessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/SendSMS", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string SendSMS(string Login, string Password, string Source, long Phone, string Text);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/SendSMS", ReplyAction="*")]
        System.Threading.Tasks.Task<string> SendSMSAsync(string Login, string Password, string Source, long Phone, string Text);
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/GetInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetInfo();
        
        [System.ServiceModel.OperationContractAttribute(Action="SMS4B/GetInfo", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetInfoAsync();
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="SMS4B")]
    public partial class GroupSMSList : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string gField;
        
        private string dField;
        
        private string bField;
        
        private int eField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string G {
            get {
                return this.gField;
            }
            set {
                this.gField = value;
                this.RaisePropertyChanged("G");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string D {
            get {
                return this.dField;
            }
            set {
                this.dField = value;
                this.RaisePropertyChanged("D");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string B {
            get {
                return this.bField;
            }
            set {
                this.bField = value;
                this.RaisePropertyChanged("B");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public int E {
            get {
                return this.eField;
            }
            set {
                this.eField = value;
                this.RaisePropertyChanged("E");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="SMS4B")]
    public partial class ParamSMSResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int resultField;
        
        private double restField;
        
        private string addressesField;
        
        private long addrMaskField;
        
        private string uTCField;
        
        private int durationField;
        
        private int limitField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public double Rest {
            get {
                return this.restField;
            }
            set {
                this.restField = value;
                this.RaisePropertyChanged("Rest");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Addresses {
            get {
                return this.addressesField;
            }
            set {
                this.addressesField = value;
                this.RaisePropertyChanged("Addresses");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public long AddrMask {
            get {
                return this.addrMaskField;
            }
            set {
                this.addrMaskField = value;
                this.RaisePropertyChanged("AddrMask");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string UTC {
            get {
                return this.uTCField;
            }
            set {
                this.uTCField = value;
                this.RaisePropertyChanged("UTC");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public int Duration {
            get {
                return this.durationField;
            }
            set {
                this.durationField = value;
                this.RaisePropertyChanged("Duration");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public int Limit {
            get {
                return this.limitField;
            }
            set {
                this.limitField = value;
                this.RaisePropertyChanged("Limit");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="SMS4B")]
    public partial class SMSList : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string gField;
        
        private string dField;
        
        private string bField;
        
        private int eField;
        
        private int aField;
        
        private int pField;
        
        private string mField;
        
        private string tField;
        
        private string sField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string G {
            get {
                return this.gField;
            }
            set {
                this.gField = value;
                this.RaisePropertyChanged("G");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string D {
            get {
                return this.dField;
            }
            set {
                this.dField = value;
                this.RaisePropertyChanged("D");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string B {
            get {
                return this.bField;
            }
            set {
                this.bField = value;
                this.RaisePropertyChanged("B");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public int E {
            get {
                return this.eField;
            }
            set {
                this.eField = value;
                this.RaisePropertyChanged("E");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int A {
            get {
                return this.aField;
            }
            set {
                this.aField = value;
                this.RaisePropertyChanged("A");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public int P {
            get {
                return this.pField;
            }
            set {
                this.pField = value;
                this.RaisePropertyChanged("P");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string M {
            get {
                return this.mField;
            }
            set {
                this.mField = value;
                this.RaisePropertyChanged("M");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string T {
            get {
                return this.tField;
            }
            set {
                this.tField = value;
                this.RaisePropertyChanged("T");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string S {
            get {
                return this.sField;
            }
            set {
                this.sField = value;
                this.RaisePropertyChanged("S");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="SMS4B")]
    public partial class LoadSMSResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int resultField;
        
        private SMSList[] listField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=1)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public SMSList[] List {
            get {
                return this.listField;
            }
            set {
                this.listField = value;
                this.RaisePropertyChanged("List");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="SMS4B")]
    public partial class CheckSMSResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int resultField;
        
        private CheckSMSList[] listField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=1)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public CheckSMSList[] List {
            get {
                return this.listField;
            }
            set {
                this.listField = value;
                this.RaisePropertyChanged("List");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="SMS4B")]
    public partial class CheckSMSList : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string gField;
        
        private int rField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string G {
            get {
                return this.gField;
            }
            set {
                this.gField = value;
                this.RaisePropertyChanged("G");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int R {
            get {
                return this.rField;
            }
            set {
                this.rField = value;
                this.RaisePropertyChanged("R");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="SMS4B")]
    public partial class GroupSMSResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int resultField;
        
        private long groupField;
        
        private CheckSMSList[] listField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public long Group {
            get {
                return this.groupField;
            }
            set {
                this.groupField = value;
                this.RaisePropertyChanged("Group");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=2)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public CheckSMSList[] List {
            get {
                return this.listField;
            }
            set {
                this.listField = value;
                this.RaisePropertyChanged("List");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GroupSMS", WrapperNamespace="SMS4B", IsWrapped=true)]
    public partial class GroupSMSRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="SMS4B", Order=0)]
        public long SessionId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="SMS4B", Order=1)]
        public long Group;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="SMS4B", Order=2)]
        public string Source;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="SMS4B", Order=3)]
        public int Encoding;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="SMS4B", Order=4)]
        public string Body;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="SMS4B", Order=5)]
        public string Off;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="SMS4B", Order=6)]
        public string Start;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="SMS4B", Order=7)]
        public string Period;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="SMS4B", Order=8)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public AdvantShop.Module.SmsNotifications.sms4b.GroupSMSList[] List;
        
        public GroupSMSRequest() {
        }
        
        public GroupSMSRequest(long SessionId, long Group, string Source, int Encoding, string Body, string Off, string Start, string Period, AdvantShop.Module.SmsNotifications.sms4b.GroupSMSList[] List) {
            this.SessionId = SessionId;
            this.Group = Group;
            this.Source = Source;
            this.Encoding = Encoding;
            this.Body = Body;
            this.Off = Off;
            this.Start = Start;
            this.Period = Period;
            this.List = List;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GroupSMSResponse", WrapperNamespace="SMS4B", IsWrapped=true)]
    public partial class GroupSMSResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="SMS4B", Order=0)]
        public AdvantShop.Module.SmsNotifications.sms4b.GroupSMSResult GroupSMSResult;
        
        public GroupSMSResponse() {
        }
        
        public GroupSMSResponse(AdvantShop.Module.SmsNotifications.sms4b.GroupSMSResult GroupSMSResult) {
            this.GroupSMSResult = GroupSMSResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WSSMSoapChannel : AdvantShop.Module.SmsNotifications.sms4b.WSSMSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WSSMSoapClient : System.ServiceModel.ClientBase<AdvantShop.Module.SmsNotifications.sms4b.WSSMSoap>, AdvantShop.Module.SmsNotifications.sms4b.WSSMSoap {
        
        public WSSMSoapClient() {
        }
        
        public WSSMSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WSSMSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSSMSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSSMSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public long StartSession(string Login, string Password, short Gmt) {
            return base.Channel.StartSession(Login, Password, Gmt);
        }
        
        public System.Threading.Tasks.Task<long> StartSessionAsync(string Login, string Password, short Gmt) {
            return base.Channel.StartSessionAsync(Login, Password, Gmt);
        }
        
        public int CancelGroup(long SessionID, long Group) {
            return base.Channel.CancelGroup(SessionID, Group);
        }
        
        public System.Threading.Tasks.Task<int> CancelGroupAsync(long SessionID, long Group) {
            return base.Channel.CancelGroupAsync(SessionID, Group);
        }
        
        public int CloseSession(long SessionID) {
            return base.Channel.CloseSession(SessionID);
        }
        
        public System.Threading.Tasks.Task<int> CloseSessionAsync(long SessionID) {
            return base.Channel.CloseSessionAsync(SessionID);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AdvantShop.Module.SmsNotifications.sms4b.GroupSMSResponse AdvantShop.Module.SmsNotifications.sms4b.WSSMSoap.GroupSMS(AdvantShop.Module.SmsNotifications.sms4b.GroupSMSRequest request) {
            return base.Channel.GroupSMS(request);
        }
        
        public AdvantShop.Module.SmsNotifications.sms4b.GroupSMSResult GroupSMS(long SessionId, long Group, string Source, int Encoding, string Body, string Off, string Start, string Period, AdvantShop.Module.SmsNotifications.sms4b.GroupSMSList[] List) {
            AdvantShop.Module.SmsNotifications.sms4b.GroupSMSRequest inValue = new AdvantShop.Module.SmsNotifications.sms4b.GroupSMSRequest();
            inValue.SessionId = SessionId;
            inValue.Group = Group;
            inValue.Source = Source;
            inValue.Encoding = Encoding;
            inValue.Body = Body;
            inValue.Off = Off;
            inValue.Start = Start;
            inValue.Period = Period;
            inValue.List = List;
            AdvantShop.Module.SmsNotifications.sms4b.GroupSMSResponse retVal = ((AdvantShop.Module.SmsNotifications.sms4b.WSSMSoap)(this)).GroupSMS(inValue);
            return retVal.GroupSMSResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<AdvantShop.Module.SmsNotifications.sms4b.GroupSMSResponse> AdvantShop.Module.SmsNotifications.sms4b.WSSMSoap.GroupSMSAsync(AdvantShop.Module.SmsNotifications.sms4b.GroupSMSRequest request) {
            return base.Channel.GroupSMSAsync(request);
        }
        
        public System.Threading.Tasks.Task<AdvantShop.Module.SmsNotifications.sms4b.GroupSMSResponse> GroupSMSAsync(long SessionId, long Group, string Source, int Encoding, string Body, string Off, string Start, string Period, AdvantShop.Module.SmsNotifications.sms4b.GroupSMSList[] List) {
            AdvantShop.Module.SmsNotifications.sms4b.GroupSMSRequest inValue = new AdvantShop.Module.SmsNotifications.sms4b.GroupSMSRequest();
            inValue.SessionId = SessionId;
            inValue.Group = Group;
            inValue.Source = Source;
            inValue.Encoding = Encoding;
            inValue.Body = Body;
            inValue.Off = Off;
            inValue.Start = Start;
            inValue.Period = Period;
            inValue.List = List;
            return ((AdvantShop.Module.SmsNotifications.sms4b.WSSMSoap)(this)).GroupSMSAsync(inValue);
        }
        
        public AdvantShop.Module.SmsNotifications.sms4b.CheckSMSResult CheckSMS(long SessionId, string[] Guids) {
            return base.Channel.CheckSMS(SessionId, Guids);
        }
        
        public System.Threading.Tasks.Task<AdvantShop.Module.SmsNotifications.sms4b.CheckSMSResult> CheckSMSAsync(long SessionId, string[] Guids) {
            return base.Channel.CheckSMSAsync(SessionId, Guids);
        }
        
        public AdvantShop.Module.SmsNotifications.sms4b.LoadSMSResult LoadSMS(long SessionId, string ChangesFrom, int Flags) {
            return base.Channel.LoadSMS(SessionId, ChangesFrom, Flags);
        }
        
        public System.Threading.Tasks.Task<AdvantShop.Module.SmsNotifications.sms4b.LoadSMSResult> LoadSMSAsync(long SessionId, string ChangesFrom, int Flags) {
            return base.Channel.LoadSMSAsync(SessionId, ChangesFrom, Flags);
        }
        
        public AdvantShop.Module.SmsNotifications.sms4b.ParamSMSResult ParamSMS(long SessionId) {
            return base.Channel.ParamSMS(SessionId);
        }
        
        public System.Threading.Tasks.Task<AdvantShop.Module.SmsNotifications.sms4b.ParamSMSResult> ParamSMSAsync(long SessionId) {
            return base.Channel.ParamSMSAsync(SessionId);
        }
        
        public string SendSMS(string Login, string Password, string Source, long Phone, string Text) {
            return base.Channel.SendSMS(Login, Password, Source, Phone, Text);
        }
        
        public System.Threading.Tasks.Task<string> SendSMSAsync(string Login, string Password, string Source, long Phone, string Text) {
            return base.Channel.SendSMSAsync(Login, Password, Source, Phone, Text);
        }
        
        public string GetInfo() {
            return base.Channel.GetInfo();
        }
        
        public System.Threading.Tasks.Task<string> GetInfoAsync() {
            return base.Channel.GetInfoAsync();
        }
    }
}
