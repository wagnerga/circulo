using System;
using System.Collections.Generic;

namespace Database
{
    public partial class PharmacyVisited
    {
        public Guid Id { get; set; }
        public string PharmacyName { get; set; } = null!;
        public bool SupportsPriorAuth { get; set; }
    }
}
