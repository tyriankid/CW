<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductOnAuditing.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ProductOnAuditing" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>
                ��Ӧ���ϴ���Ʒ���</h1>
            <span>��������˹�Ӧ���ϴ�����Ʒ�����ͨ���������ǰ�����ۡ�</span>
        </div>
        <div class="datalist">
            <!--����-->
            
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li><asp:HiddenField ID="txtProductIds" runat="server" ClientIDMode="Static" /></li>
                    <li><span>��Ʒ���ƣ�</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
                    <li><span>��Ʒ���룺</span><span><asp:TextBox ID="txtSKU" Width="74" runat="server" CssClass="forminput" /></span></li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--��ѡ����Ʒ����--"
                                Width="150" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--��ѡ��Ʒ��--"
                                Width="153" />
                        </abbr>
                    </li>
                    <li style="display: none;">
                        <abbr class="formselect">
                            <Hi:ProductTagsDropDownList runat="server" ID="dropTagList" NullToDisplay="--��ѡ���ǩ--"
                                Width="153" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductTypeDownList ID="dropType" runat="server" NullToDisplay="--��ѡ������--" Width="153" />
                        </abbr>
                    </li>
                </ul>
            </div>
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span>��Ʒ��Դ��</span></li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductSourceDownList ID="dropSource" runat="server" AllowCw="false"  NullToDisplay="--��ѡ����Դ--" Width="153" />
                        </abbr>
                    </li>
                    <li><span>���ʱ�䣺</span></li>
                    <li>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">��</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>
                    <li>
                        <asp:Button ID="btnSearch" runat="server" Text="��ѯ" CssClass="searchbutton" /></li>
                </ul>
            </div>
            </div>
            <!--����-->
            <div class="functionHandleArea clearfix">
                <!--��ҳ����-->
                <div class="pageHandleArea">
                    <ul>
                        <li class="paginalNum"><span>ÿҳ��ʾ������</span><UI:PageSize runat="server" ID="hrefPageSize" />
                        </li>
                    </ul>
                </div>
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                    </div>
                </div>
                <!--����-->
                <%--<div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span><span class="allSelect">
                            <a href="javascript:void(0)" onclick="SelectAll()">ȫѡ</a></span> <span class="reverseSelect">
                                <a href="javascript:void(0)" onclick="ReverseSelect()">��ѡ</a></span> 
                            <span class="delete"><a href="javascript:void()" onclick="SetAuditing()">���</a></span>
                        </li>
                    </ul>
                </div>--%>
            </div>
            <!--�����б�����-->
            <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="ProductId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title" CssClass="goods-list">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <%--<asp:TemplateField ItemStyle-Width="30px" HeaderText="ѡ��" ItemStyle-CssClass="td_txt_cenetr">
                        <ItemTemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField HeaderText="����" DataField="DisplaySequence" ItemStyle-Width="35px"
                       ItemStyle-CssClass="td_txt_cenetr" />
                    <asp:TemplateField ItemStyle-Width="46%" HeaderText="��Ʒ" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="float: left; margin-right: 10px;">
                                <a href='<%#"../../Vshop/ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                    <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                                </a>
                            </div>
                            <div style="float: left;">
                                <span class="Name"><a href='<%#"../../Vshop/ProductDetails.aspx?isbackshow=1&productId="+Eval("ProductId")%>'
                                    target="_blank">
                                    <%# Eval("ProductName") %></a></span> <span class="colorC" style="display:block">�̼ұ��룺<%# Eval("ProductCode") %>
                                        ��棺<%# Eval("Stock") %>
                                        ����ۣ�<%# Eval("CostPrice", "{0:f2}")%>
                                    </span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="22%" HeaderText="��Ʒ�۸�" >
                        <ItemTemplate>
                            <span class="Name">һ�ڼۣ�<%# Eval("SalePrice", "{0:f2}")%>
                                �г��ۣ�<asp:Literal ID="litMarketPrice" runat="server" Text='<%#Eval("MarketPrice", "{0:f2}")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="��Ʒ״̬" ItemStyle-Width="80" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litSaleStatus" runat="server" Text='<%#Eval("SaleStatus")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="����" HeaderStyle-Width="95" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span class="submit_bianji"><a href="javascript:void(0);" onclick="javascript:SetCurrentAuditing('<%# Eval("ProductId")%>')">���</a></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>
            <div class="blank12 clearfix">
            </div>
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="producttag.helper.js"></script>
    <script type="text/javascript">

        function GetProductId() {
            var v_str = "";

            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });

            if (v_str.length == 0) {
                alert("��ѡ����Ʒ");
                return "";
            }
            return v_str.substring(0, v_str.length - 1);
        }

        //���������Ʒ
        function SetCurrentAuditing(id) {
            if (id > 0) {
                DialogFrame("product/SetAuditing.aspx?ProductIds=" + id, "��Ʒ���", 480, 320);
            }
        }

        //���������Ʒ
        function SetAuditing() {
            //�õ���ѡ����ƷId���ϣ��жϴ��ڹ�ѡ����Ʒ�򵯳���������
            var productIds = GetProductId();
            if (productIds.length > 0) {
                DialogFrame("product/SetAuditing.aspx?ProductIds=" + productIds, "��Ʒ���", 480, 320);
            }
        }

        ////���ô��������ƷId����
        //function SetRemove() {
        //    $("#txtProductIds").val(null);
        //    var productIds = GetProductIdEx();
        //    if (productIds.length > 0) {
        //        //��֤�Ƿ����
        //        $("#txtProductIds").val(productIds);
        //    }
        //}

        //function GetProductIdEx() {
        //    var v_str = "";

        //    $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
        //        v_str += $(rowItem).attr("value") + ",";
        //    });

        //    if (v_str.length == 0) {
        //        return "";
        //    }
        //    return v_str.substring(0, v_str.length - 1);
        //}

    </script>
</asp:Content>
