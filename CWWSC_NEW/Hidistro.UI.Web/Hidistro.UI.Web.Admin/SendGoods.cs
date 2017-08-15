using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using Hishop.Weixin.MP.Util;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.OrderSendGoods)]
	public class SendGoods : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnSendGoods;
		protected ExpressRadioButtonList expressRadioButtonList;
		protected Order_ItemsList itemsList;
		protected System.Web.UI.WebControls.Label lblOrderId;
		protected FormatedTimeLabel lblOrderTime;
		protected System.Web.UI.WebControls.Literal litReceivingInfo;
		protected System.Web.UI.WebControls.Label litRemark;
		protected System.Web.UI.WebControls.Literal litShippingModeName;
		protected System.Web.UI.WebControls.Label litShipToDate;
		private string orderId;
		protected ShippingModeRadioButtonList radioShippingMode;
		protected System.Web.UI.WebControls.TextBox txtShipOrderNumber;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtShipOrderNumberTip;
		private void BindExpressCompany(int modeId)
		{
			this.expressRadioButtonList.ExpressCompanies = SalesHelper.GetExpressCompanysByMode(modeId);
			this.expressRadioButtonList.DataBind();
		}
		private void BindOrderItems(OrderInfo order)
		{
			this.lblOrderId.Text = order.OrderId;
			this.lblOrderTime.Time = order.OrderDate;
			this.itemsList.Order = order;
		}
		private void BindShippingAddress(OrderInfo order)
		{
			string shippingRegion = string.Empty;
			if (!string.IsNullOrEmpty(order.ShippingRegion))
			{
				shippingRegion = order.ShippingRegion;
			}
			if (!string.IsNullOrEmpty(order.Address))
			{
				shippingRegion += order.Address;
			}
			if (!string.IsNullOrEmpty(order.ShipTo))
			{
				shippingRegion = shippingRegion + "  " + order.ShipTo;
			}
			if (!string.IsNullOrEmpty(order.ZipCode))
			{
				shippingRegion = shippingRegion + "  " + order.ZipCode;
			}
			if (!string.IsNullOrEmpty(order.TelPhone))
			{
				shippingRegion = shippingRegion + "  " + order.TelPhone;
			}
			if (!string.IsNullOrEmpty(order.CellPhone))
			{
				shippingRegion = shippingRegion + "  " + order.CellPhone;
			}
			this.litReceivingInfo.Text = shippingRegion;
		}
		private void btnSendGoods_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			if (orderInfo != null)
			{
				ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
				if (currentManager != null)
				{
					if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Success)
					{
						this.ShowMsg("当前订单为团购订单，团购活动还未成功结束，所以不能发货", false);
					}
					else
					{
						if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS))
						{
							this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
						}
						else
						{
							if (!this.radioShippingMode.SelectedValue.HasValue)
							{
								this.ShowMsg("请选择配送方式", false);
							}
							else
							{
								if (string.IsNullOrEmpty(this.txtShipOrderNumber.Text.Trim()) || this.txtShipOrderNumber.Text.Trim().Length > 20)
								{
									this.ShowMsg("运单号码不能为空，在1至20个字符之间", false);
								}
								else
								{
									if (string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
									{
										this.ShowMsg("请选择物流公司", false);
									}
									else
									{
										ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);

										orderInfo.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
										orderInfo.RealModeName = shippingMode.Name;
										ExpressCompanyInfo info4 = ExpressHelper.FindNode(this.expressRadioButtonList.SelectedValue);
										if (info4 != null)
										{
											orderInfo.ExpressCompanyAbb = info4.Kuaidi100Code;
											orderInfo.ExpressCompanyName = info4.Name;
										}
										orderInfo.ShipOrderNumber = this.txtShipOrderNumber.Text;
                                        //如果是供应商订单，则先调用AH发货接口
                                        if (orderInfo.OrderSource == 2)
                                        {
                                            //先访问接口  接口成功执行后面代码
                                            StringBuilder strJson = new StringBuilder();
                                            strJson.Append("{");
                                            strJson.AppendFormat("\"ReceivName\":\"{0}\",", orderInfo.ShipTo);//收货人名称
                                            strJson.AppendFormat("\"ReceivTel\":\"{0}\",", orderInfo.TelPhone);//收货人手机
                                            strJson.AppendFormat("\"ReceivPhone\":\"{0}\",", orderInfo.CellPhone);//收货人电话
                                            strJson.AppendFormat("\"ReceivProvice\":\"{0}\",", orderInfo.RegionId.ToString().Substring(0, 2));//收货人地址行政区域省的编码
                                            strJson.AppendFormat("\"ReceivCity\":\"{0}\",", orderInfo.RegionId.ToString().Substring(0, 4));//收货人地址行政区域市的编码
                                            strJson.AppendFormat("\"ReceivArea\":\"{0}\",", orderInfo.RegionId.ToString());//收货人地址行政区域区的编码
                                            strJson.AppendFormat("\"ReceivAddress\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Address));//收货人详细地址
                                            strJson.AppendFormat("\"TaxName\":\"{0}\",", "创维直销部");//发票抬头
                                            strJson.AppendFormat("\"TaxPhone\":\"{0}\",", "027-81234567");//发票电话
                                            strJson.AppendFormat("\"TaxMailAdd\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Address));//发票邮寄地址
                                            strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);//订单号
                                            strJson.AppendFormat("\"OrderNote\":\"{0}\",", orderInfo.Remark);//买家留言
                                            strJson.AppendFormat("\"DelivCode\":\"{0}\",", orderInfo.ExpressCompanyAbb);//快递公司代码
                                            strJson.AppendFormat("\"DelivName\":\"{0}\",", orderInfo.ExpressCompanyName);//快递公司名称
                                            strJson.AppendFormat("\"DelivOrderCode\":\"{0}\"", orderInfo.ShipOrderNumber);//快递单号
                                            //strJson.AppendFormat("\"SerialNum\":\"{0}\"", serialNum);//serialNum 交易流水号 
                                            strJson.Append("}");
                                            CwAHAPI.CwapiLog("**************发送数据：" + strJson.ToString());
                                            AllHereServiceReference.MPFTOJLClient aa = new AllHereServiceReference.MPFTOJLClient();
                                            try
                                            {
                                                string strResult = aa.MPFTOJL_DHD_FH(strJson.ToString());
                                                CwAHAPI.CwapiLog("**************返回数据：" + strResult);

                                                string orderid;
                                                string message;
                                                if (string.IsNullOrEmpty(strResult))
                                                {
                                                    //接口没有返回值
                                                    this.ShowMsg("发货失败，AH接口返回数据错误！", false);
                                                    return;
                                                }
                                                if (CwAHAPI.ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                                                {
                                                    //接口返回订单编号错误
                                                    this.ShowMsg(string.Format("发货失败，原因：{0}", message), false);
                                                    return;
                                                }
                                            }
                                            catch 
                                            {
                                                //接口返回订单编号错误
                                                this.ShowMsg(string.Format("操作失败，原因：访问金力发货接口无响应。"), false);
                                                return;
                                            }
                                        }
                                        //处理发货相关内容
										if (OrderHelper.SendGoods(orderInfo))
										{
											SendNoteInfo info5 = new SendNoteInfo();
											info5.NoteId = Globals.GetGenerateId();
											info5.OrderId = this.orderId;
											info5.Operator = currentManager.UserName;
											info5.Remark = "后台" + info5.Operator + "发货成功";
											OrderHelper.SaveSendNote(info5);
											MemberInfo member = MemberHelper.GetMember(orderInfo.UserId);
											Messenger.OrderShipping(orderInfo, member);
											if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
											{
												if (orderInfo.Gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")
												{
													PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
													if (paymentMode != null)
													{
														PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
														{
															paymentMode.Gateway
														})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
														{
															paymentMode.Gateway
														})), "").SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
													}
												}
												if (orderInfo.Gateway == "hishop.plugins.payment.weixinrequest")
												{
													SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
													PayClient client = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
													DeliverInfo deliver = new DeliverInfo
													{
														TransId = orderInfo.GatewayOrderId,
														OutTradeNo = orderInfo.OrderId,
														OpenId = MemberHelper.GetMember(orderInfo.UserId).OpenId
													};
													client.DeliverNotify(deliver);
												}
											}
											orderInfo.OnDeliver();

                                            //发送数据到AH接口，提示AH已经发货

                                            this.ShowMsg("发货成功", true);
										}
										else
										{
											this.ShowMsg("发货失败", false);
										}
									}
								}
							}
						}
					}
				}
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.orderId = this.Page.Request.QueryString["OrderId"];

                //判断当前订单的状态,是否处理申请退款/退货中
                System.Data.DataTable dtReturns = RefundHelper.GetOrderReturnsBySwr(string.Format("orderId='{0}' order by ApplyForTime desc ", this.orderId));
                if (dtReturns.Rows.Count > 0 && (dtReturns.Rows[0]["HandleStatus"].ToInt() == 4 || dtReturns.Rows[0]["HandleStatus"].ToInt() == 6))
                {
                    btnSendGoods.Enabled = false;
                    this.ShowMsg(string.Format("当前订单已申请{0}", dtReturns.Rows[0]["HandleStatus"].ToInt() == 4 ? "退货" : "退款"), false);
                }

				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
				this.BindOrderItems(orderInfo);
				this.btnSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
				this.radioShippingMode.SelectedIndexChanged += new System.EventHandler(this.radioShippingMode_SelectedIndexChanged);
				if (!this.Page.IsPostBack)
				{
					if (orderInfo == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.radioShippingMode.DataBind();
						this.radioShippingMode.SelectedValue = new int?(orderInfo.ShippingModeId);
						this.BindExpressCompany(orderInfo.ShippingModeId);
						this.expressRadioButtonList.SelectedValue = orderInfo.ExpressCompanyAbb;
						this.BindShippingAddress(orderInfo);
						this.litShippingModeName.Text = orderInfo.ModeName;
						this.litShipToDate.Text = orderInfo.ShipToDate;
						this.litRemark.Text = orderInfo.Remark;
						this.txtShipOrderNumber.Text = orderInfo.ShipOrderNumber;
					}
                    checkRadio();
                    if (CustomConfigHelper.Instance.IsSanzuo)
                    {
                        radioShippingMode.SelectedIndex=0;
                        expressRadioButtonList.SelectedIndex = 0;
                        txtShipOrderNumber.Text = "店内配送";
                        btnSendGoods_Click(null,null);
                    }
				}
                
			}
		}
		private void radioShippingMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            checkRadio();
		}


        private void checkRadio()
        {
            
                if (this.radioShippingMode.SelectedValue.HasValue)
                {
                    this.BindExpressCompany(this.radioShippingMode.SelectedValue.Value);
                    //新增判断:如果选择是上门自提,则不需要绑定数据,并且快递和快递单号无效
                    if (this.radioShippingMode.SelectedItem.Text.IndexOf("自提") > -1)
                    {
                        this.expressRadioButtonList.SelectedIndex = 0;
                        this.expressRadioButtonList.Enabled = false;
                        this.txtShipOrderNumber.Text = "上门自提";
                        this.txtShipOrderNumber.Enabled = false;
                    }
                    else
                    {
                        this.expressRadioButtonList.Enabled = true;
                        this.txtShipOrderNumber.Text = "";
                        this.BindExpressCompany(this.radioShippingMode.SelectedValue.Value);
                        this.expressRadioButtonList.SelectedIndex = 0;
                        this.txtShipOrderNumber.Enabled = true;
                    }
                }
            
        }

	}
}
