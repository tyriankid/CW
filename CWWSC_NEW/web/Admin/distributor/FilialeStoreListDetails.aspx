<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="FilialeStoreListDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.FilialeStoreListDetails" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/04.gif" width="32" height="32" />
            </em>
            <h1>
                 <%=this.Page.Request["FilialeName"] %>下门店详细列表
            </h1>
            <span>管理员查询分公司下门店的统计数据。</span>
            <span>可以导出分公司下门店统计列表详情</span>
        </div>
        <!--搜索-->
        <!--数据列表区域-->
        <div class="datalist">
           <div class="searcharea clearfix br_search">
                <ul>
                    <li><span>金力帐号：</span> <span>
                        <asp:TextBox ID="txtAccountALLHere" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li><span>门店名：</span> <span>
                        <asp:TextBox ID="txtStoreName" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li><span>时间：</span> <span>
                        <UI:WebCalendar runat="server"   CssClass="forminput1" ID="txtStartTime" Width="100" /></span>
                    </li>
                    <li><span>至</span></li>
                    <li><span>
                        <UI:WebCalendar runat="server"   CssClass="forminput1" ID="txtEndTime" Width="100" /></span>
                    </li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="搜索" />
                    </li>
                    <li>
                        <asp:Button ID="btnExportDetail" runat="server" class="submit_queding" Text="导出门店明细" />
                    </li>
                    <li>
                        <asp:Button ID="btnExportDetailAll" runat="server" class="submit_queding" Text="导出门店明细(全部)" />
                    </li>
                    <li>
                        <asp:Button ID="btnSubmint" runat="server" class="submit_queding" Text="返回上一层" />
                    </li>
                </ul>
            </div>
            <table>
                <thead>
                    <tr class="table_title">
                        <td>
                            序号
                        </td>
                        <td>
                            金力帐号
                        </td>
                        <td>
                            门店名称
                        </td>
                        <td>
                            有效订单总数
                        </td>
                        <td>
                            订单总额（元）
                        </td>
                        <td>
                            佣金总额（元）
                        </td>
                        <td>
                            用户数
                        </td>
                        <%--<td>
                             操作
                        </td>--%>
                    </tr>
                </thead>
                <asp:Repeater ID="reList" runat="server">
                    <ItemTemplate>
                        <tbody>
                            <tr>
                                <td class="td_txt_cenetr">
                                     &nbsp;<asp:Literal runat="server" ID="litph"  Text="0"/>
                                </td>
                                <td>&nbsp;<%#Eval("accountALLHere") %></td>
                                <td>&nbsp;<%#Eval("StoreName") %></td>
                                <td>&nbsp;<%#Eval("OrderCount").ToString() != "" ? Eval("OrderCount").ToString() : "0" %></td>
                                <td>&nbsp;<%#Eval("OrderTotal").ToString() != "" ?  Eval("OrderTotal","{0:F2}") : "0.00" %></td>
                                <td>&nbsp;<%#Eval("CommTotal").ToString() != "" ?  Eval("CommTotal","{0:F2}") : "0.00" %></td>
                                <td>&nbsp;<%#Eval("UserCount").ToString() != "" ?  Eval("UserCount").ToString() : "0" %></td>
                            </tr>
                        </tbody>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="blank12 clearfix">
            </div>
        </div>

        <!--数据列表底部功能区域-->
        <%--<div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <ui:pager runat="server" ShowTotalPages="true" ID="pager" />
                </div>
            </div>
        </div>--%>
    </div>
</asp:Content>