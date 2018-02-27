using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IWMS.Aider.Service
{
    [Table("TBillUnicodeB2BRecord")]
    public class TBillUnicodeB2BRecord
    {
        [Key]
        public long FlowNo { get; set; }
        public string BillId { get; set; }
        public string UniCode { get; set; }
        public string Sku { get; set; }
        public int Qty { get; set; }
        public string Operator { get; set; }
        public DateTime OperDate { get; set; }
    }
}
