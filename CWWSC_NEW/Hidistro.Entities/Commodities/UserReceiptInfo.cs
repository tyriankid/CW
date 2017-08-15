using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class UserReceiptInfo
    {
        public int ReceiptId { get; set; }
        public int UserId { get; set; }
        public int Type { get; set; }
        public int Type1 { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string TaxesCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Bank { get; set; }
        public string BankNumber { get; set; }
        public string RegistrationImg { get; set; }
        public string EmpowerEntrustImg { get; set; }
        public string TaxpayerProveImg { get; set; }
        public int IsDefault { get; set; }
    }
}
