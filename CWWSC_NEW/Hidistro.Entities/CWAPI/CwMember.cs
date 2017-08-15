using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.CWAPI
{
    public class CwMember
    {
        /// <summary>
        /// 用户Code
        /// </summary>
        public string usercode { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string cellphone { get; set; }

        /// <summary>
        /// 所属门店DZ号
        /// </summary>
        public string dzcode { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string storename { get; set; }

        /// <summary>
        /// 商品内码
        /// </summary>
        public string productcode { get; set; }

        /// <summary>
        /// 商品型号
        /// </summary>
        public string productmodel { get; set; }


        /// <summary>
        ///用户地址
        /// </summary>
        public string address { get; set; }




        //usercode	必须	String	50	会员编码
        //username	必须	String	50	会员姓名
        //cellphone	必须	String	50	会员电话
        //dzcode		String	50	所属门店DZ号
        //storename		String	256	门店名称
        //productcode		String	20	商品内码
        //productmodel		String	50	商品型号
        //address		String	300	地址

    }
}
