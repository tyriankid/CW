<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ListDistributorSales"  MasterPageFile="~/Admin/Admin.Master"  MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">


<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>店员管理</h1>
	    <span>对门店的店员进行管理，允许新增、编辑、删除等</span>
     </div>
	  <!-- 添加按钮-->
	 <%-- <div class="btn">--%>
	   
        <%--<a href="ImportFileStore.aspx?Label=fgs" class="submit_jia"">批量导入创维门店</a>--%>
        <%--<asp:Button ID="ExportFgs" runat="server" Text="导出所有创维门店" class="submit_jia"/>--%>
   <%-- </div>        --%>
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="datalist">

	    <div class="clearfix search_titxt2">
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
			<ul>
				<li><span>关键字(姓名/电话)：</span>
				    <span><asp:TextBox ID="txtSearchText" runat="server" Button="btnSearchButton" CssClass="forminput" /></span>
				</li>
			    <li>
				    <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
				</li>
                <li>
                     <a href="EditDistributorSales.aspx" class="submit_DAqueding float">添加新店员</a>
                </li>
			</ul>
	</div>
            </div>
	    <UI:Grid ID="grdProductTypes" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="DsID" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns> 
                    <asp:TemplateField HeaderText="所属门店" SortExpression="StoreName" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
	                            <asp:Label ID="lblStoreName" runat="server" Text='<%# Bind("StoreName") %>'></asp:Label>      
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="店长姓名" SortExpression="storeRelationPerson" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblstoreRelationPerson" runat="server" Text='<%# Bind("storeRelationPerson") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="金力账号" SortExpression="accountALLHere" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblaccountALLHere" runat="server" Text='<%# Bind("accountALLHere") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="店员姓名" SortExpression="DsName" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Literal ID="lblDsName" runat="server" Text='<%# Bind("DsName") %>'></asp:Literal>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="店员电话" SortExpression="DsPhone" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <asp:Literal ID="lblDsPhone" runat="server" Text='<%# Bind("DsPhone") %>'></asp:Literal>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="是否认证" SortExpression="IsRz" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate> 
                                <%# Eval("IsRz").ToString() == "1" ? "<span style='color:green'>是</span>" : "<span style='color:red'>否</span>" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="认证时间" SortExpression="RzTime" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("RzTime") %> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="排序号" SortExpression="Scode" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("Scode") %> 
                            </ItemTemplate>
                        </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="是否启用" SortExpression="IsStart" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Bind("IsStart").ToString() == "1" ? "是" : "否" %>  
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                     <asp:TemplateField HeaderText="操作" ItemStyle-CssClass="td_txt_cenetr" ItemStyle-Width="120px" >
                         <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
                             <%--<Hi:ImageLinkButton ID="lkbAuditing" runat="server"  CommandName="Auditing" OnClientClick="return ConfirmAuditing()" Text="<%# Bind("Auditing").ToString() == "1" ? "" : "审核通过" %>" CommandArgument="<%# Bind("id") %>"></Hi:ImageLinkButton>--%>
                             <%--<Hi:ImageLinkButton ID="lkbAuditing" IsShow="true" runat="server"  OnClientClick="return ConfirmAuditing()" CommandName="Auditing" CommandArgument='<%# Eval("id") %>'  Text='<%# Eval("Auditing").ToString() == "1" ? "" : "审核通过" %>' />--%>
                             <asp:HyperLink ID="hlEdit" runat="server" Text="编辑" NavigateUrl='<%# "EditDistributorSales.aspx?id=" + Eval("DsID")%>' ></asp:HyperLink> 
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



