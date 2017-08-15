<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ListFiliale"  MasterPageFile="~/Admin/Admin.Master"  MaintainScrollPositionOnPostback="true" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">


<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>分公司管理</h1>
	    <span>对分公司进行管理，还可以更改分公司信息</span>
     </div>
	  <!-- 添加按钮-->
	  <div class="btn">
	    <a href="AddFiliale.aspx" class="submit_jia">添加新分公司</a>
        <a href="ImportFgsFile.aspx?Label=fgs" class="submit_jia"">批量导入分公司</a>
    </div>        
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="datalist">
	    <div class="search clearfix">
			<ul>
				<li><span>分公司名称：</span>
				    <span><asp:TextBox ID="txtSearchText" runat="server" Button="btnSearchButton" CssClass="forminput" /></span>
				</li>
			    <li>
				    <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
				</li>
			</ul>
	</div>
	    <UI:Grid ID="grdProductTypes" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="Id" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns> 
                    <asp:TemplateField HeaderText="公司名称" SortExpression="fgsName" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("fgsName") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="公司电话" SortExpression="fgsPhone" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblAgentName" runat="server" Text='<%# Bind("fgsPhone") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="公司地址" SortExpression="fgsAddress" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="240px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Literal ID="lblRoleName" runat="server" Text='<%# Bind("fgsAddress") %>'></asp:Literal>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="金力同步编码" SortExpression="filialecode" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <asp:Literal ID="lblRolescode" runat="server" Text='<%# Bind("filialecode") %>'></asp:Literal>      
                            </ItemTemplate>
                        </asp:TemplateField>
                     <asp:TemplateField HeaderText="操作" ItemStyle-CssClass="td_txt_cenetr" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px">
                         <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
                             <asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# "EditFiliale.aspx?id=" + Eval("id")%>' ></asp:HyperLink> 
                             <Hi:ImageLinkButton ID="lkbDelete" IsShow="true" runat="server" CommandName="Delete"  Text="删除" />
                             <a href="javascript:void(0)" onclick="javascript:judge('<%# Eval("id") %>');">授予后台用户</a>
                         </ItemTemplate>
                     </asp:TemplateField>              
            </Columns>
        </UI:Grid>
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
  </div>
    <script type="text/javascript">

        function judge(id) {
            $.ajax({
                type: "POST",
                async: false,
                url: location.href,
                data: {
                    action: "judge",
                    fgsid: id
                },
                success: function (data) {
                    document.location = data.toString();
                }
            });
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>




