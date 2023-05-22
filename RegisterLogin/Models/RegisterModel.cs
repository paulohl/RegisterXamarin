using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterLogin.Models
{
    class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public int Industry { get; set; }
        public string CompanyName { get; set; }
        public string CompanyUrl { get; set; }
    }
}
