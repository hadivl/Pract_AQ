using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class Users
    {
        public int id_User { get; set; }
        public string LoginUser { get; set; }
        public string NameUser { get; set; }
        public string Password { get; set; }
    }
    
}
