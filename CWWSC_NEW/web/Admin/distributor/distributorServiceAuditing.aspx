<%@ Page Language="C#" AutoEventWireup="true" CodeFile="distributorServiceAuditing.aspx.cs" Inherits="Admin_distributor_distributorServiceAuditing"  MasterPageFile="~/Admin/Admin.Master"%>

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
            <h1>
                服务门店审核</h1>
            <span>您可以审核服务门店。</span>
        </div>
        <div class="datalist">
            <!--搜索-->
            
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li><span>门店名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
                    <li><span>店长姓名：</span><span><asp:TextBox ID="txtRealName" runat="server" CssClass="forminput" /></span></li>
                    <li><span>金立账号：</span><span><asp:TextBox ID="txtAllHere"  runat="server" CssClass="forminput" /></span></li>
                    <li><span>审核状态：</span><asp:DropDownList ID="states" runat="server">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">未审核</asp:ListItem>
                        <asp:ListItem Value="1">已通过</asp:ListItem>
                         <asp:ListItem Value="2">未通过</asp:ListItem>
                    </asp:DropDownList></li>
                     <li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" OnClick="btnSearch_Click" /></li>
                </ul>
            </div>
           
            </div>
            <!--结束-->
            <!--数据列表区域-->
            <UI:Grid runat="server" ID="grdDistributor" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="DcID" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title" CssClass="goods-list">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField  HeaderText="门店名称" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                               <%#Eval("StoreName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="店长姓名" >
                        <ItemTemplate>
                             <%#Eval("RealName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="金力账号" >
                        <ItemTemplate>
                             <%#Eval("accountALLHere")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="申请区域" >
                        <ItemTemplate>
                             <%#Eval("RegionName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="申请品类" >
                        <ItemTemplate>
                             <%#serviceName(Eval("ScIDs").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="申请时间" >
                        <ItemTemplate>
                             <%#Eval("ApplyDate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="审核时间" >
                        <ItemTemplate>
                             <%#Eval("AuditDate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="当前状态" >
                        <ItemTemplate>
                             <%#Eval("State").ToString()=="0"?"待审核":Eval("State").ToString()=="1"?"<span style=\"color:green\">通过</span>":"<span style=\"color:red\">未通过</span>"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="审核备注" >
                        <ItemTemplate>
                             <%#Eval("State").ToString()=="0"?"等待审核":Eval("State").ToString()=="1"?"符合标准,审核通过":Eval("AuditRemark").ToString()%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-Width="95" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span  class="submit_bianji"><a href="javascript:void(0);" onclick="javascript:SetdistributorClass('<%# Eval("DcID")%>','<%#Eval("State")%>')">审核</a></span>
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
        function SetdistributorClass(id, status) {
            if (status != "0")
            {
                alert("门店已审核");
                return;
            }
            else (id != "")
            {
                DialogFrame("distributor/SetdistributorClass.aspx?DcID=" + id, "服务门店审核", 480, 320);
            }
        }
    </script>
</asp:Content>

