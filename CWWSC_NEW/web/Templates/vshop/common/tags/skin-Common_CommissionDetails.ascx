<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
        <tr >
          <td class="jg">订单号：</td>
          <td class="dd"><%# Eval("OrderId") %></td>
          <td class="yj">佣金金额(元)</td>
        </tr>
        <tr class="xian">
            <td class="jg">      
                店铺信息：
            </td>
            <td>
               <%# Eval("StoreName").ToString()==""?Eval("storeName2").ToString(): Eval ("StoreName").ToString()%>
            </td>
            <td class="yj">
                <span class="money">￥<%#Eval("CommTotal","{0:F2}" )%> </span>
            </td>
        </tr>
