<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreUserChangeApplyManage.aspx.cs" Inherits="Admin_distributor_StoreUserChangeApplyManage" MasterPageFile="~/Admin/Admin.Master"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1> 店员变更店长审核</h1>
            <span>您可以审核店员申请成为店长的请求。</span>
        </div>
        <div class="datalist">
            <!--搜索-->
            
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li><span>门店名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
                    <li><span>审核状态：</span><asp:DropDownList ID="states" runat="server">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">待审核</asp:ListItem>
                        <asp:ListItem Value="1">已通过</asp:ListItem>
                         <asp:ListItem Value="2">未通过</asp:ListItem>
                    </asp:DropDownList></li>
                    <li>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">至</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>
                     <li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" OnClick="btnSearch_Click" /></li>
                </ul>
            </div>
           
            </div>
            <!--结束-->
            <!--数据列表区域-->
            <UI:Grid runat="server" ID="grdApplyList" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="ID" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title" CssClass="goods-list">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField  HeaderText="门店名称" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                               <%#Eval("StoreName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="申请人" >
                        <ItemTemplate>
                             <%#Eval("UserName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="申请时间" >
                        <ItemTemplate>
                             <%#Eval("AuditingDate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="审核状态" >
                        <ItemTemplate>
                             <%#Eval("ApplyState").ToString()=="0"?"待审核":Eval("ApplyState").ToString()=="1"?"<span style=\"color:green\">通过</span>":"<span style=\"color:red\">未通过</span>"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="审核备注" >
                        <ItemTemplate>
                             <%#Eval("ApplyState").ToString()=="0"?"等待审核":Eval("ApplyState").ToString()=="1"?"符合标准,审核通过":Eval("Reason").ToString()%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-Width="95" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span  class="submit_bianji"><a href="javascript:void(0);" onclick="javascript:SetdistributorClass($(this),'<%# Eval("ID")%>','<%#Eval("ApplyState")%>','<%# Eval("ApplyUserId")%>','<%# Eval("StoreUserId")%>')">审核</a></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>
            <div class="blank12 clearfix">
            </div>
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="producttag.helper.js"></script>
    <script type="text/javascript">
        //当行审核商品
        function SetdistributorClass(e, id, status, applyuserid, storeuserid) {
            e.removeAttr('href');//去掉a标签中的href属性
            e.removeAttr('onclick');//去掉a标签中的onclick事件
            if (status != "0")
            {
                alert("门店已审核");
            }
            else (id != "")
            {
                DialogFrame("distributor/ApplyUserChange.aspx?ID=" + id + "&applyuserid=" + applyuserid + "&storeuserid=" + storeuserid, "店员变更审核", 480, 320);
            }

        }
    </script>
</asp:Content>

