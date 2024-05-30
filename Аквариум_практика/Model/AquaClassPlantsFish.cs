using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class AquaClassPlantsFish
    {
        public int IdAquaClassPlantsAqua { get; set; }
        public int? IdPlantsAquariums { get; set; }
        public int? IdAquariumClass { get; set; }
        public int? IdAquariumOrder { get; set; }
        public int? IdAquaFish { get; set; }

        public virtual AquariumFish IdAquaFishNavigation { get; set; }
        public virtual AquariumClassification IdAquariumClassNavigation { get; set; }
        public virtual AquariumOrder IdAquariumOrderNavigation { get; set; }
        public virtual PlantsForAquariums IdPlantsAquariumsNavigation { get; set; }
    }
}
