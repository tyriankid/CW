<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master"   CodeBehind="JinLiManageMember.aspx.cs" Inherits="Hidistro.UI.Web.Admin.JinLiManageMember" %>
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
          <h1><strong>ALLHere会员列表</strong></h1>
          <span>对ALLHere会员进行管理，也可以修改会员资料和导入会员</span>
		</div>
    
    <!--数据列表区域-->
    <div class="datalist">
    <!--搜索-->
    <div class="searcharea clearfix br_search">
      <ul>
        <li> <span>用户名称：</span> <span>
          <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  />
        </span> </li>
        <li>
          <asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="searchbutton"/>
        </li>
          <li class="clickTopX" onclick="addAllHere()">添加ALLHere会员</li>
          <li class="clickTopX" onclick="InportAllHere()">导入ALLHere会员</li>
          <li id="clickTopDown" class="clickTopX">导出ALLHere会员</li>
      </ul>
        <dl id="dataArea" style="display: none;">
            <ul>
                <li>请选择需要导出的信息：</li>
                <li>
                    <Hi:ExportFieldsCheckBoxList4 ID="exportFieldsCheckBoxList" runat="server"></Hi:ExportFieldsCheckBoxList4>
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
    <div class="functionHandleArea clearfix">
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
    </div>
    <UI:Grid ID="grdMember" runat="server" AutoGenerateColumns="False"  ShowHeader="true" DataKeyNames="id" GridLines="None"
                   HeaderStyle-CssClass="table_title"  SortOrderBy="UserName" SortOrder="ASC" Width="100%">
                    <Columns>
                       
                        <asp:TemplateField HeaderText="用户名称" SortExpression="UserName" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ALLHere帐号" SortExpression="accountALLHere" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblaccountALLHere" runat="server" Text='<%# Bind("accountALLHere") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="门店名称" SortExpression="accountALLHere" ItemStyle-Width="230px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblStoreName" runat="server" Text='<%# string.IsNullOrEmpty(Eval("StoreName").ToString()) ? Eval("StoreName2") : Eval("StoreName") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="手机号" ShowHeader="true">
                        <ItemTemplate><div></div><%# Eval("CellPhone")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="商品内码"  ItemStyle-Width="100px">                            
                            <itemtemplate><%# Eval("ProductCode") %>
                            </itemtemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="商品型号" ShowHeader="true">
                            <ItemTemplate><%# Eval("ProductModel")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="详细地址" SortExpression="address" ItemStyle-Width="380px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("Address") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="单价" SortExpression="Price" ItemStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("Price") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="金力同步编码" SortExpression="UserCode" ItemStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblUserCode" runat="server" Text='<%# Bind("UserCode") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>    
                         <%--<asp:TemplateField HeaderText="购买数量" SortExpression="BuyNum" ItemStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblBuyNum" runat="server" Text='<%# Bind("BuyNum") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField> --%>                     
                         <asp:TemplateField HeaderText="操作" ItemStyle-Width="100px" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemStyle CssClass="spanD spanN" />
                             <ItemTemplate>
                                 <span class="submit_bianji"><a href='<%# Globals.GetAdminAbsolutePath("/member/EditJinLiMember.aspx?Id=" + Eval("id"))%> '>编辑</a></span>
		                         <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" ID="Delete" Text="删除" CommandName="Delete" IsShow="true" /></span>
                             </ItemTemplate>
                         </asp:TemplateField>  
                    </Columns>
       </UI:Grid>                
     <script type="text/javascript">

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

         ////会员信息导入
         //$(document).ready(function () {
         //    $("#fileUpload").click(function () {
         //        $("#dlExcel").toggle(500, changeClass)
         //    })
         //    changeClass = function () {
         //        if (status == 1) {
         //            $("#fileUpload").removeClass("clickTopX");
         //            $("#fileUpload").addClass("clickTopS");
         //            status = 0;
         //        }
         //        else {
         //            $("#fileUpload").removeClass("clickTopS");
         //            $("#fileUpload").addClass("clickTopX");
         //            status = 1;
         //        }
         //    }
         //});

         function addAllHere() {
             window.location = "AddJinLIMember.aspx";
         }

         function InportAllHere() {
             window.location = "CWExcelMember.aspx?Label=fgs";
         }

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