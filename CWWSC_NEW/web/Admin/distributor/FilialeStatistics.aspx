<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="FilialeStatistics.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.FilialeStatistics" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/04.gif" width="32" height="32" /></em>
            <h1>
                分公司统计列表</h1>
            <span>管理员查询所有分公司的统计数据。</span>
            <span>可以导出分公司统计列表详情</span>
        </div>
        <!--搜索-->
        <!--数据列表区域-->
        <div class="datalist">
           <div class="searcharea clearfix br_search">
                <ul>
                    <li><span>分公司名：</span> <span>
                        <asp:TextBox ID="txtFgsName" CssClass="forminput" runat="server" /></span>
                    </li>
                        <li><span>时间：</span> <span>
                        <UI:WebCalendar runat="server"   CssClass="forminput1" ID="txtStartTime" Width="100" /></span>
                        <span>&nbsp;至&nbsp;</span>
                        <span> 
                        <UI:WebCalendar runat="server"   CssClass="forminput1" ID="txtEndTime" Width="100"/>
                        </span>
                    </li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="搜索"/>
                    </li>
                    <li>
                        <asp:Button ID="btnExportDetail" runat="server" class="submit_queding" Text="导出分公司明细" />
                    </li>

                </ul>
            </div>
            <table>
                <thead>
                    <tr class="table_title">
                        <td style="display:none">
                            fgsid
                        </td>
                        <td>
                            分公司名称
                        </td>
                        <td>
                            已认证门店数
                        </td>
                        <td>
                            订单总数
                        </td>
                        <td>
                            订单总额（元）
                        </td>
                        <td>
                            佣金总额（元）
                        </td>
                        <td>
                             粉丝数
                        </td>
                        <td>
                             操作
                        </td>
                    </tr>
                </thead>
                <asp:Repeater ID="reList" runat="server">
                    <ItemTemplate>
                        <tbody>
                            <tr>
                                <td style="display:none">&nbsp;<%# Eval("fgsid")%></td>
                                <td>&nbsp;<%# Eval("fgsName")%></td>
                                <td>&nbsp;<%# Eval("storeNum")%></td>
                                <td>&nbsp;<%# Eval("orderNum")%></td>
                                <td>&nbsp;<%# Eval("orderTotal") != DBNull.Value ? Convert.ToDecimal(Eval("orderTotal")).ToString("0.00") : "0.00" %></td>
                                <td>&nbsp;<%# Eval("commTotal") != DBNull.Value ? Convert.ToDecimal(Eval("commTotal")).ToString("0.00") : "0.00" %></td>
                                <td>&nbsp;<%# Eval("UserNum")%></td>
                                <td>
                                    <span class="submit_bianji">
                                        <%--<a style="cursor:pointer" href="#" onclick="a1">详细</a>--%>
                                        <a style="cursor:pointer" href="javascript:showDetail('<%# Eval("fgsid") %>','<%# Eval("fgsName") %>','<%=this.Page.Request.QueryString["pageindex"] %>')">详细</a>
                                    </span> 
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
                    <ui:pager runat="server" ShowTotalPages="true" ID="pager" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        function showDetail(id,name,pageindex) {
            var ecodename = escape(name);
            location.href = "FilialeStoreListDetails.aspx?id=" + id + "&FilialeName=" + ecodename + "&ParentPage=" + pageindex;
        }
</script>
</asp:Content>


