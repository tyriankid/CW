<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditServiceClass"  MasterPageFile="~/Admin/Admin.Master"  MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
    <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1><table><tr><td>添加服务品类</td></tr></table></h1>
            <span><asp:Literal runat="server" ID="litPageTip"></asp:Literal></span>
          </div>
          <div class="Steps Pg_45">
          <ul>
              <li class="huang">添加服务品类</li>    
          </ul>
          </div>
            <div class="formitem validator4">
              <ul>
                 <li> 
                    <span class="formitemtitle Pw_110"><em >*</em>品类名称：</span>
                    <asp:TextBox ID="txtClassName" CssClass="forminput" runat="server" Width="320"></asp:TextBox>
                    <p id="txtClassNameTip" runat="server">品类名称长度限制在1-20个字符之间</p>
                </li>
                <li> 
                    <span class="formitemtitle Pw_110">排序：</span>
                    <asp:TextBox ID="txtScode"  Width="320" CssClass="forminput" runat="server" ></asp:TextBox>
                    <p id="txtScodeTip" runat="server">排序的长度限制在1-5个字符之间</p>
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
        this.window.location.href = "/admin/product/ServiceClassList.aspx";
    }
    
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtClassName', 1, 20, false, null, '门店名称长度限制在1-20个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtScode', 1, 5, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
        appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtScode', 0, 99999, '输入的数值超出了系统表示范围'));
    }
    $(document).ready(function () { InitValidators(); });

</script>
</asp:Content>