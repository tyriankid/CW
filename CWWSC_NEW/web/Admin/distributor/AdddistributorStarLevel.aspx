<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdddistributorStarLevel.aspx.cs" MasterPageFile="~/Admin/Admin.Master" Inherits="Admin_distributor_AdddistributorStarLevel" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <%--<style type="text/css">
        .uploadimages {
            display: inline-block !important;
            float: none!important;
            position: relative;
            left: 60px;
        }
        .forminput1 {
            width:150px
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">
<style type="text/css">.Pw_110{ padding-right: 5px;}.errorFocus{width:220px;}.forminput{width:220px;padding:4px 0px 4px 2px}.areacolumn .columnright .formitem li{margin-bottom:0;}</style>
    <!--列表-->    
    <div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1>门店星级</h1>
            <span>门店星级的新建或修改</span>
          </div>
        <div class="formitem validator4 clearfix">
            <ul>
                <li> 
                    <span class="formitemtitle Pw_110">星级名称：</span>
                    <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtLevelName" />
                    <p id="ctl00_contentHolder_txtLevelNameTip">&nbsp;&nbsp;星级名称名称在20个字符以内</p>
                </li>
                <li> 
                    <span class="formitemtitle Pw_110">星级等级：</span>
                    <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtLevelNum" />
                    <p id="ctl00_contentHolder_txtLevelNumTip">&nbsp;&nbsp;如：1、2、3/4、5</p>
                </li>
                <li>
                    <span class="formitemtitle Pw_110">星级最低分数：</span>
                    <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMinNum" />
                    <p id="ctl00_contentHolder_txtMinNumTip">&nbsp;&nbsp;请输入正确的整数类型</p>
                </li>
                <li>
                    <span class="formitemtitle Pw_110">星级最高分数：</span>
                    <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMaxNum" />
                    <p id="ctl00_contentHolder_txtMaxNumTip">&nbsp;&nbsp;请输入正确的整数类型</p>
                </li>
                <li>
			        <span class="formitemtitle Pw_110">上浮佣金类型：</span>
				    <asp:RadioButton runat="server" ID="RadioTypeA" GroupName="CommType"  Text="比例"></asp:RadioButton>
                    <asp:RadioButton runat="server" ID="RadioTypeB" GroupName="CommType"  Text="金额"></asp:RadioButton>
                    <p id="P1">&nbsp;&nbsp;商品佣金以外根据门店星级上浮的佣金类型</p>
 			    </li>
                <li>
                    <span class="formitemtitle Pw_110">佣金上浮比例：</span>
                    <Hi:TrimTextBox runat="server" CssClass="forminput" Enabled="false" ID="txtCommissionRise" />%
                    <p id="ctl00_contentHolder_txtCommissionRiseTip">&nbsp;&nbsp;商品佣金以外根据门店星级上浮的佣金比例</p>
                </li>
                <li>
                    <span class="formitemtitle Pw_110">佣金上浮金额：</span> <%--onkeyup="value=value.replace(/\D/g,'')"--%>
                    <Hi:TrimTextBox runat="server" CssClass="forminput" Enabled="false" ID="txtCommissionMoney" />元
                    <p id="ctl00_contentHolder_txtCommissionMoneyTip">&nbsp;&nbsp;商品佣金以外根据门店星级上浮的佣金金额</p>
                </li>
                <li>
                    <span class="formitemtitle Pw_110">星级图标：</span>
                    <div class="uploadimages">
                        <Hi:UpImg runat="server" ID="IcoUpImg" IsNeedThumbnail="false" UploadType="SharpPic"/>
                    </div>
                    <p id="IcoUpImg_uploadedImageUrlTip" style="padding-left:23px;">（建议上传PNG背景透明的图片，大小50px * 50px）</p>
                </li>
            </ul>
          <ul class="btn Pa_198">
                <asp:Button ID="btnSave" runat="server" Text="保 存" CssClass="submit_DAqueding float"  OnClick="btnSaveClientSettings_Click"   OnClientClick="return CheckForm()"/>
                <%--<asp:Button ID="btnEditUser" runat="server" Text="确 定" OnClientClick="return CheckForm()"  CssClass="submit_DAqueding" />--%>
            </ul>
          </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript"> 
        function CheckForm() {
            if (PageIsValid()) {
                var info = $("#IcoUpImg_uploadedImageUrl").val();
                if (info.length < 10) {
                    $("#IcoUpImg_uploadedImageUrlTip").attr("class", "msgError").html("请上传等级图标");
                    return false;
                } else {
                    $("#IcoUpImg_uploadedImageUrlTip").attr("class", "").html("");
                }
            } else {
                return false;
            }
        }
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtLevelName', 1, 20, false, null, '星级名称名称在20个字符以内'));

            initValid(new InputValidator('ctl00_contentHolder_txtLevelNum', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));//整数
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtLevelNum', 1, 10, '星级等级必须在1-10之间'));
            
            initValid(new InputValidator('ctl00_contentHolder_txtMinNum', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMinNum', 0, 100, '星级最低分数必须在1-100之间'));

            initValid(new InputValidator('ctl00_contentHolder_txtMaxNum', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxNum', 1, 100, '星级最高分数必须在1-100之间'));

            initValid(new InputValidator('ctl00_contentHolder_txtCommissionRise', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCommissionRise', 0, 100, '佣金上浮比例必须在0-100之间'));

            initValid(new InputValidator('ctl00_contentHolder_txtCommissionMoney', 1, 10, true, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCommissionMoney', 0, 100, '佣金上浮金额必须在0-100之间'));

        }
        $(document).ready(function () {
            InitValidators();

            $("#ctl00_contentHolder_RadioTypeA").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioTypeA").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtCommissionRise").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtCommissionMoney").attr("disabled", "disabled");
                }
            });

            $("#ctl00_contentHolder_RadioTypeB").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioTypeB").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtCommissionMoney").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtCommissionRise").attr("disabled", "disabled");
                }
            });
        });
    </script>
</asp:Content>
