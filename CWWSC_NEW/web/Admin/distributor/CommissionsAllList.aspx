<%@ Page Language="C#"  MasterPageFile="~/Admin/Admin.Master"  AutoEventWireup="true" CodeBehind="CommissionsAllList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.CommissionsAllList" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/04.gif" width="32" height="32" /></em>
            <h1>
                佣金产生记录列表</h1>
            <span>管理员查询所有门店产生的佣金记录。</span>
            <span>佣金对账单导出可以查到更详细的佣金明细以及订单内商品的详情</span>
        </div>
        <!--搜索-->
        <!--数据列表区域-->
        <div class="datalist">
            <div class="clearfix search_titxt2">
                <div class="searcharea clearfix br_search">
                <ul>
                    <li><span>金力帐号：</span> <span>
                        <asp:TextBox ID="txtAccountALLHere" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li><span>分公司名：</span> <span>
                        <asp:TextBox ID="txtFgsName" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li><span>门店名：</span> <span>
                        <asp:TextBox ID="txtStoreName" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li><span>订单号：</span> <span>
                        <asp:TextBox ID="txtOrderId" CssClass="forminput" runat="server" /></span>
                    </li>
                </ul>
               <ul></ul>
               <ul>
                   <li><span>产生时间：</span> <span>
                        <UI:WebCalendar runat="server"   CssClass="forminput1" ID="txtStartTime" Width="100" />-</span>
                        <span> 
                        <UI:WebCalendar runat="server"   CssClass="forminput1" ID="txtEndTime" Width="100"/>
                        </span>
                    </li>
                   <li>
                        <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="搜索" />
                    </li>
                    <li>
                        <asp:Button ID="btnExportDetail" runat="server" class="submit_queding" Text="导出佣金对账单" />
                    </li>
               </ul>
            </div>
            </div>

           <div class="functionHandleArea m_none">

           </div>
            <table>
                <thead>
                    <tr class="table_title">
                        <td>
                            时间
                        </td>
                        <td>
                            佣金(元)
                        </td>
                        <td>
                            订单金额(元)
                        </td>
                        <td>
                            订单
                        </td>
                        <td>
                             分公司
                        </td>
                        <td>
                             AllHere账号
                        </td>
                        <td>
                             门店
                        </td>
                    </tr>
                </thead>
                <asp:Repeater ID="reCommissions" runat="server">
                    <ItemTemplate>
                        <tbody>
                            <tr>
                                <td width="180" class="td_txt_cenetr">
                                   &nbsp; <%# Eval("TradeTime", "{0:yyyy-MM-dd HH:mm:ss}")%>
                                </td>
                                <td class="td_txt_right">
                                   &nbsp; <%# Eval("CommTotal","{0:F2}")%>
                                </td>
                                <td class="td_txt_right">
                                   &nbsp; <%# Eval("OrderTotal", "{0:F2}")%>
                                </td>
                                <td class="td_txt_cenetr">
                                    &nbsp;<%# Eval("OrderId")%>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("fgsName")%>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("accountALLHere")%>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("storeName2")%>
                                </td>
                            </tr>
                        </tbody>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="blank12 clearfix">
            </div>
        </div>
        <!--数据列表底部功能区域-->
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(function () {

        });
        
    </script>
</asp:Content>