using System.ComponentModel.DataAnnotations.Schema;

namespace NtuPH2023.Models
{
    public partial class TblSetting
    {
        public Guid Uid { get; set; }
        public string CreatedUser { get; set; } = null!;
        public DateTime CreatedTimestamp { get; set; }
        public string Admins { get; set; } = null!;

        [NotMapped]
        public string[] AdminArr
        {
            get
            {
                if (string.IsNullOrEmpty(Admins) || string.IsNullOrWhiteSpace(Admins)) return Array.Empty<string>();

                return Admins.Split(';');
            }

            set
            {
                if (value == null) return;

                foreach (var item in value)
                {
                    if (string.IsNullOrEmpty(Admins))
                    {
                        Admins = item;
                    }
                    else
                    {
                        Admins += ";" + item;
                    }
                }
            }

        }
    }
}
