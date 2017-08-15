<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.AddFiliale"  MasterPageFile="~/Admin/Admin.Master"  MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
    <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1><table><tr><td>添加新公司</td><td><a class="help" href="http://video.92hidc.com/video/V2.1/tjsplx.html" title="查看帮助" target="_blank"></a></td></tr></table></h1>
            <span>添加新公司添加新公司添加新公司添加新公司添加新公司添加新公司添加新公司添加新公司添加新公司添加新公司</span>
          </div>
          <div class="Steps Pg_45">
          <ul>
              <li class="huang">添加新公司</li>    
          </ul>
          </div>
            <div class="formitem validator4">
              <ul>
                <li> 
                  <span class="formitemtitle Pw_110"><em >*</em>分公司名称：</span>
                  <asp:TextBox ID="txtfgsName" CssClass="forminput" runat="server" Width="320"></asp:TextBox>
                  <p id="txtTypeNameTip" runat="server">长度限制在1-30个字符之间</p>
                </li>
   
                <li> 
                  <span class="formitemtitle Pw_110"><em >*</em>分公司电话：</span>
                  <asp:TextBox ID="txtfgsPhone" CssClass="forminput" Width="320" runat="server"  ></asp:TextBox>
                  <p id="txtRemarkTip" runat="server">分公司电话的长度限制在1-11个字符之间</p>
                </li>
                <li> 
                <span class="formitemtitle Pw_110">分公司排序：</span>
                <asp:TextBox ID="txtscode"  Width="320" CssClass="forminput"  runat="server" ></asp:TextBox>
                <p id="P2" runat="server">分公司排序的长度限制在1-11个字符之间</p>
                </li>
                <li> 
                    <span class="formitemtitle Pw_110">分公司地址：</span>
                    <asp:TextBox ID="txtfgsAddress" TextMode="MultiLine" Width="320" Height="90" runat="server" ></asp:TextBox>
                    <p id="P1" runat="server">备注的长度限制在0-100个字符之间</p>
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
        initValid(new InputValidator('ctl00_contentHolder_txtfgsName', 1, 30, false, null, '分公司名称不能为空，长度限制在1-30个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtfgsAddress', 0, 300, true, null, '分公司地址的长度限制在0-100个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtfgsPhone', 1, 11, true, null, '分公司电话的长度限制在1-11个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtscode', 1, 11, true, null, '分公司排序的长度限制在1-11个字符之间'))


    }
    $(document).ready(function () { InitValidators(); });

</script>
</asp:Content>
