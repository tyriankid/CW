<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="AddProductMaintainRemind.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.AddProductMaintainRemind" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .uploadimages {
            display: inline-block !important;
            float: none!important;
            position: relative;
            left: 60px;
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
            <h1>商品提醒维护
            </h1>
            <span>对商品提醒进行编辑和修改</span>
        </div>
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                    <li>
                        <span>商品名称:</span>
                        <asp:TextBox ClientIDMode="Static" Style="height: 20px; padding: 4px 0; width: 400px" ID="product" oninput="inputchange(this)" onfocus="inputgetfocus(this)" onblur="inputlosefocus(this)" runat="server" />
                        <ul style="width:500px; position: absolute; border: 1px solid #cecaca;background: #fff; left: 30rem;display:none; margin-top: -2rem;height:750px;overflow-y:auto;" id="bcul">
                            <asp:Repeater runat="server" ID="ReProduct">
                                <ItemTemplate>
                                    <li>
                                        <Hi:ListImage ID="ListImage1" runat="server" DataField="ImageUrl1" Width="30px" Height="30px" /><span name="<%#Eval("productId") %>" onclick="selectde(this)" style="height: 1rem; line-height: 1rem; margin: 0; padding: 0.25rem 0; cursor: pointer"> <%#Eval("ProductName") %></span></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </li>
                    <li>警告周期:
                      <Hi:TrimTextBox runat="server" CssClass="forminput1" ID="RemindCycle" onkeyup="value=value.replace(/\D/g,'')" Style="height: 20px; padding: 4px 0; width: 400px;" ClientIDMode="Static" />
                    </li>
                     <li>提醒次数:
                      <Hi:TrimTextBox runat="server" CssClass="forminput1" ID="RemindNum" onkeyup="value=value.replace(/\D/g,'')" Style="height: 20px; padding: 4px 0; width: 400px;" ClientIDMode="Static" />
                    </li>
                    <li>提醒备注:
                      <Hi:TrimTextBox runat="server" CssClass="forminput1" ID="RemindRemark" TextMode="MultiLine" Rows="10" Style="width: 400px;"   ClientIDMode="Static"/>
                    </li>
                    <li>
                        <asp:Button ID="btnSave" runat="server" Text="保 存" CssClass="submit_DAqueding float"  OnClientClick="return save()" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <input type="hidden" runat="server" id="hidProduct" clientidmode="Static" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function save() {
            var flag = true;
            var msg = "";
            if ($("#RemindRemark").val() == "") {
                flag = false; msg = "提醒备注不能为空";
            }
            if ($("#RemindNum").val() == "") {
                flag = false; msg = "请填写提醒次数";
            }
            if ($("#RemindCycle").val() == "") {
                flag = false; msg = "请填写警告周期";
            }
            if ($("#hidProduct").val() == "") {
                flag = false; msg = "未选择维护商品";
            }

            if (!flag)
            {
                alert(msg);
                return false;
            }
        }

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
            //选中值
            var productId = $(e).attr("name");
            $("#hidProduct").val(productId);//将选中值赋给隐藏域  后台取值
            $("#product").val(e.innerHTML);
            document.getElementById("bcul").style.display = "none";
        }
    </script>
</asp:Content>
