using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.ExtensionWebsite.Models.ExtensionModels
{
    public class Extension
    {
        [Required] [Key] public Guid ID { get; set; }
        [Required] public Guid CollectionID { get; set; }
        [Required] public string ExtensionID { get; set; }
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        [Required] public string Author { get; set; }
        
        public virtual ExtensionCollection Collection { get; set; }
        public virtual ICollection<ExtensionVersion> Versions { get; set; }
    }

}
