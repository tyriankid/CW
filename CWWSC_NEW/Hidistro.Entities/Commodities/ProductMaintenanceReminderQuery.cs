using Hidistro.Core.Entities;
using System;
using System.Runtime.CompilerServices;

namespace Hidistro.Entities.Commodities
{
    public class ProductMaintenanceReminderQuery : Pagination
    {

        public string productName { get; set; }
        public string isReminder { get; set; }
    }
}
