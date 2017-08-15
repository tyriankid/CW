<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<script>
    $(function () {
        if ($("img").length > 0) {
            if ($("img").eq(0).height() > 309) {
                $("img").height(309);
            }
            if ($("img").eq(0).width() > 309) {
                $("img").width(309);
            }
        }
    });

</script>
<a href="<%# Globals.ApplicationPath +(Eval("ProductSource").ToString()=="3"?"/Vshop/ProductServiceDetails.aspx?ProductId=":"/Vshop/ProductDetails.aspx?ProductId=") + Eval("ProductId") %>">
            <div>
                <div  style="text-align:center">
                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl310"/>
                    <div class="info" style="text-align:left">
                        <div class="name bcolor"><%# Eval("ProductName") %></div>
                        <div class="price font-s text-danger">
                            ¥<%# Eval("SalePrice", "{0:F2}") %> <del class="text-muted font-xs">¥<%# Eval("MarketPrice", "{0:F2}") %> </del>
                        </div>
                        <div class="sales text-muted font-xs">已售<b><%# Eval("SaleCounts")%></b>件</div>
                    </div>
                </div>
            </div>
        </a>


