<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="addProductReservePrice.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.ProductReservePrice" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .formitem ul li p {
            padding-left: 130px;
        } 
        </style>              
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">
    <!--列表-->
    <div class="blank12 clearfix">
    </div>
    <div class="dataarea mainwidth databody">
        <div class="title m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>商品价格预约
            </h1>
            <span>对商品价格定时修改进行编辑和修改</span>
        </div>
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                    <li>
                        <span class="formitemtitle Pw_128">商品名称:</span>
                        <asp:TextBox ClientIDMode="Static" Style="height: 20px; padding: 4px 0; width: 400px" ID="product" oninput="inputchange(this)" onfocus="inputgetfocus(this)" onblur="inputlosefocus(this)" runat="server" />
                        <ul style="width: 500px; position: absolute; border: 1px solid #cecaca; background: #fff; left: 34rem; margin-top: -2rem; height: 750px;display: none; overflow-y: auto;" id="bcul">
                            <asp:Repeater runat="server" ID="ReProduct"  OnItemCommand="ReProduct_ItemCommand"> 
                                <ItemTemplate >
                                    <li>
                                    <asp:LinkButton runat="server" IsShow="true" Text='<%#Eval("ProductName") %>'  CommandName="click"  CommandArgument='<%#Eval("productId") %>' OnClientClick="selectde(this)" ClientIDMode="Static"/></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                         <p id="ctl00_contentHolder_ProductTip">请选择商品</p>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_128">商品规格:</span>
                        <Hi:ProductSkuDropDownList ID="dropProductSku" runat="server" ClientIDMode="Static" />
                        <p id="ctl00_contentHolder_dropProductSkuTip">请选择商品规格</p>
                    </li>

                    <li><span class="formitemtitle Pw_128"><em>*</em>预约生效时间：</span>
                        <UI:WebCalendar runat="server" CssClass="forminput" ID="StartDate"  ClientIDMode="Static"/><abbr class="formselect"><Hi:HourDropDownList ID="dropStartHours" runat="server" Style="margin-left: 5px;"  ClientIDMode="static"/></abbr>
                             <p id="GotimeTip">当达到预约时间时，商品价格会自动变为预约价格。</p>
                    </li>
                    <li><span class="formitemtitle Pw_128">预约成本价:</span>
                      <Hi:TrimTextBox runat="server" CssClass="forminput1" ID="CostPrice" Style="height: 20px; padding: 4px 0; width: 400px;"  />
                        <p id="ctl00_contentHolder_CostPriceTip">商品的成本价，如果为供应商商品则当做结算价格传入AH供财务结算使用</p>
                    </li>
                    <li><span class="formitemtitle Pw_128">预约一口价:</span>
                      <Hi:TrimTextBox runat="server" CssClass="forminput1" ID="SalePrice"  />
                        <p id="ctl00_contentHolder_SalePriceTip">本站会员所看到的商品零售价</p>
                    </li>
                    <li><span class="formitemtitle Pw_128">预约内购价:</span>
                      <Hi:TrimTextBox runat="server" CssClass="forminput1" ID="NeigouPrice"/>
                         <p id="ctl00_contentHolder_NeigouPriceTip">内购门店会员所看到的商品零售价</p>
                    </li>
                    <li>
                        <asp:Button ID="btnSave" runat="server" Text="保 存" CssClass="submit_DAqueding float" OnClientClick="return save()" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <input type="hidden" runat="server" id="hidProduct" clientidmode="Static"/>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
            //文本框得到焦点
            function inputgetfocus(e) {
                document.getElementById("bcul").style.display = "block";
                inputchange(e)
            }
            //文本框失去焦点
            function inputlosefocus(e) {
                setTimeout(function () {
                    document.getElementById("bcul").style.display = "none";
                }, 200)
            }
            //文本框联动下拉框筛选事件
            function inputchange(e) {
                var li = document.getElementById("bcul").children;
                for (var i = 0; i < li.length; i++) {
                    if (li[i].innerText.toLowerCase().indexOf($(e).val().toLowerCase()) >= 0) {
                        li[i].style.display = "block";
                    }
                    else {
                        li[i].style.display = "none";
                    }
                }
            }
            //点击li选中值
            function selectde(e) {
                $("#product").val(e.innerHTML);
                document.getElementById("bcul").style.display = "none";
            }

            $(document).ready(function () {
                InitValidators();
            })
            function save() {
                if (PageIsValid()) {
                    if ($("#hidProduct").val() == "")
                    { $("#ctl00_contentHolder_ProductTip").attr("class", "msgError").html("请选择商品");
                        return false;
                    } else {
                        $("#ctl00_contentHolder_ProductTip").attr("class", "").html("");
                    }
                    if ($("#dropProductSku").val() == "")
                    {
                        $("#ctl00_contentHolder_dropProductSkuTip").attr("class", "msgError").html("请选择规格");
                        return false;
                    } else {
                        $("#ctl00_contentHolder_dropProductSkuTip").attr("class", "").html("");
                    }
                    var info = $("#StartDate").val();
                    var infos = $("#dropStartHours").val();
                  
                    if (info == "" || infos == "") {
                        $("#GotimeTip").attr("class", "msgError").html("请选择生效时间");
                        return false;
                    } else {
                        $("#GotimeTip").attr("class", "").html("");
                    }
                } else {
                    return false;
                }


            }

            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_CostPrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
                appendValid(new MoneyRangeValidator('ctl00_contentHolder_CostPrice', 0.01, 10000000, '输入的数值超出了系统表示范围'));
                initValid(new InputValidator('ctl00_contentHolder_SalePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
                appendValid(new MoneyRangeValidator('ctl00_contentHolder_SalePrice', 0.01, 10000000, '输入的数值超出了系统表示范围'));
                initValid(new InputValidator('ctl00_contentHolder_NeigouPrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
                appendValid(new MoneyRangeValidator('ctl00_contentHolder_NeigouPrice', 0.01, 10000000, '输入的数值超出了系统表示范围'));
            }
    </script>
</asp:Content>
