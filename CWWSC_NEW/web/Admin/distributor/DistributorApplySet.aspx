<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="DistributorApplySet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.DistributorApplySet" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title ">
                <em>
                    <img src="../images/01.gif" width="32" height="32" /></em>
                <h1>
                    门店设置</h1>
                <span>管理员可为门店设置提现金额和介绍。</span>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_110">门店认证入口：</span>
                        <input id="radiorequeston" type="radio" name="radiorequest" runat="server" value="1" />开启
                        <input id="radiorequestoff" type="radio" name="radiorequest" runat="server" value="2" />关闭
                    </li>
                    <%--<li style="display: none" id="li_requestmoney"><span class="formitemtitle Pw_110">门店申请条件：</span>
                        <span>累计消费金额必须达到</span><input type="text" id="txtrequestmoney" class="forminput"
                            style="width: 100px" runat="server" />元（付款为准） </li>--%>
                    <li><span class="formitemtitle Pw_110">门店提现设置：</span>
                        <asp:TextBox ID="txtApplySet" runat="server" CssClass="forminput" />
                        <p id="ctl00_contentHolder_txtApplySetTip" style="padding-left: 20px;">
                            输入整数大于0，并不能为空(门店提现必须大于等于设置金额)</p>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_110">店铺信息配置：</span>
                        <input id="radioOpenStoreInfoSetOn" type="radio" name="OpenStoreInfo" runat="server" value="1"/>开启
                        <input id="radioOpenStoreInfoSetOff" type="radio" name="OpenStoreInfo" runat="server" value="2"/>关闭（默认为开启，关闭后，会员认证门店后不需要填写店铺信息并且不需要上架任何商品便可快速开启门店）
                    </li>
                    <li><span class="formitemtitle Pw_110">店铺商品同步：</span>
                        <input id="radioOpenStoreProducAutoOn" type="radio" name="OpenStoreProducAuto" runat="server" value="1" />开启
                        <input id="radioOpenStoreProducAutoOff" type="radio" name="OpenStoreProducAuto" runat="server" value="0" />关闭（开启后，所有门店的店铺商品自动与后台商品同步)
                    </li>
                    <li style="display: none" id="li_OpenAgentProducRangeO"><span class="formitemtitle Pw_110">商品上架范围：</span>
                        <input id="radioOpenAgentProducRangeOn" type="radio" name="radioOpenAgentProducRangeOn" runat="server" value="1" />开启
                        <input id="radioOpenAgentProducRangeOff" type="radio" name="radioOpenAgentProducRangeOn" runat="server" value="0" />关闭（开启后，可设置门店商上架商品的分类、品牌、类型)
                    </li>
                    <li style="display:none"><span class="formitemtitle Pw_110">开启佣金：</span>
                      <input id="radioCommissionon" type="radio" name="radioCommission" runat="server" value="1" />开启
                        <input id="radioCommissionoff" type="radio" name="radioCommission" runat="server" value="2" />关闭（开启后销售额部分将计入佣金)
                    </li>

                    <%--<li><span class="formitemtitle Pw_110">门店特殊优惠方式：</span>
                        <input id="radioDistributorCutDefault" type="radio" name="radioDistributorCut" runat="server" value="1" />默认
                        <input id="radioDistributorCutByCostPrice" type="radio" name="radioDistributorCut" runat="server" value="2" />按成本价
                        <input type="checkbox" id="chkByRate"/>按百分比 
                        <input id="textDistributorCutByRate" type="text" name="textDistributorCutByRate" runat="server" style="width:20px" />%（设置门店购买自己店铺的商品时的特殊优惠方式。 80%即为八折）
                    </li>--%>

                    <li><span class="formitemtitle Pw_110">佣金计算方式：</span>
                      <input id="radioProfitoff" type="radio" name="radioProfit" runat="server" value="1" />按销售价
                        <input id="radioProfit" type="radio" name="radioProfit" runat="server" value="2" />按利润
                    </li>

                    <li><span class="formitemtitle Pw_110">门店升级方式：</span>
                      <input id="radioUpgradeComm" type="radio" name="radioUpgrade" runat="server" value="byComm" />按佣金
                        <input id="radioUpgradeOrdersTotal" type="radio" name="radioUpgrade" runat="server" value="byOrdersTotal" />按销售额
                    </li>

                    <%--<li><span class="formitemtitle Pw_110">门店招募细则：</span>
                        <abbr class="formselect">
                            <Kindeditor:KindeditorControl ID="ApplicationDescription" runat="server" Width="630px"
                                Height="400px" />
                        </abbr>
                        <p style="padding-left: 20px;">
                            用户在申请门店前需要阅读的门店招募细则。</p>
                    </li>
                    <li><span class="formitemtitle Pw_110">门店攻略设置：</span>
                        <abbr class="formselect">
                            <Kindeditor:KindeditorControl ID="fkContent" ImportLib="false" runat="server" Width="630px"
                                Height="400px" />
                        </abbr>
                        <p style="padding-left: 20px;">
                            用户在申请成为门店时需阅读的销售攻略。
                        </p>
                    </li>--%>
                </ul>
                <ul class="btn Pa_100 clearfix">
                    <asp:Button ID="btnSave" runat="server" OnClientClick="return PageIsValid();" OnClick="btnSave_Click"
                        Text="保存" CssClass="submit_DAqueding float" />
                </ul>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(function () {
            InitValidators();
            $("input[type='radio'][name='ctl00$contentHolder$radiorequest']").bind("click", function () {
                if ($(this).is(":checked") && $(this).val() == "1") {
                    $("#li_requestmoney").css({ display: 'block' });
                } else {
                    $("#li_requestmoney").css({ display: 'none' });
                }
            });
            if ($("#ctl00_contentHolder_radiorequeston").is(":checked")) {
                $("#li_requestmoney").css({ display: 'block' });
            }

            $("input[type='radio'][name='ctl00$contentHolder$OpenStoreProducAuto']").bind("click", function () {
                if ($(this).is(":checked") && $(this).val() == "1") {
                    $("#li_OpenAgentProducRangeO").css({ display: 'none' });
                } else {
                    $("#li_OpenAgentProducRangeO").css({ display: 'block' });
                }
            });
            if (!$("#ctl00_contentHolder_radioOpenStoreProducAutoOn").is(":checked")) {
                $("#li_OpenAgentProducRangeO").css({ display: 'block' });
            }
            //门店特殊优惠的radio和checkbox的点击互相屏蔽逻辑
            $("#chkByRate").bind("click", function () {
                if ($("#chkByRate").prop('checked')) {
                    $("input[type='radio'][name='ctl00$contentHolder$radioDistributorCut']").attr("checked", false);
                    $("#ctl00_contentHolder_textDistributorCutByRate").attr("disabled", false);
                }
                else {
                    $("#ctl00_contentHolder_radioDistributorCutDefault").attr("checked", true);
                    $("#ctl00_contentHolder_textDistributorCutByRate").attr("disabled", true);
                }
            });
            $("input[type='radio'][name='ctl00$contentHolder$radioDistributorCut']").bind("click", function () {
                $("#ctl00_contentHolder_textDistributorCutByRate").attr("disabled", true);
                $("#chkByRate").attr("checked", false);
                $("#ctl00_contentHolder_textDistributorCutByRate").val("");
            });
            if ($("#ctl00_contentHolder_textDistributorCutByRate").val().length == 2) {
                $("#chkByRate").attr("checked", true);
            }
            //互相屏蔽逻辑end
        });

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtApplySet', 1, 10, false, '[0-9]\\d*', '限制门店填写提现金额必须该数值的倍数，不能为空，必须是整数'));
        }


    </script>
</asp:Content>
