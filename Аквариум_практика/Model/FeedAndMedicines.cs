using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class FeedAndMedicines
    {
        public FeedAndMedicines()
        {
            FishAndFeedMedicines = new HashSet<FishAndFeedMedicines>();
        }

        public int IdFeedAndMedicines { get; set; }
        public string NameFeed { get; set; }
        public string TypeFeed { get; set; }
        public string NameMedicine { get; set; }
        public string TypeMedicine { get; set; }

        public virtual ICollection<FishAndFeedMedicines> FishAndFeedMedicines { get; set; }
    }
}
