using System;
using System.Collections.Generic;

namespace NtuPH2023.Models
{
    public partial class TblRelatedInfo
    {
        public Guid Uid { get; set; }
        public string CreatedUser { get; set; } = null!;
        public DateTime CreatedTimestamp { get; set; }
        public string LastModifiedUser { get; set; } = null!;
        public DateTime LastModifiedTimestamp { get; set; }
        public string? HtmlContent { get; set; }
    }
}
