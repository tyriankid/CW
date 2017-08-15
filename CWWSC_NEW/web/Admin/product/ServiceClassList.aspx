<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ServiceClassList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">

<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1>服务品类管理</h1>
  <span>管理服务商品的品类，提供新建/编辑/删除</span></div>
  	
		<!-- 添加按钮-->
   
    
<!--结束-->
		<!--数据列表区域-->
	<div class="datalist">
        <div class="clearfix search_titxt2">
		    <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
			    <ul>
				    <li><span>品类名称：</span>
				        <span><asp:TextBox ID="txtSearchText" runat="server" Button="btnSearchButton" CssClass="forminput" /></span>
				    </li>
			        <li style=" margin-top:3px;">
				        <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
				    </li>
			    </ul>
	        </div>
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
			    <ul>
                    <li><a href="EditServiceClass.aspx" class="submit_jia100">添加新品类</a></li>
                    <li><asp:LinkButton ID="btnorder" CssClass="btn_paixu" runat="server">批量保存排序</asp:LinkButton></li>
			    </ul>
            </div>
        </div>
	    <UI:Grid ID="grdServiceClassList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="ScID" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="服务品类名称" ItemStyle-Width="50%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Literal ID="litName" runat="server" Text='<%# Bind("ClassName") %>'></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <UI:SortImageColumn Visible="false" HeaderText="排序" ReadOnly="true" HeaderStyle-CssClass="td_right td_left"/>
                    <asp:TemplateField HeaderText="显示顺序" ItemStyle-Width="70px"  HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                      <input id="Text1" type="text" runat="server" value='<%# Eval("Scode") %>' style="width:60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                   </ItemTemplate>
                   </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                             <span class="submit_bianji"><asp:HyperLink ID="lkEdit" runat="server" Text="编辑" NavigateUrl='<%# "EditServiceClass.aspx?id="+Eval("ScID")%>' /></span> 
                             <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="lkbtnDelete" CommandName="Delete" IsShow="true" Text="删除" /></span>
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
            </UI:Grid>
	</div>
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
       
</asp:Content>
