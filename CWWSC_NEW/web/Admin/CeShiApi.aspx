<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CeShiApi.aspx.cs" Inherits="Admin_CeShiApi" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" Text="提交商品" OnClick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" Text="发送订单到" OnClick="Button2_Click" />
        <asp:Button ID="Button3" runat="server" Text="订单发货接口" OnClick="Button3_Click" />
        <asp:Button ID="Button4" runat="server" Text="订单完成接口" OnClick="Button4_Click" />
        <asp:Button ID="Button5" runat="server" Text="订单退货接口" OnClick="Button5_Click" />
        <asp:Button ID="Button6" runat="server" Text="发票接口" OnClick="Button6_Click" />
    </div>
    </form>
</body>
</html>
