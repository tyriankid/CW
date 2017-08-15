namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SupplierInfo
    {
        [StringLengthValidator(0, 100, Ruleset = "ValProductType", MessageTemplate = "供应商地址的长度限制在0-100个字符之间"), HtmlCoding]
        public string gysAddress { get; set; }
        public int Id { get; set; }
        [StringLengthValidator(1, 30, Ruleset = "ValProductType", MessageTemplate = "供应商名称不能为空，长度限制在1-30个字符之间")]
        public string gysName { get; set; }
        public string gysPhone { get; set; }
        public string gysCode { get; set; }
        public string scode { get; set; }
    }
}


