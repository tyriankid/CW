<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddmemberTags.aspx.cs" Inherits="Admin_member_AddmemberTags"  MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .uploadimages {
            display: inline-block !important;
            float: none!important;
            position: relative;
            left: 60px;
        }
        .forminput1 {
            width:150px
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">
    <!--列表-->
    <div class="blank12 clearfix">
    </div>
    <div class="dataarea mainwidth databody">
        <div class="title m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>标签维护
            </h1>
            <span>对会员标签进行编辑和新增</span>
        </div>
        <div class="datafrom">
            <div class="formitem">
                <div class="formitem_con">
                    <div class="webcal">
                        <div style="text-indent: 2em; margin-bottom: 10px;">
                            标签名:
                      <Hi:TrimTextBox runat="server" CssClass="forminput1" ID="txtTagsName" />
                        </div>
                         <div style="text-indent: 2em; margin-bottom: 10px;">
                            标签类别:
                         <asp:DropDownList ID="dropMemberTagsType" runat="server">
                                <asp:ListItem Value="0">微会员</asp:ListItem>
                                <asp:ListItem Value="1">金立会员</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div style="text-indent: 2em; margin-bottom: 10px;">
                            排序:
                      <Hi:TrimTextBox runat="server" CssClass="forminput1" ID="txtTagsSort"  onkeyup="value=value.replace(/\D/g,'')"/>
                        </div>
                    </div>
                    <ul class="btntf Pa_140" style="margin-top: 15px;">
                        <asp:Button ID="btnSave" runat="server" Text="保 存" CssClass="submit_DAqueding float"  OnClick="btnSaveClientSettings_Click" />
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript"> 
    </script>
</asp:Content>

