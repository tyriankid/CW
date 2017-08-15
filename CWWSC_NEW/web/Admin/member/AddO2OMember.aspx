<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddO2OMember.aspx.cs" Inherits="Admin_member_AddO2OMember" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <link rel="stylesheet" href="/Admin/css/chosen.css" />
    <script src="<%= ResolveUrl("/Admin/js/chosen.jquery.min.js")%>" type="text/javascript"></script>

    <style>
        #province_select li, #city_select li, #area_select li {
            overflow: hidden;
        }
    </style>
    <div class="areacolumn clearfix" style="padding-bottom: 50px;">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/04.gif" width="32" height="32" /></em>
                <h1>添加新的录入会员</h1>
                <span>添加新的录入会员信息</span>
            </div>
            <asp:Panel ID="PanelID" runat="server">
                <div class="formtab">
                    <ul class="nav nav-tabs product_detail font-m">
                        <li class="visited">基本设置</li>
                        <li><a href="O2OexpandState.aspx?userid=<%=userId %>">扩展属性</a></li>
                       
                    </ul>
                </div>

                <div class="formitem validator4" style="margin-right:2px">
                    

                    <ul>
                        <li>
                            <span class="formitemtitle Pw_140"><em>*</em>会员姓名：</span>
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" />
                            <p id="ctl00_contentHolder_txtUserNameTip">会员姓名不能为空，只能是汉字或字母开头，长度在2-20个字符之间</p>
                        </li>
                        <li>
                            <span class="formitemtitle Pw_140"><em>*</em>会员电话：</span>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="forminput" />
                            <p id="ctl00_contentHolder_txtPhoneTip">电话号码的长度限制在20个字符以内</p>
                        </li>
                        <%--<li><span class="formitemtitle Pw_140"><em>*</em>所属门店：</span>
                            <Hi:StoreDropDownList ID="drpstroe" runat="server" AllowNull="false" />
                        </li>--%>
                        <%--<li>--%>
                            <table>
                                <tr>
                                    <td><span class="formitemtitle Pw_140"><em>*</em>所属门店：</span></td>
                                    <td><asp:DropDownList ID="ddlStores" runat="server" data-placeholder="测试" Width="400px" class="chosen-select" ></asp:DropDownList></td>
                                </tr>
                            </table>
                            <br />
                        <%--</li>--%>
                        <li><span class="formitemtitle Pw_140">职业：</span>
                            <asp:TextBox ID="txtProfession" runat="server" CssClass="forminput" />
                            <%--<p id="ctl00_contentHolder_txtProfessionTip">职业的长度限制在20个字符以内</p>--%>
                        </li>
                        <li><span class="formitemtitle Pw_140">会员等级：</span>
                            <asp:TextBox ID="MemberRankList" runat="server" CssClass="forminput" />
                           <%-- <Hi:MemberGradeDropDownList ID="drpMemberRankList" runat="server" AllowNull="false" />--%>
                        </li>
                        <li><span class="formitemtitle Pw_140">会员生日：</span>
                            <UI:WebCalendar CalendarType="StartDate" ID="timeBirthday" runat="server" CssClass="forminput" />
                        </li>
                        <li><span class="formitemtitle Pw_140">性别：</span>
                            <asp:RadioButtonList ID="rdoSex" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CellPadding="2">
                                <asp:ListItem Value="男">男</asp:ListItem>
                                <asp:ListItem Value="女">女</asp:ListItem>
                            </asp:RadioButtonList>

                        </li>
                        <li><span class="formitemtitle Pw_140">产品型号：</span>
                            <asp:TextBox ID="txtModel" runat="server" CssClass="forminput" />
                        </li>
                        <li><span class="formitemtitle Pw_140">购买时间：</span>
                            <UI:WebCalendar CalendarType="StartDate" ID="timeBuydate" runat="server" CssClass="forminput" />
                        </li>
                        <li><span class="formitemtitle Pw_140">产品类型：</span>
                            <asp:TextBox runat="server" ID="ProductTypes" CssClass="forminput"></asp:TextBox>
                            <%--  <Hi:ProductTypeDownList runat="server" CssClass="productType" ID="dropProductTypes" NullToDisplay="--请选择--" />--%>
                        </li>
                        <li><span class="formitemtitle Pw_140">产品价格：</span>
                            <asp:TextBox ID="txtprice" runat="server" CssClass="forminput" />
                        </li>
                        <li style="overflow: visible"><span class="formitemtitle Pw_140">所属区域：</span>
                            <abbr class="formselect">
                                <Hi:RegionSelector runat="server" ID="ddlReggion" />
                            </abbr>
                        </li>
                        <li><span class="formitemtitle Pw_140">详细地址：</span>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="forminput" />
                        </li>
                        <li>
                            <span class="formitemtitle Pw_140">是否使用创维净水器：</span>
                            <abbr class="formselect">
                            <Hi:YesNoRadioButtonList ID="radIsUserWaterDarifier" SelectedValue="false" runat="server" RepeatLayout="Flow" />
                        </li>
                        <%--<li><span class="formitemtitle Pw_140">是否购买创维净水器：</span>
                            <asp:CheckBoxList ID="ckIsUserWaterDarifier" runat="server">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>
                            </asp:CheckBoxList>
                        </li>--%>
                        <li>
                            <span class="formitemtitle Pw_140">净水器购买时间：</span>
                            <UI:WebCalendar CalendarType="StartDate" ID="timeBuyWaterDarifierDate" runat="server" CssClass="forminput" />
                        </li>
                        <li><span class="formitemtitle Pw_140">家庭成员构成：</span>
                            <asp:TextBox ID="txtJiatingchengyuan" runat="server" Width="400px" CssClass="forminput" />
                        </li>
                        <li><span class="formitemtitle Pw_140">用户住房信息：</span>
                            <asp:TextBox ID="txtZhufangxinxi" runat="server" Width="400px" CssClass="forminput" />
                        </li>
                        <li><span class="formitemtitle Pw_140">房屋家电配置：</span>
                            <asp:TextBox ID="txtFangyujiadian" runat="server" Width="400px" CssClass="forminput" />
                        </li>
                        <li><span class="formitemtitle Pw_140">家电使用情况：</span>
                            <asp:TextBox ID="txtJiadianshiyong" runat="server" Width="400px" CssClass="forminput" />
                        </li>
                        <li><span class="formitemtitle Pw_140">个人品牌倾向：</span>
                            <asp:TextBox ID="txtGerenqingxiang" runat="server" Width="400px" CssClass="forminput" />
                        </li>
                        <li><span class="formitemtitle Pw_140">近期购买需求：</span>
                            <asp:TextBox ID="txtJinqixuqiu" runat="server" Width="400px" CssClass="forminput" />
                        </li>
                        <li><span class="formitemtitle Pw_140">备注：</span>
                            <asp:TextBox ID="txtRemark" runat="server" Width="400px" CssClass="forminput" />
                        </li>
                        </ul>
                    <ul class="btn Pa_110 clear">
                        <asp:Button ID="btnCreate" runat="server" OnClientClick="return PageIsValid();" Text=" 保  存" CssClass="submit_DAqueding" Style="float: left;" OnClick="btnCreate_Click" />
                    </ul>
                </div>
            </asp:Panel>
        </div>
    </div>

    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        


        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtUserName', 2, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '会员姓名不能为空，只能是汉字或字母开头，长度在2-20个字符之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtPhone', 1, 20, false, null, '电话号码的长度限制在20个字符以内'));
            //initValid(new InputValidator('ctl00_contentHolder_txtProfession', 1, 20, false, null, '职业的长度限制在20个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_drpstroe', 1, 50, false, null, '请选择所属门店不能为空'));
            initValid(new InputValidator('ctl00_contentHolder_txtprice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
        }

        $(document).ready(function () {
            InitValidators();

            $(".chosen-select").chosen({
                no_results_text: "没有找到结果！",//搜索无结果时显示的提示
                search_contains: true,   //关键字模糊搜索，设置为false，则只从开头开始匹配
                allow_single_deselect: true, //是否允许取消选择
                max_selected_options: 6  //当select为多选时，最多选择个数
            });

            $(".dept-select").chosen().change(function () {
                alert("点击了我");
            });

        });

    </script>
</asp:Content>
