using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class AquariumCleaningEquipment
    {
        public int IdCleaningEquipment { get; set; }
        public string PumpingOutWater { get; set; }
        public string WaterInlet { get; set; }
        public string EquipPlantingFish { get; set; }
        public string Cleaning { get; set; }
        public string EquipCatchingFish { get; set; }
        public int? IdAquariumClass { get; set; }

        public virtual AquariumClassification IdAquariumClassNavigation { get; set; }
    }
}
