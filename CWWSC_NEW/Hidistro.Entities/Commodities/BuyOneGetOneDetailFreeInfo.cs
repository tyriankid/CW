using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class BuyOneGetOneDetailFreeInfo
    {
        /// <summary>
        ///买一送一实体
        /// </summary>
        public int DetailId { get; set; }
        public int buyoneId { get; set; }
        public DateTime buyDate { get; set; }
        public int userId { get; set; }
    }
}
