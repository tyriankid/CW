<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApplyUserChange.aspx.cs" Inherits="Admin_distributor_ApplyUserChange"  MasterPageFile="~/Admin/Admin.Master"  %>
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
            var win = art.dialog.open.origin;
            win.location.reload();
        }

        //提交保存
        function doPass() {
            if (confirm("确认要通过审核吗？")) {
                return true;
            }
            return false;
        }

        function doNoPass() {
            if ($("#txtAccount").val().trim() == "") {
                alert("请输入不通过原因。");
                return false;
            }
            if (confirm("确认要不通过审核吗？")) {
                return true;
            }
            return false;
        }
    </script>
    <div class="dataarea mainwidth databody">
        <div class="title  m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>审核店员店长身份交换</h1>
        </div>
        <div class="datafrom">
            <div>
                <asp:HiddenField ID="txtDcID" runat="server" ClientIDMode="Static" />
                <table>
                    <tr>
                        <td>
                            不通过原因：
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccount" runat="server" Width="320" TextMode="MultiLine" Rows="6" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnPass" Text="通 过" OnClientClick="return doPass();" CssClass="submit_DAqueding inbnt" OnClick="btnPass_Click" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnNoPass" Text="不 通 过" OnClientClick="return doNoPass();" CssClass="submit_DAqueding inbnt" OnClick="btnNoPass_Click" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnClose" Text="关 闭" OnClientClick="CloseDialogFrame();" CssClass="submit_DAqueding inbnt" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>