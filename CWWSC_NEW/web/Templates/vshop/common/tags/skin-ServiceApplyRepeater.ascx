<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<tr>
    <td colspan="3" style="border:none;white-space:nowrap; overflow:hidden; text-overflow:ellipsis;padding-left:10px;text-align:left;">
       品类：<%# Hidistro.ControlPanel.Commodities.ServiceClassHelper.GetClassNameById(Eval("ScIDs").ToString())%>
    </td>   
</tr>
<tr>
    <td colspan="3" style="white-space:nowrap; overflow:hidden; text-overflow:ellipsis;padding-left:10px;text-align:left;">
        区域：<%# Eval("RegionName")%>        
    </td>
</tr>
<tr>
    <td style="border-bottom: 10px solid #E5E5E5;text-align:left;padding:6px 0 6px 10px ;">
        <%# Eval("ApplyDate") %>
    </td>
    <td style="border-bottom: 10px solid #E5E5E5;padding:6px 0;">
       <%# Eval("State").ToString()=="0"?"待审核": Eval("State").ToString()=="1" ? "已通过" : "未通过"%>
    </td>
    <td style="border-bottom: 10px solid #E5E5E5;padding:6px 0;text-align: right;">
        <a class="doc" DcID="<%#Eval("DcID")%>"><img style="width:1.8rem;" src="../../templates/vshop/common/images/editMember.png" /> 编辑</a><a  class="del" Date="<%# Eval("ApplyDate") %>" DcID="<%#Eval("DcID")%>"><img style="width:2rem;" src="../../templates/vshop/common/images/delMember.png" /> 删除</a>
    </td>
</tr>
   
    
