<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<tr>
    <td style="padding:10px 0 10px 10px;">
        <%# Eval("DsName") %>
    </td>
    <td style="padding:10px 0">
       <%# Eval("DsPhone")%>
    </td>
    <td style="padding:10px 0">
       <%# Eval("DsType").ToString()=="0"?"导购店员":"服务店员"%>
    </td>
</tr>
<tr>
    <td style="padding-left:10px; border-bottom: 10px solid #E5E5E5">
       <%# Eval("IsRz").ToString()=="0"?"状态：<span style='color:red'>未认证</span>":"状态：<span style='color:green'>已认证</span>"%>
    </td>
    <td colspan="2" style="text-align:right;padding-right:10px;border-bottom: 10px solid #E5E5E5">
       <a class="doc"   style="display:<%# Eval("IsRz").ToString()=="0"?"black":"none"%>"     href="ClerkAuthentication.aspx?DisUserId=<%#Eval("DisUserId")%>">
           <img style="width:2rem;" src="../../templates/vshop/common/images/config.png" /> 认证</a><a class="doc docEdit" DsId="<%#Eval("DsID")%>"><img style="width:1.8rem;" src="../../templates/vshop/common/images/editMember.png" /> 编辑</a><a class="del" DsID="<%#Eval("DsID")%>"><img style="width:2rem;" src="../../templates/vshop/common/images/delMember.png" /> 删除</a>
    </td>
</tr>
   
    
