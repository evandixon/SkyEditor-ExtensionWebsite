using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.ExtensionWebsite.Models.ExtensionModels
{
    public class ExtensionCollection
    {
        [Required] [Key] public Guid ID { get; set; }
        public Guid? ParentID { get; set; }
        [Required] public string Name { get; set; }

        public virtual ExtensionCollection Parent { get; set; }
    }
}
