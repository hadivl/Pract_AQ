using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class AquariumFish
    {
        public AquariumFish()
        {
            AquaClassPlantsFish = new HashSet<AquaClassPlantsFish>();
            FishAndFeedMedicines = new HashSet<FishAndFeedMedicines>();
            PlantsForAquariums = new HashSet<PlantsForAquariums>();
        }

        public int IdAquaFish { get; set; }
        public string Name { get; set; }
        public int MaximumSize { get; set; }
        public int WaterTemperature { get; set; }
        public string pHOfTheWater { get; set; }
        public int RequiredVolumeAquarium { get; set; }
        public string TheNeedShelters { get; set; }

        public virtual ICollection<AquaClassPlantsFish> AquaClassPlantsFish { get; set; }
        public virtual ICollection<FishAndFeedMedicines> FishAndFeedMedicines { get; set; }
        public virtual ICollection<PlantsForAquariums> PlantsForAquariums { get; set; }
    }
}
