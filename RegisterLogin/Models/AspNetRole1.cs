using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterLogin.Models
{
    public partial class AspNetRole1
    {
        public AspNetRole1()
        {
            AspNetUserRoles = new HashSet<AspNetUserRole>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}
