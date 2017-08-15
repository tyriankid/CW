<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddbuyOnegetOneFree.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddbuyOnegetOneFree" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function ResetGroupBuyProducts() {
        var categoryId = $("#ctl00_contentHolder_dropCategories").val();
        var sku = $("#ctl00_contentHolder_txtSKU").val();
        var productName = $("#ctl00_contentHolder_txtSearchText").val();
        var postUrl = "AddbuyOnegetOneFree.aspx?isCallback=true&action=BuyGetOneProducts&timestamp=";
        postUrl += new Date().getTime() + "&categoryId=" + categoryId + "&sku=" + encodeURI(sku) + "&productName=" + encodeURI(productName);
        document.getElementById("buyProduct").options.length = 0;
        //document.getElementById("GetProduct").options.length = 0;
        $.ajax({
            url: postUrl,
            type: 'GET', dataType: 'json', timeout: 10000,
            async: false,
            success: function (resultData) {
                if (resultData.Status == "0") {
                }
                else if (resultData.Status == "OK") {
                    FillProducts(resultData.Product);
                }
            }
        });
    }

    function FillProducts(product) {
        var productSelector = $("#buyProduct");
        //var GetproductSelector = $("#GetProduct");
        productSelector.append("<option selected=\"selected\" value=\"0\">--\u8BF7\u9009\u62E9--<\/option>");
        //GetproductSelector.append("<option selected=\"selected\" value=\"0\">--\u8BF7\u9009\u62E9--<\/option>");
        $.each(product, function (i, product) {
            productSelector.append(String.format("<option value=\"{0}\">{1}<\/option>", product.ProductId, decodeURI(product.ProductName)));
            //GetproductSelector.append(String.format("<option value=\"{0}\">{1}<\/option>", product.ProductId, decodeURI(product.ProductName)));
        });
    }


    $(document).ready(function () {
        InitValidators();
    })
    function save() {
        if (PageIsValid()) {
            if ($("#buyProduct").val() == "" || $("#buyProduct").val() == null || $("#buyProduct").val()==0) {
                $("#ctl00_contentHolder_ProductTip").attr("class", "msgError").html("请选择买一送一购买商品");
                return false;
            } else {
                $("#ctl00_contentHolder_ProductTip").attr("class", "").html("");
            }
            //if ($("#GetProduct").val() == "" || $("#GetProduct").val()==null||$("#GetProduct").val() ==0) {
            //    $("#ctl00_contentHolder_GetProductTip").attr("class", "msgError").html("请选择买一送一赠送商品");
            //    return false;
            //} else {
            //    $("#ctl00_contentHolder_GetProductTip").attr("class", "").html("");
            //}
            var info = $("#calendarStartDate").val();
            var infos = $("#dropStartHours").val();
            var infoss = $("#dropStartMinute").val();
            if (info == "" || infos == "" || infoss=="") {
                $("#GotimeTip").attr("class", "msgError").html("请选择开始时间");
                return false;
            } else {
                $("#GotimeTip").attr("class", "").html("");
            }

            var Endinfo = $("#calendarEndDate").val();
            var Endinfos = $("#dropEndHours").val();
            var Endinfoss = $("#dropEndMinute").val();
            if (Endinfo == "" || Endinfos == "" || Endinfoss == "") {
                $("#GotimeEndTip").attr("class", "msgError").html("请选择结束时间");
                return false;
            } else {
                $("#GotimeEndTip").attr("class", "").html("");
            }




        } else {
            return false;
        }
    }
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtMaxCount', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
        appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMaxCount', 0.01, 10000000, '输入的数值超出了系统表示范围'));
    }

  </script>     
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/06.gif" width="32" height="32" /></em>
            <h1>添加买一送一活动</h1>
            <span>填写一送一活动详细信息</span>
          </div>          
          
      <div class="formitem validator5" style="padding-left:15px;">      
		<ul class="kuang_ul">
				<table border="0" cellspacing="5" cellpadding="0" style="width:775px;" class=float">
                  <tr>
                    <td><span class="formitemtitle Pw_100">商品名称：</span></td>
                    <td><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></td>
                    <td><abbr class="formselect">
						 <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" />
					     </abbr></td>
                    <td ><span class="formitemtitle Pw_100" style="white-space:nowrap;">商品编码：</span></td>
                    <td><asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></td>
                    <td><input type="button" id="btnSearch" value="查询" onclick="ResetGroupBuyProducts()" class="searchbutton"/></td>
                  </tr>
                </table>
		 </ul>
        <ul>
        <li></li>
        <li><span class="formitemtitle Pw_128">活动商品：</span>
			<abbr class="formselect">
						<Hi:BuyProductDropDownList ID="buyProduct" runat="server"  ClientIDMode="Static"/>
					</abbr>
					<p id="ctl00_contentHolder_ProductTip">买一送一购买商品</p>
			</li>
           <li style="display:none"><span class="formitemtitle Pw_128">赠送商品：</span>
			<abbr class="formselect">
						<Hi:BuyProductDropDownList ID="GetProduct" runat="server"  ClientIDMode="Static"/>
					</abbr>
					<p id="ctl00_contentHolder_GetProductTip">买一送一赠送商品</p>
			</li>
					 <li> <span class="formitemtitle Pw_128"><em >*</em>开始日期：</span>
          <UI:WebCalendar runat="server" CssClass="forminput" ID="calendarStartDate"  ClientIDMode="Static"/><abbr class="formselect"><Hi:HourDropDownList ID="dropStartHours" runat="server"  style=" margin-left:5px;" ClientIDMode="Static"/></abbr>
                        <abbr class="formselect"><Hi:MinuteDropDownList ID="dropStartMinute" runat="server"  style=" margin-left:5px;" ClientIDMode="Static"/></abbr>
          <p id="GotimeTip">当达到开始日期时，活动会自动变为正在参与活动状态。</p>
          </li>
          <li> <span class="formitemtitle Pw_128"><em >*</em>结束日期：</span>
          <UI:WebCalendar runat="server" CssClass="forminput" ID="calendarEndDate"  ClientIDMode="Static"/><abbr class="formselect" ><Hi:HourDropDownList ID="dropEndHours" runat="server" style=" margin-left:5px;" ClientIDMode="Static"/></abbr>
                        <abbr class="formselect"><Hi:MinuteDropDownList ID="dropEndMinute" runat="server"  style=" margin-left:5px;" ClientIDMode="Static"/></abbr>
          <p id="GotimeEndTip">当达到结束日期时，活动会结束。</p>
          </li>
             <li><span class="formitemtitle Pw_128"><em >*</em>限赠数量：</span>
            <asp:TextBox ID="txtMaxCount" runat="server" CssClass="forminput" Text="1"></asp:TextBox>
           <p id="ctl00_contentHolder_txtMaxCountTip">每人赠送次数。</p>
          </li>
      </ul>
      <ul class="btn Pa_100 clear">
      <li class="li_pa_left">
         <asp:Button ID="btnAddBuyOneGetOne" runat="server" Text="保  存" OnClientClick="return save();"  CssClass="submit_DAqueding"  />
         </li>
        </ul>
      </div>
  </div>
  </div>

</asp:Content>
