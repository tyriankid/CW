﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hidistro.UI.Web.KuKaiServiceReferenceReturn {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://service.pub.streetorder.so.bs.nc/IStreetReturnReceiveWebInfor", ConfigurationName="KuKaiServiceReferenceReturn.IStreetReturnReceiveWebInfor")]
    public interface IStreetReturnReceiveWebInfor {
        
        // CODEGEN: 操作 sendReturnOrderToNC 以后生成的消息协定不是 RPC，也不是换行文档。
        [System.ServiceModel.OperationContractAttribute(Action="http://service.pub.streetorder.so.bs.nc/IStreetReturnReceiveWebInfor/sendReturnOr" +
            "derToNC", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNCResponse1 sendReturnOrderToNC(Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNCRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.pub.streetorder.so.bs.nc/IStreetReturnReceiveWebInfor")]
    public partial class sendReturnOrderToNC : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string voField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string vo {
            get {
                return this.voField;
            }
            set {
                this.voField = value;
                this.RaisePropertyChanged("vo");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.pub.streetorder.so.bs.nc/IStreetReturnReceiveWebInfor")]
    public partial class sendReturnOrderToNCResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string returnArgField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string returnArg {
            get {
                return this.returnArgField;
            }
            set {
                this.returnArgField = value;
                this.RaisePropertyChanged("returnArg");
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
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class sendReturnOrderToNCRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.pub.streetorder.so.bs.nc/IStreetReturnReceiveWebInfor", Order=0)]
        public Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNC sendReturnOrderToNC;
        
        public sendReturnOrderToNCRequest() {
        }
        
        public sendReturnOrderToNCRequest(Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNC sendReturnOrderToNC) {
            this.sendReturnOrderToNC = sendReturnOrderToNC;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class sendReturnOrderToNCResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.pub.streetorder.so.bs.nc/IStreetReturnReceiveWebInfor", Order=0)]
        public Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNCResponse sendReturnOrderToNCResponse;
        
        public sendReturnOrderToNCResponse1() {
        }
        
        public sendReturnOrderToNCResponse1(Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNCResponse sendReturnOrderToNCResponse) {
            this.sendReturnOrderToNCResponse = sendReturnOrderToNCResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IStreetReturnReceiveWebInforChannel : Hidistro.UI.Web.KuKaiServiceReferenceReturn.IStreetReturnReceiveWebInfor, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class StreetReturnReceiveWebInforClient : System.ServiceModel.ClientBase<Hidistro.UI.Web.KuKaiServiceReferenceReturn.IStreetReturnReceiveWebInfor>, Hidistro.UI.Web.KuKaiServiceReferenceReturn.IStreetReturnReceiveWebInfor {
        
        public StreetReturnReceiveWebInforClient() {
        }
        
        public StreetReturnReceiveWebInforClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public StreetReturnReceiveWebInforClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StreetReturnReceiveWebInforClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StreetReturnReceiveWebInforClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNCResponse1 Hidistro.UI.Web.KuKaiServiceReferenceReturn.IStreetReturnReceiveWebInfor.sendReturnOrderToNC(Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNCRequest request) {
            return base.Channel.sendReturnOrderToNC(request);
        }
        
        public Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNCResponse sendReturnOrderToNC(Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNC sendReturnOrderToNC1) {
            Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNCRequest inValue = new Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNCRequest();
            inValue.sendReturnOrderToNC = sendReturnOrderToNC1;
            Hidistro.UI.Web.KuKaiServiceReferenceReturn.sendReturnOrderToNCResponse1 retVal = ((Hidistro.UI.Web.KuKaiServiceReferenceReturn.IStreetReturnReceiveWebInfor)(this)).sendReturnOrderToNC(inValue);
            return retVal.sendReturnOrderToNCResponse;
        }
    }
}
