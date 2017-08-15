<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"  CodeBehind="AddProductService.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddProductService" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
     <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
     <Hi:Script ID="Script2" runat="server" Src="/utility/jquery_hashtable.js" />
     <Hi:Script ID="Script1" runat="server" Src="/utility/jquery-powerFloat-min.js" />
    <link href="/utility/flashupload/flashupload.css" rel="stylesheet" type="text/css" />
    <Hi:Script ID="Script3" runat="server" Src="/utility/flashupload/flashupload.js" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
	  <div class="title  m_none td_bottom"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1>���ӷ�����Ʒ</h1>
      </div>
	  <div class="datafrom">
	    <div class="formitem validator1">
	      <ul>
          <li><h2 class="colorE">������Ϣ</h2></li>
            <li class=" clearfix"> <span class="formitemtitle Pw_198">��Ʒ��Դ��</span>
	            <asp:DropDownList ID="DDLProductSource" runat="server">
                    <asp:ListItem Value="1">��ά</asp:ListItem>
                    <asp:ListItem Value="2">��Ӧ��</asp:ListItem>
                    <asp:ListItem Value="3">����</asp:ListItem>
                </asp:DropDownList>
                 <p id="P1">ϵͳ�Զ�ѡ�񣬲����޸�</p>
	        </li>
	        <li>
	            <span class="formitemtitle Pw_198">��Ʒ���ͣ�</span>
                <abbr class="formselect"><Hi:ProductTypeDownList runat="server" CssClass="productType" ID="dropProductTypes" NullToDisplay="--��ѡ��--" /></abbr>
                <p id="P2">ϵͳ�Զ�ѡ�񣬲����޸�</p>
            </li>
            <li>
                <span class="formitemtitle Pw_198"><em >*</em>����Ʒ�ࣺ</span>
                <abbr class="formselect"><Hi:ProductServiceClassDropDownList  runat="server" ID="dropClassList" NullToDisplay="--��ѡ��--" /></abbr>
                <p id="ctl00_contentHolder_dropClassListTip">��ѡ�����Ʒ��</p>
            </li>
	        <li class=" clearfix"> <span class="formitemtitle Pw_198"><em >*</em>��Ʒ���ƣ�</span>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductName" Width="350px"/>
              <p id="ctl00_contentHolder_txtProductNameTip">�޶���60���ַ�</p>
	        </li>
	        <li><span class="formitemtitle Pw_198">��Ʒ���룺</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductCode" />
                <p id="ctl00_contentHolder_txtProductCodeTip">���Ȳ��ܳ���20���ַ�</p>
            </li>
            <li><span class="formitemtitle Pw_198">�г��ۣ�</span>
	            <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMarketPrice" />&nbsp;Ԫ
                <p id="ctl00_contentHolder_txtMarketPriceTip">��վ��Ա����������Ʒ�г���</p>
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
              <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSalePrice" />&nbsp;Ԫ<%--<input type="button" onclick="editProductMemberPrice();" value="�༭��Ա��" style="margin-left:10px; display:none" />--%>
              <%--<Hi:TrimTextBox runat="server" ID="txtMemberPrices" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox>--%>
              <p id="ctl00_contentHolder_txtSalePriceTip">��վ��Ա����������Ʒ���ۼ�</p>
	        </li>
            <li id="neigouPriceRow"><span class="formitemtitle Pw_198">�ڹ��ۣ�</span>
                <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtNeigouPrice" />&nbsp;Ԫ
                <p id="ctl00_contentHolder_txtNeigouPriceTip">�ڹ��ŵ��Ա����������Ʒ���ۼ�</p>
            </li>
	        <li id="costPriceRow"><span class="formitemtitle Pw_198"><em >*</em>����ۣ�</span>
	            <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtCostPrice" />&nbsp;Ԫ
	            <p id="ctl00_contentHolder_txtCostPriceTip">��Ʒ�Ľ���ۣ�����AH���������ʹ��</p>
	        </li>
            <li id="qtyRow"><span class="formitemtitle Pw_198"><em >*</em>��Ʒ��棺</span><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtStock" /></li>
            <%--<li id="weightRow"><span class="formitemtitle Pw_198">��Ʒ������</span><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtWeight" />&nbsp;��</li>--%>
            <li id="skuTitle" style="display:none;"><h2 class="colorE">��Ʒ���</h2></li>
            <li id="enableSkuRow" style="display:none;"><span class="formitemtitle Pw_198">���</span><input class="inp_guige" id="btnEnableSku" type="button" value="�������" /> 
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
            <%--<li>
			  <span class="formitemtitle Pw_198">��Ʒ���ʣ�
                </span>
				<asp:CheckBox ID="ChkisfreeShipping" 
                    runat="server" />
 			&nbsp;</li>--%>
             <li class="clearfix" id="l_tags" runat="server" style="display:none;">
			   <span class="formitemtitle Pw_198">��Ʒ��ǩ���壺<br /><a id="a_addtag" href="javascript:void(0)" onclick="javascript:AddTags()" class="add">����</a></span>
			   
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="attributes.service.helper.js"></script>
    <script type="text/javascript" src="grade.price.helper.js"></script>
    <script type="text/javascript" src="producttag.helper.js"></script>
        
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtProductName', 1, 60, false, null, '��Ʒ�����ַ�������1-60֮��'));
            initValid(new InputValidator('ctl00_contentHolder_txtProductCode', 0, 20, true, null, '��Ʒ����ĳ��Ȳ��ܳ���20���ַ�'));
            //�Ὺ��Ʒ����
            //initValid(new InputValidator('ctl00_contentHolder_txtKuKaiProductCode', 0, 20, true, null, '�Ὺ��Ʒ����ĳ��Ȳ��ܳ���20���ַ�'));

            //�޹�����
            //initValid(new InputValidator('ctl00_contentHolder_txtRestrictNeigouNum', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            //appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtRestrictNeigouNum', 0, 99999999, '�������ֵ������ϵͳ��ʾ��Χ'));

            //initValid(new InputValidator('ctl00_contentHolder_txtUnit', 1, 20, false, '[a-zA-Z\/\u4e00-\u9fa5]*$', '����������20���ַ�������ֻ����Ӣ�ĺ�������:g/Ԫ'));
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

            //initValid(new InputValidator('ctl00_contentHolder_txtWeight', 0, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            //appendValid(new NumberRangeValidator('ctl00_contentHolder_txtWeight', 1, 9999999, '�������ֵ������ϵͳ��ʾ��Χ'));
            initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 0, 300, true, null, '��Ʒ������������20���ַ�����'));
        }

        $(document).ready(function () {
            InitValidators();
        });

  </script>
</asp:Content>