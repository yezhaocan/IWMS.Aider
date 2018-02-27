using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IWMS.Aider.Service
{
    [Table("TDefWareHouse")]
    public class TDefWareHouse
    {
        [Key]
        public string WHId { get; set; }
        public string WHName { get; set; }
    }
}
