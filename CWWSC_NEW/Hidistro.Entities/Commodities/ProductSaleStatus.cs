namespace Hidistro.Entities.Commodities
{
    using System;

    public enum ProductSaleStatus
    {
        All = -9,
        Delete = 0,
        OnSale = 1,
        OnStock = 3,
        UnSale = 2,
        OnAuditing = -1,
        NotAuditing = -2,
        line=-1,
    }
}

