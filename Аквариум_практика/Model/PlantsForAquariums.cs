using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class PlantsForAquariums
    {

        public PlantsForAquariums()
        {
            AquaClassPlantsFish = new HashSet<AquaClassPlantsFish>();
        }

        public int IdPlantsAquariums { get; set; }
        public string Type { get; set; }
        public int LeafLength { get; set; }
        public int IdAquaFish { get; set; }

        public virtual AquariumFish IdAquaFishNavigation { get; set; }
        public virtual ICollection<AquaClassPlantsFish> AquaClassPlantsFish { get; set; }
    }

}
