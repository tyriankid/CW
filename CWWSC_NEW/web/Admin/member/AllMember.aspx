<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AllMember.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AllMember" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>粘性会员列表</h1>
  <span>显示所有会员信息，包括录入会员信息、复购会员信息（电话相同，则只显示录入会员信息）</span>
</div>
		<!--搜索-->
	
		<!--数据列表区域-->
		<div class="datalist">
			<div class="searcharea clearfix br_search">
			<ul>
          	    <li>
                <span>姓名：</span>
                <span><asp:TextBox ID="txtRealName" CssClass="forminput" runat="server" /></span>
                </li>
                <li>
                <span>电话：</span>
                <span><asp:TextBox ID="txtCellPhone" CssClass="forminput" runat="server" /></span>
          </li>
                <li><span>购机时间：</span> <span>
                <UI:WebCalendar runat="server" CssClass="forminput1" ID="txtStartTime" Width="100" /></span>
                <span>至</span>
                <span> 
                <UI:WebCalendar runat="server" CssClass="forminput1" ID="txtEndTime" Width="100"/>
                </span>
            </li>		
				<li>
				    <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="搜索" />
				</li>
			</ul>
          <ul>
          <li id="clickTopDown" class="clickTopX">导出会员信息</li>
	      </ul>
          <dl id="dataArea" style="display:none;">
		  <ul>
		    <li>请选择需要导出的信息：</li>
            <li>
                <Hi:ExportFieldsCheckBoxList2 ID="exportFieldsCheckBoxList" runat="server"></Hi:ExportFieldsCheckBoxList2>
           </li>
	      </ul>
          <ul>
		    <li style="padding-left:47px;">请选择导出格式：</li>
            <li>
           <Hi:ExportFormatRadioButtonList ID="exportFormatRadioButtonList" runat="server" />
            </li>
	      </ul>
           <ul>
		    <li style=" width:135px;"></li>
             <li><asp:Button ID="btnExport" runat="server" CssClass="submit_queding" Text="导出" /></li>
	      </ul>
         </dl>
	  </div>
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea" style="float:left;">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	          
	      </div>
		 <div class="pageNumber" style="float:right;"> 
		     <div class="pagination"><UI:Pager runat="server" ShowTotalPages="false" ID="pager" /></div>
        </div>
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
	  </div>
		    <UI:Grid ID="grdMemberList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="UserId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="会员姓名" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblUserName" runat="server" Text='<%# string.IsNullOrEmpty(Eval("name").ToString()) ? Eval("MemberName") : Eval("name")  %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="会员电话" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblMobile" runat="server" Text='<%# string.IsNullOrEmpty(Eval("mobile").ToString()) ? Eval("Membermobile") : Eval("mobile") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="职业" ShowHeader="true">
                        <ItemTemplate>&nbsp;<%#Eval("profession")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DZ号" ShowHeader="true">
                        <ItemTemplate>&nbsp;<%# string.IsNullOrEmpty(Eval("storeCode").ToString()) ? Eval("MemberStoreCode") : Eval("storeCode") %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="所属门店" ShowHeader="true">
                        <ItemTemplate>
                            &nbsp;<%# string.IsNullOrEmpty(Eval("storeNameNew").ToString()) ? (string.IsNullOrEmpty(Eval("StoreName").ToString()) ? Eval("MemberstoreName") : Eval("StoreName")) : Eval("storeNameNew") %></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="会员等级"  ItemStyle-Width="100px"  SortExpression="GradeName">                            
                            <itemtemplate>
                                &nbsp;<%# string.IsNullOrEmpty(Eval("GradeName").ToString()) ? Eval("MembergradeName")  : Eval("GradeName") %>
                            </itemtemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="购买产品型号" ShowHeader="true">
                            <ItemTemplate>&nbsp;<%#Eval("model")%></ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="购买时间" ItemStyle-Width="120px" ShowHeader="true" >
                            <ItemTemplate>&nbsp;<%# string.IsNullOrEmpty(Eval("buydate").ToString()) ? "" : Convert.ToDateTime(Eval("buydate")).ToShortDateString() %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="详细地址" SortExpression="address" ItemStyle-Width="280px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblAddress" runat="server" Text='<%# Eval ("Province").ToString()+Eval ("City").ToString()+Eval ("County").ToString()+Eval ("Village").ToString()+ Eval ("Address").ToString() %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>                   
                         <asp:TemplateField HeaderText="操作" ItemStyle-Width="100px" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemStyle CssClass="spanD spanN" />
                             <ItemTemplate>
                                 <span class="submit_bianji" style="<%# string.IsNullOrEmpty(Eval("userid").ToString()) ? "display:none;" : "" %>"><a href='<%# Globals.GetAdminAbsolutePath("/member/AddO2OMember.aspx?Id=" + Eval("userid"))%> '>编辑</a></span>
                                 <span class="submit_bianji" style="<%# string.IsNullOrEmpty(Eval("Memberuserid").ToString()) ? "display:none;" : "" %>"><a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/EditMember.aspx?userId={0}", Eval("Memberuserid")))%>'>编辑</a></span>
		                         <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" ID="Delete" Text="删除" CommandName="Delete" IsShow="true" /></span>
                             </ItemTemplate>
                         </asp:TemplateField>  
                    </Columns>
                </UI:Grid>               
<div class="blank12 clearfix"></div>
</div>
		<!--数据列表底部功能区域-->
  <div class="bottomBatchHandleArea clearfix" style="display:none">
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
						<span class="bottomSignicon"></span>
						<span class="allSelect"><a href="javascript:void(0);" onclick="CheckClickAll()">全选</a></span>
						<span class="reverseSelect"><a href="javascript:void(0);" onclick="CheckReverse()">反选</a></span>
					    <span class="delete" style=" display:none"><Hi:ImageLinkButton ID="lkbDelectCheck" IsShow="true" Height="25px" runat="server" Text="删除" /></span>
                        <span id="Span3" class="sendsite"  runat="server"><a href="javascript:void(0);" onclick="return SendMessage()">短信群发</a></span>
                        <span id="Span4" class="sendemail" runat="server"><a href="javascript:void(0);" onclick="return SendEmail()">邮件群发</a></span>
					</li>
				</ul>
			</div>
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination" style="width:auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
		</div>
	</div>

	 <!--会员短信群发-->
   <div  id="div_sendmsg" style="display: none;">
         <p>短信群发<span>(您还剩余短信<font color="red"><asp:Literal ID="litsmscount" runat="server" Text="0"></asp:Literal></font>条)</span></p>
          <p> <h4>发送对象(共<font style="color:Red">0</font>个会员)</h4></p>
          <div id="send_member" style="overflow-x:hidden;overflow-y:auto;margin-bottom:20px"><ul class="menber"></ul></div>
          <p><textarea id="txtmsgcontent" runat="server" style="width:750px; height:240px;" class="forminput" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea></p>
   </div>

<!--邮件群发-->
<div id="div_email" style="display: none;">
      <div class="frame-content">
       <p><h4>发送对象(共<font style="color:Red">0</font>个会员)</h4></p>
       <div id="send_email" style="overflow-x:hidden;overflow-y:auto;margin-bottom:20px">
             <ul class="menber"></ul>
       </div>
       <p>
            <textarea id="txtemailcontent" runat="server" style="width:700px; height:240px;" class="forminput" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea>
       </p>
      </div>
</div>

<div style="display:none"> 
<input type="hidden" id="hdenablemsg" runat="server" value="0" />
<input type="hidden" id="hdenableemail" runat="server" value="0" />
<asp:Button ID="btnSendEmail" runat="server" Text="邮件群发" CssClass="submit_DAqueding" />
<asp:Button ID="btnSendMessage" runat="server" Text="短信群发" CssClass="submit_DAqueding" />
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">	
<script type="text/javascript" language="javascript">
    var formtype = "";
    //jquery控制上下显示
    $(document).ready(function () {
        var status = 1;
        $("#clickTopDown").click(function () {
            $("#dataArea").toggle(500, changeClass)
        })

        changeClass = function () {
            if (status == 1) {
                $("#clickTopDown").removeClass("clickTopX");
                $("#clickTopDown").addClass("clickTopS");
                status = 0;
            }
            else {
                $("#clickTopDown").removeClass("clickTopS");
                $("#clickTopDown").addClass("clickTopX");
                status = 1;
            }
        }
    });
    //样式控制
    function showcss(divobj, rownumber) {
        if (rownumber > 12) {
            $("#" + divobj).css("height", 100);
        }
    }

</script>

</asp:Content>  
