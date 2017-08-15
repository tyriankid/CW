<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ChoosePro.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ChoosePro" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
  <div class="dataarea mainwidth databody">
 


		<!--数据列表区域-->
		<div class="datalist clearfix">
				<div class="searcharea clearfix br_search">
		  <ul>
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
		  <div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton">
              <span class="signicon"></span> <span class="allSelect"><a onclick="choosePro()" href="javascript:void(0)">选中</a></span> 
	        </ul>
	      </div>
      </div>
		  <UI:Grid ID="grdGift" runat="server" AutoGenerateColumns="false" DataKeyNames="GiftId" SortOrderBy="GiftId" SortOrder="DESC" GridLines="None" HeaderStyle-CssClass="table_title"  Width="100%" >
            <Columns>
                <UI:CheckBoxColumn  ItemStyle-CssClass="td_txt_cenetr" />
                <asp:TemplateField HeaderText="礼品图片" ItemStyle-CssClass="td_txt_cenetr" HeaderStyle-Width="15%">
                    <ItemTemplate>
                        <Hi:HiImage ID="HiImage1"  runat="server" DataField="ThumbnailUrl40"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="礼品名称" SortExpression="Name" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="25%">
                    <itemtemplate>	
                        <asp:HiddenField ID="hiProductId" runat="server" Value='<%# Eval("GiftId")%>' />                  
                        <asp:Label ID="lblGiftName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="成本价" SortExpression="CostPrice" ItemStyle-CssClass="td_txt_right" HeaderStyle-Width="15%">
                    <itemtemplate>
                        <Hi:FormatedMoneyLabel ID="lblCostPrice" runat="server" Money='<%# Eval("CostPrice")%>' />
                    </itemtemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="市场参考价" SortExpression="CostPrice" ItemStyle-CssClass="td_txt_right" HeaderStyle-Width="15%">
                    <itemtemplate>
                        <Hi:FormatedMoneyLabel ID="lblMarketPrice" runat="server" Money='<%# Eval("MarketPrice")%>' />
                    </itemtemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="兑换所需积分" SortExpression="CostPrice" ItemStyle-CssClass="td_txt_right" HeaderStyle-Width="15%">
                    <itemtemplate>
                        <asp:Label ID="lblNeedPoint" runat="server" Text='<%# Eval("NeedPoint")%>' />
                    </itemtemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="库存" SortExpression="CostPrice" ItemStyle-CssClass="td_txt_right" HeaderStyle-Width="12%">
                    <itemtemplate>
                        <%# Eval("Stock")%>
                    </itemtemplate>
                 </asp:TemplateField>
            </Columns>
        </UI:Grid>
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
   

    <script type="text/javascript">
        

        //批量发货
        function choosePro() {
            if ($("input:checked[type='checkbox']").length == 0) {
                alert("请选择商品");
                return false;
            }
            if ($("input:checked[type='checkbox']").length > 1) {
                alert("只能选择一种商品");
                return false;
            }
            var name = "";
            var id = "";
            $("input:checked[type='checkbox']").each(function () {
                name = $(this).parent().siblings().eq(1).find("span").text();
                id = $(this).parent().siblings().eq(1).find("input").val();
            });
            if (name != null && name != "" && id!=null && id!="") {
                artDialog.opener.setSelectProduct(name,id);
                artDialog.close();
            }
        }

        

    </script>
</asp:Content>

