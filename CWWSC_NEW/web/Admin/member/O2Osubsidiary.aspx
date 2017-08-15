<%@ Page Language="C#" AutoEventWireup="true" CodeFile="O2Osubsidiary.aspx.cs" Inherits="Admin_member_O2Osubsidiary" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<div class="dataarea mainwidth databody">
     <div class="title">
		  <em><img src="../images/04.gif" width="32" height="32" /></em>
          <h1><strong>录入会员附属信息列表</strong></h1>
          <span>对录入会员附属信息列进行管理，也可以修改会员附属信息列和添加会员附属信息列</span>
		</div>
    
    <!--数据列表区域-->
    <div class="datalist">
    <!--搜索-->
    <div class="clearfix search_titxt2">
    <div class="searcharea clearfix br_search">
      <ul>
        <li> <span>列名：</span> <span>
          <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  />
        </span> </li>
        <li>
          <asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="searchbutton" OnClick="btnSearchButton_Click"/>
        </li>
      </ul>
    </div>
    </div>
    <div class="functionHandleArea clearfix">
      <!--分页功能-->
      <div class="pageHandleArea">
        <ul>
          <li><a href="AddO2OSubsidiary.aspx" class="submit_jia">添加附属信息</a>
          </li>
	       </ul> 
      </div>
      <!--结束-->
    </div>
    <UI:Grid ID="grdSubsidiary" runat="server" AutoGenerateColumns="False"  ShowHeader="true"  GridLines="None"
                   HeaderStyle-CssClass="table_title"  SortOrderBy="CreateDate" SortOrder="ASC" Width="100%" DataKeyNames="ColId">
                    <Columns>
                        <asp:TemplateField HeaderText="排序" SortExpression="scode" ItemStyle-Width="100px" HeaderStyle-CssClass="td_center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" >
                            <ItemTemplate>
	                                <asp:Label ID="lblListscode" runat="server" Text='<%# Eval("scode") %>' ></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="列名" SortExpression="UserName" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblListName" runat="server" Text='<%# Eval("ColName") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="所属大类" SortExpression="Mobile" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblListType" runat="server" Text='<%# Eval("type") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="操作" ItemStyle-Width="100px" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemStyle CssClass="spanD spanN" />
                             <ItemTemplate>
                                 <span class="submit_bianji"><a href='<%# Globals.GetAdminAbsolutePath("/member/AddO2OSubsidiary.aspx?ColId=" + Eval("ColId"))%> '>编辑</a></span>
		                         <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" ID="Delete" Text="删除" CommandName="Delete" IsShow="true" /></span>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

