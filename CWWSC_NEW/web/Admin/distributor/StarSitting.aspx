<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StarSitting.aspx.cs" Inherits="Admin_distributor_StarSitting" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .formitemtitle, .forminput {
            /*float: left;*/
            float: inherit;
        }

        .Pw_110 {
            position: relative;
            top: -32px;
        }

        .validator2 li p.msgError {
             width:250px;
            margin: 5px 0px 0px 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title ">
                <em>
                    <img src="../images/01.gif" width="32" height="32" /></em>
                <h1>纬度权重设置</h1>
                <span>管理员可为星级纬度权重,并计算部门星级。</span>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li>
                        <p>
                            注意事项：1.六个维度的权重分值和满分标准都是必填项；
                                      2.权重分值的总和不能大于十分；
                                      3.维度实际得分计算方法（实际数据/满分标准*权重分值=实际得分）
                                      4.星级评分计算方式【六项维度的实际得分总和】

                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_110"><em>*</em>门店评价：</span>
                        <div style="display: inline-block;">
                            <%--onkeyup="value=value.replace(/\D/g,'')"--%>
                            <Hi:TrimTextBox ID="txtstorecommon" runat="server" CssClass="forminput" placeholder="权重分值" />
                            <p id="ctl00_contentHolder_txtstorecommonTip">门店评价权重分值不能为空,且为正整数</p>
                        </div>
                        <div style="display: inline-block; margin-left: 4rem;">
                            <Hi:TrimTextBox ID="txtstandardStorecommon" runat="server" CssClass="forminput" placeholder="满分标准" />
                            <p id="ctl00_contentHolder_txtstandardStorecommonTip">门店评价满分标准不能为空,且为正整数</p>
                        </div>
                    </li>
                    <li><span class="formitemtitle Pw_110"><em>*</em>在线销售：</span>
                        <div style="display: inline-block;">

                            <Hi:TrimTextBox ID="txtonlinesale" runat="server" CssClass="forminput" placeholder="权重分值" />
                            <p id="ctl00_contentHolder_txtonlinesaleTip">在线销售权重分值不能为空,且为正整数</p>
                        </div>
                        <div style="display: inline-block; margin-left: 4rem;">
                            <Hi:TrimTextBox ID="txtstandardOnlinesale" runat="server" CssClass="forminput" placeholder="满分标准" />
                            <p id="ctl00_contentHolder_txtstandardOnlinesaleTip">在线销售满分标准不能为空,且为正整数</p>
                        </div>
                    </li>
                    <li>
                        <span class="formitemtitle Pw_110"><em>*</em>金立销售：</span>
                        <div style="display: inline-block;">
                            <Hi:TrimTextBox ID="txtjinlisale" runat="server" CssClass="forminput" placeholder="权重分值" />
                            <p id="ctl00_contentHolder_txtjinlisaleTip">金立销售权重分值不能为空,且为正整数</p>
                        </div>
                        <div style="display: inline-block; margin-left: 4rem;">
                            <Hi:TrimTextBox ID="txtstandardJinlisale" runat="server" CssClass="forminput" placeholder="满分标准" />
                            <p id="ctl00_contentHolder_txtstandardJinlisaleTip">金立销售满分标准不能为空,且为正整数</p>
                        </div>
                    </li>
                    <li><span class="formitemtitle Pw_110"><em>*</em>用户数量：</span>
                        <div style="display: inline-block;">
                            <Hi:TrimTextBox ID="txtmembernum" runat="server" CssClass="forminput" placeholder="权重分值" />
                            <p id="ctl00_contentHolder_txtmembernumTip">用户数量权重分值不能为空,且为正整数</p>
                        </div>
                        <div style="display: inline-block; margin-left: 4rem;">
                            <Hi:TrimTextBox ID="txtstandardMembernum" runat="server" CssClass="forminput" placeholder="满分标准" />
                            <p id="ctl00_contentHolder_txtstandardMembernumTip">用户数量满分标准不能为空,且为正整数</p>
                        </div>
                    </li>
                    <li><span class="formitemtitle Pw_110"><em>*</em>粘性会员数：</span>
                        <div style="display: inline-block;">
                            <Hi:TrimTextBox ID="txtnxmembernum" runat="server" CssClass="forminput" placeholder="权重分值" />
                            <p id="ctl00_contentHolder_txtnxmembernumTip">粘性会员数权重分值不能为空,且为正整数</p>
                        </div>
                        <div style="display: inline-block; margin-left: 4rem;">
                            <Hi:TrimTextBox ID="txtstandardNXmembernum" runat="server" CssClass="forminput" placeholder="满分标准" />
                            <p id="ctl00_contentHolder_txtstandardNXmembernumTip">粘性会员数满分标准不能为空,且为正整数</p>
                        </div>
                    </li>
                    <li><span class="formitemtitle Pw_110"><em>*</em>服务订单数：</span>
                        <div style="display: inline-block;">
                            <Hi:TrimTextBox ID="txtordernum" runat="server" CssClass="forminput" placeholder="权重分值" />
                            <p id="ctl00_contentHolder_txtordernumTip">服务订单数权重分值不能为空,且为正整数</p>
                        </div>
                        <div style="display: inline-block; margin-left: 4rem;">
                            <Hi:TrimTextBox ID="txtstandardOrdernum" runat="server" CssClass="forminput" placeholder="满分标准" />
                            <p id="ctl00_contentHolder_txtstandardOrdernumTip">服务订单数满分标准不能为空,且为正整数</p>
                        </div>
                    </li>

                </ul>

                <ul class="btn Pa_100 clearfix">
                    <asp:Button ID="btnSave" runat="server" OnClientClick="return save()" Text="保存" CssClass="submit_DAqueding float" OnClick="btnSave_Click" />
                    <asp:Button ID="btnScore" runat="server" Text="门店评分" CssClass="submit_DAqueding float" BackColor="Red" />
                </ul>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function save() {
            if (PageIsValid()) {
                var info = parseFloat($("#ctl00_contentHolder_txtstorecommon").val());
                var info1 =parseFloat($("#ctl00_contentHolder_txtonlinesale").val());
                var info2 =parseFloat($("#ctl00_contentHolder_txtjinlisale").val());
                var info3 =parseFloat($("#ctl00_contentHolder_txtmembernum").val());
                var info4 = parseFloat($("#ctl00_contentHolder_txtnxmembernum").val());
                var info5 =parseFloat( $("#ctl00_contentHolder_txtordernum").val());
                if (info + info1 + info2 + info3 + info4 + info5 !=100) {
                    alert("权重分值综合为100，目前总和已超出十分或不足一百分")
                    return false;
                } else {
                    $("#IcoUpImg_uploadedImageUrlTip").attr("class", "").html("");
                }
            } else {
                return false;
            }
        }
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtstorecommon', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtstorecommon', 0, 100, '门店评价权重分值必须在0-100之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtstandardStorecommon', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数

            initValid(new InputValidator('ctl00_contentHolder_txtonlinesale', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtonlinesale', 0, 100, '在线销售权重分值必须在0-100之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtstandardOnlinesale', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数

            initValid(new InputValidator('ctl00_contentHolder_txtjinlisale', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtjinlisale', 0, 100, '金立销售权重分值必须在0-100之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtstandardJinlisale', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数

            initValid(new InputValidator('ctl00_contentHolder_txtmembernum', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtmembernum', 0, 100, '用户数量权重分值必须在0-100之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtstandardMembernum', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数

            initValid(new InputValidator('ctl00_contentHolder_txtnxmembernum', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtnxmembernum', 0, 100, '粘性会员数重分值必须在0-100之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtstandardNXmembernum', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数

            initValid(new InputValidator('ctl00_contentHolder_txtordernum', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtordernum', 0, 100, '服务订单数权重分值必须在0-100之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtstandardOrdernum', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数

        }
        $(document).ready(function () {
            InitValidators();
        });
    </script>
</asp:Content>
