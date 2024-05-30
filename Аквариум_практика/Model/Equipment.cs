using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class Equipment
    {
        public int IdEquipment { get; set; }
        public string Lighting { get; set; }
        public string TemperatureControl { get; set; }
        public string WaterPurification { get; set; }
        public string AdditionalEquipment { get; set; }
        public int? IdAquariumClass { get; set; }

        public virtual AquariumClassification IdAquariumClassNavigation { get; set; }
    }
}
