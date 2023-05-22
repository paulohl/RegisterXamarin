using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterLogin.Models
{
    public partial class AspNetUserClaim
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public virtual AspNetUser1 User { get; set; }
    }
}
