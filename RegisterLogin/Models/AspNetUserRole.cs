using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterLogin.Models
{
    public partial class AspNetUserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual AspNetRole1 Role { get; set; }
        public virtual AspNetUser1 User { get; set; }
    }
}
