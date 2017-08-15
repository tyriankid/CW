<%@ Page Language="C#" AutoEventWireup="true" CodeBehind ="EditJinLiMember.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditJinLiMember" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" Runat="Server">
    <div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1>编辑ALLHere会员信息</h1>
            <span>编辑ALLHere会员各项信息资料</span>
          </div>
      <div class="formitem validator4 clearfix">
        <ul>
          <li> <span class="formitemtitle Pw_110">用户昵称：</span>
               <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtUserName">姓名不能为空,长度在20个字符以内</p>
          </li>

          <li> <span class="formitemtitle Pw_110">门店DZ号：</span>
            <asp:TextBox ID="txtAccountALLHere" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtAccountALLHere">门店DZ号不能为空,长度在50个字符以内</p>
          </li>

          <li> <span class="formitemtitle Pw_110">门店名称：</span>
            <asp:TextBox ID="txtStoreName" runat="server" CssClass="forminput"></asp:TextBox>
            <p  id="ctl00_contentHolder_txtStoreName">门店名称不能为空,长度限制在256个字符以内</p>
          </li>

          <li> <span class="formitemtitle Pw_110">手机号码：</span>
            <asp:TextBox ID="txtCellPhone" runat="server" CssClass="forminput"></asp:TextBox>
            <p  id="ctl00_contentHolder_txtCellPhone">手机号码不能为空,长度限制在3-20个字符之间,只能输入数字</p>
          </li>

          <li> <span class="formitemtitle Pw_110">商品内码：</span>
            <asp:TextBox ID="txtProductCode" runat="server" CssClass="forminput"></asp:TextBox>
            <p  id="ctl00_contentHolder_txtProductCode">商品内码不能为空,长度限制在20个字符以内，只能输入数字</p>
          </li>

          <li> <span class="formitemtitle Pw_110">商品型号：</span>
            <asp:TextBox ID="txtProductModel" runat="server" CssClass="forminput"></asp:TextBox>
            <p  id="ctl00_contentHolder_txtProductModel">商品型号不能为空,长度限制在50个字符以内</p>
          </li>

          <li> <span class="formitemtitle Pw_110">单价：</span>
            <asp:TextBox ID="txtPrice" runat="server" CssClass="forminput"></asp:TextBox>
            <p  id="ctl00_contentHolder_txtPrice">请输入正确单价，长度在10个字符以内</p>
          </li>

          <li> <span class="formitemtitle Pw_110">购买数量：</span>
            <asp:TextBox ID="txtBuyNum" runat="server" CssClass="forminput"></asp:TextBox>
            <p  id="ctl00_contentHolder_txtBuyNum">购买数量限制在1-10个字符之间，只能输入数字</p>
          </li>

          <li> <span class="formitemtitle Pw_110">详细地址：</span>
              <asp:TextBox ID="txtAddress" runat="server" MultiLine="true" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtAddress">详细地址必须控制在300个字符以内</p>
          </li>
      </ul>
      <ul class="btn Pa_198">
        <asp:Button ID="btnEditUser" runat="server" Text="确 定"   CssClass="submit_DAqueding" />
        </ul>
      </div>

      </div>
  </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
    <script type="text/javascript" language="javascript">
        //function InitValidators() {
        //    initValid(new InputValidator('ctl00_contentHolder_txtRealName', 0, 20, false, '姓名长度在20个字符以内'))
        //    initValid(new InputValidator('ctl00_contentHolder_txtAddress', 0, 100, false, '详细地址必须控制在100个字符以内'))
        //    initValid(new InputValidator('ctl00_contentHolder_txtQQ', 3, 20, false, '^[0-9]*$', 'QQ号长度限制在3-20个字符之间，只能输入数字'))
        //    initValid(new InputValidator('ctl00_contentHolder_txtprivateEmail', 1, 256, false, '[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\.[\\w-]+)+', '请输入有效的电子邮件地址，电子邮件地址的长度在256个字符以内'))
        //    initValid(new InputValidator('ctl00_contentHolder_txtCellPhone', 3, 20, false, '^[0-9]*$', '手机号码不能为空,长度限制在3-20个字符之间,只能输入数字'));
        //}
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtUserName', 1, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '用户昵称不能为空，必须以汉字或是字母开头,且在3-20个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtAccountALLHere', 1, 50, false, null, '门店DZ号不能为空,长度在50个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtStoreName', 1, 256, false, null, '门店名称不能为空,长度限制在256个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtCellPhone', 3, 20, false, '^[0-9]*$', '手机号码不能为空，长度限制在3-20个字符之间,只能输入数字'))
            initValid(new InputValidator('ctl00_contentHolder_txtProductCode', 1, 20, false, null, '商品内码不能为空,长度限制在20个字符以内，只能输入数字'))
            initValid(new InputValidator('ctl00_contentHolder_txtProductModel', 1, 50, false, null, '商品型号不能为空,长度限制在50个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtPrice', 1, 10, false, null, '请输入正确单价，长度在10个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtBuyNum', 1, 10, false, null, '购买数量限制在1-10个字符之间，只能输入数字'))
            initValid(new InputValidator('ctl00_contentHolder_txtAddress', 1, 300, false, null, '详细地址必须控制在300个字符以内'))
            //initValid(new InputValidator('ctl00_contentHolder_txtRealName', 3, 20, true, null, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '用户姓名不能为空，必须以汉字或是字母开头,且在3-20个字符之间'))
            //initValid(new InputValidator('ctl00_contentHolder_txtEmail', 1, 256, true, null, '[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\.[\\w-]+)+', '请输入有效的电子邮件地址，电子邮件地址的长度在256个字符以内'))
            //initValid(new InputValidator('ctl00_contentHolder_txtQQ', 3, 20, true, '^[0-9]*$', 'QQ号长度限制在3-20个字符之间，只能输入数字'));
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>