<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddProductVirtualCode.aspx.cs" Inherits="Admin_product_AddProductVirtualCode" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">
<style type="text/css">.Pw_110{ padding-right: 5px;}.errorFocus{width:220px;}.forminput{width:220px;padding:4px 0px 4px 2px}.areacolumn .columnright .formitem li{margin-bottom:0;}</style>
    <!--列表-->    
    <div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1><Hi:HiLiteral runat="server" ID="litPageTip"></Hi:HiLiteral></h1>
            <span>设置商品的虚拟码</span>
          </div>
        <div class="formitem validator4 clearfix">
            <ul>
                <li> 
                    <span class="formitemtitle Pw_110">商品名称：</span>
                    <Hi:HiLiteral ID="hiProductName" runat="server"></Hi:HiLiteral>
                    <%--<asp:HiddenField ID="hiProduct" runat="server" />--%>
                    <p id="ctl00_contentHolder_txtLevelNameTip">正在为虚拟商品配置虚拟码</p>
                </li>
                <li> 
                    <span class="formitemtitle Pw_110">商品规格：</span>
                    <Hi:ProductSkuDropDownList ID="dropProductSku" runat="server" />
                    <p id="ctl00_contentHolder_dropProductSkuTip">请选择商品规格</p>
                </li>
                <li>
                    <span class="formitemtitle Pw_110">商品虚拟码：</span>
                    <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtVirtualCode" />
                    <p id="ctl00_contentHolder_txtVirtualCodeTip">请输入商品虚拟码</p>
                </li>
            </ul>
          <ul class="btn Pa_198">
                <asp:Button ID="btnSave" runat="server" Text="保 存" CssClass="submit_DAqueding float"  OnClick="btnSaveClientSettings_Click"   OnClientClick="return CheckForm()"/>
                <%--<asp:Button ID="btnEditUser" runat="server" Text="确 定" OnClientClick="return CheckForm()"  CssClass="submit_DAqueding" />--%>
            </ul>
          </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function CheckForm() {
            if (PageIsValid()) {
                if ($("#ctl00_contentHolder_dropProductSku").val() == "") {
                    alert("请选择商品规格");
                    return false;
                }
            } else {
                return false;
            }
        }
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtVirtualCode', 1, 20, false, null, '商品虚拟码在30个字符以内'));
        }
        $(document).ready(function () {
            InitValidators();
        });
    </script>
</asp:Content>
