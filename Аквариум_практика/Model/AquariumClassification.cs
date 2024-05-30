using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class AquariumClassification
    {
        public AquariumClassification()
        {
            AquaClassPlantsFish = new HashSet<AquaClassPlantsFish>();
            AquariumCleaningEquipment = new HashSet<AquariumCleaningEquipment>();
            Equipment = new HashSet<Equipment>();
        }

        public int IdAquariumClass { get; set; }
        public string TypeConstruction { get; set; }
        public string Location { get; set; }
        public string Scope { get; set; }
        public string Shape { get; set; }
        public int Volume { get; set; }

        public virtual ICollection<AquaClassPlantsFish> AquaClassPlantsFish { get; set; }
        public virtual ICollection<AquariumCleaningEquipment> AquariumCleaningEquipment { get; set; }
        public virtual ICollection<Equipment> Equipment { get; set; }
    }
}
