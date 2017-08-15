<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CW_O2OMembers.aspx.cs" Inherits="Admin_member_CW_O2OMembers" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/04.gif" width="32" height="32" /></em>
            <h1><strong>录入会员列表</strong></h1>
            <span>对录入会员进行管理，也可以修改会员资料和添加会员</span>
        </div>

        <!--数据列表区域-->
        <div class="datalist">
            <!--搜索-->
                <div class="searcharea clearfix br_search">
                    <ul>
                        <li>
                            <span>会员姓名/电话：</span> <span>
                                <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" />
                            </span>
                        </li>
                        <li><span>购买时间：</span> <span>
                            <UI:WebCalendar runat="server" CssClass="forminput1" ID="txtStartTime" Width="100" /></span>
                            <span>至</span>
                            <span>
                                <UI:WebCalendar runat="server" CssClass="forminput1" ID="txtEndTime" Width="100" />
                            </span>
                        </li>
                        <li>
                            <asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="searchbutton" OnClick="btnSearchButton_Click" />
                        </li>
                        <li class="clickTopX" onclick="InportMember()">导入会员数据</li>
                        <li id="clickTopDown" class="clickTopX">导出会员信息</li>
                    </ul>
                    <dl id="dataArea" style="display: none;">
                        <ul>
                            <li>请选择需要导出的信息：</li>
                            <li>
                                <Hi:ExportFieldsCheckBoxList5 ID="exportFieldsCheckBoxList" runat="server"></Hi:ExportFieldsCheckBoxList5>
                            </li>
                        </ul>
                        <ul>
                            <li style="padding-left: 47px;">请选择导出格式：</li>
                            <li>
                                <Hi:ExportFormatRadioButtonList ID="exportFormatRadioButtonList" runat="server" />
                            </li>
                        </ul>
                        <ul>
                            <li style="width: 605px;"></li>
                            <li>
                                <asp:Button ID="btnExport" runat="server" CssClass="submit_queding" Text="导出" /></li>
                        </ul>
                    </dl>
                </div>
            <div class="functionHandleArea m_none">
                <!--分页功能-->
                <div class="pageHandleArea" style="float: left;">
                    <ul>
                        <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" />
                        </li>
                    </ul>

                </div>
                <div class="pageNumber" style="float: right;">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                    </div>
                </div>
                <!--结束-->
            </div>
            <UI:Grid ID="grdMember" runat="server" AutoGenerateColumns="False" ShowHeader="true" GridLines="None"
                HeaderStyle-CssClass="table_title" SortOrderBy="CreateDate" SortOrder="ASC" Width="100%" DataKeyNames="userid">
                <Columns>

                    <asp:TemplateField HeaderText="会员姓名" SortExpression="UserName" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="会员电话" SortExpression="Mobile" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Label ID="lblMobile" runat="server" Text='<%# Eval("mobile") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="职业" ShowHeader="true">
                        <ItemTemplate>&nbsp;<%#Eval("profession")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="所属门店DZ号" ShowHeader="true">
                        <ItemTemplate>&nbsp;<%#Eval("storeCode")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="所属门店" ShowHeader="true">
                        <ItemTemplate>&nbsp;<%#Eval("StoreName")%></ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="会员等级" ItemStyle-Width="100px" SortExpression="GradeName">
                        <ItemTemplate>
                            &nbsp;<%#Eval("GradeName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="购买产品型号" ShowHeader="true">
                        <ItemTemplate>&nbsp;<%#Eval("model")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="购买时间" ItemStyle-Width="120px" ShowHeader="true">
                        <ItemTemplate>&nbsp;<%# string.IsNullOrEmpty(Eval("buydate").ToString()) ? "" : Convert.ToDateTime(Eval("buydate")).ToShortDateString() %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="详细地址" SortExpression="address" ItemStyle-Width="280px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval ("Province").ToString()+Eval ("City").ToString()+Eval ("County").ToString()+Eval ("Village").ToString()+ Eval ("Address").ToString() %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" ItemStyle-Width="100px" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemStyle CssClass="spanD spanN" />
                        <ItemTemplate>
                            <span class="submit_bianji"><a href='<%# Globals.GetAdminAbsolutePath("/member/AddO2OMember.aspx?Id=" + Eval("userid"))%> '>编辑</a></span>
                            <span class="submit_shanchu">
                                <Hi:ImageLinkButton runat="server" ID="Delete" Text="删除" CommandName="Delete" IsShow="true" /></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>
            <script type="text/javascript">
       
            </script>
            <div class="blank5 clearfix"></div>
        </div>
        <!--数据列表底部功能区域-->
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
            </div>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <script type="text/javascript">
        //跳转到导入页面
        function InportMember() {
            window.location.href = 'O2OMemberInPort.aspx';
        }

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
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
