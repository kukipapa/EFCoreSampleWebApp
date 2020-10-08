using System.Collections.Generic;

namespace EFCoreWebApp.Models
{
    public partial class User
    {
        public User()
        {
            Codes = new HashSet<Code>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Code> Codes { get; set; }
    }
}
