﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hidistro.Jobs.AllHereServiceReferenceJob {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://jlinterface.jlserver", ConfigurationName="AllHereServiceReferenceJob.MPFTOJL")]
    public interface MPFTOJL {
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="Sql2JsonReturn")]
        string Sql2Json(string sInputSql, string sparameterMC);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="MPFTOJL_SPXXReturn")]
        string MPFTOJL_SPXX(string JSONXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="MPFTOJL_DHD_CJReturn")]
        string MPFTOJL_DHD_CJ(string JSONXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="MPFTOJL_DHD_FHReturn")]
        string MPFTOJL_DHD_FH(string JSONXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="MPFTOJL_DHD_QSReturn")]
        string MPFTOJL_DHD_QS(string JSONXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="MPFTOJL_DHD_THReturn")]
        string MPFTOJL_DHD_TH(string JSONXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="MPFTOJL_FPReturn")]
        string MPFTOJL_FP(string JSONXML);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface MPFTOJLChannel : Hidistro.Jobs.AllHereServiceReferenceJob.MPFTOJL, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MPFTOJLClient : System.ServiceModel.ClientBase<Hidistro.Jobs.AllHereServiceReferenceJob.MPFTOJL>, Hidistro.Jobs.AllHereServiceReferenceJob.MPFTOJL {
        
        public MPFTOJLClient() {
        }
        
        public MPFTOJLClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MPFTOJLClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MPFTOJLClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MPFTOJLClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Sql2Json(string sInputSql, string sparameterMC) {
            return base.Channel.Sql2Json(sInputSql, sparameterMC);
        }
        
        public string MPFTOJL_SPXX(string JSONXML) {
            return base.Channel.MPFTOJL_SPXX(JSONXML);
        }
        
        public string MPFTOJL_DHD_CJ(string JSONXML) {
            return base.Channel.MPFTOJL_DHD_CJ(JSONXML);
        }
        
        public string MPFTOJL_DHD_FH(string JSONXML) {
            return base.Channel.MPFTOJL_DHD_FH(JSONXML);
        }
        
        public string MPFTOJL_DHD_QS(string JSONXML) {
            return base.Channel.MPFTOJL_DHD_QS(JSONXML);
        }
        
        public string MPFTOJL_DHD_TH(string JSONXML) {
            return base.Channel.MPFTOJL_DHD_TH(JSONXML);
        }
        
        public string MPFTOJL_FP(string JSONXML) {
            return base.Channel.MPFTOJL_FP(JSONXML);
        }
    }
}
