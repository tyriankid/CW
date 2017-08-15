using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class ProductBackAccount
    {
        public int id { get; set; }

        public int productId { get; set; }

        public DateTime backTime { get; set; }

        public string backAccount { get; set; }
    }
}
