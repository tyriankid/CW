<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WenBen.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.SouSuo" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='SouSuo' class='small ss'>
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
					<div class="search_div"><input class='search' id="txtKeywords" type='text' placeholder='商品搜索：请输入商品名称' oninput="inputchange(this)" onfocus="inputgetfocus(this)" onblur="inputlosefocus(this)"/>
                        <ul style="width: 94%; position: absolute; border: 1px solid #cecaca;z-index:9999;background: #fff;display:none; margin-top: -2rem;height: 276px;overflow-y:auto;" id="bcul">
                            <asp:Repeater runat="server" ID="ReUserSearch">
                                <ItemTemplate>
                                    <li style="padding-left:10px;line-height:26px;background:#e6e3e3;margin-top:1px !important;">
                                        <span onclick="selectde(this)" style="height: 1rem; line-height: 1rem; margin: 0; padding: 0.25rem 0; cursor: pointer"> <%#Eval("SearchText") %></span></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
					<button type='submit' class='searchBtn' onclick="search(this)"></button></div>
    <script>
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
            $("#txtKeywords").val(e.innerHTML);
            document.getElementById("bcul").style.display = "none";
        }
    </script>
</div>