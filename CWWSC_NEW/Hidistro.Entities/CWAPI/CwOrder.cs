using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.CWAPI
{
    public class CwOrder
    {
        /// <summary>
        /// 
        /// </summary>
        public string ReceivName { get; set;}

        public string ReceivTel { get; set; }

        public string ReceivPhone { get; set; }

        public string ReceivProvice { get; set; }

        public string ReceivCity { get; set; }

        public string ReceivArea { get; set; }

        public string ReceivAddress { get; set; }

        public string TaxName { get; set; }

        public string TaxPhone { get; set; }

        public string TaxMailAdd { get; set; }

        public string OrderNo { get; set; }

        public string OrderTime { get; set; }

        public string FValue { get; set; }

        public string ReValue { get; set; }

        public string OrderNote { get; set; }

        //private IList<CwDetails> details;
        //public IList<CwDetails> Details
        //{
        //    get
        //    {
        //        if (this.details == null)
        //        {
        //            this.details = new List<CwDetails>();
        //        }
        //        return this.details;
        //    }
        //}


        private Dictionary<string, CwDetails> details;
        /// <summary>
        /// 明细集合
        /// </summary>
        public Dictionary<string, CwDetails> Details
        {
            get
            {
                if (this.details == null)
                {
                    this.details = new Dictionary<string, CwDetails>();
                }
                return this.details;
            }
        }


    }
}
