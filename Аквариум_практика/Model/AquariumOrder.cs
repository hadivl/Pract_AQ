using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class AquariumOrder
    {
        public AquariumOrder()
        {
            AquaClassPlantsFish = new HashSet<AquaClassPlantsFish>();
        }

        public int IdAquariumOrder { get; set; }
        public int CompositionOfPlants { get; set; }
        public int Aquarium { get; set; }
        public int SetOfFish { get; set; }
        public int? OrderDate { get; set; }
        public string Customer { get; set; }
        public string Сashier { get; set; }

        public virtual ICollection<AquaClassPlantsFish> AquaClassPlantsFish { get; set; }
    }
}
