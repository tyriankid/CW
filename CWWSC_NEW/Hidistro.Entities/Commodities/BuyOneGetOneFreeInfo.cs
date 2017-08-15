using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class BuyOneGetOneFreeInfo
    {
        /// <summary>
        ///买一送一实体
        /// </summary>
        public int buyoneId { get; set; }
        public int productId { get; set; }
        public int GetProductId { get; set; }
        public DateTime startime { get; set; }
        public DateTime endtime { get; set; }
        public int getNum { get; set; }
    }
}
