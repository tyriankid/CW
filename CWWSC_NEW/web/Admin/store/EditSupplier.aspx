<%@ Page Language="C#" AutoEventWireup="true"  Inherits="Hidistro.UI.Web.Admin.EditSupplier"  MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>编辑供应商信息</h1>
            <span>编辑供应商信息，编辑供应商信息，编辑供应商信息编辑分公司信息</span>
          </div>
          <div class="formtab">
           <ul>
              <li class="visited">基本设置</li>                                      

            </ul>
          </div>
          <div class="formitem validator2">
            <ul>
              <li> <span class="formitemtitle Pw_100">供应商名称：</span>
                <asp:TextBox ID="txtgysName" CssClass="forminput" runat="server" Width="320"></asp:TextBox>
                <p id="txtTypeNameTip" runat="server">供应商不能为空，长度限制在1-30个字符之间</p>
              </li>
              <li> <span class="formitemtitle Pw_100">供应商电话：</span>
                <asp:TextBox ID="txtgysPhone" CssClass="forminput" runat="server" Width="320"></asp:TextBox>
                <p id="P1" runat="server">供应商电话不能为空，长度限制在1-11个字符之间</p>
              </li>
              <li> <span class="formitemtitle Pw_100">供应商排序：</span>
                <asp:TextBox ID="txtscode" CssClass="forminput" runat="server" Width="320"></asp:TextBox>
                <p id="P2" runat="server">供应商排序不能为空，长度限制在1-30个字符之间</p>
              </li>
              <li> <span class="formitemtitle Pw_100">供应商地址：</span>
                <asp:TextBox ID="txtgysAddress" TextMode="MultiLine" Width="320" Height="90" runat="server" ></asp:TextBox>
                <p id="txtRemarkTip" runat="server">地址的长度限制在0-100个字符之间</p>
              </li>

            </ul>
            <ul class="btn Pa_100">
            <asp:Button ID="btnEditProductType" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding"  />
            </ul>
          </div>
  </div>        
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtfgsName', 1, 30, false, null, '商品类型名称不能为空，长度限制在1-30个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtfgsPhone', 0, 11, true, null, '分公司电话不能为空，长度限制在1-11个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtscode', 0, 100, true, null, '分公司排序不能为空，长度限制在1-30个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtfgsAddress', 0, 100, true, null, '地址的长度限制在0-100个字符之间'))

    }
    $(document).ready(function () { InitValidators(); });
</script>
</asp:Content>
