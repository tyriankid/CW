<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IntegralLotteryRule.aspx.cs" Inherits="Admin_IntegralLottery_IntegralLotteryRule" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
  <div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/06.gif" width="32" height="32" /></em>
  <h1> 积分大转盘 </h1>
  <span>积分大转盘列表</span>
  </div>


		<!--数据列表区域-->
		<div class="datalist clearfix">
				<div class="searcharea clearfix br_search">
		  <ul>
              <li><a class="btn" href="AddIntegralLotteryRule.aspx?IsAdd=true">添加大转盘奖项</a></li>
				<li><span>关键字：</span><span>
				  <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"></asp:TextBox>
				  <input type="checkbox" id="chkPromotion" runat="server" /><span style="display:none">参与促销赠送的礼品</span>
			  </span></li>
				
				<li><asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" /></li>
             
		  </ul>
	</div>
		<div class="advanceSearchArea clearfix">
			<!--预留显示高级搜索项区域-->
	    </div>
		<!--结束-->
		
          <div class="functionHandleArea m_none clearfix">
		  <!--分页功能-->
		  <div class="pageHandleArea">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	      </div>
		  <div class="pageNumber">
			<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
				</div>
			</div>
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
		  <%--<div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton">
              <span class="signicon"></span> <span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span> 
              <span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span> 
              <span class="delete"><Hi:ImageLinkButton ID="lkbDelectCheck"  IsShow="true" Height="25px" runat="server" Text="删除" /></span></li>
	        </ul>
	      </div>--%>
      </div>
		  <table class="table table-striped">
                        <thead>
                            <tr>
                                <th>奖项等级</th>
                                <th>奖项名称</th>
                                <th>奖励商品</th>
                                <th>中奖占比数</th>
                                <th style="text-align: center;">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rptRule">
                                <ItemTemplate>
                                    <tr>
                                        <td><%#Eval("LotteryClass") %></td>
                                        <td><%#Eval("LotteryItem") %></td>
                                        <td><%#Eval("Name") %></td>
                                        <td><%#Eval("LotteryProportion") %></td>
                                        <td style="text-align: center;" class="opration"><a href="/Admin/IntegralLottery/AddIntegralLotteryRule.aspx?RuleId=<%#Eval("RuleId") %>&Action=Update">编辑</a> | <a onclick="goDelete('<%#Eval("LotteryItem") %>','<%#Eval("RuleId") %>')">删除</a></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
		  <div class="blank12 clearfix"></div>
</div>
		<!--数据列表底部功能区域-->
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
    <script type="text/javascript" language="javascript">


        function goDelete(name,id) {
            if (!confirm("确认删除" + name + "?")) { return false; }
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "IntegralLotteryRule.aspx/Del",
                data: "{id:'" + id + "'}",
                dataType: 'json',
                success: function (result) {
                    if (result.d==true) {
                        alert("删除成功");
                        location.href = location.href;
                    } else {
                        alert("删除失败");
                    }
                    
                }
            });
        }


        function setSelectProduct(name, id) {
            $("#ctl00_contentHolder_txtProductName").val(name);
            $("#ctl00_contentHolder_hiProductId").val(id);
        }
</script>
</asp:Content>
