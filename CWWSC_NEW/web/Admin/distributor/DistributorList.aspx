﻿ <%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="DistributorList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.DistributorList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/04.gif" width="32" height="32" /></em>
            <h1><%=TypeTitle %>列表</h1>
            <span>对<%=TypeTitle %>进行管理，您可以查询<%=TypeTitle %>的佣金和详细信息。</span>
        </div>
        <!--搜索-->
        <!--数据列表区域-->
        <div class="datalist">
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix br_search">
                <ul>
                    <li><span>金力帐号：</span> <span>
                        <asp:TextBox ID="txtAccountALLHere" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li><span>门店名称：</span> <span>
                        <asp:TextBox ID="txtStoreName" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li><span>联系人：</span> <span>
                        <asp:TextBox ID="txtRealName" CssClass="forminput" runat="server" /></span>
                    </li>            
                    </ul><ul>
                     <li><span>手机号码：</span> <span>
                        <asp:TextBox ID="txtCellPhone" CssClass="forminput" runat="server" /></span>
                    </li>
                     <li><span>微信号：</span> <span>
                        <asp:TextBox ID="txtMicroSignal" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li ><span>门店等级：</span>                      
                        	<abbr class="formselect">
						<Hi:DistributorGradeDropDownList ID="DrGrade" runat="server" AllowNull="true" NullToDisplay="全部"  />
				</abbr>
                    </li>
                    <li style="display:none"><span>状态：</span> <span><abbr class="formselect"> <asp:DropDownList ID="DrStatus" runat="server">
                    <asp:ListItem Value="0">全部</asp:ListItem>
                     <asp:ListItem Value="1">未审核</asp:ListItem>
                      <asp:ListItem Value="2">通过审核</asp:ListItem>
                       <asp:ListItem Value="3">拒绝审核</asp:ListItem>
                        </asp:DropDownList></abbr></span></li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="搜索" />
                        <asp:HiddenField ID="hiIsAgent" Value="0" runat="server" />
                    </li>
                        <li>
                        <asp:Button ID="btnExportButton" runat="server" class="submit_queding" Text="导出" />
                    </li>
                </ul>
            </div>
            </div>
            <div class="functionHandleArea m_none">
                   <!--分页功能-->
		  <div class="pageHandleArea" style="float:left;">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>          
	      </div>             
            </div>
            <table>
                <thead>
                    <tr class="table_title">
                      <td>头像</td>
                        <td>
                            店铺名
                        </td>
                          <%--<td >
                            门店等级/分销等级
                        </td>--%>
                        <td >
                            门店状态
                        </td>
                        <td>联系人</td>            
                          <td>手机号码</td>
                            <td>认证时间</td>
                              <td>微信昵称</td>  
                        <td>金力帐号</td>                                  
                        <td>操作</td>
                    </tr>
                </thead>
                <asp:Repeater ID="reDistributor" runat="server" OnItemCommand="reDistributor_ItemCommand" EnableViewState="true">
                    <ItemTemplate>
                        <tbody>
                            <tr>
                            <td class="td_txt_cenetr">  &nbsp;<%# Eval("UserHead").ToString()!=""?"<img  src='"+Eval("UserHead")+"' width=\"50\" height=\"50\"/>":""%></td>
                                <td>
                                   &nbsp; <%# Eval("IsNeiGouStore").ToString()=="1" ? "<span style='color:red'>☆</span>" + Eval("StoreName").ToString() + "<span style='color:red'>☆</span>" : Eval("StoreName") %>
                                </td>
                                <%--<td  >
                                 &nbsp; <%# Eval("Name")%> 
                                </td>--%>
                                <td class="td_txt_cenetr">
                                 &nbsp; <%# Eval("ReferralStatus").ToString().Trim()=="0"?"正常":"<span style=\"color:red;\">已冻结</span>"%>
                                </td>
                                <td class="td_txt_cenetr">
                                 &nbsp; <%# Eval("RealName")%>
                                </td>                                               
                                 <td class="td_txt_cenetr">  &nbsp;<%# Eval("CellPhone")%></td>
                                <%--<td>  &nbsp;<%# Eval("QQ")%></td>--%>
                                <td>  &nbsp;<%# string.IsNullOrEmpty(Eval("CreateTime").ToString()) ? "" : Convert.ToDateTime(Eval("CreateTime").ToString()).ToShortDateString()%></td>
                                <td>  &nbsp;<%# Eval("UserName")%></td>
                                <td>  &nbsp;<%# Eval("accountALLHere") %></td>
                                <td class="td_txt_cenetr">
                                 <span class="submit_bianji" ><asp:HyperLink ID="qlkbView1" runat="server" Text="详细" NavigateUrl='<%# "DistributorDetails.aspx?UserId="+Eval("UserId")+"&IsAgent="+Eval("IsAgent")%>' ></asp:HyperLink></span>
                                 <span class="submit_bianji"><asp:HyperLink ID="HyperLink1" runat="server" Text="佣金明细" NavigateUrl='<%# "CommissionsList.aspx?UserId="+Eval("UserId")%>' ></asp:HyperLink></span>
                                 <span class="submit_bianji"><Hi:ImageLinkButton ID="btnFrozen" CommandName='<%# Eval("ReferralStatus").ToString().Trim()=="0"?"Frozen":"UnFre"%>' CommandArgument='<%# Eval("UserId")%>' runat="server" Text='<%# Eval("ReferralStatus").ToString().Trim()=="0"?"冻结":"解冻"%>' IsShow="true"
                                    DeleteMsg='<%# Eval("ReferralStatus").ToString().Trim()=="0"?"确定要冻结【"+Eval("StoreName")+"】吗?":"确定要解冻【"+Eval("StoreName")+"】吗?"%>' /> </span>
                                 <span class="submit_bianji" style='<%# Eval("IsAgent").ToString().Trim()=="0"?"display:none":"display:inline"%>'><Hi:ImageLinkButton runat="server" id="ReSetPwd" text="重置密码" CommandName="ReSetPwd" CommandArgument='<%# Eval("UserId") %>' DeleteMsg='<%# "确定要重置【"+Eval("StoreName")+"】的密码为888888吗?"%>' IsShow="true"></Hi:ImageLinkButton></span>
                                 <span class="submit_bianji" style='<%# Eval("IsAgent").ToString().Trim()=="0"?"display:none":"display:inline"%>'><asp:HyperLink ID="lkbView" runat="server" Text="编辑" NavigateUrl='<%# "EditDistributor.aspx?UserId="+Eval("UserId")%>' ></asp:HyperLink></span>
                                 <span class="submit_bianji" style='<%# (Eval("IsAgent").ToString().Trim()=="1" && ViewState["IsSetProductRange"].ToString()=="1")?"display:inline":"display:none"%>'><a href="javascript:DialogFrame('distributor/AddProducRange.aspx?UserId=<%# Eval("UserId") %>','设置(<%# Eval("StoreName")%>)商品上架范围',null,null)">设置商品上架范围 </a></span>
                                 <span class="submit_bianji" ><Hi:ImageLinkButton runat="server" id="ImageLinkButton1" text="下载推广码" CommandName="Down" CommandArgument='<%# Eval("UserId") %>'></Hi:ImageLinkButton></span>
                                 <span class="submit_bianji" ><Hi:ImageLinkButton runat="server" id="ImageLinkButton2" text="删除认证门店" CommandName="Delete" CommandArgument='<%# Eval("UserId") %>' DeleteMsg='<%# "确定要删除门店【"+Eval("StoreName")+"】吗?"%>' IsShow="true"></Hi:ImageLinkButton></span>
                                 <span class="submit_bianji" style='<%# Eval("IsNeiGouStore").ToString()=="1"?"display:none":"display:inline"%>'><Hi:ImageLinkButton runat="server" id="ImageLinkButton3" text="设置内购门店" CommandName="SetNeigou" CommandArgument='<%# Eval("UserId") %>' DeleteMsg='<%# "确定要将门店【"+Eval("StoreName")+"】设置为内购门店吗?"%>' IsShow="true"></Hi:ImageLinkButton></span>
                                 <span class="submit_bianji" style='<%# Eval("IsNeiGouStore").ToString()=="1"?"display:inline":"display:none"%>'><Hi:ImageLinkButton runat="server" id="ImageLinkButton4" text="取消内购门店" CommandName="CancelNeigou" CommandArgument='<%# Eval("UserId") %>' DeleteMsg='<%# "确定要取消内购门店【"+Eval("StoreName")+"】变为普通门店吗?"%>' IsShow="true"></Hi:ImageLinkButton></span>
                                </td>
                            </tr>
                        </tbody>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
          
            <div class="blank12 clearfix">
            </div>
        </div>
        <!--数据列表底部功能区域-->
     
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(function () {

        });

        function BindDistributorStreet(distributorId) {
            DialogFrame("distributor/DistributorRegion.aspx?distributorId=" + distributorId, "设置配送范围", 880, null);
        }

    </script>
</asp:Content>
