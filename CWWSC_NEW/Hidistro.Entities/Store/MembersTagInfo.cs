using System;
using System.Runtime.CompilerServices;

namespace Hidistro.Entities.Store
{
    public class MembersTagInfo
    {
        public MembersTagInfo()
        {
           
        }
        public virtual string userId { get; set; }

        public virtual string tagName { get; set; }

        public virtual string tagValue { get; set; }

        public virtual string scode { get; set; }

    }
}
