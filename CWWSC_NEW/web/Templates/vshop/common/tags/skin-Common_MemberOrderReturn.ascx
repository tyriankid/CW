<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<style>

    .my-apply .my-apply-content img{position: absolute;width: 60px;height: 60px;top: 27px;}
</style>
<script language="c#" runat="server">
    public string getShowText(string itemsStatus, string productSource, string productId, string orderId, string takeGoodsDate, string kukaiCode)
    {
        string strResult = "<button class=\"btn btn-danger\" >已申请退货</button>";
        switch (int.Parse(itemsStatus))
        {
            case (int)Hidistro.Entities.Orders.OrderStatus.BuyerAlreadyPaid:
                //if (productSource == "1" || !string.IsNullOrEmpty(kukaiCode))
                    strResult = "<button class=\"btn btn-danger\" onclick=\"urllink(" + productId + "," + orderId + ")\">申请退款</button>";
                //else
                    //strResult = "<button class=\"btn btn-danger\">已配舱</button>";
                break;
            case (int)Hidistro.Entities.Orders.OrderStatus.SellerAlreadySent:
                strResult = "<button class=\"btn btn-danger\" onclick=\"urllink(" + productId + "," + orderId + ")\">申请退货</button>";
                break;
            case (int)Hidistro.Entities.Orders.OrderStatus.ConfirmTakeGoods:
                strResult = "<button class=\"btn btn-danger\">已收货</button>"; 
                //验证用户确认收货后N天才允许退货，验证是否在允许退货的时间内
                DateTime confirmDate = DateTime.MinValue;
                if (DateTime.TryParse(takeGoodsDate, out confirmDate))
                {
                    Hidistro.Core.Entities.SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                    if (DateTime.Now <= confirmDate.AddDays(masterSettings.TakeGoodsDays))
                    {
                        strResult = "<button class=\"btn btn-danger\" onclick=\"urllink(" + productId + "," + orderId + ")\">申请退货</button>";
                    }
                }
                break;    
            case (int)Hidistro.Entities.Orders.OrderStatus.ApplyForRefund:
                strResult = "<button class=\"btn btn-danger\" >已申请退款</button>";
                break;
            case (int)Hidistro.Entities.Orders.OrderStatus.Refunded:
                strResult = "<button class=\"btn btn-danger\" >已退款</button>";
                break;
            case (int)Hidistro.Entities.Orders.OrderStatus.Returned:
                strResult = "<button class=\"btn btn-danger\" >已退货</button>";
                break;
        }
        return strResult;
    }
</script>

<div class="my-apply well" <asp:Literal ID="litStyle" runat="server"></asp:Literal> >
    <div class="title">
        订单编号：<em>
            <%# Eval("OrderId")%></em> <span>¥<%# Eval("OrderTotal","{0:F2}")%></span></div>
    <div class="my-apply-content">
        
            <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# Eval("OrderItems") %>'>
                <ItemTemplate>
                    <Hi:ListImage runat="server" DataField="ThumbnailsUrl" />
                    <div class="info">
                        <p>
                            <%# Eval("ItemDescription")%></p>
                        <p>
                            <span class="specification" style="float: left; ">      
                                  <input type="hidden" value="<%# Eval("SkuContent")%>" />
                            </span>
                        </p>
                        <p>
                              
                            数量：<i><%# Eval("Quantity")%></i> 
                                <%# getShowText(Eval("OrderItemsStatus").ToString(),Eval("ProductSource").ToString(), Eval("ProductId").ToString(), Eval("OrderId").ToString(),Eval("TakeGoodsDate").ToString(),Eval("KukaiCode").ToString()) %>    
                                 
                                <%--<%# int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.BuyerAlreadyPaid ? (Eval("ProductSource").ToString() == "1" ? "<button class=\"btn btn-danger\" onclick=\"urllink(" + Eval("ProductId") + "," + Eval("OrderId") + ")\">申请退款</button>" : "<button class=\"btn btn-danger\">已配舱</button>") : (int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.SellerAlreadySent || int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.ConfirmTakeGoods) ? "<button class=\"btn btn-danger\" onclick=\"urllink(" + Eval("ProductId") + "," + Eval("OrderId") + ")\">申请退货</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.ApplyForRefund ? "<button class=\"btn btn-danger\" >已申请退款</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.Refunded ? "<button class=\"btn btn-danger\" >已退款</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.Returned ? "<button class=\"btn btn-danger\" >已退货</button>" : "<button class=\"btn btn-danger\" >已申请退货</button>"%>        --%>
                        </p>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        
        <Hi:OrderStatusLabel ID="OrderStatusLabel1" Visible="false" OrderStatusCode='<%# Eval("OrderStatus") %>'
            runat="server" />
    </div>
    </div>

    <script language="javascript">
        function urllink(productid, orderid) {
            location.href = "RequestReturn.aspx?orderId=" + orderid + "&ProductId=" + productid + "";
        }
        $(function () {
            var skuInputs = $('.specification input');
            $.each(skuInputs, function (j, input) {
                var text = '';
                var sku = $(input).val().split(';');
                var changsku = '';
                for (var i = sku.length - 2; i >= 0; i--) {
                    changsku += sku[i] + ';';
                }
                $.each(changsku.split(';'), function (i, sku) {
                    if ($.trim(sku))
                        text += '<span class="property" style="color:black;">' + sku.split('：')[1] + '</span>';
                });
                $(input).parent().html(text);


            });

        });
    </script>