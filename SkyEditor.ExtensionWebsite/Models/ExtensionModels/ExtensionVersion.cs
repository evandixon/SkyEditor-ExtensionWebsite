using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.ExtensionWebsite.Models.ExtensionModels
{
    public class ExtensionVersion
    {
       [Required] [Key, Column(Order=0)] public Guid ID { get; set; }
       [Required] [Key, Column(Order=1)] public string Version { get; set; }
       [Required] public Guid ExtensionID { get; set; }

        public virtual Extension Extension { get; set; }
    }

}
