<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ImportFromProducts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.ImportFromProducts" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
        <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>批量导入商品</h1>
            <span>店铺中所有的商品，您可以对商品进行搜索，也能对商品进行编辑、上架、入库等操作</span>
        </div>

    <div>
        <ul>
            <li style="float:left">商品分类：</li>
            <li style="float:left; margin-right:25px;"><asp:DropDownList ID="dropCategories" runat="server" Width="200"></asp:DropDownList></li>
            <li style="float:left">商品类型：</li>
            <li style="float:left; margin-right:25px;"><asp:DropDownList ID="ProductType" runat="server" Width="200" OnSelectedIndexChanged="ProductType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></li>
            <li style="float:left">商品品牌：</li>
            <li style="float:left"><asp:DropDownList ID="dropBrandList" runat="server" Width="200"></asp:DropDownList></li>
        </ul>
        <ul style="clear:both; padding-top:10px;">
            <li style="float:left">商品导入状态：</li>
            <li style="float:left; margin-right:25px;"><asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus" Checked="true"  Text="出售中"></asp:RadioButton><asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus"  Text="仓库中"></asp:RadioButton></li>
            <li style="float:left; margin-right:25px;"><asp:FileUpload runat="server" id="ProductFile" /></li>
            <li style="float:left"><asp:Button runat="server" ID="BcBtn" Text="确定" OnClick="BcBtn_Click" /></li>
            <li style="float:left; width: 38px;"><asp:Button runat="server" ID="BcButton" Text="保存" OnClick="BcButton_Click" /></li>
            <li style="float:left; width: 38px;"><asp:Button runat="server" ID="XZ_Product" Text="下载导入商品模板" OnClick="XZ_Product_Click" /></li>
        </ul>
    </div>
            <asp:Repeater ID="repeateExcel" runat="server">
	   
             <HeaderTemplate>
            <table width="0" border="0" cellspacing="0">
                <tr class="table_title">
                    <td style="width:2%;" class="td_right">编号</td>
                    <td style="width:2%;" class="td_right">商品名称</td>
                    <td style="width:2%;" class="td_right">货号</td>
                    <td style="width:4%;" class="td_right">市场价</td>
                    <td style="width:4%;" class="td_right">一口价</td>
                    <td style="width:3%;" class="td_right">成本价 </td>
                    <td style="width:3%;" class="td_right">商品库存</td>
                    <td style="width:3%;" class="td_right">状态</td>
                </tr>
        </HeaderTemplate>

                    <ItemTemplate>
            <tr class="td_bg noboer_tr">
                <td class="td_txt_cenetr" style='background-color: <%#Eval("状态").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("编号") %></span>
                </td>
                <td class="td_txt_cenetr" style='background-color: <%#Eval("状态").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("商品名称") %></span>

                </td>
                <td class="td_txt_cenetr" style='background-color: <%#Eval("状态").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("货号") %></span>

                </td>
                <td class="td_txt_cenetr" style='background-color: <%#Eval("状态").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("市场价") %></span>

                </td>
                 <td class="td_txt_cenetr" style='background-color: <%#Eval("状态").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("一口价") %></span>

                </td>
                <td class="td_txt_cenetr" style='background-color: <%#Eval("状态").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("成本价") %></span>

                </td>
                <td class="td_txt_cenetr" style='background-color: <%#Eval("状态").ToString().Count()>0?"#FB6363":" "%>;'>
                    <span><%#Eval("商品库存") %></span>

                </td>
                <td class="td_txt_cenetr"  style='background-color: <%#Eval("状态").ToString().Count()>0?"#FB6363":" "%>;'>
                 <span><%#Eval("状态") %></span>
                </td>
              
            </tr>
        </ItemTemplate>
                    <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

</div>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
