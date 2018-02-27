using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IWMS.Aider.Service
{
    [Table("TDefStyle")]
    public class TDefStyle
    {
        [Key]
        public string Style { get; set; }
        public string StyleName { get; set; }
    }
}
