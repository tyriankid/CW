<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddO2OSubsidiary.aspx.cs" Inherits="Admin_member_AddO2OSubsidiary" MasterPageFile="~/Admin/Admin.Master"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
  
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
             <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1><asp:Literal ID="litTitle" runat="server" Text ="添加新的录入会员附属信息列名"></asp:Literal></h1>
          </div>
          <asp:Panel ID="PanelID" runat="server">
                <div class="formitem validator4">
                <ul>
                    
                  <li> <span class="formitemtitle Pw_110"><em >*</em>所属类别：</span>
                      <asp:DropDownList ID="rdoType" runat="server" RepeatLayout="Flow" RepeatDirection ="Horizontal" CellPadding="2">
                          <asp:ListItem Value="家庭成员构成">家庭成员构成</asp:ListItem>
                                    <asp:ListItem Value="用户住房信息">用户住房信息</asp:ListItem>
                                    <asp:ListItem Value="房屋家电配置">房屋家电配置</asp:ListItem>
                                    <asp:ListItem Value="家电使用情况">家电使用情况</asp:ListItem>
                                    <asp:ListItem Value="个人品牌倾向">个人品牌倾向</asp:ListItem>
                                    <asp:ListItem Value="近期购买需求">近期购买需求</asp:ListItem>

                      </asp:DropDownList>
                  </li>
                   <li> 
                       <span class="formitemtitle Pw_110"><em >*</em>列名：</span>
                        <asp:TextBox ID="txtListName" runat="server" CssClass="forminput" />
                       <p id="ctl00_contentHolder_txtListNameTip">列名不能为空，只能是汉字或字母开头，长度在2-20个字符之间</p>
                  </li>
                    <li> 
                       <span class="formitemtitle Pw_110"><em >*</em>排序：</span>
                        <asp:TextBox ID="txtSort" runat="server" CssClass="forminput" />
                  </li>
                     </ul>
              <ul class="btn Pa_110 clear">
                <asp:Button ID="btnCreate" runat="server" OnClientClick="return PageIsValid();" Text=" 保  存"  CssClass="submit_DAqueding" style="float:left;" OnClick="btnCreate_Click"/>
                </ul>
              </div>
        </asp:Panel>
    </div>
</div>



<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    $(document).ready(function () { InitValidators(); });
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtListName', 2, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '列名不能为空，只能是汉字或字母开头，长度在2-20个字符之间'));
    }
</script>  
</asp:Content>
