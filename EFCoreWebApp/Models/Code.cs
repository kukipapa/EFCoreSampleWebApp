using System.Text.Json.Serialization;

namespace EFCoreWebApp.Models
{
    public partial class Code
    {
        public int Id { get; set; }
        public int CityCode { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
