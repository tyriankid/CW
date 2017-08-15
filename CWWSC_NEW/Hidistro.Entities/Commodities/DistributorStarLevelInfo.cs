using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class DistributorStarLevelInfo
    {
        /// <summary>
        ///星级评分等级实体
        /// </summary>
        public Guid StarLevelID { get; set; }
        public string  LevelName { get; set; }
        public int LevelNum { get; set; }
        public int  MinNum { get; set; }
        public int  MaxNum { get; set; }
        public int CommissionType { get; set; }
        public decimal CommissionRise { get; set; }
        public decimal CommissionMoney { get; set; }
        public bool  IsDefault { get; set; }
        public string  Ico { get; set; }
    }
}
