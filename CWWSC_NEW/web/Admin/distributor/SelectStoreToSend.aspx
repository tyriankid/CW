<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectStoreToSend.aspx.cs" Inherits="Admin_distributor_SelectStoreToSend" MasterPageFile="~/Admin/Admin.Master"%>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datalist">
            <!--搜索-->
            
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix" style="padding: 0px 0px 3px 0px;">
                <ul>
                    <li><span>门店名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
                     <li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" OnClick="btnSearch_Click" /></li>
                </ul>
                <ul>
                    <li><span>消息内容：</span><span><asp:TextBox ID="txtSendMsg" runat="server" TextMode="MultiLine" MaxLength="300" Rows="3" style="width:300px;height:34px"/></span></li>
                    <li><asp:Button ID="btnSendMsg" runat="server" Text="发送" CssClass="searchbutton" OnClick="btnSendMsg_Click" /></li>
                </ul>
            </div>
           
            </div>
            <!--结束-->
            <!--数据列表区域-->
            <UI:Grid runat="server" ID="grdStoreInfo" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="userid" SortOrder="Desc" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" CssClass="goods-list">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" ItemStyle-CssClass="td_txt_cenetr">
                        <ItemTemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("UserId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="门店名" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                               <%#Eval("StoreName")%>
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
    <script type="text/javascript">

        
    </script>
</asp:Content>

