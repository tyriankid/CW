using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class OrderSales
    {
        /// <summary>
        ///
        /// </summary>
        public int OsId { get; set; }

        public string OrderId { get; set; }

        public int State { get; set; }

        public Guid ServiceSalesId { get; set; }

        public DateTime CreateDate { get; set; }

        public string RefuseRemark { get; set; }

    }
}
