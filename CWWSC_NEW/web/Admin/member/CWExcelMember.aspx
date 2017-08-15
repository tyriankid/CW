
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master"   CodeBehind="CWExcelMember.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CWExcelMember" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/04.gif" width="32" height="32" /></em>
	    <h1>导入ALLHere会员列表</h1>
	    <span>说明：大批量导入会员信息，数据验证通过后请点击保存。</span>
     </div>
	  <asp:FileUpload ID="fileUpload" runat="server"  />
      <asp:Button runat="server" ID="btnExcelPrint" CssClass="searchbutton" Text="保存"  />
        <asp:Button runat="server" ID="btnDownLoad" CssClass="searchbutton" Text="下载模板表格" style="float:right;"/>
	  <!--数据列表区域-->
  <div class="datalist">
        <asp:Repeater ID="repeateExcel" runat="server">
	   
             <HeaderTemplate>
            <table width="0" border="0" cellspacing="0">
                <tr class="table_title">
                     <td style="width:2%;" class="td_right">用户昵称
                    </td>
                    <td style="width:2%;" class="td_right">会员姓名
                    </td>
                    <td style="width:4%;" class="td_right">邮箱
                    </td>
                    <td style="width:4%;" class="td_right">电话
                    </td>
                    <td style="width:3%;" class="td_right">QQ
                    </td>
                     <td style="width:3%;" class="td_right">详细地址
                    </td>
                    <td style="width:3%;" class="td_right">错误列表
                    </td>
        
                </tr>
        </HeaderTemplate>

                    <ItemTemplate>
            <tr class="td_bg noboer_tr">
                <td class="td_txt_cenetr"'>
                    <span style="<%#Eval("errorInfo").ToString().IndexOf("用户昵称不能为空或已被使用")>-1?"color:red": "" %>"><%#Eval("会员昵称") %></span>
                </td>
                <td class="td_txt_cenetr"' >
                    <span style="<%#Eval("errorInfo").ToString().IndexOf("会员姓名")>-1?"color:red": "" %>"><%#Eval("会员姓名") %></span>

                </td>
                <td class="td_txt_cenetr"'>
                    <span style="<%#Eval("errorInfo").ToString().IndexOf("邮箱格式错误")>-1?"color:red": "" %>"><%#Eval("邮箱") %></span>

                </td>
                <td class="td_txt_cenetr" '>
                    <span style="<%#Eval("errorInfo").ToString().IndexOf("电话不能为空或格式错误")>-1?"color:red": "" %>"><%#Eval("电话") %></span>

                </td>
                 <td class="td_txt_cenetr"'>
                    <span style="<%#Eval("errorInfo").ToString().IndexOf("QQ")>-1?"color:red": "" %>"><%#Eval("QQ") %></span>
                </td>
                 <td class="td_txt_cenetr"'>
                    <span style="<%#Eval("errorInfo").ToString().IndexOf("详细地址")>-1?"color:red": "" %>"><%#Eval("详细地址") %></span>
                </td>
                <td class="td_txt_cenetr">
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


 