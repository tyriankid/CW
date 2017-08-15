<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetPFProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetPFProduct" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
    <div class="columnright">
        <div class="title">
        <em><img src="../images/03.gif" width="32" height="32" /></em>
        <h1>设置批发价格</h1>
        <span>设置供应商商品的批发价格</span>
        </div>
        <div>
            <asp:Label ID="labTitle" runat="server"></asp:Label>
            <br />
            <table>
                <tr style="padding:5px; height:40px">
                    <td><apan>数量：</apan></td>
                    <td><asp:TextBox ID="txtNum1" CssClass="forminput" runat="server"></asp:TextBox></td>
                    <td>&nbsp;&nbsp;价格：</td>
                    <td><asp:TextBox ID="txtPrice1" CssClass="forminput" runat="server"></asp:TextBox></td>
                </tr>
                <tr style="padding:5px; height:40px">
                    <td>数量：</td>
                    <td><asp:TextBox ID="txtNum2" CssClass="forminput" runat="server"></asp:TextBox></td>
                    <td>&nbsp;&nbsp;价格：</td>
                    <td><asp:TextBox ID="txtPrice2" CssClass="forminput" runat="server"></asp:TextBox></td>
                </tr>
                <tr style="padding:5px; height:40px">
                    <td>数量：</td>
                    <td><asp:TextBox ID="txtNum3" CssClass="forminput" runat="server"></asp:TextBox></td>
                    <td>&nbsp;&nbsp;价格：</td>
                    <td><asp:TextBox ID="txtPrice3" CssClass="forminput" runat="server"></asp:TextBox></td>
                </tr>
                <tr style="padding:5px; height:40px">
                    <td>数量：</td>
                    <td><asp:TextBox ID="txtNum4" CssClass="forminput" runat="server"></asp:TextBox></td>
                    <td>&nbsp;&nbsp;价格：</td>
                    <td><asp:TextBox ID="txtPrice4" CssClass="forminput" runat="server"></asp:TextBox></td>
                </tr>
                <tr style="padding:5px; height:40px">
                    <td>数量：</td>
                    <td><asp:TextBox ID="txtNum5" CssClass="forminput" runat="server"></asp:TextBox></td>
                    <td>&nbsp;&nbsp;价格：</td>
                    <td><asp:TextBox ID="txtPrice5" CssClass="forminput" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="cboxIsStore" TextMode="SingleLine" runat="server" Text="是否只适用于店长" />
                        &nbsp;&nbsp;&nbsp;&nbsp;<p style="color:red">提示：如果勾选了此项，则只有店长才可以享受批发价格，默认不勾选则表示所有会员都可以享受批发价格。</p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="formitem validator2"> 
            <ul class="btntf Pa_100 clear">
                <asp:Button ID="btnSetPFPrices" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding" runat="server"/>
                <asp:Button ID="btnReturn" Text="返 回" CssClass="submit_DAqueding" runat="server"/>
            </ul>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
