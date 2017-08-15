<%@ Page Language="C#" AutoEventWireup="true" CodeFile="userHotSearch.aspx.cs" Inherits="Admin_member_userHotSearch" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title  m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>用户搜索热词配置</h1>
        </div>
        <div class="datafrom">
            <div class="formitem validator1">
                <ul id="clearfix">
                    <li>
                        <h2 class="colorE">热搜词</h2> <span><input type="button" id="btnFamily" value="新增" class="searchbutton" onclick="add()" /></span>
                    </li>
                     <asp:Literal runat="server" ID="txtSearch"></asp:Literal> 
                </ul>
                <ul class="btntf Pa_198 clear">
                    <li>  <input id="btnCreate"  type="button" value="保存"  onclick="Save()" class="submit_DAqueding" style="float: left;" /></li>
                  
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        var rowCount = 0;
        function add() {
            rowCount++;
            var strText = "";
            strText += "<li id='searchText" + rowCount + "'><input type='text' class='forminput formwidth'  /><a href=\"javascript:delRow('" + rowCount + "')\">删除</a></li>";
            $("#clearfix").append(strText);
        }
        //删除行  
        function delRow(rowIndex) {
            $("#searchText" + rowIndex).remove();
            rowCount--;
        }
        function Save() {
             var text = "";
             $("#clearfix").find("[type='text']").each(function () {
                text+=$(this).val()+"&";
            })
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "userHotSearch.aspx/Save",
                data: "{text:'" + text + "'}",
                dataType: 'json',
                success: function (result) {
                    alert(result.d)
                }
            });
        }
    </script>
</asp:Content>
