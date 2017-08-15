<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box">
    <%--<a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>" target="_blank">--%>
    <a role="chatUser" FQUserId="<%# Eval("FQUserId")%>" JSUserId="<%# Eval("JSUserId")%>" roomNum="<%# Eval("roomnum") %>" href="#">
        <img id="userhead_<%# Eval("DialogId")%>" src=""/>
        <div class="info">
            <div style="overflow: hidden;height:30px;">
                <div class="name font-l bcolor" style="float:left;" id="username_<%# Eval("DialogId")%>"></div><div class="price text-danger" style="float:right;" ><%# Convert.ToDateTime(Eval("CreateTime")).ToString("MM-dd")%></div>
            </div>
            <div style="overflow: hidden;height:25px;line-height: 25px;"><div class="ly" id='lastMsg_<%# Eval("DialogId")%>'><%#Eval("lastMsg") %></div></div>
        </div>
    </a>
</div>
<script>
    var fquserid = "<%# Eval("FQUserId")%>";
    var userid = "<%# Eval("currentUserid")%>";
    var notreadCount = "<%# Eval("NotReadMsgCount") %>";
    if (fquserid == userid) {
        $("#userhead_<%# Eval("DialogId")%>").attr("src","<%#Eval("JSUserHead")%>");
        $("#username_<%# Eval("DialogId")%>").html("<%#Eval("JSUserName")%>");
    }
    else { //接收者为自己
        $("#lastMsg_<%# Eval("DialogId")%>").after(parseInt(notreadCount) > 0 ? "<i style='float:right;font-style:normal;text-align:center;width:20px;height: 20px;line-height:20px;background: red;color:#ffffff;border-radius:50%;'>" + notreadCount + "</i>" : "");
        $("#userhead_<%# Eval("DialogId")%>").attr("src","<%#Eval("FQUserHead")%>");
        $("#username_<%# Eval("DialogId")%>").html("<%#Eval("FQUserName")%>");
    }
</script>