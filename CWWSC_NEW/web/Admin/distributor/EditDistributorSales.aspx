<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditDistributorSales"  MasterPageFile="~/Admin/Admin.Master"  MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
    <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1><table><tr><td>添加店员</td></tr></table></h1>
            <span><asp:Literal runat="server" ID="litPageTip"></asp:Literal></span>
          </div>
          <div class="Steps Pg_45">
          <ul>
              <li class="huang">添加门店店员</li>    
          </ul>
          </div>
            <div class="formitem validator4">
              <ul>
                 <li> 
                    <span class="formitemtitle Pw_110"><em >*</em>所属门店：</span>
                    <asp:TextBox ID="txtStoreName" CssClass="forminput" runat="server" Width="320"></asp:TextBox>&nbsp;自动获取
                    <p id="txtStoreNameTip" runat="server">门店名称长度限制在1-30个字符之间</p>
                </li>
                <li> 
                  <span class="formitemtitle Pw_110"><em >*</em>店长姓名：</span>
                  <asp:TextBox ID="txtstoreRelationPerson" CssClass="forminput" runat="server" Width="320"></asp:TextBox>&nbsp;自动获取
                  <p id="txtstoreRelationPersonTip" runat="server">门店名称长度限制在1-30个字符之间</p>
                </li>
   
                <li> 
                  <span class="formitemtitle Pw_110"><em >*</em>金力账号：</span>
                  <asp:TextBox ID="txtaccountALLHere" CssClass="forminput" Width="320" runat="server"  ></asp:TextBox>&nbsp;自动获取
                  <p id="txtaccountALLHereTip" runat="server">负责人的长度限制在1-6个字符之间</p>
                </li>
                <li> 
                    <span class="formitemtitle Pw_110"><em >*</em>店员姓名：</span>
                    <asp:TextBox ID="txtDsName"  Width="320" CssClass="forminput"  runat="server" ></asp:TextBox>
                    <p id="txtDsNameTip" runat="server">店员姓名的长度限制在1-20个字符之间</p>
                </li>
                <li> 
                    <span class="formitemtitle Pw_110"><em >*</em>店员电话：</span>
                    <asp:TextBox ID="txtDsPhone"  Width="320" CssClass="forminput" runat="server" ></asp:TextBox>
                    <p id="txtDsPhoneTip" runat="server">店员电话的长度限制在6-20个字符之间</p>
                </li>
                <li> 
                    <span class="formitemtitle Pw_110">排序：</span>
                    <asp:TextBox ID="txtScode"  Width="320" CssClass="forminput" runat="server" ></asp:TextBox>
                    <p id="txtScodeTip" runat="server">排序的长度限制在1-10个字符之间</p>
                </li>


              </ul>
             <ul class="btn Pa_340 clear">
                <li>
                    <asp:Button ID="btnSave" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding"  />
                </li>
                 <li>
                    <input type="button" value="返回列表" onclick="javascript:returnList();" class="submit_DAqueding" />
                </li>
            </ul>
       </div>     
     </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function returnList() {
        this.window.location.href = "/admin/distributor/ListDistributorSales.aspx";
    }
    
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtStoreName', 1, 30, false, null, '门店名称长度限制在1-30个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtstoreRelationPerson', 0, 300, false, null, '负责人的长度限制在1-6个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtaccountALLHere', 1, 11, false, null, '联系电话的长度限制在1-11个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtDsName', 1, 10, false, null, '店员姓名的长度限制在1-10个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtDsPhone', 1, 20, false, null, '店员电话的长度限制在1-20个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtScode', 1, 10, true, null, '排序的长度限制在1-10个字符之间'))
    }
    $(document).ready(function () { InitValidators(); });

</script>
</asp:Content>