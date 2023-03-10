using System;
using System.Collections.Generic;

namespace Database
{
    public partial class PharmacyNearby
    {
        public Guid Id { get; set; }
        public string PharmacyName { get; set; } = null!;
        public bool SupportsPriorAuth { get; set; }
        public int Distance { get; set; }
        public int Copay { get; set; }
    }
}
