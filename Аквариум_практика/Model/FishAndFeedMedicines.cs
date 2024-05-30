using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class FishAndFeedMedicines
    {
        public int IdAquariumFeedMedicines { get; set; }
        public int? IdAquaFish { get; set; }
        public int? IdFeedAndMedicines { get; set; }

        public virtual AquariumFish IdAquaFishNavigation { get; set; }
        public virtual FeedAndMedicines IdFeedAndMedicinesNavigation { get; set; }
    }
}
