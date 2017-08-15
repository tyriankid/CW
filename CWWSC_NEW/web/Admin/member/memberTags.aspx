<%@ Page Language="C#" AutoEventWireup="true" CodeFile="memberTags.aspx.cs" Inherits="Admin_member_memberTags" MasterPageFile="~/Admin/Admin.Master" %>

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
            <h1>会员标签列表</h1>
            <span>对会员标签进行管理，您可以修改会员标签或删除会员标签</span>
        </div>
        <!--搜索-->

        <!--数据列表区域-->
        <div class="datalist">
            <div class="searcharea clearfix br_search">
                <ul>
                    <li>
                        <span>标签名称：</span>
                        <span>
                            <asp:TextBox ID="txtSearchText" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li>
                        <span>标签类型：</span>
                        <span>
                            <asp:DropDownList ID="dropMemberTagsType" runat="server">
                                  <asp:ListItem Value="">全部</asp:ListItem>
                                <asp:ListItem Value="0">微会员</asp:ListItem>
                                <asp:ListItem Value="1">金立会员</asp:ListItem>
                            </asp:DropDownList></span>
                    </li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="搜索" OnClick="btnSearchButton_Click" />
                         
                    </li>
                    <li >
                       <a href="AddmemberTags.aspx"  class="submit_DAqueding float">新增</a>
                    </li>
                    <li style="float:right;position:relative;top: 4px;">
                        <span>
                                    <asp:LinkButton ID="btnOrder" CssClass="btn_paixu" runat="server" Text="保存排序" OnClick="btnOrder_Click" />
                                </span>
                    </li>
                </ul>
            </div>
            <UI:Grid ID="grdmemberTags" DataKeyNames="TagID" runat="server" ShowHeader="true"
                AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None"
                Width="100%">
                 <Columns>
                    <asp:TemplateField HeaderText="标签类型" HeaderStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <%# Eval("TagType").ToString()=="0"?"微会员":"金立会员"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="标签名称" HeaderStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <%# Eval("TagName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="排序号" HeaderStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                             <asp:TextBox ID="txtSort" runat="server" Text='<%# Eval("Scode") %>'
                                Width="80px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <a href="AddmemberTags.aspx?TagID=<%#Eval("TagID")%>">编辑</a>
                                <Hi:ImageLinkButton runat="server" ID="Delete" IsShow="true" Text="删除" CommandName="Delete" CommandArgument='<%# Eval("TagID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                     </Columns>
                </UI:Grid>
          
    </div>
     </div>
    <!--数据列表底部功能区域-->
	  <div class="page">
	  <div class="bottomPageNumber clearfix">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
        　　　</div>
		</div>
      </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

