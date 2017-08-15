<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportFileStore.aspx.cs"  MasterPageFile="~/Admin/Admin.Master"  Inherits="Admin_store_ImportFileStore" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">


<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>导入门店列表</h1>
	    <span>说明：若列表中提示源数据中有供销商名称，点击保存，则更新原分销商的数据。</span>
     </div>
       
	  <asp:FileUpload ID="fileUpload" runat="server"  />
      <asp:Button runat="server" ID="btnUnPack" class="searchbutton" Text="导入信息" OnClick="btnUnPack_Click"></asp:Button>
    <asp:Button runat="server" ID="btnBc" class="searchbutton" Text="保存" OnClick="btnBc_Click" />

    <asp:Button runat="server" ID="file" class="searchbutton" Text="下载模板表格" style="float:right;" OnClick="file_Click" />
	  <!--数据列表区域-->
  <div class="datalist">
        <div style="text-align:center; width:100%">
            <asp:TextBox ID="tboxErr" runat="server" TextMode="MultiLine" Width="100%" Rows="10"></asp:TextBox>
        </div>
        <asp:Repeater ID="repeateExcel" runat="server">
             <HeaderTemplate>
            <table width="0" border="0" cellspacing="0">
                <tr class="table_title" >
                     <td style="width:2%;" class="td_right">编号
                    </td>
                    <td style="width:2%;" class="td_right">所属分公司
                    </td>
                    <td style="width:2%;" class="td_right">门店名称
                    </td>
                    <td style="width:4%;" class="td_right">负 责 人
                    </td>
                    <td style="width:4%;" class="td_right">联系电话
                    </td>
                    <td style="width:3%;" class="td_right">金力账号
                    </td>
                    <%--<td style="width:3%;" class="td_right">排序
                    </td>--%>
                     <td style="width:6%;" class="td_right">状态
                    </td>
        
                </tr>
        </HeaderTemplate>

                    <ItemTemplate>
            <tr class="td_bg noboer_tr">
                <td class="td_txt_cenetr" style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("编号") %></span>
                </td>
                <td class="td_txt_cenetr" style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("所属分公司") %></span>
                </td>
                <td class="td_txt_cenetr" style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("门店名称") %></span>
                </td>
                <td class="td_txt_cenetr" style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("负责人") %></span>

                </td>
                <td class="td_txt_cenetr" style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("联系电话") %></span>

                </td>
                <td class="td_txt_cenetr" style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("金力账号") %></span>

                </td>
                <%--<td class="td_txt_cenetr" style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("排序") %></span>

                </td>--%>
                <td class="td_txt_cenetr"  style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#FB6363":" "%>;'>
                 <span><%#Eval("errorInfo") %></span>
                </td>
              
            </tr>
        </ItemTemplate>
                    <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

   

    </div>
  </div>

  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
