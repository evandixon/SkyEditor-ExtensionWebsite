using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.ExtensionWebsite.Models.ExtensionModels
{
    public class ApiKey
    {
        [Required] [Key] public Guid ID { get; set; }
        [Required] public string Key { get; set; }
        [Required] public Guid CollectionID { get; set; }

        public virtual ExtensionCollection Collection { get; set; }
    }
}
 