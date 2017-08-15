using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Members
{
    public  class MemberTagsQuery:Pagination
    {
        public string keyword { get; set; }

        public string tagstype { get; set; }
    }
}
