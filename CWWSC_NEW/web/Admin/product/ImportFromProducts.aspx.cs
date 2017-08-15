using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.TransferManager;
using Hidistro.Entities.Comments;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Config;
using Hidistro.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Commodities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;



namespace Hidistro.UI.Web.Admin.product
{

    [PrivilegeCheck(Privilege.ProductBatchUpload)]
    public partial class ImportFromProducts : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定数据
                BindCategories();
                BindProductType();
            }
        }
        /// <summary>
        /// 绑定商品分类
        /// </summary>
        public void BindCategories()
        {
            List<CategoryQuery> category = new List<CategoryQuery>();
            category = CouponHelper.GetHishop_Categories();
            dropCategories.DataSource = category;
            dropCategories.DataTextField = "Name";
            dropCategories.DataValueField = "CategoryId";
            dropCategories.DataBind();

        }
        /// <summary>
        /// 绑定商品分类、商品品牌
        /// </summary>
        public void BindProductType()
        {
            List<ProductTypeInfo> listProductTypeInfo = (List<ProductTypeInfo>)ProductTypeHelper.GetProductTypes();
            ProductType.DataSource = listProductTypeInfo;
            ProductType.DataTextField = "TypeName";
            ProductType.DataValueField = "TypeId";
            ProductType.DataBind();
            dropBrandList.DataSource = ProductTypeHelper.GetBrandCategoriesByTypeId(Convert.ToInt32(ProductType.SelectedValue));
            dropBrandList.DataTextField = "BrandName";
            dropBrandList.DataValueField = "BrandId";
            dropBrandList.DataBind();
        }
        protected override void OnInitComplete(System.EventArgs e)
        {

        }
        /// <summary>
        /// 商品分类选中事件， 绑定商品品牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropBrandList.DataSource = ProductTypeHelper.GetBrandCategoriesByTypeId(Convert.ToInt32(ProductType.SelectedValue));
            dropBrandList.DataTextField = "BrandName";
            dropBrandList.DataValueField = "BrandId";
            dropBrandList.DataBind();
        }
        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BcBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //获取商品状态
                int SaleStatus = 0;
                if (radOnSales.Checked == true)
                {
                    SaleStatus = 1;
                }
                else
                {
                    SaleStatus = 3;
                }
                //获取商品分类、商品类型、商品品牌值
                int productCategories = Convert.ToInt32(dropCategories.SelectedValue);
                int TypeProduct = Convert.ToInt32(ProductType.SelectedValue);
                int productBrand = Convert.ToInt32(dropBrandList.SelectedValue);
                //判断选择文件的相关信息
                if (!string.IsNullOrEmpty(ProductFile.FileName) && ProductFile.FileContent.Length > 0)
                {
                    string filePath = "";
                    filePath = MapPath("/Storage/temp/") + "yyyyMMddHHmmss" + ProductFile.FileName;
                    ProductFile.SaveAs(filePath);
                    if (Path.GetExtension(filePath).ToLower() != ".xls")
                    {
                        this.ShowMsg("导入的文件不是xls文件。", false);
                        return;
                    }
                    ExcelDBClass excelDBClass = new ExcelDBClass(filePath, true);
                    //Excel转为DataTable   将选中的表格转换为datatable
                    DataTable dtproductExcel = excelDBClass.ExportToDataSet().Tables[0];
                    //表头格式验证
                    string strHeaders = "商品名称,货号,市场价,一口价,成本价,商品库存";
                    //将表头字符串用,切割，装到arrayHeader数组
                    string[] arrayHeader = strHeaders.Split(',');
                    //循环表头与选中的表格里面标题是否一致
                    for (int i = 0; i < dtproductExcel.Columns.Count; i++)
                    {
                        if (dtproductExcel.Columns[i].ColumnName != arrayHeader[i])
                        {
                            this.ShowMsg("导入的文件格式不正确,请检查模板文件。", false);
                            return;
                        }
                    }
                    //获取数据库数据 转成DataTable
                    string strSql = "select * from Hishop_Products";
                    DataTable dtproduct = DataBaseHelper.GetDataSet(strSql).Tables[0];
                    string strSqlSKUs = "select * from Hishop_SKUs";
                    DataTable dtSKUs = DataBaseHelper.GetDataSet(strSqlSKUs).Tables[0];
                    //判断最后保存状态
                    bool isOk = true;
                    //判断货号是否为空，如果为空则删除
                    DataRow[] blank = dtproductExcel.Select("货号 is null");
                    for (int i = 0; i < blank.Count(); i++)
                    {
                        blank[i].Delete();
                    }
                    //在选择文件表格中，添加一下字段
                    dtproductExcel.AcceptChanges();
                    dtproductExcel.Columns.Add("商品分类", typeof(Int32));
                    dtproductExcel.Columns.Add("商品类型", typeof(Int32));
                    dtproductExcel.Columns.Add("商品品牌", typeof(Int32));
                    dtproductExcel.Columns.Add("商品状态", typeof(Int32));
                    dtproductExcel.Columns.Add("编号", typeof(Int32));
                    dtproductExcel.Columns.Add("状态", typeof(string));
                    string dtproductProductCode = null;
                    for (int i = 0; i < dtproductExcel.Rows.Count; i++)
                    {
                        //通过循环，将商品货号累加成字符串，便于保存时添加库存表
                        if (i == dtproductExcel.Rows.Count-1)
                        {
                            dtproductProductCode += dtproductExcel.Rows[i]["货号"].ToString() ;
                        }
                        else
                        {
                            dtproductProductCode += dtproductExcel.Rows[i]["货号"].ToString() + ",";                        
                        }
                        //对添加字段进行赋值
                        dtproductExcel.Rows[i]["编号"] = i + 1;
                        dtproductExcel.Rows[i]["商品分类"] = productCategories;
                        dtproductExcel.Rows[i]["商品类型"] = TypeProduct;
                        dtproductExcel.Rows[i]["商品品牌"] = productBrand;
                        dtproductExcel.Rows[i]["商品状态"] = SaleStatus;
                        //判断选中文件时候有重复的货号
                        DataRow[] dr = dtproductExcel.Select(string.Format("货号 = '{0}'", dtproductExcel.Rows[i]["货号"].ToString()), "", DataViewRowState.CurrentRows);
                        if (dr.Length > 1)
                        {
                            isOk = false;
                            dr[dr.Length - 1]["状态"] = "文件中有重复货号！";
                        }
                        else
                        {
                            //判断数据库Product表中和选中文件表中是否有相同货号
                            DataRow[] drs = dtproduct.Select(string.Format(@"ProductCode='{0}'", dtproductExcel.Rows[i]["货号"].ToString()), "", DataViewRowState.CurrentRows);
                            if (drs.Length > 0)
                            {
                                isOk = false;
                                dtproductExcel.Rows[i]["状态"] = "选中文件和数据库中有重复的货号!";
                            }
                            else
                            {
                                //将选中文件添加到数据库product内存表中
                                DataRow drnew = dtproduct.NewRow();
                                drnew["CategoryId"] = dtproductExcel.Rows[i]["商品分类"];
                                drnew["TypeId"] = dtproductExcel.Rows[i]["商品类型"];
                                drnew["ProductName"] = dtproductExcel.Rows[i]["商品名称"];
                                drnew["ProductCode"] = dtproductExcel.Rows[i]["货号"];
                                drnew["BrandId"] = dtproductExcel.Rows[i]["商品品牌"];
                                drnew["MarketPrice"] = dtproductExcel.Rows[i]["市场价"];
                                drnew["SaleStatus"] = dtproductExcel.Rows[i]["商品状态"];
                                drnew["ProductSource"] = 1;
                                drnew["AddedDate"] = DateTime.Now;
                                drnew["HasSKU"] = 0;
                                drnew["Range"] = 0;

                                dtproduct.Rows.Add(drnew);
                            }
                        }
                    }
                    if (!isOk)
                    {
                        //this.ShowMsg("选择文件中有相同分公司名称，请检查后重新导入", false);
                        ViewState["dtPr"] = dtproductExcel;
                        //把当前保存状态存到ViewState中
                        ViewState["state"] = "no";
                    }
                    else
                    {
                        ViewState["ProductCode"] = dtproductProductCode;
                        ViewState["state"] = "ok";
                        //数据库中Product 内存表存到ViewState中
                        ViewState["dtPr"] = dtproduct;
                        //选中文件表存到ViewState中
                        ViewState["dtExcel"] = dtproductExcel;
                        //数据库 库存表 所有数据存到ViewState中
                        ViewState["dtus"] = dtSKUs;
                    }
                    //将选中文件绑定到repeateExcel中
                    repeateExcel.DataSource = dtproductExcel;
                    repeateExcel.DataBind();
                }
                else
                {
                    this.ShowMsg("Excel文件未导入或渠道商未选择！", false);
                }
            }
            catch
            {
                this.ShowMsg("系统错误！请联系管理员！", false);
            }


        }
        //保存事件
        protected void BcButton_Click(object sender, EventArgs e)
        {
            try
            {
                //获取内存表dtPr
                DataTable dt = (DataTable)ViewState["dtPr"];
                //获取保存状态
                string state = ViewState["state"].ToString();
                DataSet ds = new DataSet();

                ds.Tables.Add(dt);

                if (state == "no")
                {
                    this.ShowMsg("请确保没有重复数据", false);
                }
                else
                {
                    string sqls = "select * from Hishop_Products";
                    string[] sql = sqls.Split(';');

                    int count = DataBaseHelper.CommitDataSet(ds, sql);
                    //判断是否插入成功
                    if (count > 0)
                    {
                        //获取选中文件表
                        DataTable dtExcel = (DataTable)ViewState["dtExcel"];
                        //获取库存表
                        DataTable dtSKUs = (DataTable)ViewState["dtus"];
                        string ProductCode = ViewState["ProductCode"].ToString();
                        //通过商品货号，查询所有商品。主要通过货号查询商品编号，若不用货号查询，将查询到所有商品数据，在下面循环添加库存表商品ID是会出现错误
                        string strSql = "select * from Hishop_Products where ProductCode in (" + ProductCode+")";
                        //所查询到的行数应该和导入文件的行数一样
                        //获取最新product表所有内容
                        DataTable dtproduct = DataBaseHelper.GetDataSet(strSql).Tables[0];

                        for (int i = 0; i < dtExcel.Rows.Count; i++)
                        {
                            //判断product表中货号 和选中文件表中货号是否有相同
                            DataRow[] drs = dtproduct.Select(string.Format(@"ProductCode='{0}'", dtExcel.Rows[i]["货号"].ToString()), "", DataViewRowState.CurrentRows);
                            if (drs.Length > 0)
                            {
                                // 将选中文件表内容赋值到库存表
                                DataRow drnew = dtSKUs.NewRow();
                                drnew["SkuId"] = dtproduct.Rows[i]["ProductId"] + "_" + i;
                                drnew["ProductId"] = dtproduct.Rows[i]["ProductId"];
                                drnew["SKU"] = dtExcel.Rows[i]["货号"];
                                drnew["Stock"] = dtExcel.Rows[i]["商品库存"];
                                drnew["CostPrice"] = dtExcel.Rows[i]["成本价"];
                                drnew["SalePrice"] = dtExcel.Rows[i]["一口价"];
                                dtSKUs.Rows.Add(drnew);
                            }
                            else
                            {
                                
                            }
                        }
                        DataSet dss = new DataSet();
                        dss.Tables.Add(dtSKUs);
                        string[] sqlSKUs = " select * from Hishop_SKUs".Split(';');
                        int counts = DataBaseHelper.CommitDataSet(dss, sqlSKUs);
                        //判断是否执行成功
                        if (counts > 0)
                        {
                            this.ShowMsg("导入成功！", true);
                        }
                    }
                }
            }
            catch
            {
                this.ShowMsg("操作有误！", false);
            }
        }
        /// <summary>
        /// 下载表格模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XZ_Product_Click(object sender, EventArgs e)
        {
            string fileName = "Excel模板.xls";//客户端保存的文件名
            string filePath = Server.MapPath("../../Storage/Templates/商品导入模板.xls");//路径
            FileInfo fileInfo = new FileInfo(filePath);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            Response.End();
        }
    }
}

