using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterLogin.Models
{
    public partial class AspNetUserLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUser1 User { get; set; }
    }
}
