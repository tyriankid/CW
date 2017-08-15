<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <span style="color:red">此地址是测试微平台接收港口系统提交来的数据使用</span><br /><br />
    <form action="API/skycheckorder.ashx" method="post">
        状态 ：<input type="text" name="status" value="3"/>说明：2配仓，3发货，5，确认收货，只有3或5状态才会改订单状态<br />
        数量：<input type="text" name="sendnum" value="1"/><br />
        订单Id:<input type="text" name="orderid" value="24"/>说明：不是订单编号（不是类似201608253431279值）<br />
        商品内码：<input type="text" name="goodssn" value="63212"/>说明：现在是因为连接的是港口测试接口只有一个商品编码可以传输<br />
        运单号：<input type="text" name="shipno" value="551032185679"/><br />
    	<input type="submit"  value="提交测试"/>
    </form>


    <%--<span style="color:red">此地址是测试微平台接收酷开系统提交来的数据使用</span><br /><br />
    <form action="API/kukaiorderdelivergoods.ashx" method="post">
        OrderNo ：<input type="text" name="OrderNo" value="201703029898411"/><br />
        DelivOrderCode：<input type="text" name="DelivOrderCode" value="612644029472"/><br />
    	<input type="submit"  value="提交测试"/>
    </form>--%>

</body>
</html>
