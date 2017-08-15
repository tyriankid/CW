<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CWStoreInfoProving.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CWStoreInfoProving" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

	<div class="dataarea mainwidth databody">
     <div class="title">
		  <em><img src="../images/02.gif" width="32" height="32" /></em>
          <h1><strong>门店信息认证</strong></h1>
          <span>认证门店信息，认证通过后才可开店！</span>
	</div>
        </div>
<!--验证门店-->
<div id="EditRole">
    <div class="frame-content">
        <p><span class="frame-span frame-input90">门店名称：<em >*</em></span><asp:TextBox ID="txtStoreName" runat="server" CssClass="forminput"></asp:TextBox></p>
        <p><span class="frame-span frame-input90">ALLHere帐号：</span> <asp:TextBox ID="txtAccountALLHere" runat="server" CssClass="forminput"></asp:TextBox></p>
    </div>
</div>
<div>

<asp:Button ID="btnInfoProing" runat="server" Text="进行认证"/>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>