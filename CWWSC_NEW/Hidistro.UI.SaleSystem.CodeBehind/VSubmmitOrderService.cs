namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Config;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VSubmmitOrderService : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlAnchor aLinkToShipping;
        private int buyAmount;
        private Common_CouponSelect dropCoupon;
        private Common_UserRedPagerSelect dropRedPager;
        private Common_ShippingTypeSelect dropShippingType;
        private HtmlInputControl groupbuyHiddenBox;
        private int groupBuyId;
        private HtmlInputControl countdownHiddenBox;
        private int countDownId;
        private HtmlInputControl cutdownHiddenBox;
        private int cutDownId;

        private HtmlInputHidden hiddenCartTotal;
        private Literal litAddAddress;
        private Literal litAddReceipt1;//电子发票 设置连接
        private Literal litAddReceipt2;//增值税发票设置连接
        private Literal litAddress;
        private Literal litCellPhone;
        private Literal litExemption;
        private Literal litOrderTotal;
        private Literal litTotalPoint;//所需积分
        private Literal litProductTotalPrice;
        private Literal litShipTo;
        private Literal litDefalutReceiptName;//公司名称

        private string productSku;
        private HtmlInputHidden regionId;
        private VshopTemplatedRepeater rptAddress;
        private VshopTemplatedRepeater rptReceipt;
        private VshopTemplatedRepeater rptCartProducts;//商品
        private VshopTemplatedRepeater rptCartGifts;//礼品
        private HtmlInputHidden selectShipTo;
        private HtmlInputHidden selectReceipt;
        private HtmlInputHidden productSource;//商品来源(服务)
        private HtmlInputHidden productId;//服务商品ID

        protected override void AttachChildControls()
        {
            this.litShipTo = (Literal)this.FindControl("litShipTo");
            this.litCellPhone = (Literal)this.FindControl("litCellPhone");
            this.litAddress = (Literal)this.FindControl("litAddress");

            this.litDefalutReceiptName = (Literal)this.FindControl("litDefalutReceiptName");//公司名称

            this.rptCartProducts = (VshopTemplatedRepeater)this.FindControl("rptCartProducts");//商品
            this.rptCartGifts = (VshopTemplatedRepeater)this.FindControl("rptCartGifts");//礼品
            this.dropShippingType = (Common_ShippingTypeSelect)this.FindControl("dropShippingType");
            this.dropCoupon = (Common_CouponSelect)this.FindControl("dropCoupon");
            this.dropRedPager = (Common_UserRedPagerSelect)this.FindControl("dropRedPager");
            this.litOrderTotal = (Literal)this.FindControl("litOrderTotal");
            this.litTotalPoint = (Literal)this.FindControl("litTotalPoint");//所需积分
            this.hiddenCartTotal = (HtmlInputHidden)this.FindControl("hiddenCartTotal");
            this.aLinkToShipping = (HtmlAnchor)this.FindControl("aLinkToShipping");
            this.groupbuyHiddenBox = (HtmlInputControl)this.FindControl("groupbuyHiddenBox");
            this.countdownHiddenBox = (HtmlInputControl)this.FindControl("countdownHiddenBox");
            this.cutdownHiddenBox = (HtmlInputControl)this.FindControl("cutdownHiddenBox");
            this.rptAddress = (VshopTemplatedRepeater)this.FindControl("rptAddress");
            this.rptReceipt = (VshopTemplatedRepeater)this.FindControl("rptReceipt");
            this.selectShipTo = (HtmlInputHidden)this.FindControl("selectShipTo");
            this.selectReceipt = (HtmlInputHidden)this.FindControl("selectReceipt");
            this.regionId = (HtmlInputHidden)this.FindControl("regionId");
            Literal literal = (Literal)this.FindControl("litProductTotalPrice");
            this.litExemption = (Literal)this.FindControl("litExemption");
            this.litAddAddress = (Literal)this.FindControl("litAddAddress");
            this.litAddReceipt1 = (Literal)this.FindControl("litAddReceipt1");
            this.litAddReceipt2 = (Literal)this.FindControl("litAddReceipt2");
            this.productSource = (HtmlInputHidden)this.FindControl("productSource");
            this.productId = (HtmlInputHidden)this.FindControl("productId");

            this.litExemption.Text = "0.00";
            IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
            this.rptAddress.DataSource = from item in shippingAddresses
                                         orderby item.IsDefault
                                         select item;
            this.rptAddress.DataBind();
            this.rptCartProducts.ItemDataBound += new RepeaterItemEventHandler(this.rptCartProducts_ItemDataBound);
            ShippingAddressInfo info = shippingAddresses.FirstOrDefault<ShippingAddressInfo>(item => item.IsDefault);
            if (info == null)
            {
                info = (shippingAddresses.Count > 0) ? shippingAddresses[0] : null;
            }
            if (info != null)
            {
                this.litShipTo.Text = info.ShipTo;
                this.litCellPhone.Text = info.CellPhone;
                this.litAddress.Text = info.Address;
                this.selectShipTo.SetWhenIsNotNull(info.ShippingId.ToString());
                this.regionId.SetWhenIsNotNull(info.RegionId.ToString());
            }
            this.litAddAddress.Text = " href='/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "'";
            if ((shippingAddresses == null) || (shippingAddresses.Count == 0))
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
            }
            else
            {
                this.aLinkToShipping.HRef = Globals.ApplicationPath + "/Vshop/ShippingAddresses.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString());
                ShoppingCartInfo shoppingCart = null;
                string msg = "";
                string str = this.Page.Request.QueryString["from"];
                if (((int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"])) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"])) && ((this.Page.Request.QueryString["from"] == "signBuy") || (this.Page.Request.QueryString["from"] == "groupBuy") || (this.Page.Request.QueryString["from"] == "countDown") || (this.Page.Request.QueryString["from"] == "cutDown")))
                {
                    this.productSku = this.Page.Request.QueryString["productSku"];

                    //团购//待完善
                    if ((str == "groupBuy") && int.TryParse(this.Page.Request.QueryString["groupbuyId"], out this.groupBuyId))
                    {
                        this.groupbuyHiddenBox.SetWhenIsNotNull(this.groupBuyId.ToString());
                        shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(this.groupBuyId, this.productSku, this.buyAmount);
                    }
                    //限时抢购
                    else if ((str == "countDown") && int.TryParse(this.Page.Request.QueryString["countdownId"], out this.countDownId))
                    {
                        this.countdownHiddenBox.SetWhenIsNotNull(this.countDownId.ToString());
                        CountDownInfo info4 = ProductBrowser.GetCountDownInfo(this.countDownId, this.buyAmount, out msg);
                        if (info4 == null)
                        {
                            base.GotoResourceNotFound(msg);
                            return;
                        }
                        if (string.IsNullOrEmpty(this.productSku) || (this.productSku.Split(new char[] { '_' })[0] != info4.ProductId.ToString()))
                        {
                            base.GotoResourceNotFound("错误的商品信息");
                            return;
                        }

                        shoppingCart = ShoppingCartProcessor.GetCountDownShoppingCart(this.productSku, this.buyAmount);
                    }
                    //砍价
                    else if ((str == "cutDown") && int.TryParse(this.Page.Request.QueryString["cutDownId"], out this.cutDownId))
                    {
                        this.cutdownHiddenBox.SetWhenIsNotNull(this.cutDownId.ToString());
                        CutDownInfo info5 = PromoteHelper.GetCutDown(this.cutDownId);
                        if (info5 == null)
                        {
                            base.GotoResourceNotFound();
                            return;
                        }
                        if (string.IsNullOrEmpty(this.productSku) || (this.productSku.Split(new char[] { '_' })[0] != info5.ProductId.ToString()))
                        {
                            base.GotoResourceNotFound("错误的商品信息");
                            return;
                        }

                        shoppingCart = ShoppingCartProcessor.GetCutDownShoppingCart(this.productSku, this.buyAmount, info5.CutDownId);
                    }
                    else
                    {
                        shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
                    }
                }
                else
                {
                    //先判断是否是礼品提交
                    string IsGift = this.Page.Request.QueryString["isGift"];
                    if (!string.IsNullOrEmpty(IsGift) && IsGift == "1" && buyAmount > 0)
                    {
                        //清楚礼品购物车
                        ShoppingCartProcessor.ClearGiftShoppingCart();
                        int GiftId = 0;
                        if (int.TryParse(this.Page.Request.QueryString["giftid"].ToString(), out GiftId))
                        {
                            //先添加礼品信息到购物车表
                            if (ShoppingCartProcessor.AddGiftItem(GiftId, buyAmount))
                            {
                                //获取礼品商品信息
                                shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                            }
                        }
                    }
                    else
                        shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                }
                

                #region 设置发票信息

                IList<UserReceiptInfo> userReceiptList = UserReceiptInfoHelper.GetUserReceiptInfo();
                this.rptReceipt.DataSource = from item in userReceiptList
                                             orderby item.IsDefault descending, item.Type   
                                             select item;
                this.rptReceipt.DataBind();

                UserReceiptInfo receiptinfo = userReceiptList.FirstOrDefault<UserReceiptInfo>(item => (item.IsDefault == 1));
                if (receiptinfo == null)
                {
                    this.litDefalutReceiptName.Text = "请选择发票类型";
                    receiptinfo = (userReceiptList.Count > 0) ? userReceiptList[0] : null;
                }
                if (receiptinfo != null)
                {
                    if(receiptinfo.Type == 0)
                        this.litDefalutReceiptName.Text = "电子发票：" + (receiptinfo.Type1 == 0 ? "【个人】 " : "【单位】 ") + "&nbsp;&nbsp;" + receiptinfo.CompanyName + " " + receiptinfo.TaxesCode;
                    else
                        this.litDefalutReceiptName.Text = "增值税发票：" + receiptinfo.CompanyName + " " + receiptinfo.TaxesCode;
                    this.selectReceipt.SetWhenIsNotNull(receiptinfo.ReceiptId.ToString());
                }

                this.litAddReceipt1.Text = " href='/Vshop/AddReceiptDz.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "'";
                this.litAddReceipt2.Text = " href='/Vshop/AddReceipt.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "'";

                #endregion 设置发票信息

                //获取/新建了购物车对象后,进行商品价格根据分销商特殊优惠设置进行减价处理.
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                int distributorid = Globals.GetCurrentDistributorId();
                //根据分销商的特殊优惠设置进行计算返佣
                switch (SettingsManager.GetMasterSettings(false).DistributorCutOff)
                {
                    case "default"://直接退出,没有任何操作一切照常

                        break;
                    case "bycostprice"://根据进货价进行返佣
                        if (currentMember.UserId != distributorid)//如果不是分销商购买自己的产品,则退出特殊处理
                            break;
                        for (int i = 0; i < shoppingCart.LineItems.Count; i++)
                        {
                            shoppingCart.LineItems[i].AdjustedPrice = ProductHelper.GetSkuCostPrice(shoppingCart.LineItems[i].SkuId);
                        }
                        break;

                    default://根据指定折扣进行返佣(此种情况下,不存在直接分佣)
                        if (currentMember.UserId != distributorid)//如果不是分销商购买自己的产品,则退出特殊处理
                            break;
                        decimal rate = Convert.ToDecimal(SettingsManager.GetMasterSettings(false).DistributorCutOff) / 100;//获取折扣比例
                        for (int i = 0; i < shoppingCart.LineItems.Count; i++)
                        {
                            shoppingCart.LineItems[i].AdjustedPrice = shoppingCart.LineItems[i].AdjustedPrice * rate;
                        }
                        break;
                }

                if (shoppingCart != null)
                {
                    //提交订单前先检查是否达到规则要求上限
                    if (!TradeHelper.CheckShoppingStock(shoppingCart, out msg))
                    {
                        this.ShowMessage("订单中有商品(" + msg + ")库存不足", false);
                        if (!this.Page.ClientScript.IsClientScriptBlockRegistered("AlertStockScript"))
                        {
                            StringBuilder builder = new StringBuilder();
                            builder.AppendLine("var stockError = true;");
                            builder.AppendLine(string.Format("var stockErrorInfo=\"订单中有商品({0})库存不足,请返回购物车修改库存.\";", msg));
                            this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "AlertStockScript", "var stockError=true;function AlertStock(){alert('订单中有商品(" + msg + ")库存不足,请返回购物车修改库存.');}", true);
                        }
                    }

                    bool isPf = false;//是否启用了批发价格
                    bool isNeigou = false;//是否启用了内购价格，如果是内购门店并且有内购价格才有值
                    if (shoppingCart.LineItems.Count > 0)
                    {
                        this.productSource.SetWhenIsNotNull(shoppingCart.LineItems[0].ProductSource.ToString());//存入前端变量值。
                        this.productId.SetWhenIsNotNull(shoppingCart.LineItems[0].ProductId.ToString());//存入前端变量值。

                        //2017-08-14-wt 验证批发价格、内购价格，  如果购买是批发价格、内购价格则不享有其他的价格优惠
                        VshopBrowser.ValidatePriceAndSetValue(currentMember, shoppingCart.LineItems[0], out isPf, out isNeigou);
                    }

                    #region *************Star   2017-8-11 yk  买一送一商品修改***********************
                    if (!isPf && !isNeigou)
                    {
                        foreach (ShoppingCartItemInfo infoFree in shoppingCart.LineItems)
                        {
                            #region 如果该商品是买一送一活动商品并且在活动时间内并且该用户拥有该活动资格，则添加赠送商品
                            //得到该商品活动的实体
                            BuyOneGetOneFreeInfo buyOneInfo = BuyOneGetOnrFreeHelper.GetProductBuyOneGetOneFreeInfo(infoFree.ProductId);
                            if (buyOneInfo != null)
                            {
                                //得到该用户参与该商品活动的【总次数】
                                int getNum = BuyOneGetOnrFreeHelper.getUserGetNum(currentMember.UserId, buyOneInfo.buyoneId);
                                //当前用户对于活动的【剩余赠送次数】
                                int Surplus = buyOneInfo.getNum - getNum;

                                if (buyOneInfo != null && DateTime.Now > buyOneInfo.startime && DateTime.Now < buyOneInfo.endtime)//活动时间验证
                                {
                                    if (Surplus > 0)//表示当前用户还有资格参与活动
                                    {

                                        if (infoFree.Quantity > Surplus)//如果【购买数量】大于【剩余赠送次数】则赠送最大数量为【剩余赠送次数】
                                        {
                                            infoFree.Quantity = infoFree.Quantity + buyOneInfo.getNum;
                                            infoFree.GiveQuantity = buyOneInfo.getNum;
                                        }
                                        else
                                        {
                                            infoFree.GiveQuantity = infoFree.Quantity;//赠送数量 
                                            infoFree.Quantity = infoFree.Quantity * 2;//实际数量
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion *************End       2017-8-11 yk  买一送一商品修改***********************

                    this.rptCartProducts.DataSource = shoppingCart.LineItems;
                    this.rptCartProducts.DataBind();
                    
                    //绑定礼品
                    this.rptCartGifts.DataSource = shoppingCart.LineGifts;
                    this.rptCartGifts.DataBind();
                    //绑定配送方式
                    this.dropShippingType.ShoppingCartItemInfo = shoppingCart.LineItems;
                    this.dropShippingType.RegionId = 0;
                    this.dropShippingType.Weight = shoppingCart.Weight;
                    this.dropCoupon.CartTotal = shoppingCart.GetTotal();

                    
                    string noCouponIds = string.Empty;
                    string tempCategoryId = string.Empty;
                    DataTable dtDB = CouponHelper.GetCouponAllCate(); //new DataTable();//设置的使用权限的有效期内的优惠卷 两表关联信息
                    foreach (DataRow dr in dtDB.Rows)
                    {
                        tempCategoryId = "," + dr["CategoryIds"].ToString() + ",";
                        decimal dAmount = (decimal)dr["Amount"];

                        //计算排除掉不能参与的分类金额
                        decimal dAmount2 = dropCoupon.CartTotal;
                        foreach (ShoppingCartItemInfo scii in shoppingCart.LineItems)
                        {
                            if (tempCategoryId.IndexOf("," + scii.CategoryId + ",") > -1)
                                dAmount2 -= scii.SubTotal;
                        }

                        //待排除的
                        if (dAmount > dAmount2)
                        {
                            noCouponIds += dr["CouponId"] + ",";
                        }

                    }
                    if (noCouponIds != "")
                    {
                        noCouponIds = noCouponIds.TrimEnd(',');
                    }
                    this.dropCoupon.CouponIds = noCouponIds;
                    this.dropRedPager.CartTotal = shoppingCart.GetTotal();
                    this.hiddenCartTotal.Value = literal.Text = shoppingCart.GetTotal().ToString("F2");
                    
                    /******Start 2017-08-14-wt 修改*************/
                    decimal num = 0;
                    //无批发价、无内购价，则计算活动价格
                    if (!isPf && !isNeigou)
                    {
                        num = this.DiscountMoney(shoppingCart.LineItems);
                    }
                    /*******************/
                    this.litOrderTotal.Text = (shoppingCart.GetTotal() - num).ToString("F2");
                    this.litTotalPoint.Text = getTotalPoints(shoppingCart).ToString();
                    this.litExemption.Text = num.ToString("0.00");
                }
                else
                {
                    this.Page.Response.Redirect("/Vshop/ShoppingCart.aspx");
                }
                PageTitle.AddSiteNameTitle("订单确认");
            }
        }

        /// <summary>
        /// 计算所需积分
        /// </summary>
        /// <returns></returns>
        private int getTotalPoints(ShoppingCartInfo shoppingCart)
        {
            int result = 0;
            foreach (ShoppingCartGiftInfo gift in shoppingCart.LineGifts)
            {
                result += gift.NeedPoint * gift.Quantity;
            }

            return result;
        }


        public decimal DiscountMoney(IList<ShoppingCartItemInfo> infoList)
        {
            decimal num = 0M;
            decimal num2 = 0M;
            decimal num3 = 0M;
            DataTable type = ProductBrowser.GetType();
            for (int i = 0; i < type.Rows.Count; i++)
            {
                decimal num5 = 0M;
                foreach (ShoppingCartItemInfo info in infoList)
                {
                    if (!string.IsNullOrEmpty(info.MainCategoryPath) && ((int.Parse(type.Rows[i]["ActivitiesType"].ToString()) == int.Parse(info.MainCategoryPath.Split(new char[] { '|' })[0].ToString())) || (int.Parse(type.Rows[i]["ActivitiesType"].ToString()) == 0)))
                    {
                        num5 += info.SubTotal;
                    }
                }
                if (num5 != 0M)
                {
                    DataTable allFull = ProductBrowser.GetAllFull(int.Parse(type.Rows[i]["ActivitiesType"].ToString()));
                    if (allFull.Rows.Count > 0)
                    {
                        for (int j = 0; j < allFull.Rows.Count; j++)
                        {
                            if (num5 >= decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["MeetMoney"].ToString()))
                            {
                                num2 = decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["MeetMoney"].ToString());
                                num = decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["ReductionMoney"].ToString());
                                break;
                            }
                            if (num5 <= decimal.Parse(allFull.Rows[0]["MeetMoney"].ToString()))
                            {
                                num2 = decimal.Parse(allFull.Rows[0]["MeetMoney"].ToString());
                                num += decimal.Parse(allFull.Rows[0]["ReductionMoney"].ToString());
                                break;
                            }
                            if (num5 >= decimal.Parse(allFull.Rows[j]["MeetMoney"].ToString()))
                            {
                                num2 = decimal.Parse(allFull.Rows[j]["MeetMoney"].ToString());
                                num = decimal.Parse(allFull.Rows[j]["ReductionMoney"].ToString());
                            }
                        }
                        if (num5 >= num2)
                        {
                            num3 += num;
                        }
                    }
                }
            }
            return num3;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VSubmmitOrderService.html";
            }
            base.OnInit(e);
        }

        private void rptCartProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
    }
}

