<%@ Page Language="C#" AutoEventWireup="true" CodeFile="distributorStarLevel.aspx.cs" MasterPageFile="~/Admin/Admin.Master" Inherits="Admin_distributor_distributorStarLevel" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/04.gif" width="32" height="32" /></em>
            <h1>星级等级列表</h1>
            <span>对星级等级进行管理，您可以修改星级等级或删除星级等级</span>
        </div>
        <!--搜索-->

        <!--数据列表区域-->
        <div class="datalist">
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li>
                        <a href="AdddistributorStarLevel.aspx" class="submit_DAqueding float">新增</a>
                    </li>
                </ul>
            </div>
    </div>
        <table style="width:100%">
            <thead>
                <tr class="table_title" style="width:100%">
                    <td>图片</td>
                    <td>星级名称</td>
                    <td>星级等级</td>
                    <td>最低分数</td>
                    <td>最高分数</td>
                    <td>操作</td>
                </tr>
            </thead>
            <asp:Repeater ID="reStarLevel" runat="server" OnItemCommand="reStarLevel_ItemCommand" EnableViewState="true">
                <ItemTemplate>
                    <tbody>
                        <tr style="text-align:center">
                            <td><Hi:ListImage ID="ListImage1" runat="server" DataField="Ico"  Width="30px" Height="30px"/></td>
                            <td><%# Eval("LevelName") %></td>
                            <td><%# Eval("LevelNum") %></td>
                            <td><%# Eval("MinNum") %></td>
                            <td><%# Eval("MaxNum") %></td>
                            <td><a href="AdddistributorStarLevel.aspx?StarLevelID=<%#Eval("StarLevelID")%>">编辑</a>
                            <Hi:ImageLinkButton runat="server" ID="Delete" IsShow="true" Text="删除" CommandName="Delete"  CommandArgument='<%# Eval("StarLevelID") %>'/>
                            </td>
                        </tr>
                    </tbody>
                </ItemTemplate>
            </asp:Repeater>

        </table>
    </div>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
