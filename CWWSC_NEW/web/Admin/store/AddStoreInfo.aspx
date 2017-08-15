<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.AddStoreInfo"  MasterPageFile="~/Admin/Admin.Master"  MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
    <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1><table><tr><td>添加创维门店</td><td><a class="help" href="http://video.92hidc.com/video/V2.1/tjsplx.html" title="查看帮助" target="_blank"></a></td></tr></table></h1>
            <span>添加创维门店。</span>
          </div>
          <div class="Steps Pg_45">
          <ul>
              <li class="huang">添加创维门店</li>    
          </ul>
          </div>
            <div class="formitem validator4">
              <ul>
                 <li> 
                    <span class="formitemtitle Pw_110"><em >*</em>选择分公司：</span>
                    <asp:DropDownList ID="ddlfgs" Width="150" runat="server"></asp:DropDownList>
                    <p id="P4" style="color:red" runat="server">分公司为必选项</p>
                </li>
                <li> 
                  <span class="formitemtitle Pw_110"><em >*</em>门店名称：</span>
                  <asp:TextBox ID="storeName" CssClass="forminput" runat="server" Width="320"></asp:TextBox>
                  <p id="txtTypeNameTip" runat="server">门店名称长度限制在1-30个字符之间</p>
                </li>
   
                <li> 
                  <span class="formitemtitle Pw_110"><em >*</em>负 责 人：</span>
                  <asp:TextBox ID="storeRelationPerson" CssClass="forminput" Width="320" runat="server"  ></asp:TextBox>
                  <p id="txtRemarkTip" runat="server">负责人的长度限制在1-6个字符之间</p>
                </li>
                <li> 
                <span class="formitemtitle Pw_110"><em >*</em>联系电话：</span>
                <asp:TextBox ID="storeRelationCell"  Width="320" CssClass="forminput"  runat="server" ></asp:TextBox>
                <p id="P2" runat="server">联系电话的长度限制在1-11个字符之间</p>
                </li>
                <li> 
                    <span class="formitemtitle Pw_110"><em >*</em>金力账号：</span>
                    <asp:TextBox ID="accountALLHere"  Width="320" CssClass="forminput" runat="server" ></asp:TextBox>
                    <p id="P1" runat="server">金力账号的长度限制在1-20个字符之间</p>
                </li>
                <li> 
                    <span class="formitemtitle Pw_110">排序：</span>
                    <asp:TextBox ID="scode"  Width="320" CssClass="forminput" runat="server" ></asp:TextBox>
                    <p id="P3" runat="server">排序的长度限制在1-10个字符之间</p>
                </li>


              </ul>
             <ul class="btn Pa_100">
              <asp:Button ID="btnAddProductType" runat="server" OnClientClick="return PageIsValid();" Text="添 加"  CssClass="submit_DAqueding"  />
         </ul>
       </div>     
     </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_storeName', 1, 30, false, null, '门店名称长度限制在1-30个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_storeRelationPerson', 0, 300, true, null, '负责人的长度限制在1-6个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_storeRelationCell', 1, 11, true, null, '联系电话的长度限制在1-11个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_accountALLHere', 1, 11, true, null, '金力账号的长度限制在1-20个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_scode', 1, 11, true, null, '排序的长度限制在1-10个字符之间'))


    }
    $(document).ready(function () { InitValidators(); });

</script>
</asp:Content>