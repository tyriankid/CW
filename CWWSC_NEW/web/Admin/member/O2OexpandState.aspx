<%@ Page Language="C#" AutoEventWireup="true" CodeFile="O2OexpandState.aspx.cs" Inherits="Admin_member_O2OexpandState" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="cc1" TagName="AttributeView" Src="~/Admin/product/ascx/AttributeView.ascx" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/04.gif" width="32" height="32" /></em>
                <h1>添加新的录入会员</h1>
                <span>添加新的录入会员信息</span>
            </div>
            <asp:Panel ID="PanelID" runat="server">
                <div class="formtab">
                    <ul class="nav nav-tabs product_detail font-m">
                        <li><a href="addO2OMember.aspx?Id=<%=uid %>" >基本设置</a></li>
                        <li class="visited">扩展属性</li>
                    </ul>
                </div>
                <div class="formitem validator4">
                    <ul>
                        <li id="Family">
                            <span class="formitemtitle Pw_110">家庭成员构成</span>&nbsp;&nbsp;
                            <span><input type="button" id="btnFamily" value="新增" class="searchbutton" onclick="addFamily()" /></span>
                           <asp:Literal runat="server" ID="talbe1"></asp:Literal> 
                        </li>
                    </ul>
                     <ul>
                        <li id="House">
                            <span class="formitemtitle Pw_110">用户住房信息</span>&nbsp;&nbsp;
                             <span><input type="button" id="btnHouse" value="新增" class="searchbutton" onclick="addHouse()" /></span>
                            <asp:Literal runat="server" ID="talbe2"></asp:Literal>
                         
                        </li>
                    </ul>
                     <ul>
                        <li id="Configure">
                            <span class="formitemtitle Pw_110">房屋家电配置</span>&nbsp;&nbsp;
                             <span><input type="button" id="btnConfigure" value="新增" class="searchbutton" onclick="addConfigure()" /></span>
                            <asp:Literal runat="server" ID="talbe3"></asp:Literal>
                           
                        </li>
                    </ul>
                     <ul>
                        <li  id="makeElectrical">
                            <span class="formitemtitle Pw_110">家电使用情况</span>&nbsp;&nbsp;
                             <span><input type="button" id="btnMake" value="新增" class="searchbutton" onclick="addMake()" /></span>
                            <asp:Literal runat="server" ID="talbe4"></asp:Literal>
                           
                        </li>
                    </ul>
                     <ul>
                        <li id="Brand">
                            <span class="formitemtitle Pw_110">个人品牌倾向</span>&nbsp;&nbsp;
                             <span><input type="button" id="btnBrand" value="新增" class="searchbutton" onclick="addBrand()" /></span>
                            <asp:Literal runat="server" ID="talbe5"></asp:Literal>
                        </li>
                    </ul>
                     <ul>
                        <li id="Shop">
                            <span class="formitemtitle Pw_110">近期购买需求</span>&nbsp;&nbsp;
                             <span><input type="button" id="btnShop" value="新增" class="searchbutton" onclick="addShop()" /></span>
                             <asp:Literal runat="server" ID="talbe6"></asp:Literal>
                        </li>
                    </ul>
                    <ul class="btn Pa_110 clear">
                        <li>
                        <input id="btnCreate"  type="button" value="保存"  onclick="Save()" class="submit_DAqueding" style="float: left;" />
                    </li>
                            </ul>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        //家庭成员构成
       var rowCount = 0;
       function addFamily() {
            rowCount++;
            var strText = "";
            strText += "<tr id='Family" + rowCount + "' style='height:40px'>";
            $("#trFamily").find("td").each(function (i, e) {
                //i为元素的索引，从0开始,
                //e为当前处理的元素
                strText += "<td style='padding:5px'><input type='text' id='" + $(this).attr("id") + "' value='' class='forminput'></td>";
            });
            strText += "<td>&nbsp;<a href=\"javascript:delFamilyRow('" + rowCount + "')\">删除</a></td></tr>";
            $("#tbFamily").append(strText);
       }
       //删除行  
       function delFamilyRow(rowIndex) {
           $("#Family" + rowIndex).remove();
           rowCount--;
       }
       //用户住房信息
       var rowCount1=0
       function addHouse() {
           rowCount1++;
           var strText= "";
           strText += "<tr id='House" + rowCount1 + "' style='height:40px'>";
           $("#trHouse").find("td").each(function (i, e) {
               //i为元素的索引，从0开始,
               //e为当前处理的元素
               //alert($(this).html());
               strText += "<td style='padding:5px'><input type='text' id='" + $(this).attr("id") + "' value='' class='forminput'></td>";
           });
           strText += "<td>&nbsp;<a href=\"javascript:onclick=delHouseRow('" + rowCount1 + "')\" >删除</a></td></tr>";
           $("#tbHouse").append(strText);
       }
       function delHouseRow(rowIndex) {
           $("#House" + rowIndex).remove();
           rowCount1--;
       }
       //房屋家电配置
       var rowCount2= 0
       function addConfigure() {
           rowCount2++;
           var strText = "";
           strText += "<tr id='Configure" + rowCount2 + "' style='height:40px'>";
           
           $("#trConfigure").find("td").each(function (i, e) {
               //i为元素的索引，从0开始,
               //e为当前处理的元素
               //alert($(this).html());
               strText += "<td style='padding:5px'><input type='text' id='" + $(this).attr("id") + "' value='' class='forminput'></td>";
           });
           strText += "<td>&nbsp;<a href=\"javascript:onclick=delConfigureRow('" + rowCount2 + "')\">删除</a></td></tr>";
           $("#tbConfigure").append(strText);
       }

       function delConfigureRow(rowIndex) {
           $("#Configure" + rowIndex).remove();
           rowCount2--;
       }

        //家电使用情况
       var rowCount3= 0
       function addMake() {
           rowCount3++;
           var strText = "";
           strText += "<tr id='Make" + rowCount3 + "' style='height:40px'>";
           
           $("#trMake").find("td").each(function (i, e) {
               //i为元素的索引，从0开始,
               //e为当前处理的元素
               //alert($(this).html());
               strText += "<td style='padding:5px'><input type='text' id='" + $(this).attr("id") + "' value='' class='forminput'></td>";
           });
           strText += "<td>&nbsp;<a href=\"javascript:delMakeRow('" + rowCount3 + "')\">删除</a></td></tr>";
           $("#tbMake").append(strText);
       }
       function delMakeRow(rowIndex) {
           $("#Make" + rowIndex).remove();
           rowCount3--;
       }
        //个人品牌倾向
       var rowCount4= 0
       function addBrand() {
           rowCount4++;
           var strText = "";
           strText += "<tr id='Brand" + rowCount4 + "' style='height:40px'>";
           
           $("#trBrand").find("td").each(function (i, e) {
               //i为元素的索引，从0开始,
               //e为当前处理的元素
               //alert($(this).html());
               strText += "<td style='padding:5px'><input type='text' id='" + $(this).attr("id") + "' value='' class='forminput'></td>";
           });
           strText += "<td>&nbsp;<a href=\"javascript:delBrandRow('" + rowCount4 + "')\" >删除</a></td></tr>";
           $("#tbBrand").append(strText);
       }
       function delBrandRow(rowIndex) {
           $("#Brand" + rowIndex).remove();
           rowCount4--;
       }
        //近期购买需求
       var rowCount5= 0
       function addShop() {
           rowCount3++;
           var strText = "";
           strText += "<tr id='Shop" + rowCount5 + "' style='height:40px'>";
           
           $("#trShop").find("td").each(function (i, e) {
               //i为元素的索引，从0开始,
               //e为当前处理的元素
              // alert($(this).html());
               strText += "<td style='padding:5px'><input type='text' id='" + $(this).attr("id") + "' value='' class='forminput'></td>";
           });
           strText += "<td>&nbsp;<a href=\"javascript:delShopRow('" + rowCount5 + "')\">删除</a></td></tr>";
           $("#tbShop").append(strText);
       }
       function delShopRow(rowIndex) {
           $("#Shop" + rowIndex).remove();
           rowCount5--;
       }

       function Save() {
            //家庭成员构成
            var content = "";
            var FId="";
            var Family = document.getElementById('tbFamily').getElementsByTagName('input');
    
            $("#tbFamily tr").find("input").each(function (i, e)
            {
                FId+= $(this).attr("id")+",";
            })
           //获取所填的值
            for (var i =0; i < Family.length; i++)
            {
                content+= Family[i].value + ","; 
            }
            //用户住房信息
            var content1 = "";
            var HId = "";
            var House = document.getElementById('tbHouse').getElementsByTagName('input');
           
            for (var i = 0; i < House.length; i++)
            {
                content1 += House[i].value + ",";
            }
            $("#tbHouse tr").find("input").each(function (i, e){
                HId += $(this).attr("id") + ",";
            })
            //房屋家电配置
            var content2 = "";
            var EId = "";
            var Configure = document.getElementById('tbConfigure').getElementsByTagName('input');
            for (var i = 0; i < Configure.length; i++) {
                content2 += Configure[i].value + ",";
            }
          
            $("#tbConfigure tr").find("input").each(function (i, e) {
                EId += $(this).attr("id") + ",";
            })
            //家电使用情况
            var content3 = "";
            var MId = "";
            var makeElectrical = document.getElementById('tbMake').getElementsByTagName('input');
            for (var i = 0; i < makeElectrical.length; i++) {
                content3 += makeElectrical[i].value + ",";
            }
            $("#tbMake").find("input").each(function (i, e) {
                MId += $(this).attr("id") + ",";
            })
            //个人品牌倾向
            var content4 = "";
            var BId = "";
            var Brand = document.getElementById('tbBrand').getElementsByTagName('input');
            for (var i = 0; i < Brand.length; i++) {
                content4 += Brand[i].value + ",";
            }
            $("#tbBrand  tr").find("input").each(function (i, e) {
                BId += $(this).attr("id") + ",";
            })
            //近期购买需求
            var content5 = "";
            var SId = "";
            var Shop = document.getElementById('tbShop').getElementsByTagName('input');
            for (var i = 0; i < Shop.length; i++) {
                content5 += Shop[i].value + ",";

            }
            $("#tbShop").find("input").each(function (i, e) {
                SId += $(this).attr("id") + ",";
            })
            var ListInfo = content + "&" + content1 + "&" + content2 + "&" + content3 + "&" + content4 + "&" + content5;
            var ListId = FId + "&" + HId + "&" + EId + "&" + MId + "&" + BId + "&" + SId;
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "O2OexpandState.aspx/Save",
                data: "{ListInfo:'" + ListInfo + "',ListIdInfo:'" + ListId + "'}",
                dataType: 'json',
                success: function (result) {
                    alert(result.d)
                }
            });
        }
    </script>
</asp:Content>
