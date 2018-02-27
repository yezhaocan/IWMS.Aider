using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IWMS.Aider.Service
{
    [Table("TDefSku")]
    public class TDefSku
    {
        [Key]
        public string Sku { get; set; }
        public string Style { get; set; }
        public string NationSku { get; set; }

    }
}
