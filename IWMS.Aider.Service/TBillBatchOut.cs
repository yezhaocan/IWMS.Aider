using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IWMS.Aider.Service
{

    [Table("TBillBatchOut")]
    public class TBillBatchOut
    {
        [Key]
        public string BillId { get; set; }
        public string CKID { get; set; }
    }
}
