using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Members
{
    public class CW_O2OMenbersInfo
    {
        public int userid { get; set; }
        public string  name { get; set; }
        public string mobile { get; set; }
        public string  profession { get; set; }
        public string  OpenId { get; set; }
        public string gradeName { get; set; }
        public string storeCode { get; set; }
        public string  birthday { get; set; }
        public string  sex { get; set; }
        public string  model { get; set; }
        public string buydate { get; set; }
        public int  typeid { get; set; }
        public decimal price { get; set; }
        public int  regionid { get; set; }
        public string Address { get; set; }
        public string remark { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public int RelationUserId { get; set; }
        public string typename { get; set; }
        public string Village { get; set; }
        public string OldRegion { get; set; }
        public DateTime CreateDate { get; set; }
        public string jiatingchengyuan { get; set; }
        public string zhufangxinxi { get; set; }
        public string fangyujiadian { get; set; }
        public string jiadianshiyong { get; set; }
        public string gerenqingxiang { get; set; }
        public string jinqixuqiu { get; set; }

        public int? IsUserWaterDarifier { get; set; }
        public string BuyWaterDarifierDate { get; set; }
    }
}
