<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberIndividuality.aspx.cs" Inherits="Hidistro.UI.Web.Admin.MemberIndividuality"  MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" Runat="Server">
<style>
    .Pa_198{
        margin:15px 0;
    }
    .Pa_198 input{
        display:inline-block !important;
        padding:5px 10px;
        color:#fff;
        border-radius:3px;
        outline:none;
        border:none;
    }
    .Pa_198 input:first-child{
        background:#2196F3;
    }
    .Pa_198 input:last-child{
        background:#F44336;
    }

    .searchbutton {
        height: 25px;
        line-height: 25px;
        background: #F90;
    }
</style>
<div class="areacolumn clearfix">
    <div class="columnright">
        <div class="title">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1>编辑会员个性标签</h1>
            <span>编辑会员个性标签，可添加、修改、删除、新增。</span>
        </div>
        <p>提示：标签名称和标签内容是必填项，若为空是点击保存无效，请注意！</p>

        <div class="formitem validator4 clearfix">
            <table id="tableMembersTag">
                <asp:Repeater id="ShowIndividuality" runat="server">
                    <ItemTemplate>
                        <tr id='<%#Eval("paixu") %>' style="height:40px">
                            <td><span>标签名称：</span></td>
                            <td><input type="text" class="forminput" value="<%#Eval("tagName") %>" /></td>
                            <td><span>&nbsp;&nbsp;标签内容：</span></td>
                            <td><input type="text" value= "<%#Eval("tagValue") %>" class="forminput" /></td>
                            <td>&nbsp;&nbsp;<span><input onclick="javascript:deleterow(<%#Eval("paixu") %>);" class="searchbutton" type="button" value="删除" /></span></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <ul class="btn Pa_198">
                <input type="button" value="添加新的标签" CssClass="submit_DAqueding" id="AddRow" />
                <asp:HiddenField ID="HiddenFieldSave" runat="server" Value="" ClientIDMode="Static" />
                <asp:Button Text="保存" runat="server" OnClientClick="return SaveTag()" ID="btnEditUser" />    
            </ul>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
    <script type="text/javascript" language="javascript">
        var a = 0;
        
        $(function () {
            $("#AddRow").click(function () {
                if (a == 0 && $("#tableMembersTag tr") != null && $("#tableMembersTag tr").length > 0) {
                    a = $("#tableMembersTag tr").length;
                }
                a++;
                $("#tableMembersTag").append("<tr id=" + a + " style=\"height:40px\"><td> <span>标签名称：</span></td><td><input type=\"text\" class=\"forminput\" /></td><td><span>&nbsp;&nbsp;标签内容：</span></td><td><input type=\"text\" class=\"forminput\"  /></td><td>&nbsp;&nbsp;<span><input onclick=\"javascript:deleterow(" + a + ");\" type=\"button\" class=\"searchbutton\" value=\"删除\" /></span></td></tr>")

                /*
                if ($("#tableMembersTag tr td  span").last().children().attr("id") == null) {
                    var a = 0;
                }
                else {
                    var a = $("#tableMembersTag tr td  span").last().children().attr("id");
                }
                var b = a * 1 + 1;
                if ($("#tableMembersTag tr").text() == "") {
                    $("#tableMembersTag").append("<tr><td style=display:block> <span>标签名称：</span><input type=text  /><span>&nbsp;&nbsp;标签内容：</span><input type=text  />&nbsp;&nbsp;<span><input id=" + b + "  onclick='javascript:deleterow(" + b + ");' type=button value=删除 /></span></td></tr>")
                }
                else {
                    $("#tableMembersTag tr").append("<td style=display:block> <span>标签名称：</span><input type=text  /><span>&nbsp;&nbsp;标签内容：</span><input type=text  />&nbsp;&nbsp;<span><input id=" + b + "  onclick='javascript:deleterow(" + b + ");' type=button value=删除 /></span></td>")
                }*/
            });
        })
        function deleterow(id) {
            $("[id=" + id + "]").remove();
        }
        function SaveTag() {
            var valuesd = "";
            $("#tableMembersTag").find("tr").each(function (i, e) {
                if($(this).find("input").length == 3){
                    valuesd += $(this).find("input").eq(0).val()  + "," + $(this).find("input").eq(1).val() + "/";
                }
            });
            $("#HiddenFieldSave").val(valuesd);
            if( $("#HiddenFieldSave").val()==null)
            {
                return false;
            }
            else{
                return true;
            }
        }
    </script>
</asp:Content>
