<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ListStoreInfo"  MasterPageFile="~/Admin/Admin.Master"  MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">


<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>创维门店管理</h1>
	    <span>对创维门店进行管理，还可以更改创维门店信息</span>
     </div>
	  <!-- 添加按钮-->
	  <div class="btn">
	    <a href="AddStoreInfo.aspx" class="submit_jia">添加新门店</a>
        <a href="ImportFileStore.aspx?Label=fgs" class="submit_jia"">批量导入创维门店</a>
           <asp:Button ID="ExportFgs" runat="server" Text="导出所有创维门店" class="submit_jia"/>
    </div>        
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="datalist">
	    <div class="search clearfix">
			<ul>
				<li><span>关键字(名称/负责人/金力账号)：</span>
				    <span><asp:TextBox ID="txtSearchText" runat="server" Button="btnSearchButton" CssClass="forminput" /></span>
				</li>
			    <li>
				    <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
				</li>
			</ul>
	</div>
	    <UI:Grid ID="grdProductTypes" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="Id" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns> 
                    <asp:TemplateField HeaderText="所属分公司" SortExpression="fgsName" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
	                            <asp:Label ID="lblfgsName" runat="server" Text='<%# Bind("fgsName") %>'></asp:Label>      
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="门店名称" SortExpression="storeName" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="280px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("storeName") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="门店负责人" SortExpression="storeRelationPerson" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblAgentName" runat="server" Text='<%# Bind("storeRelationPerson") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="联系电话" SortExpression="storeRelationCell" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Literal ID="lblRoleName" runat="server" Text='<%# Bind("storeRelationCell") %>'></asp:Literal>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="金力账号" SortExpression="accountALLHere" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <asp:Literal ID="JLID" runat="server" Text='<%# Bind("accountALLHere") %>'></asp:Literal>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="金力同步编码" SortExpression="storekeyid" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <asp:Literal ID="JLKEYID" runat="server" Text='<%# Bind("storekeyid") %>'></asp:Literal>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="是否认证" SortExpression="UserId" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%--<asp:Literal ID="IsOk" runat="server" Text='<%# Eval("UserId").ToString() != "" ? "是" : "否" %>'></asp:Literal>   --%>  
                                <%# Eval("UserId").ToString() != "" ? "<span style='color:green'>是</span>" : "<span style='color:red'>否</span>" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                                  <%--<asp:TemplateField HeaderText="公司排序" SortExpression="scode" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <asp:Literal ID="lblRolescode" runat="server" Text='<%# Bind("scode") %>'></asp:Literal>      
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                     <asp:TemplateField HeaderText="操作" ItemStyle-CssClass="td_txt_cenetr" ItemStyle-Width="120px" >
                         <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
                             <%--<Hi:ImageLinkButton ID="lkbAuditing" runat="server"  CommandName="Auditing" OnClientClick="return ConfirmAuditing()" Text="<%# Bind("Auditing").ToString() == "1" ? "" : "审核通过" %>" CommandArgument="<%# Bind("id") %>"></Hi:ImageLinkButton>--%>
                             <Hi:ImageLinkButton ID="lkbAuditing" IsShow="true" runat="server"  OnClientClick="return ConfirmAuditing()" CommandName="Auditing" CommandArgument='<%# Eval("id") %>'  Text='<%# Eval("Auditing").ToString() == "1" ? "" : "审核通过" %>' />
                             <asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# "EditStoreInfo.aspx?id=" + Eval("id")%>' ></asp:HyperLink> 
                             <Hi:ImageLinkButton ID="lkbDelete" IsShow="true" runat="server" CommandName="Delete"  Text="删除" />
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

    function ConfirmAuditing() {
        return confirm("通过审核后该门店可以在微信端认证\n\n确认要通过审核吗？");
    }

</script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>



