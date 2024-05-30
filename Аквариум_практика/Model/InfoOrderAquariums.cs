using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class InfoOrderAquariums
    {
        public int? OrderDate { get; set; }
        public string Customer { get; set; }
        public string Scope { get; set; }
        public string Type { get; set; }
    }
}
