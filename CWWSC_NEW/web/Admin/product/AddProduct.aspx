<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"  CodeBehind="AddProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddProduct" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
     <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
     <Hi:Script ID="Script2" runat="server" Src="/utility/jquery_hashtable.js" />
     <Hi:Script ID="Script1" runat="server" Src="/utility/jquery-powerFloat-min.js" />
    <link href="/utility/flashupload/flashupload.css" rel="stylesheet" type="text/css" />
    <Hi:Script ID="Script3" runat="server" Src="/utility/flashupload/flashupload.js" />
    <style type="text/css">
        .disabledIput {background: #e0dfdf;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
	  <div class="title  m_none td_bottom"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1>�����Ʒ</h1>
      </div>
	  <div class="datafrom">
	    <div class="formitem validator1">
	      <ul>
          <li><h2 class="colorE">������Ϣ</h2></li>
	        <li>
	            <span class="formitemtitle Pw_198">������Ʒ���ࣺ</span>
                <span class="colorE float fonts"><asp:Literal runat="server" ID="litCategoryName"></asp:Literal></span>
                [<asp:HyperLink runat="server" ID="lnkEditCategory" CssClass="a" Text="�༭"></asp:HyperLink>]
            </li>
	        <li>
	            <span class="formitemtitle Pw_198">��Ʒ���ͣ�</span>
                <abbr class="formselect"><Hi:ProductTypeDownList runat="server" CssClass="productType" ID="dropProductTypes" NullToDisplay="--��ѡ��--" /></abbr>
	            Ʒ�ƣ�<abbr class="formselect"><Hi:BrandCategoriesDropDownList  runat="server" ID="dropBrandCategories" NullToDisplay="--��ѡ��--" /></abbr>
            </li>
             <li class=" clearfix"> <span class="formitemtitle Pw_198"><em >*</em>��Ʒ��Դ��</span>
	            <asp:DropDownList ID="DDLProductSource" runat="server">
                    <asp:ListItem Value="1">��ά</asp:ListItem>
                    <asp:ListItem Value="2">��Ӧ��</asp:ListItem>
                </asp:DropDownList>
                 <p id="P1">ϵͳ�Զ�ѡ�񣬲����޸�</p>
	        </li>
	        <li class=" clearfix"> <span class="formitemtitle Pw_198"><em >*</em>��Ʒ���ƣ�</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductName" Width="350px"/>
              <p id="ctl00_contentHolder_txtProductNameTip">�޶���60���ַ�</p>
	        </li>
	        <li><span class="formitemtitle Pw_198">��Ʒ���룺</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductCode" />
                <p id="ctl00_contentHolder_txtProductCodeTip">���Ȳ��ܳ���20���ַ���ֻ�д�ά��Ʒ��������д���룬��Ӧ����Ʒ������AH�ش�Ϊ��</p>
            </li>
            <li id="KuKaiProductCodePriceRow"><span class="formitemtitle Pw_198">�Ὺ��Ʒ���룺</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtKuKaiProductCode" />
                <p id="ctl00_contentHolder_txtKuKaiProductCodeTip">���Ȳ��ܳ���20���ַ���ֻ�пῪ��Ʒ��������д���룬������ǿῪ����Ʒ����Դ����������֤�Ὺ��Ʒ��Ψһ���ݡ�</p>
            </li>
	        <li><span class="formitemtitle Pw_198"><em >*</em>������λ��</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtUnit" />
                <p id="ctl00_contentHolder_txtUnitTip">����������20���ַ�������ֻ����Ӣ�ĺ�������:g/Ԫ</p>
            </li>
            <li><span class="formitemtitle Pw_198">�г��ۣ�</span>
	            <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMarketPrice" />&nbsp;Ԫ
                <p id="ctl00_contentHolder_txtMarketPriceTip">��վ��Ա����������Ʒ�г���</p>
	        </li>
            <li><span class="formitemtitle Pw_198">�޹�������</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtRestrictNeigouNum" />
                <p id="ctl00_contentHolder_txtRestrictNeigouNumTip">���ڹ��ŵ�������ͬһ��Ա��๺�������</p>
            </li>
	        <li><h2 class="colorE">��չ����</h2></li>
	        <li id="attributeRow" style="display:none;"><span class="formitemtitle Pw_198">��Ʒ���ԣ�</span>
	        <div class="attributeContent" id="attributeContent"></div>
            <Hi:TrimTextBox runat="server" ID="txtAttributes" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox>
            </li>
	        <li id="skuCodeRow"><span class="formitemtitle Pw_198"><em >*</em>�ͺţ�</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSku" />
                <p id="ctl00_contentHolder_txtSkuTip">�޶���30���ַ�</p>
            </li>
	        <li id="salePriceRow"><span class="formitemtitle Pw_198"><em >*</em>һ�ڼۣ�</span>
              <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSalePrice" />&nbsp;Ԫ<input type="button" onclick="editProductMemberPrice();" value="�༭��Ա��" style="margin-left:10px; display:none" />
              <Hi:TrimTextBox runat="server" ID="txtMemberPrices" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox>
              <p id="ctl00_contentHolder_txtSalePriceTip">��վ��Ա����������Ʒ���ۼ�</p>
	        </li>
            <li id="neigouPriceRow"><span class="formitemtitle Pw_198">�ڹ��ۣ�</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtNeigouPrice" />&nbsp;Ԫ
                <p id="ctl00_contentHolder_txtNeigouPriceTip">�ڹ��ŵ��Ա����������Ʒ���ۼ�</p>
            </li>
	        <li id="costPriceRow"><span class="formitemtitle Pw_198"><em >*</em>�ɱ�(����)�ۣ�</span>
	            <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtCostPrice" />&nbsp;Ԫ
	            <p id="ctl00_contentHolder_txtCostPriceTip">��Ʒ�ĳɱ��ۣ����Ϊ��Ӧ����Ʒ��������۸���AH���������ʹ��</p>
	        </li>
            <li id="qtyRow"><span class="formitemtitle Pw_198"><em >*</em>��Ʒ��棺</span><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtStock" /></li>
            <li id="weightRow"><span class="formitemtitle Pw_198">��Ʒ������</span><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtWeight" />&nbsp;��</li>
            <%--<li id="rangeRow"><span class="formitemtitle Pw_198">��Ʒ��ʾ��Χ��</span>
                <asp:DropDownList ID="DDLRange" runat="server">
                    <asp:ListItem Value="0">ȫ��</asp:ListItem>
                    <asp:ListItem Value="1">PC��</asp:ListItem>
                    <asp:ListItem Value="2">�ֻ���</asp:ListItem>
                </asp:DropDownList>
            </li>--%>
            <%--start��Ӷ���--%>
            <li><h2 class="colorE">Ӷ�����</h2></li>
            <li>
			  <span class="formitemtitle Pw_198">һ�ڼ۷�Ӷ���ͣ�</span>
				<asp:RadioButton runat="server" ID="RadioSaleA" GroupName="SaleType" Checked="true" Text="����"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="RadioSaleB" GroupName="SaleType"  Text="���"></asp:RadioButton>
 			</li>
            <li id="SaleRatioPriceRow"><span class="formitemtitle Pw_198">��Ӷ������</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSaleCommisionRatio" />&nbsp;%
                <p id="ctl00_contentHolder_txtSaleCommisionRatioTip">��Ӷ����ֵ</p>
            </li>
            <li id="SaleMoneyPriceRow"><span class="formitemtitle Pw_198">��Ӷ��</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSaleCommisionMoney" />&nbsp;Ԫ
                <p id="ctl00_contentHolder_txtSaleCommisionMoneyTip">��Ӷ���ֵ</p>
            </li>
            <li id="NeigouTypePriceRow">
			  <span class="formitemtitle Pw_198">�ڹ��۷�Ӷ���ͣ�</span>
				<asp:RadioButton runat="server" ID="RadioNeigouA" GroupName="NeigouType" Checked="true" Text="����"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="RadioNeigouB" GroupName="NeigouType"  Text="���"></asp:RadioButton>
 			</li>
            <li id="NeigouRatioPriceRow"><span class="formitemtitle Pw_198">��Ӷ������</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtNeigouCommisionRatio" />&nbsp;%
                <p id="ctl00_contentHolder_txtNeigouCommisionRatioTip">��Ӷ����ֵ</p>
            </li>
            <li id="NeigouMoneyPriceRow"><span class="formitemtitle Pw_198">��Ӷ��</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtNeigouCommisionMoney" />&nbsp;Ԫ
                <p id="ctl00_contentHolder_txtNeigouCommisionMoneyTip">��Ӷ���ֵ</p>
            </li>

            <li id="WholesaleTypePriceRow">
			  <span class="formitemtitle Pw_198">�����۷�Ӷ���ͣ�</span>
				<asp:RadioButton runat="server" ID="RadioWholesaleA" GroupName="WholesaleType" Checked="true" Text="����"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="RadioWholesaleB" GroupName="WholesaleType" Text="���"></asp:RadioButton>
                <p id="P2">�����۸�Ӷֻ�����ڹ�Ӧ����Ʒ</p>
 			</li>
            <li id="WholesaleRatioPriceRow"><span class="formitemtitle Pw_198">��Ӷ������</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtWholesaleCommisionRatio" />&nbsp;%
                <p id="ctl00_contentHolder_txtWholesaleCommisionRatioTip">��Ӷ����ֵ</p>
            </li>
            <li id="WholesaleMoneyPriceRow"><span class="formitemtitle Pw_198">��Ӷ��</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtWholesaleCommisionMoney" />&nbsp;Ԫ
                <p id="ctl00_contentHolder_txtWholesaleCommisionMoneyTip">��Ӷ���ֵ</p>
            </li>
            <%--end��Ӷ���--%>

            <li id="skuTitle" style="display:none;"><h2 class="colorE">��Ʒ���</h2></li>
            <li id="enableSkuRow" style="display:none;"><span class="formitemtitle Pw_198">���</span><input id="btnEnableSku" type="button" value="�������" /> 
                �������ǰ����д������Ϣ�����Զ�������Ϣ��ÿ�����</li>
            <li id="skuRow"  style="display:none;">
                <p id="skuContent">
                    <input type="button" id="btnshowSkuValue" value="���ɲ��ֹ��" />
                    <input type="button" id="btnAddItem" value="����һ�����" />
                    <input type="button" id="btnCloseSku" value="�رչ��" />
                    <input type="button" id="btnGenerateAll" value="�������й��" />
                </p>
                <p id="skuFieldHolder" style="margin:0px auto;display:none;"></p>
                <div id="skuTableHolder">
                </div>
                <Hi:TrimTextBox runat="server" ID="txtSkus" TextMode="MultiLine" style="display:none;" ></Hi:TrimTextBox>
                <asp:CheckBox runat="server" ID="chkSkuEnabled" style="display:none;" />
            </li>
            <li><h2 class="colorE">ͼƬ������</h2></li>
           
              <li style="height: 126px;"><span class="formitemtitle Pw_198">��ƷͼƬ��</span><Hi:ProductFlashUpload ID="ucFlashUpload1" runat="server" />                 
                  </li>
              <li><p class="Pa_198 clearfix m_none" style="padding-left:200px;">֧�ֶ�ͼ�ϴ�,���5��,ÿ��ͼӦС��120k,jpg,gif,png��ʽ������Ϊ500x500����</p></li>
            <li class="clearfix"><span class="formitemtitle Pw_198">��Ʒ��飺</span>
                <Hi:TrimTextBox runat="server" Rows="6" Height="100px" Columns="76" ID="txtShortDescription" TextMode="MultiLine" />
                <p class="Pa_198">�޶���300���ַ�����</p>
            </li>
            <li class="clearfix"><span class="formitemtitle Pw_198">��Ʒ������</span>
               <Kindeditor:KindeditorControl ID="editDescription" runat="server" Height="300"  />
               <p style="color:Red;"><asp:CheckBox runat="server" ID="ckbIsDownPic" Text="�Ƿ�������Ʒ������վͼƬ" /></p>
                <p class="Pa_198">�����ѡ��ѡ��ʱ����Ʒ��������վ��ͼƬ������ص����أ��෴�򲻻ᣬ����Ҫ����ͼƬ�����ͼƬ�����ͼƬ�ܴ���Ҫ���ص�ʱ��Ͷ࣬������ѡ��</p>
				<p class="Pa_198">����ͼƬ�ߴ罨�鲻Ҫ������320px * 580px</p>
            </li>
             <%-- ****--%>
           <li class="clearfix"><span class="formitemtitle Pw_198">��Ʒ���</span>
               <Kindeditor:KindeditorControl ID="specification" runat="server" Height="300"  />
               <p style="color:Red;"><asp:CheckBox runat="server" ID="chkPic" Text="�Ƿ�������Ʒ������վͼƬ" /></p>
                <p class="Pa_198">�����ѡ��ѡ��ʱ����Ʒ��������վ��ͼƬ������ص����أ��෴�򲻻ᣬ����Ҫ����ͼƬ�����ͼƬ�����ͼƬ�ܴ���Ҫ���ص�ʱ��Ͷ࣬������ѡ��</p>
				<p class="Pa_198">����ͼƬ�ߴ罨�鲻Ҫ������320px * 580px</p>
            </li>
              <%--****--%>
	       <li><h2 class="colorE clear">�������</h2></li>
	        <li>
			  <span class="formitemtitle Pw_198">��Ʒ����״̬��</span>
                <asp:RadioButton runat="server" ID="radOnAuditing" GroupName="SaleStatus" Text="�����" Visible="false"></asp:RadioButton>
				<asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus" Checked="true" Text="������"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus"  Text="�¼���" Visible="false"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus"  Text="�ֿ���"></asp:RadioButton>
 			</li>
            <li>
			  <span class="formitemtitle Pw_198">��Ʒ���ʣ�
                </span>
				<asp:CheckBox ID="ChkisfreeShipping" 
                    runat="server" />
 			&nbsp;</li>
             <li class="clearfix" id="l_tags" runat="server" style="display:none;">
			   <span class="formitemtitle Pw_198">��Ʒ��ǩ���壺<br /><a id="a_addtag" href="javascript:void(0)" onclick="javascript:AddTags()" class="add">���</a></span>
			   
			   <div id="div_tags"> <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral></div>
			   <div id="div_addtag" style="display:none"><input type="text" id="txtaddtag" /><input type="button" value="����" onclick="return AddAjaxTags()" /></div>
			     <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox> 
            </li>    
	      </ul>
	      <ul class="btntf Pa_198 clear">
	        <asp:Button runat="server" ID="btnAdd" Text="�� ��" OnClientClick="return doSubmit();" CssClass="submit_DAqueding inbnt" />
          </ul>
        </div>
      </div>
</div>
<div class="Pop_up" id="priceBox" style="display: none;">
    <h1>
        <span id="popTitle"></span>
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" alt="�ر�" width="38" height="20" />
   </div>
    <div class="mianform ">
        <div id="priceContent">
        </div>
        <div style="margin-top:10px;text-align:center;">
            <input type="button" value="ȷ��" onclick="doneEditPrice();" />
        </div>
    </div>
</div>

<div class="Pop_up" id="skuValueBox" style="display: none;">
    <h1>
        <span>ѡ��Ҫ���ɵĹ��</span>
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" alt="�ر�" width="38" height="20" />
   </div>
    <div class="mianform ">
        <div id="skuItems" >
        </div>
        <div style="margin-top:10px;text-align:center;">
            <input type="button" value="ȷ��" id="btnGenerate" />
        </div>
    </div>
</div>
<asp:HiddenField ID="managertype" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="attributes.helper.js"></script>
    <script type="text/javascript" src="grade.price.helper.js"></script>
    <script type="text/javascript" src="producttag.helper.js"></script>
    <script type="text/javascript" language="javascript">     
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtProductName', 1, 60, false, null, '��Ʒ�����ַ�������1-60֮��'));
            initValid(new InputValidator('ctl00_contentHolder_txtProductCode', 0, 20, true, null, '��Ʒ����ĳ��Ȳ��ܳ���20���ַ�'));
            //�Ὺ��Ʒ����
            initValid(new InputValidator('ctl00_contentHolder_txtKuKaiProductCode', 0, 20, true, null, '�Ὺ��Ʒ����ĳ��Ȳ��ܳ���20���ַ�'));

            //�޹�����
            initValid(new InputValidator('ctl00_contentHolder_txtRestrictNeigouNum', 1, 10, true, '-?[0-9]\\d*', '�������ʹ���ֻ��������������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtRestrictNeigouNum', 0, 9999999, '�������ֵ������ϵͳ��ʾ��Χ'));

            initValid(new InputValidator('ctl00_contentHolder_txtUnit', 1, 20, false, '[a-zA-Z\/\u4e00-\u9fa5]*$', '����������20���ַ�������ֻ����Ӣ�ĺ�������:g/Ԫ'));
            initValid(new InputValidator('ctl00_contentHolder_txtSalePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSalePrice', 0.01, 10000000, '�������ֵ������ϵͳ��ʾ��Χ'));
            //�ڹ���
            initValid(new InputValidator('ctl00_contentHolder_txtNeigouPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtNeigouPrice', 0, 99999999, '�������ֵ������ϵͳ��ʾ��Χ'));

            initValid(new InputValidator('ctl00_contentHolder_txtCostPrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCostPrice', 0, 99999999, '�������ֵ������ϵͳ��ʾ��Χ'));
            initValid(new InputValidator('ctl00_contentHolder_txtMarketPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMarketPrice', 0, 99999999, '�������ֵ������ϵͳ��ʾ��Χ'));
            initValid(new InputValidator('ctl00_contentHolder_txtSku', 0, 30, false, null, '��Ʒ�ͺŵĳ��Ȳ��ܳ���30���ַ�'));
            initValid(new InputValidator('ctl00_contentHolder_txtStock', 1, 10, false, '-?[0-9]\\d*', '�������ʹ���ֻ��������������ֵ'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtStock', 1, 9999999, '�������ֵ������ϵͳ��ʾ��Χ'));

            initValid(new InputValidator('ctl00_contentHolder_txtWeight', 0, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtWeight', 1, 9999999, '�������ֵ������ϵͳ��ʾ��Χ'));

            /**start��Ӷֵ��֤***/
            initValid(new InputValidator('ctl00_contentHolder_txtSaleCommisionRatio', 1, 6, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSaleCommisionRatio', 0, 100, '�������ֵ������ϵͳ��ʾ��Χ'));
            
            initValid(new InputValidator('ctl00_contentHolder_txtSaleCommisionMoney', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSaleCommisionMoney', 0, 99999999, '�������ֵ������ϵͳ��ʾ��Χ'));
            
            initValid(new InputValidator('ctl00_contentHolder_txtNeigouCommisionRatio', 1, 6, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtNeigouCommisionRatio', 0, 100, '�������ֵ������ϵͳ��ʾ��Χ'));
            
            initValid(new InputValidator('ctl00_contentHolder_txtNeigouCommisionMoney', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtNeigouCommisionMoney', 0, 99999999, '�������ֵ������ϵͳ��ʾ��Χ'));
            
            initValid(new InputValidator('ctl00_contentHolder_txtWholesaleCommisionRatio', 1, 6, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtWholesaleCommisionRatio', 0, 100, '�������ֵ������ϵͳ��ʾ��Χ'));
            
            initValid(new InputValidator('ctl00_contentHolder_txtWholesaleCommisionMoney', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtWholesaleCommisionMoney', 0, 99999999, '�������ֵ������ϵͳ��ʾ��Χ'));
            /**start��Ӷֵ��֤***/
            
            initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 0, 300, true, null, '��Ʒ������������20���ַ�����'));
        }

        $(document).ready(function () {
            if ($("#ctl00_contentHolder_DDLProductSource").val() == "1") {
                $("#KuKaiProductCodePriceRow").hide();
            }

            //���ÿؼ���֤
            InitValidators();

            //һ�ڼ�Ӷ��ʼ����
            if ($("#ctl00_contentHolder_RadioSaleA").attr("checked") == "checked") {
                $("#ctl00_contentHolder_txtSaleCommisionRatio").removeAttr("disabled");
                $("#ctl00_contentHolder_txtSaleCommisionRatio").removeClass("disabledIput");

                $("#ctl00_contentHolder_txtSaleCommisionMoney").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtSaleCommisionMoney").addClass("disabledIput");
            }
            if ($("#ctl00_contentHolder_RadioSaleB").attr("checked") == "checked") {
                $("#ctl00_contentHolder_txtSaleCommisionMoney").removeAttr("disabled");
                $("#ctl00_contentHolder_txtSaleCommisionMoney").removeClass("disabledIput");

                $("#ctl00_contentHolder_txtSaleCommisionRatio").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtSaleCommisionRatio").addClass("disabledIput");
            }
            //�ڹ���Ӷ��ʼ����
            if ($("#ctl00_contentHolder_RadioNeigouA").attr("checked") == "checked") {
                $("#ctl00_contentHolder_txtNeigouCommisionRatio").removeAttr("disabled");
                $("#ctl00_contentHolder_txtNeigouCommisionRatio").removeClass("disabledIput");

                $("#ctl00_contentHolder_txtNeigouCommisionMoney").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtNeigouCommisionMoney").addClass("disabledIput");
            }
            if ($("#ctl00_contentHolder_RadioNeigouB").attr("checked") == "checked") {
                $("#ctl00_contentHolder_txtNeigouCommisionMoney").removeAttr("disabled");
                $("#ctl00_contentHolder_txtNeigouCommisionMoney").removeClass("disabledIput");

                $("#ctl00_contentHolder_txtNeigouCommisionRatio").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtNeigouCommisionRatio").addClass("disabledIput");
            }

            //������Ӷ��ʼ����
            if ($("#ctl00_contentHolder_RadioWholesaleA").attr("checked") == "checked") {
                $("#ctl00_contentHolder_txtWholesaleCommisionRatio").removeAttr("disabled");
                $("#ctl00_contentHolder_txtWholesaleCommisionRatio").removeClass("disabledIput");

                $("#ctl00_contentHolder_txtWholesaleCommisionMoney").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtWholesaleCommisionMoney").addClass("disabledIput");
            }
            if ($("#ctl00_contentHolder_RadioWholesaleB").attr("checked") == "checked") {
                $("#ctl00_contentHolder_txtWholesaleCommisionMoney").removeAttr("disabled");
                $("#ctl00_contentHolder_txtWholesaleCommisionMoney").removeClass("disabledIput");

                $("#ctl00_contentHolder_txtWholesaleCommisionRatio").attr("disabled", "disabled");
                $("#ctl00_contentHolder_txtWholesaleCommisionRatio").addClass("disabledIput");
            }

            //һ�ڼ�Ӷ��ѡ�п���
            $("#ctl00_contentHolder_RadioSaleA").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioSaleA").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtSaleCommisionRatio").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtSaleCommisionRatio").removeClass("disabledIput");
                    $("#ctl00_contentHolder_txtSaleCommisionRatio").focus();

                    $("#ctl00_contentHolder_txtSaleCommisionMoney").attr("disabled", "disabled");
                    $("#ctl00_contentHolder_txtSaleCommisionMoney").addClass("disabledIput");
                }
            });

            $("#ctl00_contentHolder_RadioSaleB").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioSaleB").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtSaleCommisionMoney").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtSaleCommisionMoney").removeClass("disabledIput");
                    $("#ctl00_contentHolder_txtSaleCommisionMoney").focus();

                    $("#ctl00_contentHolder_txtSaleCommisionRatio").attr("disabled", "disabled");
                    $("#ctl00_contentHolder_txtSaleCommisionRatio").addClass("disabledIput");
                }
            });

            //�ڹ�Ӷ��ѡ�п���
            $("#ctl00_contentHolder_RadioNeigouA").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioNeigouA").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtNeigouCommisionRatio").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtNeigouCommisionRatio").removeClass("disabledIput");
                    $("#ctl00_contentHolder_txtNeigouCommisionRatio").focus();

                    $("#ctl00_contentHolder_txtNeigouCommisionMoney").attr("disabled", "disabled");
                    $("#ctl00_contentHolder_txtNeigouCommisionMoney").addClass("disabledIput");
                }
            });

            $("#ctl00_contentHolder_RadioNeigouB").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioNeigouB").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtNeigouCommisionMoney").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtNeigouCommisionMoney").removeClass("disabledIput");
                    $("#ctl00_contentHolder_txtNeigouCommisionMoney").focus();

                    $("#ctl00_contentHolder_txtNeigouCommisionRatio").attr("disabled", "disabled");
                    $("#ctl00_contentHolder_txtNeigouCommisionRatio").addClass("disabledIput");
                }
            });
            
            //������Ӷѡ�п���
            $("#ctl00_contentHolder_RadioWholesaleA").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioWholesaleA").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtWholesaleCommisionRatio").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtWholesaleCommisionRatio").removeClass("disabledIput");
                    $("#ctl00_contentHolder_txtWholesaleCommisionRatio").focus();

                    $("#ctl00_contentHolder_txtWholesaleCommisionMoney").attr("disabled", "disabled");
                    $("#ctl00_contentHolder_txtWholesaleCommisionMoney").addClass("disabledIput");
                }
            });

            $("#ctl00_contentHolder_RadioWholesaleB").click(function () {
                var $selectedvalue = $("#ctl00_contentHolder_RadioWholesaleB").attr("checked");
                if ($selectedvalue == "checked") {
                    $("#ctl00_contentHolder_txtWholesaleCommisionMoney").removeAttr("disabled");
                    $("#ctl00_contentHolder_txtWholesaleCommisionMoney").removeClass("disabledIput");
                    $("#ctl00_contentHolder_txtWholesaleCommisionMoney").focus();

                    $("#ctl00_contentHolder_txtWholesaleCommisionRatio").attr("disabled", "disabled");
                    $("#ctl00_contentHolder_txtWholesaleCommisionRatio").addClass("disabledIput");
                }
            });
        });
  </script>     
</asp:Content>
