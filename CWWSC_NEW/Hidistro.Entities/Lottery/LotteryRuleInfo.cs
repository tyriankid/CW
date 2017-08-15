using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Lottery
{
    public class LotteryRuleInfo
    {
        public Guid RuleId { get; set; }
        public string LotteryItem { get; set; }
        public int LotteryProportion { get; set; }
        public int GiftId { get; set; }
        public string Name { get; set; }
        public int LotteryClass { get; set; }

    }
}
