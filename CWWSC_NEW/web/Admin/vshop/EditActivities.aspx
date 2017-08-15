<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="EditActivities.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.EditActivities" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="groupbuy.helper.js"></script>
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            //initValid(new InputValidator('ctl00_contentHolder_dropProduct', 1, 100, false, null, '请选择满减活动商品。'));
            initValid(new InputValidator('ctl00_contentHolder_txtMeetMoney', 1, 10, false, '[0-9]\\d*', '必须是整数。'));
            initValid(new InputValidator('ctl00_contentHolder_txtReductionMoney', 1, 10, false, '[0-9]\\d*', '必须是整数。'));
            //initValid(new InputValidator('ctl00_contentHolder_txtStartDate', 1, 10, false, null, '活动开始日期必填。'));
            //initValid(new InputValidator('ctl00_contentHolder_txtEndDate', 1, 10, false, null, '活动结束日期必填。'));
        }

        $(document).ready(function () {
            InitValidators();
            $("#li_price").hide();
            $.ajax({
                url: "../promotion/EditGroupBuy.aspx",
                data:
                        {
                            isCallback: "true",
                            productId: $("#ctl00_contentHolder_dropProduct").val()
                        },
                type: 'GET', dataType: 'json', timeout: 10000,
                async: false,
                success: function (resultData) {
                    if (resultData.Status == "OK") {
                        var price = resultData.Price;
                        $("#ctl00_contentHolder_lblPrice").html(price);
                        $("#li_price").show();
                    }
                }
            });

            $("#ctl00_contentHolder_dropProduct").change(function () {
                var pId = $(this).val();
                if (pId == "") {
                    $("#li_price").hide();
                }
                else {
                    $.ajax({
                        url: "../promotion/EditGroupBuy.aspx",
                        data:
                        {
                            isCallback: "true",
                            productId: pId
                        },
                        type: 'GET', dataType: 'json', timeout: 10000,
                        async: false,
                        success: function (resultData) {
                            if (resultData.Status == "OK") {
                                var price = resultData.Price;
                                $("#ctl00_contentHolder_lblPrice").html(price);
                                $("#li_price").show();
                            }
                        }
                    });
                }
            });
        });


        function CheckForm() {
            if (PageIsValid()) {
                if ($("#ctl00_contentHolder_dropProduct").val() == null || $("#ctl00_contentHolder_dropProduct").val() == "") {
                    alert("请选择活动商品。");
                    return false;
                }
                if ($("#ctl00_contentHolder_txtStartDate").val() == null || $("#ctl00_contentHolder_txtStartDate").val() == "") {
                    alert("请选择活动开始日期。");
                    return false;
                }
                if ($("#ctl00_contentHolder_txtEndDate").val() == null || $("#ctl00_contentHolder_txtEndDate").val() == "") {
                    alert("请选择活动结束日期。");
                    return false;
                }
                return true;
            } else {
                return false;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/06.gif" width="32" height="32" /></em>
                <h1>添加满减活动</h1>
                <span>填写满减活动详细信息</span>
            </div>
            <div class="formitem validator5" style="padding-left: 15px;">
                <ul class="kuang_ul">
                    <table border="0" cellspacing="5" cellpadding="0" style="width: 775px;" class="float">
                        <tr>
                            <td><span class="formitemtitle Pw_100">商品名称：</span></td>
                            <td>
                                <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></td>
                            <td>
                                <abbr class="formselect">
                                    <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" />
                                </abbr>
                            </td>
                            <td><span class="formitemtitle Pw_100" style="white-space: nowrap;">商品编码：</span></td>
                            <td>
                                <asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></td>
                            <td>
                                <input type="button" id="btnSearch" value="查询" onclick="ResetGroupBuyProducts()" class="searchbutton" /></td>
                        </tr>
                    </table>
                </ul>
                <ul>
                    <li></li>
                    <li>
                        <span class="formitemtitle Pw_128"><em>*</em>满减商品：</span>
                        <abbr class="formselect">
                            <Hi:GroupBuyProductDropDownList ID="dropProduct" runat="server" />
                        </abbr>
                        <p id="ctl00_contentHolder_dropProductTip">满减商品</p>
                    </li>
                    <li id="li_price"><span class="formitemtitle Pw_128">一口价：</span>
                        <abbr class="formselect">
                            <asp:Label ID="lblPrice" runat="server"></asp:Label>
                        </abbr>
                        <p id="P5"></p>
                    </li>
                    <%--<li>
                        <span class="formitemtitle Pw_128"><em>*</em>活动名称：</span>
                        <asp:TextBox ID="txtName" runat="server" CssClass="forminput" Width="400"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtNameTip"></p>
                    </li>--%>
                    <%--<li>
                        <span class="formitemtitle Pw_128"><em>*</em>类目名称：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--全部--"
                                Width="150" />
                        </abbr>
                    </li>--%>
                    <li><span class="formitemtitle Pw_128"><em>*</em>满足金额：</span>
                        <asp:TextBox ID="txtMeetMoney" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtMeetMoneyTip">必须是整数</p>
                    </li>
                    <li><span class="formitemtitle Pw_128"><em>*</em>减免金额：</span>
                        <asp:TextBox ID="txtReductionMoney" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtReductionMoneyTip">必须是整数</p>
                    </li>
                    <li><span class="formitemtitle Pw_128"><em>*</em>开始日期：</span>
                        <UI:WebCalendar runat="server" CssClass="forminput" ID="txtStartDate" /><abbr class="formselect"><Hi:HourDropDownList ID="dropStartHours" runat="server"  style=" margin-left:5px;"/></abbr>
                        <abbr class="formselect"><Hi:MinuteDropDownList ID="dropStartMinute" runat="server"  style=" margin-left:5px;"/></abbr>
                        <p id="ctl00_contentHolder_txtStartDateTip">必须是整数</p>
                    </li>
                    <li><span class="formitemtitle Pw_128"><em>*</em>结束日期：</span>
                        <UI:WebCalendar runat="server" CssClass="forminput" ID="txtEndDate" /><abbr class="formselect"><Hi:HourDropDownList ID="dropEndHours" runat="server"  style=" margin-left:5px;"/></abbr>
                        <abbr class="formselect"><Hi:MinuteDropDownList ID="dropEndMinute" runat="server"  style=" margin-left:5px;"/></abbr>
                        <p id="ctl00_contentHolder_txtEndDateTip">必须是整数</p>
                    </li>
                    <li><span class="formitemtitle Pw_128">活动简介：</span>
                        <Kindeditor:KindeditorControl ID="txtDescription" runat="server" Width="550px" Height="200px" />
                    </li>
                </ul>
                <ul class="btn Pa_100 clear">
                    <li class="li_pa_left">
                        <%--<asp:Button ID="btnAddActivity" runat="server" Text="添加" OnClientClick="return PageIsValid();" CssClass="submit_DAqueding" />--%>
                        <asp:Button ID="btnEditActivity" runat="server" Text="添加" OnClientClick="return CheckForm();" CssClass="submit_DAqueding" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>