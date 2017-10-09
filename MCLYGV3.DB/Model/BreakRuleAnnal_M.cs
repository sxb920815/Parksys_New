using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MCLYGV3.DB
{

    [Table("BreakRuleAnnal")]
    public class M_BreakRuleAnnal
    {
        [Key]
        public long BreakRuleAnnalId { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        public long? AreaId { get; set; }

        public long? EquipmentId { get; set; }

        [MaxLength(32)]
        public string State { get; set; }

        [MaxLength(32)]
        public string License { get; set; }

        [MaxLength(128)]
        public string ImagePath { get; set; }

        [MaxLength(256)]
        public string Note { get; set; }
    }

}
