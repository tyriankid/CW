
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master"  CodeFile="O2OMemberInPort.aspx.cs" Inherits="Admin_member_O2OMemberInPort" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/04.gif" width="32" height="32" /></em>
	    <h1>导入录入会员列表</h1>
	    <span>说明：大批量导入录入会员信息，数据验证通过后请点击保存。</span>
     </div>
	  <asp:FileUpload ID="fileUpload" runat="server"  />
      <asp:Button runat="server" ID="btnExcelPrint" CssClass="searchbutton" Text="导入并保存数据" />
        &nbsp;&nbsp;》&nbsp;&nbsp;
      <asp:Button runat="server" ID="btnSave" CssClass="searchbutton" Text="在-保存至数据库" />
      <asp:Button runat="server" ID="btnDownLoad" CssClass="searchbutton" Text="下载模板表格" style="float:right;"/>
	  <!--数据列表区域-->
  <div class="datalist">
        <div style="text-align:center; width:100%;">
            <asp:TextBox id="txtErrorInfo" runat="server" TextMode="MultiLine" Rows="6" Width="100%" ToolTip="错误提示" ></asp:TextBox><br />
        </div>
        <asp:Repeater ID="repeateExcel" runat="server">
            <HeaderTemplate>
                <table width="0" border="0" cellspacing="0">
                    <tr class="table_title">
                         <td style="width:2%;" class="td_right">会员名称
                        </td>
                        <td style="width:2%;" class="td_right">性别
                        </td>
                        <td style="width:2%;" class="td_right">会员电话
                        </td>
                        <td style="width:4%;" class="td_right">门店DZ号
                        </td>
                        <td style="width:4%;" class="td_right">门店名称
                        </td>
                        <td style="width:3%;" class="td_right">会员等级
                        </td>
                         <td style="width:3%;" class="td_right">购买产品类型
                        </td>
                        <td style="width:3%;" class="td_right">产品型号
                        </td>        
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <%--class="td_bg noboer_tr"--%>
                    <tr style=" <%#string.IsNullOrEmpty(Eval("errorInfo").ToString())?"": "color:red" %>">
                        <td class="td_txt_cenetr"'>
                            <span><%#Eval("会员名称") %></span>
                        </td>
                        <td class="td_txt_cenetr"' >
                            <span><%#Eval("性别") %></span>
                        </td>
                        <td class="td_txt_cenetr"'>
                            <span><%#Eval("会员电话") %></span>
                        </td>
                        <td class="td_txt_cenetr" '>
                            <span><%#Eval("门店DZ号") %></span>
                        </td>
                        <td class="td_txt_cenetr" '>
                            <span><%#Eval("门店名称") %></span>
                        </td>
                         <td class="td_txt_cenetr"'>
                            <span><%#Eval("会员等级") %></span>
                        </td>
                         <td class="td_txt_cenetr"'>
                            <span><%#Eval("产品类型") %></span>
                        </td>
                        <td class="td_txt_cenetr">
                            <span><%#Eval("购买产品型号") %></span>
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
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>