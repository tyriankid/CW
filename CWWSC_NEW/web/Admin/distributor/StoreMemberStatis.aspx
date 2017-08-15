<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="StoreMemberStatis.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.StoreMemberStatis" %>

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
                门店统计</h1>
            <span>对门店进行统计排名。</span>
        </div>
        <!--搜索-->
        <div class="searcharea clearfix br_search">
            <ul>
            <li><span>DZ号或名称：</span><asp:TextBox ID="txtSearchkey" CssClass="forminput" runat="server"></asp:TextBox></li>
            <li><span>时间：</span> <span>
                <UI:WebCalendar runat="server" CssClass="forminput1" ID="txtStartTime" Width="100" /></span>
                <span>至</span>
                <span> 
                <UI:WebCalendar runat="server" CssClass="forminput1" ID="txtEndTime" Width="100"/>
                </span>
            </li>
            <li>
                <asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="searchbutton" OnClick="btnSearchButton_Click"/>
            </li>
                <li id="clickTopDown" class="clickTopX">导出会员信息</li>
            </ul>
            <%--<ul>
          
	            </ul>--%>
                <dl id="dataArea" style="display:none;">
		        <ul>
		        <li>请选择需要导出的信息：</li>
                <li>
                <Hi:ExportFieldsCheckBoxList3 ID="exportFieldsCheckBoxList" runat="server"></Hi:ExportFieldsCheckBoxList3>
                </li>
	            </ul>
                <ul>
		        <li style="padding-left:47px;">请选择导出格式：</li>
                <li>
                <Hi:ExportFormatRadioButtonList ID="exportFormatRadioButtonList" runat="server" />
                </li>
	            </ul>
                <ul>
		        <li style=" width:605px;"></li>
                    <li><asp:Button ID="btnExport" runat="server" CssClass="submit_queding" Text="导出" /></li>
	            </ul>
                </dl>
        </div>

        <!--数据列表区域-->
        <div class="datalist">
            <table>
                <thead>
                    <tr class="table_title">
                        <td>
                            销量排名
                        </td>
                        <td>
                            分公司名称
                        </td>
                        <td>
                            金力帐号
                        </td>
                        <td>
                            门店名称
                        </td>
                        <td>
                            微会员数
                        </td>
                        <td>
                            录入会员数
                        </td>
                        <td>
                            复购会员数
                        </td>
                        <td>
                            粘性会员数
                        </td>
                         
                    </tr>
                </thead>
                <asp:Repeater ID="reDistributor" runat="server">
                    <ItemTemplate>
                        <tbody>
                            <tr>
                                <td class="td_txt_cenetr">
                                     &nbsp;<asp:Literal runat="server" ID="litph"  Text="0"/>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("fgsName")%>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("AccountALLHere")%>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("StoreName")%>
                                </td>
                                <td class="td_txt_cenetr">
                                    &nbsp;<%# string.IsNullOrEmpty(Eval("MemerNum").ToString()) ? "0" : Eval("MemerNum")%>
                                </td>
                                <td class="td_txt_cenetr">
                                    &nbsp;<%# string.IsNullOrEmpty(Eval("lrNum").ToString()) ? "0" : Eval("lrNum")%>
                                </td>
                                <td class="td_txt_cenetr">
                                    &nbsp;<%# string.IsNullOrEmpty(Eval("fgNum").ToString()) ? "0" : Eval("fgNum") %>
                                </td>
                                <td class="td_txt_cenetr">
                                    &nbsp;<%# string.IsNullOrEmpty(Eval("nxNum").ToString()) ? "0" : Eval("nxNum") %>
                                </td>
                            </tr>
                        </tbody>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="blank12 clearfix">
            </div>
        </div>
    </div>
    <script type="text/javascript">

        //jquery控制上下显示
        $(document).ready(function () {
            var status = 1;
            $("#clickTopDown").click(function () {
                $("#dataArea").toggle(500, changeClass)
            })

            changeClass = function () {
                if (status == 1) {
                    $("#clickTopDown").removeClass("clickTopX");
                    $("#clickTopDown").addClass("clickTopS");
                    status = 0;
                }
                else {
                    $("#clickTopDown").removeClass("clickTopS");
                    $("#clickTopDown").addClass("clickTopX");
                    status = 1;
                }
            }
        });
        
    </script>
</asp:Content>
