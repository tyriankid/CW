namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class PrizeSetting
    {
        //奖品等级
        public string PrizeLevel { get; set; }
        //奖品名称
        public string PrizeName { get; set; }
        //奖品数量
        public int PrizeNum { get; set; }
        //中奖概率
        public decimal Probability { get; set; }
        //赠送积分
        public int? Integral { get; set; }

    }
}

