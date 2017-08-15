<%@ Page Language="C#"  MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ShowBackAccount.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.ShowBackAccount"  Title="无标题页" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript">
        //关闭当前弹框
        function CloseDialogFrame() {
            art.dialog.close();
        }
    </script>
    <div class="dataarea mainwidth databody">
        <div class="title  m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>审核不通过原因</h1>
        </div>
        <div class="datafrom">
            <div>
                <table>
                    <tr>
                        <td style="text-align:right">
                            时间:
                        </td>
                        <td>
                            <asp:Literal ID="litBackTime" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right">
                            不通过原因:
                        </td>
                        <td style="height:80px">
                            <asp:Literal ID="litAccount" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnClose" Text="关 闭" OnClientClick="CloseDialogFrame();" CssClass="submit_DAqueding inbnt" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
