using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class StoreInfo
    {
        public string storeName { get; set; }
        public int fgsid { get; set; }
        public int Id { get; set; }
        public string storeRelationPerson { get; set; }
        public string storeRelationCell { get; set; }
        public string accountALLHere { get; set; }
        public string scode { get; set; }
        public string storekeyid { get; set; }
        public int Auditing { get; set; }

        
        public string NameAndCode 
        {
            get { return storeName + "(" + accountALLHere + ")"; }
        }
    }
}
