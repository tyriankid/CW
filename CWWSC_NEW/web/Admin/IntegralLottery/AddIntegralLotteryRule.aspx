<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddIntegralLotteryRule.aspx.cs" Inherits="Admin_IntegralLottery_AddIntegralLotteryRule" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>



<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
    <div class="areacolumn clearfix validator4">
	<div class="columnright">
	   <div class="title"> <em><img src="../images/06.gif" width="32" height="32" /></em>
        <h1>添加大转盘奖项</h1>
        <span>填写大转盘奖项详细信息</span>
      </div>
            <div class="datafrom">
        <div class="formitem validator2">
          <ul>
              <li class="clearfix"><span class="formitemtitle Pw_110">奖项等级：</span>
             <asp:TextBox ID="txtLotteryClass" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtLotteryClassTip">当前最大奖项级别,<%=getMaxClass() %>,奖项等级不能重复,按抽奖转盘逆时针由小到大排序</p>               
            </li>
            <li class="clearfix"><span class="formitemtitle Pw_110">奖项名称：</span>
             <asp:TextBox ID="txtLotteryItem" runat="server" CssClass="forminput"></asp:TextBox>        
            </li>
            <li> <span class="formitemtitle Pw_110">中奖占比：</span>
               <asp:TextBox ID="txtLotteryProportion" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtLotteryProportionTip">中奖实际概率算法=后台设置中奖占比数 / 占比数和</p>
            </li>
            <li> <span class="formitemtitle Pw_110">中奖商品：</span>
               <asp:TextBox ID="txtProductName" runat="server" CssClass="forminput"></asp:TextBox>
                <asp:HiddenField ID="hiProductId" runat="server" /> 
             <p id="ctl00_contentHolder_txtNameTip"><a onclick="ShowSelectProduct()">选择商品</a></p>
            </li>
          </ul>
           <ul class="btntf Pw_110">
		     <asp:Button ID="btnCreate" runat="server" Text="添加" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding inbnt" OnClick="btnCreate_Click"  />
            
		  </ul>
</div>
      </div>
	</div>
</div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">

        //获取url中"?"符后的字串
        function getrequest() {
            var url = location.search;
            var theRequest = new Object();
            if (url.indexOf("?") != -1) {
                var str = url.substr(1);
                strs = str.split("&");
                for (var i = 0; i < strs.length; i++) {
                    theRequest[strs[i].split("=")[0]] = (strs[i].split("=")[1]);
                }
                return theRequest;
            }
        }

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtLotteryClass', 1, 10, false, '^(10|[1-9])$', '奖项等级必须是数字，必须大于O，不能超过10'))
            initValid(new InputValidator('ctl00_contentHolder_txtLotteryItem', 1, 60, false, null, '奖项名称不合法'))
            initValid(new InputValidator('ctl00_contentHolder_txtLotteryProportion', 1, 10000000000000, false, '^[0-9]*$', '中奖占比数输入不合法'))
            initValid(new InputValidator('ctl00_contentHolder_txtProductName', 1, 10000, false, null, '中奖商品不能为空'))
        }
        $(document).ready(function () { InitValidators(); });

        function ShowSelectProduct() {
            if (getrequest().Action == "Update")  //修改
            {
                var str = String.format("IntegralLottery/ChoosePro.aspx?Action={0}&RuleId={1}", getrequest().Action, getrequest().RuleId);
                DialogFrame(str, "商品选择", 1000, null);
            }else {
                var str = String.format("IntegralLottery/ChoosePro.aspx?IsAdd={0}",  true);
                DialogFrame(str, "商品选择", 1000, null);
            }
        }


        function setSelectProduct(name, id) {
            $("#ctl00_contentHolder_txtProductName").val(name);
            $("#ctl00_contentHolder_hiProductId").val(id);
        }
</script>
</asp:Content>