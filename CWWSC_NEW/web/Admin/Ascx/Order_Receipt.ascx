<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_Receipt.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Order_Receipt" %>

<h1>发票信息</h1>
        <div id="divelectron">发票类型：电子发票
            <ol style="overflow:hidden;">
                <li><span style="width:500px;display:inline-block; line-height:30px; margin-left:30px;">
                    <asp:RadioButtonList ID="radioReceiptType" runat="server" Enabled="false">
                        <asp:ListItem Value="0" Text="个人"></asp:ListItem>
                        <asp:ListItem Value="1" Text="单位"></asp:ListItem>
                    </asp:RadioButtonList>
                <%--<asp:Label ID="labelType1" runat="server"></asp:Label>--%></span></li>
                <li><span style="width:500px;display:inline-block; line-height:30px; margin-left:30px;"><asp:Label ID="lableCompanyName" runat="server"></asp:Label></span></li>
                <li><span style="width:500px;display:inline-block; line-height:30px; margin-left:30px;"><asp:Label ID="LabelTaxesCode" runat="server"></asp:Label></span></li>
            </ol>
        </div>
        <div id="divreceipt">发票类型：增值税发票
        <div class="Settlement">
            <h4>提示：<span style="color:red">证书可以鼠标反键下载！</span></h4>
            <ul style="overflow:hidden;">
                <li style="float:left">税务登记证：<span><asp:Image ID="RegistrationImg" runat="server"   /></span></li>
                <li style="float:left">专票授权委托书：<span><asp:Image ID="EmpowerEntrustImg" runat="server"  /></span></li>
                <li style="float:left">一般纳税人证明：<span><asp:Image ID="TaxpayerProveImg" runat="server"   /></span></li>
            </ul>   

            <ol style="overflow:hidden;">
                <li><span style="width:500px;display:inline-block; line-height:30px; margin-left:30px;">公司名称：<asp:Label ID="Name" runat="server"></asp:Label></span></li>
                <li><span style="width:500px;display:inline-block; line-height:30px; margin-left:30px;">公司电话：<asp:Label ID="phone" runat="server"></asp:Label></span></li>
                <li><span style="width:500px;display:inline-block; line-height:30px; margin-left:30px;">公司开户行：<asp:Label ID="bank" runat="server"></asp:Label></span></li>
                <li><span style="width:500px;display:inline-block; line-height:30px; margin-left:30px;">开户行账号：<asp:Label ID="banknum" runat="server"></asp:Label></span></li>
                <li><span style="width:500px;display:inline-block; line-height:30px; margin-left:30px;">公司地址：<asp:Label ID="address" runat="server"></asp:Label></span></li>
                <li><span style="width:500px;display:inline-block; line-height:30px; margin-left:30px;">纳税识别号：<asp:Label ID="TaxesCode" runat="server"></asp:Label></span></li>
            </ol>
        </div>
    </div>
    <asp:HiddenField ID="hiReceiptType" runat="server" />


    <script type="text/javascript">
        $(function () {
            if ($("#ctl00_contentHolder_Receipt_hiReceiptType").val() != "" && $("#ctl00_contentHolder_Receipt_hiReceiptType").val() != "0")
                $("#divelectron").hide();
            else
                $("#divreceipt").hide();

            //var orderId = $('#OrderId').val();
            //if (orderId) {
            //    var expressData = getExpressData(orderId);

            //    var html = '<table>';
            //    var data = expressData.data;
            //    if (expressData.message != "ok")
            //        html += '<tr><td>该单号暂无物流进展，请稍后再试，或检查公司和单号是否有误。</td></tr>';
            //    else {
            //        for (var i = 0; i < data.length; i++) {
            //            html += '<tr><td>' + data[i].time + '</td>\
            //                 <td>' + data[i].context + '</td>';
            //            html += '</tr>';
            //        }
            //    }
            //    html += '<tr><td><a href="http://www.kuaidi100.com" target="_blank" id="power" runat="server" visible="false" style="color:Red;">此物流信息由快递100提供</a></td></tr>';

            //    html += '</table>';
            //    $('#expressInfo').html(html);
            //}
        });

        //function getExpressData(orderId) {
        //    var url = '/API/VshopProcess.ashx';
        //    var expressData;
        //    $.ajax({
        //        type: "get",
        //        url: url,
        //        data: { action: 'Logistic', orderId: orderId },
        //        dataType: "json",
        //        async: false,
        //        success: function (data) {
        //            expressData = data;
        //        }
        //    });
        //    return expressData;
        //}
    </script>
    </asp:Panel>