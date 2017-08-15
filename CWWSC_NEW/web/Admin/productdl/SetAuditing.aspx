﻿
<%@ Page Language="C#"  MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetAuditing.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.SetAuditing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        //关闭当前弹框
        function CloseDialogFrame() {
            //window.parent
            art.dialog.close();
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title  m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>审核供应商商品</h1>
        </div>
        <div class="datafrom">
            <div>
                <asp:HiddenField ID="txtProductIds" runat="server" ClientIDMode="Static" />
                <table>
                    <tr>
                        <td>
                            不通过原因：
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccount" runat="server" Width="320" TextMode="MultiLine" Rows="6"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnPass" Text="通 过" OnClientClick="return doPass();" CssClass="submit_DAqueding inbnt" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnNoPass" Text="不 通 过" OnClientClick="return doNoPass();" CssClass="submit_DAqueding inbnt" />
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