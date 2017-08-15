﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeBehind="AddFilialeProductLine.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddFilialeProductLine" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
     <style type="text/css">
        #province_select  li{width: auto;
    margin: 0px 10px 0px 0px;
    clear: none;
    float: left;
    padding: 0 !important;}
        .ap_city #city_select li{width: auto;
    margin: 0px 10px 0px 0px;
    clear: none;
    float: left;
    padding: 0 !important;}
        .ap_area #area_select li{width: auto;
    margin: 0px 10px 0px 0px;
    clear: none;
    float: left;
    padding: 0 !important;}

    .disabledIput {background: #e0dfdf;}
    </style>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery_hashtable.js" />
    <Hi:Script ID="Script1" runat="server" Src="/utility/jquery-powerFloat-min.js" />
    <link href="/utility/flashupload/flashupload.css" rel="stylesheet" type="text/css" />
    <Hi:Script ID="Script3" runat="server" Src="/utility/flashupload/flashupload.js" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>编辑商品信息</h1>
            <span>商品信息修改</span>
        </div>
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                    <li>
                        <h2 class="colorE">基本信息</h2>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_198">所属商品分类：</span>
                        <span class="colorE float fonts">
                            <asp:Literal runat="server" ID="litCategoryName"></asp:Literal></span>
                        [<asp:HyperLink runat="server" ID="lnkEditCategory" CssClass="a" Text="编辑"></asp:HyperLink>]
                    </li>
                    <li>
                        <span class="formitemtitle Pw_198">商品类型：</span>
                        <abbr class="formselect">
                            <Hi:ProductTypeDownList runat="server" CssClass="productType" ID="dropProductTypes" NullToDisplay="--请选择--" /></abbr>
                        品牌：<abbr class="formselect"><Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandCategories" NullToDisplay="--请选择--" /></abbr>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198"><em>*</em>商品来源：</span>
                        <asp:DropDownList ID="DDLProductSource" runat="server">
                            <asp:ListItem Value="4">线下</asp:ListItem>
                        </asp:DropDownList>
                        <p id="P1">系统自动选择，不可修改</p>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198">商品名称：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductName" Width="350px" />
                        <p id="ctl00_contentHolder_txtProductNameTip">限定在60个字符</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">排序：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtDisplaySequence" />
                        <p id="ctl00_contentHolder_txtDisplaySequenceTip">商品显示顺序，越大排在越前</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">商品内码：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductCode" />
                        <p id="ctl00_contentHolder_txtProductCodeTip">长度不能超过20个字符，商品内码是与港口系统或AH系统对接的关键信息。</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">计量单位：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtUnit" />
                        <p id="ctl00_contentHolder_txtUnitTip">必须限制在20个字符以内且只能是英文和中文例:g/元</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">市场价：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMarketPrice" />元
                <p id="ctl00_contentHolder_txtMarketPriceTip">本站会员所看到的商品市场价</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">限购数量：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtRestrictNeigouNum" />
                        <p id="ctl00_contentHolder_txtRestrictNeigouNumTip">在内购门店中限制同一会员最多购买的数量</p>
                    </li>
                    <!-- 新增一个商品虚假数量后台更改 -->
                    <li><span class="formitemtitle Pw_198">售出商品显示数量：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="showCounts" />件
                <p id="s1">客户看到的自定义商品售出数量</p>
                    </li>

                    <li>
                        <h2 class="colorE">扩展属性</h2>
                    </li>
                    <li id="attributeRow" style="display: none;"><span class="formitemtitle Pw_198">商品属性：</span>
                        <div class="attributeContent" id="attributeContent"></div>
                        <Hi:TrimTextBox runat="server" ID="txtAttributes" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                    </li>
                    <li id="skuCodeRow"><span class="formitemtitle Pw_198">型号：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSku" />
                        <p id="ctl00_contentHolder_txtSkuTip">限定在30个字符</p>
                    </li>
                    <li id="salePriceRow"><span class="formitemtitle Pw_198">一口价：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSalePrice" />元<input type="button" class="inp_edit" style="display: none" onclick="editProductMemberPrice();" value="编辑会员价" />
                        <Hi:TrimTextBox runat="server" ID="txtMemberPrices" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                        <p id="ctl00_contentHolder_txtSalePriceTip">本站会员所看到的商品零售价</p>
                    </li>
                    <li id="neigouPriceRow"><span class="formitemtitle Pw_198">内购价：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtNeigouPrice" />&nbsp;元
                <p id="ctl00_contentHolder_txtNeigouPriceTip">内购门店会员所看到的商品零售价</p>
                    </li>
                    <li id="costPriceRow"><span class="formitemtitle Pw_198">成本(结算)价：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtCostPrice" />元
	            <p id="ctl00_contentHolder_txtCostPriceTip">商品的成本价，如果为供应商商品则当做结算价格传入AH供财务结算使用</p>
                    </li>
                    <li id="qtyRow"><span class="formitemtitle Pw_198">商品库存：<em>*</em></span><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtStock" /></li>
                    <li id="weightRow"><span class="formitemtitle Pw_198">商品重量：</span><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtWeight" />克</li>

                    <%--start返佣相关--%>

                    <li>
                        <h2 class="colorE">佣金规则</h2>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_198">一口价返佣类型：</span>
                        <asp:RadioButton runat="server" ID="RadioSaleA" GroupName="SaleType" Checked="true" Text="比例"></asp:RadioButton>
                        <asp:RadioButton runat="server" ID="RadioSaleB" GroupName="SaleType" Text="金额"></asp:RadioButton>
                    </li>
                    <li id="SaleRatioPriceRow"><span class="formitemtitle Pw_198">返佣比例：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSaleCommisionRatio" />&nbsp;%
                        <p id="ctl00_contentHolder_txtSaleCommisionRatioTip">返佣比例值</p>
                    </li>
                    <li id="SaleMoneyPriceRow"><span class="formitemtitle Pw_198">返佣金额：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSaleCommisionMoney" />&nbsp;元
                        <p id="ctl00_contentHolder_txtSaleCommisionMoneyTip">返佣金额值</p>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_198">内购价返佣类型：</span>
                        <asp:RadioButton runat="server" ID="RadioNeigouA" GroupName="NeigouType" Checked="true" Text="比例"></asp:RadioButton>
                        <asp:RadioButton runat="server" ID="RadioNeigouB" GroupName="NeigouType" Text="金额"></asp:RadioButton>
                    </li>
                    <li id="NeigouRatioPriceRow"><span class="formitemtitle Pw_198">返佣比例：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtNeigouCommisionRatio" />&nbsp;%
                        <p id="ctl00_contentHolder_txtNeigouCommisionRatioTip">返佣比例值</p>
                    </li>
                    <li id="NeigouMoneyPriceRow"><span class="formitemtitle Pw_198">返佣金额：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtNeigouCommisionMoney" />&nbsp;元
                        <p id="ctl00_contentHolder_txtNeigouCommisionMoneyTip">返佣金额值</p>
                    </li>

                    <%--end返佣相关--%>

                    <li id="skuTitle" style="display: none;">
                        <h2 class="colorE">商品规格</h2>
                    </li>
                    <li id="enableSkuRow" style="display: none;"><span class="formitemtitle Pw_198">规格：</span><input class="inp_guige" id="btnEnableSku" type="button" value="开启规格" />
                        开启规格前先填写以上信息，可自动复制信息到每个规格</li>
                    <li id="skuRow" style="display: none;">
                        <p id="skuContent" class="gray-btn-box pa-bottom-10">
                            <input type="button" id="btnshowSkuValue" value="生成部分规格" />
                            <input type="button" id="btnAddItem" value="增加一个规格" />
                            <input type="button" id="btnCloseSku" value="关闭规格" />
                            <input type="button" id="btnGenerateAll" value="生成所有规格" />
                        </p>
                        <p id="skuFieldHolder" style="margin: 0px auto; display: none;"></p>
                        <div id="skuTableHolder">
                        </div>
                        <Hi:TrimTextBox runat="server" ID="txtSkus" TextMode="MultiLine" Style="display: none"></Hi:TrimTextBox>
                        <asp:CheckBox runat="server" ID="chkSkuEnabled" Style="display: none;" />
                    </li>
                    <li id="liImgList">
                        <h2 class="colorE">图片和描述</h2>
                    </li>

                    <li style="height: 126px;"><span class="formitemtitle Pw_198">商品图片：</span><Hi:ProductFlashUpload ID="ucFlashUpload1" runat="server" MaxNum="5" />
                    </li>
                    <li>
                        <p class="Pa_198 clearfix m_none" style="padding-left: 200px;">支持多图上传,最多5个,每个图应小于120k,jpg,gif,png格式。建议为500x500像素</p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle Pw_198">商品简介：</span>
                        <Hi:TrimTextBox runat="server" Rows="6" Height="100px" Columns="76" ID="txtShortDescription" TextMode="MultiLine" />
                        <p class="Pa_198">限定在300个字符以内</p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle Pw_198">商品描述：</span>
                        <Kindeditor:KindeditorControl ID="fckDescription" runat="server" Height="300" />
                        <p style="color: Red;">
                            <asp:CheckBox runat="server" ID="ckbIsDownPic" Text="是否下载商品描述外站图片" />
                        </p>
                        <p class="Pa_198">如果勾选此选项时，商品里面有外站的图片则会下载到本地，相反则不会，由于要下载图片，如果图片过多或图片很大，需要下载的时间就多，请慎重选择。</p>
                    </li>
                    <%--****--%>
                    <li class="clearfix"><span class="formitemtitle Pw_198">商品规格：</span>
                        <Kindeditor:KindeditorControl ID="editSpecification" runat="server" Height="300" />
                        <p style="color: Red;">
                            <asp:CheckBox runat="server" ID="chkIsDownPic" Text="是否下载商品描述外站图片" />
                        </p>
                        <p class="Pa_198">如果勾选此选项时，商品里面有外站的图片则会下载到本地，相反则不会，由于要下载图片，如果图片过多或图片很大，需要下载的时间就多，请慎重选择。</p>
                    </li>
                    <li style="overflow:initial;position: relative;">
                        <span class="formitemtitle Pw_198">商品销售区域(省市区)：</span>
                        <Hi:RegionSelector ID="dropRegion" runat="server" IsShift="false" ProvinceWidth="180" CityWidth="150" CountyWidth="150" ClientIDMode="Static" />
                    </li>
                    <li>
                        <h2 class="colorE clear">相关设置</h2>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_198">商品包邮：
                        </span>
                        <asp:CheckBox ID="ChkisfreeShipping"
                            runat="server" />
                        &nbsp;</li>
                    <li class="clearfix" id="l_tags" runat="server" style="display: none;">
                        <span class="formitemtitle Pw_198">商品标签定义：<a id="a_addtag" href="javascript:void(0)" onclick="javascript:AddTags()" class="add">添加</a></span>

                        <div id="div_tags">
                            <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
                        </div>
                        <div id="div_addtag" style="display: none">
                            <input type="text" id="txtaddtag" /><input type="button" value="保存" onclick="return AddAjaxTags()" />
                        </div>
                        <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                    </li>
                </ul>
                <ul class="btntf Pa_198 clear">
                    <asp:Button runat="server" ID="btnSave" Text="保 存" OnClientClick="return doSubmit();" CssClass="submit_DAqueding inbnt" />
                </ul>
            </div>
        </div>
    </div>
    <div class="Pop_up" id="priceBox" style="display: none;">
        <h1>
            <span id="popTitle"></span>
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" alt="关闭" width="38" height="20" />
        </div>
        <div class="mianform ">
            <div id="priceContent">
            </div>
            <div style="margin-top: 10px; text-align: center;">
                <input type="button" value="确定" class="btn-blue" onclick="doneEditPrice();" />
            </div>
        </div>
    </div>

    <div class="Pop_up" id="skuValueBox" style="display: none;">
        <h1>
            <span>选择要生成的规格</span>
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" alt="关闭" width="38" height="20" />
        </div>

        <div class="mianform ">
            <div id="skuItems">
            </div>
            <div style="margin-top: 10px; text-align: center;">
                <input type="button" value="确定" id="btnGenerate" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidFiliale" runat="server" ClientIDMode="Static" />
    <!--分公司与总部辨别 -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">


        function InitValidators() {
         
            initValid(new InputValidator('ctl00_contentHolder_txtProductName', 1, 60, false, null, '商品名称字符长度在1-60之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtDisplaySequence', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtDisplaySequence', 1, 9999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtProductCode', 0, 20, true, null, '商家编码的长度不能超过20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtSalePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSalePrice', 0.01, 10000000, '输入的数值超出了系统表示范围'));

            //内购价
            initValid(new InputValidator('ctl00_contentHolder_txtNeigouPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtNeigouPrice', 0, 99999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtCostPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCostPrice', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtMarketPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMarketPrice', 0, 99999999, '输入的数值超出了系统表示范围'));
            //限购数量
            initValid(new InputValidator('ctl00_contentHolder_txtRestrictNeigouNum', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtRestrictNeigouNum', 0, 99999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtSku', 0, 30, false, null, '商品型号的长度不能超过30个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtStock', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtStock', 1, 9999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtUnit', 1, 20, true, '[a-zA-Z\/\u4e00-\u9fa5]*$', '必须限制在20个字符以内且只能是英文和中文例:g/元'))
            initValid(new InputValidator('ctl00_contentHolder_txtWeight', 0, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtWeight', 1, 9999999, '输入的数值超出了系统表示范围'));

            /**start返佣值验证***/
            initValid(new InputValidator('ctl00_contentHolder_txtSaleCommisionRatio', 1, 6, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSaleCommisionRatio', 0, 100, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtSaleCommisionMoney', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSaleCommisionMoney', 0, 99999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtNeigouCommisionRatio', 1, 6, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtNeigouCommisionRatio', 0, 100, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtNeigouCommisionMoney', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtNeigouCommisionMoney', 0, 99999999, '输入的数值超出了系统表示范围'));
            /**start返佣值验证***/
            initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 0, 300, true, null, '商品简介必须限制在20个字符以内'));
        }
        $(document).ready(function () {
            InitValidators();

            //一口价佣初始控制
            if ($("#ctl00_contentHolder_RadioSaleA").attr("checked") == "checked") {
                $("#ctl00_contentHolder_txtSaleCommisionRatio").removeAttr("disabled");
                $("#ctl00_contentHolder_txtSaleCommisionRatio").removeClass("disabledIput");

                $("#ctl00_contentHolder_txtSaleCommisionMoney").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtSaleCommisionMoney").addClass("disabledIput");
            }
            if ($("#ctl00_contentHolder_RadioSaleB").attr("checked") == "checked") {
                $("#ctl00_contentHolder_txtSaleCommisionMoney").removeAttr("disabled");
                $("#ctl00_contentHolder_txtSaleCommisionMoney").removeClass("disabledIput");

                $("#ctl00_contentHolder_txtSaleCommisionRatio").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtSaleCommisionRatio").addClass("disabledIput");
            }
            //内购价佣初始控制
            if ($("#ctl00_contentHolder_RadioNeigouA").attr("checked") == "checked") {
                $("#ctl00_contentHolder_txtNeigouCommisionRatio").removeAttr("disabled");
                $("#ctl00_contentHolder_txtNeigouCommisionRatio").removeClass("disabledIput");

                $("#ctl00_contentHolder_txtNeigouCommisionMoney").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtNeigouCommisionMoney").addClass("disabledIput");
            }
            if ($("#ctl00_contentHolder_RadioNeigouB").attr("checked") == "checked") {
                $("#ctl00_contentHolder_txtNeigouCommisionMoney").removeAttr("disabled");
                $("#ctl00_contentHolder_txtNeigouCommisionMoney").removeClass("disabledIput");

                $("#ctl00_contentHolder_txtNeigouCommisionRatio").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtNeigouCommisionRatio").addClass("disabledIput");
            }

            //一口价佣金选中控制
            $("#ctl00_contentHolder_RadioSaleA").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioSaleA").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtSaleCommisionRatio").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtSaleCommisionRatio").removeClass("disabledIput");
                    $("#ctl00_contentHolder_txtSaleCommisionRatio").focus();

                    $("#ctl00_contentHolder_txtSaleCommisionMoney").attr("disabled", "disabled");
                    $("#ctl00_contentHolder_txtSaleCommisionMoney").addClass("disabledIput");
                }
            });

            $("#ctl00_contentHolder_RadioSaleB").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioSaleB").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtSaleCommisionMoney").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtSaleCommisionMoney").removeClass("disabledIput");
                    $("#ctl00_contentHolder_txtSaleCommisionMoney").focus();

                    $("#ctl00_contentHolder_txtSaleCommisionRatio").attr("disabled", "disabled");
                    $("#ctl00_contentHolder_txtSaleCommisionRatio").addClass("disabledIput");
                }
            });

            //内购佣金选中控制
            $("#ctl00_contentHolder_RadioNeigouA").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioNeigouA").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtNeigouCommisionRatio").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtNeigouCommisionRatio").removeClass("disabledIput");
                    $("#ctl00_contentHolder_txtNeigouCommisionRatio").focus();

                    $("#ctl00_contentHolder_txtNeigouCommisionMoney").attr("disabled", "disabled");
                    $("#ctl00_contentHolder_txtNeigouCommisionMoney").addClass("disabledIput");
                }
            });

            $("#ctl00_contentHolder_RadioNeigouB").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioNeigouB").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtNeigouCommisionMoney").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtNeigouCommisionMoney").removeClass("disabledIput");
                    $("#ctl00_contentHolder_txtNeigouCommisionMoney").focus();

                    $("#ctl00_contentHolder_txtNeigouCommisionRatio").attr("disabled", "disabled");
                    $("#ctl00_contentHolder_txtNeigouCommisionRatio").addClass("disabledIput");
                }
            });

        });
    </script>
    <script type="text/javascript" src="attributes.helper.js"></script>
    <script type="text/javascript" src="grade.price.helper.js"></script>
    <script type="text/javascript" src="producttag.helper.js"></script>
</asp:Content>
